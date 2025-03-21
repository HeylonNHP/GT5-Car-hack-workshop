using System;
using System.Collections.Generic;
using System.Globalization;

namespace GT5_Car_hack_workshop_2
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

        public static byte[] ConvertToByteArray(int value, int outputSize = 0)
        {
            // Throws an exception if the value is negative
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be non-negative.");

            // Create a list to store bytes
            var byteList = new List<byte>();

            // Extract the bytes from the integer
            while (value > 0)
            {
                // Get the least significant byte
                byteList.Insert(0, (byte)(value & 0xFF));
                // Shift right by 8 bits (1 byte)
                value >>= 8;
            }

            // If outputSize is greater than the current size, pad the array with leading zeros
            while (byteList.Count < outputSize)
                byteList.Insert(0, 0); // Add padding at the beginning

            // If the array exceeds the outputSize, throw an error
            if (outputSize != 0 && byteList.Count > outputSize)
                throw new ArgumentOutOfRangeException(nameof(outputSize), "Output size is too small to represent the value.");

            // Return the resulting byte array
            return byteList.Count > 0 ? byteList.ToArray() : new byte[] { 0 };
        }

        public static byte[] ConvertToByteArray(uint value, int outputSize = 0)
        {
            // Create a list to store bytes
            var byteList = new List<byte>();

            // Extract the bytes from the unsigned integer
            while (value > 0)
            {
                // Get the least significant byte
                byteList.Insert(0, (byte)(value & 0xFF));
                // Shift right by 8 bits (1 byte)
                value >>= 8;
            }

            // If outputSize is greater than the current size, pad the array with leading zeros
            while (byteList.Count < outputSize)
                byteList.Insert(0, 0); // Add padding at the beginning

            // If the array exceeds the outputSize, throw an error
            if (outputSize != 0 && byteList.Count > outputSize)
                throw new ArgumentOutOfRangeException(nameof(outputSize), "Output size is too small to represent the value.");

            // Return the resulting byte array
            return byteList.Count > 0 ? byteList.ToArray() : new byte[] { 0 };
        }
    }
} 