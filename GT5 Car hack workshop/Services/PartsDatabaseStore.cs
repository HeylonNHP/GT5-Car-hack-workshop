using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GT5_Car_hack_workshop.Models;
using Microsoft.Data.Sqlite;

namespace GT5_Car_hack_workshop.Services
{
    /// <summary>
    /// Reads and writes the car parts catalogue - the list of cars shown in the "parts" combo
    /// boxes - as a real SQLite database stored next to the executable.
    /// <para>
    /// Older releases stored the catalogue as a comma-separated text file (one car per line).
    /// Such a file is transparently migrated the first time it is opened: every row is imported,
    /// the old text file is kept as <c>&lt;name&gt;.legacy.csv</c>, and the main file is replaced
    /// by a proper SQLite database. After that the check is a no-op.
    /// </para>
    /// </summary>
    public static class PartsDatabaseStore
    {
        /// <summary>The catalogue file name, resolved next to the executable when no path is given.</summary>
        public const string FileName = "partsdatabase.db";

        // Every part id is a ushort; 65535 ("stock/none") is the value used when a part is absent.
        private const int DefaultPartId = ushort.MaxValue;

        private static readonly byte[] SqliteMagic = Encoding.ASCII.GetBytes("SQLite format 3\0");

        private const string SchemaSql =
            "CREATE TABLE IF NOT EXISTS CarParts (" +
            "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "Name TEXT NOT NULL UNIQUE, " +
            "Engine INTEGER NOT NULL DEFAULT 65535, " +
            "Drivetrain INTEGER NOT NULL DEFAULT 65535, " +
            "Chassis INTEGER NOT NULL DEFAULT 65535, " +
            "Transmission INTEGER NOT NULL DEFAULT 65535, " +
            "Suspension INTEGER NOT NULL DEFAULT 65535, " +
            "Body INTEGER NOT NULL DEFAULT 65535, " +
            "Lsd INTEGER NOT NULL DEFAULT 65535, " +
            "Horn INTEGER NOT NULL DEFAULT 65535, " +
            "Turbo INTEGER NOT NULL DEFAULT 65535, " +
            "Exhaust INTEGER NOT NULL DEFAULT 65535, " +
            "Weight INTEGER NOT NULL DEFAULT 65535, " +
            "Brake INTEGER NOT NULL DEFAULT 65535, " +
            "BrakeController INTEGER NOT NULL DEFAULT 65535, " +
            "Displacement INTEGER NOT NULL DEFAULT 65535, " +
            "Computer INTEGER NOT NULL DEFAULT 65535, " +
            "Natune INTEGER NOT NULL DEFAULT 65535, " +
            "Flywheel INTEGER NOT NULL DEFAULT 65535, " +
            "Clutch INTEGER NOT NULL DEFAULT 65535, " +
            "PropellerShaft INTEGER NOT NULL DEFAULT 65535, " +
            "Intercooler INTEGER NOT NULL DEFAULT 65535, " +
            "Supercharger INTEGER NOT NULL DEFAULT 65535, " +
            "IntakeManifold INTEGER NOT NULL DEFAULT 65535, " +
            "ExhaustManifold INTEGER NOT NULL DEFAULT 65535, " +
            "Catalyst INTEGER NOT NULL DEFAULT 65535, " +
            "AirCleaner INTEGER NOT NULL DEFAULT 65535, " +
            "Nos INTEGER NOT NULL DEFAULT 65535);";

        private const string InsertSql =
            "INSERT INTO CarParts (Name, Engine, Drivetrain, Chassis, Transmission, Suspension, Body, Lsd, Horn, Turbo, Exhaust, Weight, " +
            "Brake, " +
            "BrakeController, " +
            "Displacement, " +
            "Computer, " +
            "Natune, " +
            "Flywheel, " +
            "Clutch, " +
            "PropellerShaft, " +
            "Intercooler, " +
            "Supercharger, " +
            "IntakeManifold, " +
            "ExhaustManifold, " +
            "Catalyst, " +
            "AirCleaner, " +
            "Nos) " +
            "VALUES (@name, @engine, @drivetrain, @chassis, @transmission, @suspension, @body, @lsd, @horn, @turbo, @exhaust, @weight, " +
            "@brake, " +
            "@brakeController, " +
            "@displacement, " +
            "@computer, " +
            "@natune, " +
            "@flywheel, " +
            "@clutch, " +
            "@propellerShaft, " +
            "@intercooler, " +
            "@supercharger, " +
            "@intakeManifold, " +
            "@exhaustManifold, " +
            "@catalyst, " +
            "@airCleaner, " +
            "@nos)";

        /// <summary>
        /// Returns every car in the catalogue, ordered by name. Missing databases are created
        /// (empty); legacy CSV databases are migrated to SQLite first.
        /// </summary>
        /// <param name="path">Absolute path, or <c>null</c> for <c>&lt;app dir&gt;/partsdatabase.db</c>.</param>
        public static List<CarParts> LoadAll(string? path = null)
        {
            path = ResolvePath(path);
            EnsureUpToDate(path);

            var cars = new List<CarParts>();
            using var connection = Open(path);
            EnsureSchema(connection);

            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT Name, Engine, Drivetrain, Chassis, Transmission, Suspension, Body, Lsd, Horn, Turbo, Exhaust, Weight, " +
                "Brake, " +
                "BrakeController, " +
                "Displacement, " +
                "Computer, " +
                "Natune, " +
                "Flywheel, " +
                "Clutch, " +
                "PropellerShaft, " +
                "Intercooler, " +
                "Supercharger, " +
                "IntakeManifold, " +
                "ExhaustManifold, " +
                "Catalyst, " +
                "AirCleaner, " +
                "Nos " +
                "FROM CarParts ORDER BY Name";
            using var reader = command.ExecuteReader();
            while (reader.Read()) cars.Add(ReadCar(reader));
            return cars;
        }

        /// <summary>
        /// Replaces the whole catalogue with <paramref name="cars"/> in a single transaction,
        /// matching the previous CSV "delete the file and rewrite it" behaviour.
        /// </summary>
        /// <param name="path">Absolute path, or <c>null</c> for <c>&lt;app dir&gt;/partsdatabase.db</c>.</param>
        public static void SaveAll(IEnumerable<CarParts> cars, string? path = null)
        {
            ArgumentNullException.ThrowIfNull(cars);
            path = ResolvePath(path);
            EnsureUpToDate(path);

            using var connection = Open(path);
            EnsureSchema(connection);

            using var transaction = connection.BeginTransaction();

            using (var delete = connection.CreateCommand())
            {
                delete.Transaction = transaction;
                delete.CommandText = "DELETE FROM CarParts";
                delete.ExecuteNonQuery();
            }

            using (var insert = CreateInsertCommand(connection, transaction))
            {
                foreach (var car in cars)
                {
                    BindCar(insert, car);
                    insert.ExecuteNonQuery();
                }
            }

            transaction.Commit();
        }

        /// <summary>
        /// Inserts <paramref name="car"/>, or updates the existing row when its <see cref="CarParts.Name"/>
        /// is already present (the <c>Name</c> column is UNIQUE).
        /// </summary>
        /// <param name="path">Absolute path, or <c>null</c> for <c>&lt;app dir&gt;/partsdatabase.db</c>.</param>
        public static void Upsert(CarParts car, string? path = null)
        {
            ArgumentNullException.ThrowIfNull(car);
            path = ResolvePath(path);
            EnsureUpToDate(path);

            using var connection = Open(path);
            EnsureSchema(connection);

            using var command = CreateInsertCommand(connection, null);
            command.CommandText = InsertSql +
                " ON CONFLICT(Name) DO UPDATE SET " +
                "Engine = excluded.Engine, Drivetrain = excluded.Drivetrain, Chassis = excluded.Chassis, " +
                "Transmission = excluded.Transmission, Suspension = excluded.Suspension, Body = excluded.Body, " +
                "Lsd = excluded.Lsd, Horn = excluded.Horn, Turbo = excluded.Turbo, " +
                "Exhaust = excluded.Exhaust, Weight = excluded.Weight, " +
                "Brake = excluded.Brake, " +
                "BrakeController = excluded.BrakeController, " +
                "Displacement = excluded.Displacement, " +
                "Computer = excluded.Computer, " +
                "Natune = excluded.Natune, " +
                "Flywheel = excluded.Flywheel, " +
                "Clutch = excluded.Clutch, " +
                "PropellerShaft = excluded.PropellerShaft, " +
                "Intercooler = excluded.Intercooler, " +
                "Supercharger = excluded.Supercharger, " +
                "IntakeManifold = excluded.IntakeManifold, " +
                "ExhaustManifold = excluded.ExhaustManifold, " +
                "Catalyst = excluded.Catalyst, " +
                "AirCleaner = excluded.AirCleaner, " +
                "Nos = excluded.Nos";
            BindCar(command, car);
            command.ExecuteNonQuery();
        }

        private static string ResolvePath(string? path) =>
            string.IsNullOrEmpty(path) ? Path.Combine(AppContext.BaseDirectory, FileName) : path;

        /// <summary>Creates the database/schema when missing, or migrates a legacy CSV in place.</summary>
        private static void EnsureUpToDate(string path)
        {
            if (File.Exists(path) && !IsSqlite(path))
            {
                MigrateLegacyCsv(path);
                return;
            }

            // Missing file, or already SQLite: just make sure the table exists.
            using var connection = Open(path);
            EnsureSchema(connection);
        }

        /// <summary>True when the file exists and starts with the SQLite magic header.</summary>
        private static bool IsSqlite(string path)
        {
            if (!File.Exists(path)) return false;

            using var stream = File.OpenRead(path);
            var header = new byte[SqliteMagic.Length];
            var read = stream.Read(header, 0, header.Length);
            if (read != header.Length) return false;

            for (var i = 0; i < header.Length; i++)
                if (header[i] != SqliteMagic[i]) return false;

            return true;
        }

        /// <summary>
        /// Imports a legacy comma-separated catalogue into a fresh SQLite database at the same
        /// path, preserving the old file as <c>&lt;name&gt;.legacy.csv</c>. Idempotent: the backup
        /// is only created if it does not already exist.
        /// </summary>
        private static void MigrateLegacyCsv(string path)
        {
            var cars = ParseLegacyCsv(path);

            // A legacy CSV can contain duplicate rows (the shipped file has one exact duplicate).
            // Name is UNIQUE - and Upsert relies on it - so import each name only once instead of
            // failing on the constraint (which would crash the app on load).
            cars = cars
                .GroupBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();

            var tempPath = path + ".tmp";
            if (File.Exists(tempPath)) File.Delete(tempPath);
            SaveAll(cars, tempPath);

            var directory = Path.GetDirectoryName(path);
            var baseName = Path.GetFileNameWithoutExtension(path);
            var legacyPath = string.IsNullOrEmpty(directory)
                ? baseName + ".legacy.csv"
                : Path.Combine(directory, baseName + ".legacy.csv");

            if (File.Exists(legacyPath))
                File.Delete(path);              // keep the original backup untouched
            else
                File.Move(path, legacyPath);    // preserve the old CSV before replacing it

            File.Move(tempPath, path, overwrite: true);
        }

        /// <summary>
        /// Parses a legacy CSV line set. Lines carry 9 fields (the original format) or 12 fields
        /// (Turbo/Exhaust/Weight appended); the extra fields default to 65535 when absent. Part
        /// fields are big-endian hex such as <c>0A 5D</c>.
        /// </summary>
        private static List<CarParts> ParseLegacyCsv(string path)
        {
            var cars = new List<CarParts>();
            if (!File.Exists(path)) return cars;

            foreach (var rawLine in File.ReadAllLines(path))
            {
                var line = rawLine.Trim();
                if (line.Length == 0) continue;

                var fields = line.Split(',');
                if (fields.Length < 9) continue; // not a complete row

                try
                {
                    cars.Add(new CarParts
                    {
                        Name = fields[0],
                        Engine = ByteUtils.HexStringToUshort(fields[1]),
                        Drivetrain = ByteUtils.HexStringToUshort(fields[2]),
                        Chassis = ByteUtils.HexStringToUshort(fields[3]),
                        Transmission = ByteUtils.HexStringToUshort(fields[4]),
                        Body = ByteUtils.HexStringToUshort(fields[5]),
                        Suspension = ByteUtils.HexStringToUshort(fields[6]),
                        Lsd = ByteUtils.HexStringToUshort(fields[7]),
                        Horn = ByteUtils.HexStringToUshort(fields[8]),
                        Turbo = fields.Length > 9 ? ByteUtils.HexStringToUshort(fields[9]) : (ushort)DefaultPartId,
                        Exhaust = fields.Length > 10 ? ByteUtils.HexStringToUshort(fields[10]) : (ushort)DefaultPartId,
                        Weight = fields.Length > 11 ? ByteUtils.HexStringToUshort(fields[11]) : (ushort)DefaultPartId
                    });
                }
                catch (Exception)
                {
                    // Skip malformed rows, matching the old CSV loader.
                }
            }

            return cars;
        }

        private static SqliteConnection Open(string path)
        {
            var connection = new SqliteConnection(
                new SqliteConnectionStringBuilder { DataSource = path }.ConnectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// The part columns the table is expected to have, in the order they are stored. Kept here
        /// so that a database created by an older build can be brought up to date - "CREATE TABLE
        /// IF NOT EXISTS" leaves an existing table exactly as it was, so new columns have to be
        /// added separately.
        /// </summary>
        private static readonly string[] PartColumns =
        {
            "Engine", "Drivetrain", "Chassis", "Transmission", "Suspension", "Body", "Lsd", "Horn",
            "Turbo", "Exhaust", "Weight",
            "Brake",
            "BrakeController",
            "Displacement",
            "Computer",
            "Natune",
            "Flywheel",
            "Clutch",
            "PropellerShaft",
            "Intercooler",
            "Supercharger",
            "IntakeManifold",
            "ExhaustManifold",
            "Catalyst",
            "AirCleaner",
            "Nos"
        };

        private static void EnsureSchema(SqliteConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = SchemaSql;
                command.ExecuteNonQuery();
            }

            // Add any column this database is missing, so an older parts database keeps working
            // instead of failing when a newer part is read or written.
            var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA table_info(CarParts)";
                using var reader = command.ExecuteReader();
                while (reader.Read()) existing.Add(reader.GetString(1));
            }

            foreach (var column in PartColumns)
            {
                if (existing.Contains(column)) continue;

                using var command = connection.CreateCommand();
                command.CommandText = $"ALTER TABLE CarParts ADD COLUMN {column} INTEGER NOT NULL DEFAULT {DefaultPartId}";
                command.ExecuteNonQuery();
            }
        }

        private static SqliteCommand CreateInsertCommand(SqliteConnection connection, SqliteTransaction? transaction)
        {
            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = InsertSql;
            command.Parameters.Add("@name", SqliteType.Text);
            command.Parameters.Add("@engine", SqliteType.Integer);
            command.Parameters.Add("@drivetrain", SqliteType.Integer);
            command.Parameters.Add("@chassis", SqliteType.Integer);
            command.Parameters.Add("@transmission", SqliteType.Integer);
            command.Parameters.Add("@suspension", SqliteType.Integer);
            command.Parameters.Add("@body", SqliteType.Integer);
            command.Parameters.Add("@lsd", SqliteType.Integer);
            command.Parameters.Add("@horn", SqliteType.Integer);
            command.Parameters.Add("@turbo", SqliteType.Integer);
            command.Parameters.Add("@exhaust", SqliteType.Integer);
            command.Parameters.Add("@weight", SqliteType.Integer);
            command.Parameters.Add("@brake", SqliteType.Integer);
            command.Parameters.Add("@brakeController", SqliteType.Integer);
            command.Parameters.Add("@displacement", SqliteType.Integer);
            command.Parameters.Add("@computer", SqliteType.Integer);
            command.Parameters.Add("@natune", SqliteType.Integer);
            command.Parameters.Add("@flywheel", SqliteType.Integer);
            command.Parameters.Add("@clutch", SqliteType.Integer);
            command.Parameters.Add("@propellerShaft", SqliteType.Integer);
            command.Parameters.Add("@intercooler", SqliteType.Integer);
            command.Parameters.Add("@supercharger", SqliteType.Integer);
            command.Parameters.Add("@intakeManifold", SqliteType.Integer);
            command.Parameters.Add("@exhaustManifold", SqliteType.Integer);
            command.Parameters.Add("@catalyst", SqliteType.Integer);
            command.Parameters.Add("@airCleaner", SqliteType.Integer);
            command.Parameters.Add("@nos", SqliteType.Integer);
            return command;
        }

        private static void BindCar(SqliteCommand command, CarParts car)
        {
            command.Parameters["@name"].Value = car.Name;
            command.Parameters["@engine"].Value = (int)car.Engine;
            command.Parameters["@drivetrain"].Value = (int)car.Drivetrain;
            command.Parameters["@chassis"].Value = (int)car.Chassis;
            command.Parameters["@transmission"].Value = (int)car.Transmission;
            command.Parameters["@suspension"].Value = (int)car.Suspension;
            command.Parameters["@body"].Value = (int)car.Body;
            command.Parameters["@lsd"].Value = (int)car.Lsd;
            command.Parameters["@horn"].Value = (int)car.Horn;
            command.Parameters["@turbo"].Value = (int)car.Turbo;
            command.Parameters["@exhaust"].Value = (int)car.Exhaust;
            command.Parameters["@weight"].Value = (int)car.Weight;
            command.Parameters["@brake"].Value = (int)car.Brake;
            command.Parameters["@brakeController"].Value = (int)car.BrakeController;
            command.Parameters["@displacement"].Value = (int)car.Displacement;
            command.Parameters["@computer"].Value = (int)car.Computer;
            command.Parameters["@natune"].Value = (int)car.Natune;
            command.Parameters["@flywheel"].Value = (int)car.Flywheel;
            command.Parameters["@clutch"].Value = (int)car.Clutch;
            command.Parameters["@propellerShaft"].Value = (int)car.PropellerShaft;
            command.Parameters["@intercooler"].Value = (int)car.Intercooler;
            command.Parameters["@supercharger"].Value = (int)car.Supercharger;
            command.Parameters["@intakeManifold"].Value = (int)car.IntakeManifold;
            command.Parameters["@exhaustManifold"].Value = (int)car.ExhaustManifold;
            command.Parameters["@catalyst"].Value = (int)car.Catalyst;
            command.Parameters["@airCleaner"].Value = (int)car.AirCleaner;
            command.Parameters["@nos"].Value = (int)car.Nos;
        }

        private static CarParts ReadCar(SqliteDataReader reader) => new CarParts
        {
            Name = reader.GetString(0),
            Engine = Part(reader, 1),
            Drivetrain = Part(reader, 2),
            Chassis = Part(reader, 3),
            Transmission = Part(reader, 4),
            Suspension = Part(reader, 5),
            Body = Part(reader, 6),
            Lsd = Part(reader, 7),
            Horn = Part(reader, 8),
            Turbo = Part(reader, 9),
            Exhaust = Part(reader, 10),
            Weight = Part(reader, 11),
            Brake = Part(reader, 12),
            BrakeController = Part(reader, 13),
            Displacement = Part(reader, 14),
            Computer = Part(reader, 15),
            Natune = Part(reader, 16),
            Flywheel = Part(reader, 17),
            Clutch = Part(reader, 18),
            PropellerShaft = Part(reader, 19),
            Intercooler = Part(reader, 20),
            Supercharger = Part(reader, 21),
            IntakeManifold = Part(reader, 22),
            ExhaustManifold = Part(reader, 23),
            Catalyst = Part(reader, 24),
            AirCleaner = Part(reader, 25),
            Nos = Part(reader, 26)
        };

        private static ushort Part(SqliteDataReader reader, int ordinal) =>
            reader.IsDBNull(ordinal) ? (ushort)DefaultPartId : (ushort)reader.GetInt32(ordinal);
    }
}
