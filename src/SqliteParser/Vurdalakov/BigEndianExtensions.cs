// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov
{
    using System;

    internal static class BigEndianExtensions
    {
        static BigEndianExtensions()
        {
            if (!BitConverter.IsLittleEndian)
            {
                throw new Exception("Only works if computer architecture is little-endian");
            }
        }

        public static UInt64 ToInt16(this Byte[] bytes, UInt64 offset) => (UInt64)(bytes[offset] << 8 | bytes[offset + 1]);

        public static UInt64 ToInt32(this Byte[] bytes, UInt64 offset) => bytes.ToInt16(offset) << 16 | bytes.ToInt16(offset + 2);

        public static UInt64 ToInt64(this Byte[] bytes, UInt64 offset) => bytes.ToInt32(offset) << 32 | bytes.ToInt32(offset + 4);

        public static Double ToDouble(this UInt64 integer) => BitConverter.Int64BitsToDouble((Int64)BigEndianExtensions.SwapEndianness(integer));

        public static UInt64 SwapEndianness(this UInt64 x)
        {
            x = (x >> 32) | (x << 32);
            x = ((x & 0xFFFF0000FFFF0000) >> 16) | ((x & 0x0000FFFF0000FFFF) << 16);
            return ((x & 0xFF00FF00FF00FF00) >> 8) | ((x & 0x00FF00FF00FF00FF) << 8);
        }
    }
}
