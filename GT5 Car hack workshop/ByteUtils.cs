using System;
using System.Collections.Generic;
using System.Globalization;

namespace GT5_Car_hack_workshop
{
    public static class ByteUtils
    {
        public static uint ConvertBytesToUnsignedInt(byte[] bytes)
        {
            Array.Reverse(bytes); // Reverse the byte order

            uint result = 0;
            for (var i = 0; i < bytes.Length; i++) result += (uint)bytes[i] << (i * 8);

            return result;
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex)) throw new ArgumentException("Input hex string cannot be null or empty.");

            // Remove any whitespace from the hex string
            hex = hex.Replace(" ", "");

            // Ensure the string contains an even number of characters
            if (hex.Length % 2 != 0) throw new FormatException("Hex string must have an even number of characters.");

            var byteList = new List<byte>();

            for (var i = 0; i < hex.Length; i += 2)
            {
                var byteString = hex.Substring(i, 2);
                byteList.Add(byte.Parse(byteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
            }

            return byteList.ToArray();
        }

        public static ushort HexStringToUshort(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex)) throw new ArgumentException("Input hex string cannot be null or empty.");

            // Remove any whitespace from the hex string
            hex = hex.Replace(" ", "");

            // Ensure the string contains exactly 4 characters (2 bytes)
            if (hex.Length != 4) throw new FormatException("Hex string must be exactly 4 characters (2 bytes) for a ushort value.");

            // Convert the hex string to a byte array
            byte[] bytes = HexStringToByteArray(hex);
            
            // Convert the bytes to a ushort (big-endian)
            return (ushort)((bytes[0] << 8) | bytes[1]);
        }

        public static string UshortToHexString(ushort value)
        {
            // Convert the ushort to two bytes (big-endian)
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(value >> 8);  // High byte
            bytes[1] = (byte)(value & 0xFF); // Low byte
            
            // Convert to hex string with a space between bytes
            return BitConverter.ToString(bytes).Replace("-", " ");
        }

        public static byte[] UshortToByteArray(ushort value)
        {
            return new byte[] { (byte)(value >> 8), (byte)(value & 0xFF) };
        }

        public static byte[] UintToByteArray(uint value)
        {
            return new byte[] 
            { 
                (byte)(value >> 24),  // Most significant byte
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)(value & 0xFF)   // Least significant byte
            };
        }

        public static ushort BytesToUshort(byte highByte, byte lowByte)
        {
            // Combine two bytes into a ushort using big-endian byte order
            return (ushort)((highByte << 8) | lowByte);
        }
    }
} 