using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace GT5_Car_hack_workshop.Services
{
    /// <summary>
    /// Wraps an in-memory SQLite database that can be loaded from, and serialized back to,
    /// a plain byte array.
    /// <para>
    /// GT5 stores its garage, item box and other tables in a SQLite database embedded inside
    /// the <c>GT5.0</c> save. That database must be edited with a real SQLite engine (its page
    /// and index structures are far too intricate to patch by hand), but we do not want to
    /// touch the disk. This class loads the database bytes into memory, lets callers run SQL
    /// against them, and hands the edited database back as a byte array.
    /// </para>
    /// </summary>
    public sealed class EmbeddedSqliteDb : IDisposable
    {
        /// <summary>The page size used by the save's embedded database.</summary>
        public const int PageSize = 1024;

        private readonly SqliteConnection _connection;
        private SqliteTransaction? _transaction;
        private bool _disposed;

        private EmbeddedSqliteDb(SqliteConnection connection) => _connection = connection;

        /// <summary>
        /// Opens an in-memory copy of the database held in <paramref name="databaseBytes"/>.
        /// The array must contain a whole number of <see cref="PageSize"/>-sized pages.
        /// </summary>
        public static EmbeddedSqliteDb OpenFromBytes(byte[] databaseBytes)
        {
            ArgumentNullException.ThrowIfNull(databaseBytes);

            if (databaseBytes.Length == 0 || databaseBytes.Length % PageSize != 0)
                throw new ArgumentException(
                    $"The database must be a positive multiple of {PageSize} bytes (was {databaseBytes.Length}).",
                    nameof(databaseBytes));

            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var buffer = raw.sqlite3_malloc64(databaseBytes.Length);
            if (buffer == IntPtr.Zero)
            {
                connection.Dispose();
                throw new OutOfMemoryException("Unable to allocate memory for the embedded database.");
            }

            Marshal.Copy(databaseBytes, 0, buffer, databaseBytes.Length);

            // On success SQLite takes ownership of the buffer (FREEONCLOSE) and frees it when the
            // connection is closed; RESIZEABLE lets the database grow when an insert needs new
            // pages. On failure SQLite does not take ownership, so we free the buffer ourselves.
            var result = raw.sqlite3_deserialize(
                connection.Handle, "main", buffer,
                databaseBytes.Length, databaseBytes.Length,
                raw.SQLITE_DESERIALIZE_FREEONCLOSE | raw.SQLITE_DESERIALIZE_RESIZEABLE);

            if (result != 0)
            {
                raw.sqlite3_free(buffer);
                var message = raw.sqlite3_errmsg(connection.Handle).utf8_to_string();
                connection.Dispose();
                throw new InvalidOperationException($"Could not load the embedded database (rc={result}: {message}).");
            }

            return new EmbeddedSqliteDb(connection);
        }

        /// <summary>Runs a non-query statement and returns the number of rows affected.</summary>
        public int Execute(string sql, Action<SqliteParameterCollection>? bind = null)
        {
            ThrowIfDisposed();
            using var command = CreateCommand(sql, bind);
            return command.ExecuteNonQuery();
        }

        /// <summary>Runs a query that returns a single integer value.</summary>
        public long ScalarInt64(string sql, Action<SqliteParameterCollection>? bind = null)
        {
            ThrowIfDisposed();
            using var command = CreateCommand(sql, bind);
            var value = command.ExecuteScalar();
            return value is null or DBNull ? 0L : Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        /// <summary>Runs a query and invokes <paramref name="read"/> for every returned row.</summary>
        public void Query(string sql, Action<SqliteDataReader> read, Action<SqliteParameterCollection>? bind = null)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(read);

            using var command = CreateCommand(sql, bind);
            using var reader = command.ExecuteReader();
            while (reader.Read()) read(reader);
        }

        /// <summary>
        /// Runs <paramref name="action"/> inside a single transaction so a batch of statements
        /// is committed atomically (and much faster). Nested calls simply join the open
        /// transaction.
        /// </summary>
        public void RunInTransaction(Action action)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(action);

            if (_transaction is not null)
            {
                action();
                return;
            }

            using var transaction = _connection.BeginTransaction();
            _transaction = transaction;
            try
            {
                action();
                transaction.Commit();
            }
            finally
            {
                _transaction = null;
            }
        }

        /// <summary>
        /// Serializes the current contents of the database back into a byte array whose length
        /// is a whole number of <see cref="PageSize"/>-sized pages.
        /// </summary>
        public byte[] Serialize()
        {
            ThrowIfDisposed();

            if (_transaction is not null)
                throw new InvalidOperationException("The database cannot be serialized while a transaction is open.");

            // With flags = 0 SQLite hands back a freshly allocated copy which we must free.
            var pointer = raw.sqlite3_serialize(_connection.Handle, "main", out var size, 0);
            if (pointer == IntPtr.Zero)
                throw new InvalidOperationException("Could not serialize the embedded database.");

            try
            {
                var result = new byte[size];
                Marshal.Copy(pointer, result, 0, (int)size);
                return result;
            }
            finally
            {
                raw.sqlite3_free(pointer);
            }
        }

        private SqliteCommand CreateCommand(string sql, Action<SqliteParameterCollection>? bind)
        {
            var command = _connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = _transaction;
            bind?.Invoke(command.Parameters);
            return command;
        }

        private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _transaction?.Dispose();
            _transaction = null;
            _connection.Dispose();
        }
    }
}
