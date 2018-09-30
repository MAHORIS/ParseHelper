using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Reflection;

namespace ParseHelper.UnitTest
{
    /// <summary>
    /// ParseHelperTest
    /// </summary>
    /// <remarks>
    /// release 2018/09/30 ver 1.0.0.0
    /// </remarks>
    [TestClass]
    public class ParseHelperTest
    {
        #region *** private assertions ***

        private class PrivateAssert
        {
            public static void ThrowsWrappedException<TException, TInnerException>(Action action)
                where TException : Exception
                where TInnerException : Exception
            {
                Exception caughtException = null;
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }
                if (caughtException == null)
                {
                    throw new AssertFailedException("No exception was thrown.");
                }
                if (!(caughtException is TException))
                {
                    throw new AssertFailedException($"Unexpected {caughtException.GetType().FullName} was thrown. Expected exception is {typeof(TException).FullName}.");
                }
                var innerException = caughtException.InnerException;
                if (innerException == null)
                {
                    throw new AssertFailedException($"No wrapped exception exist in caught {caughtException.GetType().FullName}.");
                }
                if (!(innerException is TInnerException))
                {
                    throw new AssertFailedException($"Unexpected {innerException.GetType().FullName} was wrapped in {caughtException.GetType().FullName}. Expected wrapped exception is {typeof(TInnerException).FullName}.");
                }
            }
        }

        #endregion // *** assertions ***

        #region *** non generic methods ***

        #region *** Parse(Type, ... ) overloads ***

        /// <summary>
        /// public static object Parse(Type targetType, string input)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse 01: Parse(Type, string)")]
        public void Test_Parse_01()
        {
            // int(Int32)
            int expectedInt; int parsedInt;
            expectedInt = 123;
            parsedInt = (int)ParseHelper.Parse(typeof(int), expectedInt.ToString());
            Assert.AreEqual(expectedInt, parsedInt);
            expectedInt = -123;
            parsedInt = (int)ParseHelper.Parse(typeof(int), expectedInt.ToString());
            Assert.AreEqual(expectedInt, parsedInt);
            PrivateAssert.ThrowsWrappedException<TargetInvocationException, FormatException>(() =>
                ParseHelper.Parse(typeof(int), string.Empty));

            // uint(UInt32)
            uint parsedUint = (uint)ParseHelper.Parse(typeof(uint), "123");
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            short parsedShort = (short)ParseHelper.Parse(typeof(short), "123");
            Assert.AreEqual((short)123, parsedShort);
            parsedShort = (short)ParseHelper.Parse(typeof(short), "-123");
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            ushort parsedUshort = (ushort)ParseHelper.Parse(typeof(ushort), "456");
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            long parsedLong = (long)ParseHelper.Parse(typeof(long), "123");
            Assert.AreEqual((long)123, parsedLong);
            parsedLong = (long)ParseHelper.Parse(typeof(long), "-123");
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            ulong parsedUlong = (ulong)ParseHelper.Parse(typeof(ulong), "123");
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            float parsedFloat = (float)ParseHelper.Parse(typeof(float), "123.45");
            Assert.AreEqual((float)123.45, parsedFloat);
            parsedFloat = (float)ParseHelper.Parse(typeof(float), "-123.45");
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            double parsedDouble = (double)ParseHelper.Parse(typeof(double), "123.45");
            Assert.AreEqual((double)123.45, parsedDouble);
            parsedDouble = (double)ParseHelper.Parse(typeof(double), "-123.45");
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            decimal parsedDecimal = (decimal)ParseHelper.Parse(typeof(decimal), "123.45");
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            parsedDecimal = (decimal)ParseHelper.Parse(typeof(decimal), "-123.45");
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            byte parsedByte = (byte)ParseHelper.Parse(typeof(byte), "123");
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            sbyte parsedSbyte = (sbyte)ParseHelper.Parse(typeof(sbyte), "123");
            Assert.AreEqual((sbyte)123, parsedSbyte);
            parsedSbyte = (sbyte)ParseHelper.Parse(typeof(sbyte), "-123");
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool(Boolean)
            bool parsedBool = (bool)ParseHelper.Parse(typeof(bool), "true");
            Assert.AreEqual(true, parsedBool);
            parsedBool = (bool)ParseHelper.Parse(typeof(bool), "false");
            Assert.AreEqual(false, parsedBool);

            // char(Char)
            char parsedChar = (char)ParseHelper.Parse(typeof(char), "1");
            Assert.AreEqual('1', parsedChar);
            parsedChar = (char)ParseHelper.Parse(typeof(char), "-");
            Assert.AreEqual('-', parsedChar);

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = (DateTime)ParseHelper.Parse(typeof(DateTime), n1.ToString());
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = (DateTimeOffset)ParseHelper.Parse(typeof(DateTimeOffset), offset.ToString());
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = (TimeSpan)ParseHelper.Parse(typeof(TimeSpan), span.ToString());
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            var guid = Guid.NewGuid();
            Guid parsedGuid = (Guid)ParseHelper.Parse(typeof(Guid), guid.ToString());
            Assert.AreEqual(guid, parsedGuid);
        }

        /// <summary>
        /// public static object Parse(Type targetType, string input, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse 02: Parse(Type, string, IFormatProvider)")]
        public void Test_Parse_02()
        {
            // int(Int32)
            int parsedInt = (int)ParseHelper.Parse(typeof(int), "123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual(123, parsedInt);
            parsedInt = (int)ParseHelper.Parse(typeof(int), "-123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual(-123, parsedInt);

            // uint(UInt32)
            uint parsedUint = (uint)ParseHelper.Parse(typeof(uint), "123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            short parsedShort = (short)ParseHelper.Parse(typeof(short), "123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((short)123, parsedShort);
            parsedShort = (short)ParseHelper.Parse(typeof(short), "-123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            ushort parsedUshort = (ushort)ParseHelper.Parse(typeof(ushort), "456", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            long parsedLong = (long)ParseHelper.Parse(typeof(long), "123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((long)123, parsedLong);
            parsedLong = (long)ParseHelper.Parse(typeof(long), "-123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            ulong parsedUlong = (ulong)ParseHelper.Parse(typeof(ulong), "123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            float parsedFloat = (float)ParseHelper.Parse(typeof(float), "123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((float)123.45, parsedFloat);
            parsedFloat = (float)ParseHelper.Parse(typeof(float), "-123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            double parsedDouble = (double)ParseHelper.Parse(typeof(double), "123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((double)123.45, parsedDouble);
            parsedDouble = (double)ParseHelper.Parse(typeof(double), "-123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            decimal parsedDecimal = (decimal)ParseHelper.Parse(typeof(decimal), "123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            parsedDecimal = (decimal)ParseHelper.Parse(typeof(decimal), "-123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            byte parsedByte = (byte)ParseHelper.Parse(typeof(byte), "123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            sbyte parsedSbyte = (sbyte)ParseHelper.Parse(typeof(sbyte), "123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            parsedSbyte = (sbyte)ParseHelper.Parse(typeof(sbyte), "-123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(bool), "true", NumberFormatInfo.CurrentInfo));

            // char
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(char), "1", NumberFormatInfo.CurrentInfo));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = (DateTime)ParseHelper.Parse(typeof(DateTime), n1.ToString(), DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = (DateTimeOffset)ParseHelper.Parse(typeof(DateTimeOffset), offset.ToString(), DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = (TimeSpan)ParseHelper.Parse(typeof(TimeSpan), span.ToString(), DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(Guid), Guid.NewGuid().ToString(), NumberFormatInfo.CurrentInfo));
        }

        /// <summary>
        /// public static object Parse(Type targetType, string input, NumberStyles numberStyles)
        /// </summary>
        [TestCategory("Test Parse 03: Parse(Type, string, NumberStyles)")]
        [TestMethod]
        public void Test_Parse_03()
        {
            // int(Int32)
            int parsedInt = (int)ParseHelper.Parse(typeof(int), "123", NumberStyles.Number);
            Assert.AreEqual(123, parsedInt);
            parsedInt = (int)ParseHelper.Parse(typeof(int), "-123", NumberStyles.Number);
            Assert.AreEqual(-123, parsedInt);

            // uint(UInt32)
            uint parsedUint = (uint)ParseHelper.Parse(typeof(uint), "123", NumberStyles.Number);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            short parsedShort = (short)ParseHelper.Parse(typeof(short), "123", NumberStyles.Number);
            Assert.AreEqual((short)123, parsedShort);
            parsedShort = (short)ParseHelper.Parse(typeof(short), "-123", NumberStyles.Number);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            ushort parsedUshort = (ushort)ParseHelper.Parse(typeof(ushort), "456", NumberStyles.Number);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            long parsedLong = (long)ParseHelper.Parse(typeof(long), "123", NumberStyles.Number);
            Assert.AreEqual((long)123, parsedLong);
            parsedLong = (long)ParseHelper.Parse(typeof(long), "-123", NumberStyles.Number);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            ulong parsedUlong = (ulong)ParseHelper.Parse(typeof(ulong), "123", NumberStyles.Number);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            float parsedFloat = (float)ParseHelper.Parse(typeof(float), "123.45", NumberStyles.Number);
            Assert.AreEqual((float)123.45, parsedFloat);
            parsedFloat = (float)ParseHelper.Parse(typeof(float), "-123.45", NumberStyles.Number);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            double parsedDouble = (double)ParseHelper.Parse(typeof(double), "123.45", NumberStyles.Number);
            Assert.AreEqual((double)123.45, parsedDouble);
            parsedDouble = (double)ParseHelper.Parse(typeof(double), "-123.45", NumberStyles.Number);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            decimal parsedDecimal = (decimal)ParseHelper.Parse(typeof(decimal), "123.45", NumberStyles.Number);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            parsedDecimal = (decimal)ParseHelper.Parse(typeof(decimal), "-123.45", NumberStyles.Number);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            byte parsedByte = (byte)ParseHelper.Parse(typeof(byte), "123", NumberStyles.Number);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            sbyte parsedSbyte = (sbyte)ParseHelper.Parse(typeof(sbyte), "123", NumberStyles.Number);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            parsedSbyte = (sbyte)ParseHelper.Parse(typeof(sbyte), "-123", NumberStyles.Number);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(bool), "true", NumberStyles.Number));

            // char
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(char), "1", NumberStyles.Number));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(DateTime), DateTime.Now.ToString(), NumberStyles.None));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString(), NumberStyles.None));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(TimeSpan), (DateTime.Now - DateTime.Today).ToString(), NumberStyles.None));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(Guid), Guid.NewGuid().ToString(), NumberStyles.None));
        }

        /// <summary>
        /// public static object Parse(Type targetType, string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse 04: Parse(Type, string, NumberStyles, IFormatProvider)")]
        public void Test_Parse_04()
        {
            // int(Int32)
            int parsedInt = (int)ParseHelper.Parse(typeof(int), "123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual(123, parsedInt);
            parsedInt = (int)ParseHelper.Parse(typeof(int), "-123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual(-123, parsedInt);

            // uint(UInt32)
            uint parsedUint = (uint)ParseHelper.Parse(typeof(uint), "123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            short parsedShort = (short)ParseHelper.Parse(typeof(short), "123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((short)123, parsedShort);
            parsedShort = (short)ParseHelper.Parse(typeof(short), "-123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            ushort parsedUshort = (ushort)ParseHelper.Parse(typeof(ushort), "456", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            long parsedLong = (long)ParseHelper.Parse(typeof(long), "123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((long)123, parsedLong);
            parsedLong = (long)ParseHelper.Parse(typeof(long), "-123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            ulong parsedUlong = (ulong)ParseHelper.Parse(typeof(ulong), "123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            float parsedFloat = (float)ParseHelper.Parse(typeof(float), "123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((float)123.45, parsedFloat);
            parsedFloat = (float)ParseHelper.Parse(typeof(float), "-123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            double parsedDouble = (double)ParseHelper.Parse(typeof(double), "123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((double)123.45, parsedDouble);
            parsedDouble = (double)ParseHelper.Parse(typeof(double), "-123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            decimal parsedDecimal = (decimal)ParseHelper.Parse(typeof(decimal), "123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            parsedDecimal = (decimal)ParseHelper.Parse(typeof(decimal), "-123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            byte parsedByte = (byte)ParseHelper.Parse(typeof(byte), "123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            sbyte parsedSbyte = (sbyte)ParseHelper.Parse(typeof(sbyte), "123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            parsedSbyte = (sbyte)ParseHelper.Parse(typeof(sbyte), "-123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(bool), "true", NumberStyles.Number, NumberFormatInfo.CurrentInfo));

            // char
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(char), "1", NumberStyles.Number, NumberFormatInfo.CurrentInfo));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(DateTime), DateTime.Now.ToString(), NumberStyles.None, NumberFormatInfo.CurrentInfo));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString(), NumberStyles.None, NumberFormatInfo.CurrentInfo));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(TimeSpan), (DateTime.Now - DateTime.Today).ToString(), NumberStyles.None, NumberFormatInfo.CurrentInfo));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(Guid), Guid.NewGuid().ToString(), NumberStyles.None, NumberFormatInfo.CurrentInfo));
        }

        /// <summary>
        /// public static object Parse(Type targetType, string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse 05: Parse(Type, string, IFormatProvider, DateTimeStyles)")]
        public void Test_Parse_05()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(int), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(uint), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(short), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(ushort), "456", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(long), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(ulong), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(float), "123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(double), "123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(decimal), "123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(byte), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(sbyte), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // bool
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(bool), "true", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // char
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(char), "1", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = (DateTime)ParseHelper.Parse(typeof(DateTime), n1.ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = (DateTimeOffset)ParseHelper.Parse(typeof(DateTimeOffset), offset.ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(TimeSpan), new TimeSpan(1, 0, 0).ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse(typeof(Guid), Guid.NewGuid().ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));
        }

        #endregion // *** Parse(Type, ... ) overloads ***

        #region *** ParseExact(Type, ... ) overloads ***

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string format)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 01: Parse(Type, string, string)")]
        public void Test_ParseExact_01()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(int), "123", ""));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(uint), "123", ""));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(short), "123", ""));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ushort), "123", ""));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(long), "123", ""));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ulong), "123", ""));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(float), "123.45", ""));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(double), "123.45", ""));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(decimal), "123.45", ""));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(byte), "123", ""));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(sbyte), "123", ""));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(bool), "true", ""));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(char), "1", ""));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(DateTime), DateTime.Now.ToString(), ""));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString(), ""));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(TimeSpan), (DateTime.Now - DateTime.Today).ToString(), ""));

            // Guid
            var guid = Guid.NewGuid();
            Guid parsedGuid = (Guid)ParseHelper.ParseExact(typeof(Guid), guid.ToString(), "D");
            Assert.AreEqual(guid, parsedGuid);
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string format, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 02: Parse(Type, string, string, IFormatProvider)")]
        public void Test_ParseExact_02()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(int), "123", "", NumberFormatInfo.CurrentInfo));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(uint), "123", "", NumberFormatInfo.CurrentInfo));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(short), "123", "", NumberFormatInfo.CurrentInfo));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ushort), "123", "", NumberFormatInfo.CurrentInfo));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(long), "123", "", NumberFormatInfo.CurrentInfo));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ulong), "123", "", NumberFormatInfo.CurrentInfo));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(float), "123.45", "", NumberFormatInfo.CurrentInfo));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(double), "123.45", "", NumberFormatInfo.CurrentInfo));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(decimal), "123.45", "", NumberFormatInfo.CurrentInfo));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(byte), "123", "", NumberFormatInfo.CurrentInfo));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(sbyte), "123", "", NumberFormatInfo.CurrentInfo));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(bool), "true", "", NumberFormatInfo.CurrentInfo));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(char), "1", "", NumberFormatInfo.CurrentInfo));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = (DateTime)ParseHelper.ParseExact(typeof(DateTime), n1.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = (DateTimeOffset)ParseHelper.ParseExact(typeof(DateTimeOffset), offset.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = (TimeSpan)ParseHelper.ParseExact(typeof(TimeSpan), span.ToString(), "c", DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(Guid), Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo));
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 03: Parse(Type, string, string, IFormatProvider, DateTimeStyles)")]
        public void Test_ParseExact_03()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(int), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(uint), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(short), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ushort), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(long), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ulong), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(float), "123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(double), "123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(decimal), "123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(byte), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(sbyte), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(bool), "true", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(char), "1", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = (DateTime)ParseHelper.ParseExact(typeof(DateTime), n1.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = (DateTimeOffset)ParseHelper.ParseExact(typeof(DateTimeOffset), offset.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(TimeSpan), new TimeSpan().ToString(), "c", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(Guid), Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 04: Parse(Type, string, string, IFormatProvider, TimeSpanStyles)")]
        public void Test_ParseExact_04()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(int), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(uint), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(short), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ushort), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(long), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ulong), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(float), "123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(double), "123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(decimal), "123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(byte), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(sbyte), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(bool), "true", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(char), "1", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(DateTime), DateTime.Now.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = (TimeSpan)ParseHelper.ParseExact(typeof(TimeSpan), span.ToString(), "c", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(Guid), Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 05: Parse(Type, string, string[], IFormatProvider)")]
        public void Test_ParseExact_05()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(int), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(uint), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(short), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ushort), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(long), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ulong), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(float), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(double), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(decimal), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(byte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(sbyte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(bool), "true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(char), "1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(DateTime), DateTime.Now.ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = (TimeSpan)ParseHelper.ParseExact(typeof(TimeSpan), span.ToString(), new string[] { "c", "c" }, DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(Guid), Guid.NewGuid().ToString(), new string[] { "D", "D" }, NumberFormatInfo.CurrentInfo));
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 06: Parse(Type, string, string[], IFormatProvider, DateTimeStyles)")]
        public void Test_ParseExact_06()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(int), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(uint), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(short), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ushort), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(long), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ulong), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(float), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(double), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(decimal), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(byte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(sbyte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(bool), "true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(char), "1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = (DateTime)ParseHelper.ParseExact(typeof(DateTime), n1.ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = (DateTimeOffset)ParseHelper.ParseExact(typeof(DateTimeOffset), offset.ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(TimeSpan), new TimeSpan().ToString(), new string[] { "c", "c" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(Guid), Guid.NewGuid().ToString(), new string[] { "D", "D" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 07: Parse(Type,  string, string[], IFormatProvider, TimeSpanStyles)")]
        public void Test_ParseExact_07()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(int), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(uint), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(short), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ushort), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(long), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(ulong), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(float), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(double), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(decimal), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(byte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(sbyte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(bool), "true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(char), "1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(DateTime), DateTime.Now.ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(DateTimeOffset), new DateTimeOffset().ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = (TimeSpan)ParseHelper.ParseExact(typeof(TimeSpan), span.ToString(), new string[] { "c", "c" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact(typeof(Guid), Guid.NewGuid().ToString(), new string[] { "D", "D" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));
        }

        #endregion // *** ParseExact(Type, ... ) overloads ***

        #region *** TryParse(Type, ... ) overloads ***

        /// <summary>
        /// public static bool TryParse(Type targetType, string input, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse 01: bool TryParse(Type, string, out object result)")]
        public void Test_TryParse_01()
        {
            // int(Int32)
            bool isParsedInt; int expectedInt;
            expectedInt = 123;
            isParsedInt = ParseHelper.TryParse(typeof(int), expectedInt.ToString(), out object parsedInt);
            Assert.IsTrue(isParsedInt);
            Assert.AreEqual(expectedInt, parsedInt);
            expectedInt = -123;
            isParsedInt = ParseHelper.TryParse(typeof(int), expectedInt.ToString(), out parsedInt);
            Assert.IsTrue(isParsedInt);
            Assert.AreEqual(expectedInt, parsedInt);

            // uint(UInt32)
            bool isParsedUint = ParseHelper.TryParse(typeof(uint), "123", out object parsedUint);
            Assert.IsTrue(isParsedUint);
            Assert.IsTrue(isParsedUint);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            bool isParsedShort = ParseHelper.TryParse(typeof(short), "123", out object parsedShort);
            Assert.IsTrue(isParsedShort);
            Assert.AreEqual((short)123, parsedShort);
            isParsedShort = ParseHelper.TryParse(typeof(short), "-123", out parsedShort);
            Assert.IsTrue(isParsedShort);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            bool isParsedUshort = ParseHelper.TryParse(typeof(ushort), "456", out object parsedUshort);
            Assert.IsTrue(isParsedUshort);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            bool isParsedLong = ParseHelper.TryParse(typeof(long), "123", out object parsedLong);
            Assert.IsTrue(isParsedLong);
            Assert.AreEqual((long)123, parsedLong);
            isParsedLong = ParseHelper.TryParse(typeof(long), "-123", out parsedLong);
            Assert.IsTrue(isParsedLong);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            bool isParsedUlong = ParseHelper.TryParse(typeof(ulong), "123", out object parsedUlong);
            Assert.IsTrue(isParsedUlong);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            bool isParsedFloat = ParseHelper.TryParse(typeof(float), "123.45", out object parsedFloat);
            Assert.IsTrue(isParsedFloat);
            Assert.AreEqual((float)123.45, parsedFloat);
            isParsedFloat = ParseHelper.TryParse(typeof(float), "-123.45", out parsedFloat);
            Assert.IsTrue(isParsedFloat);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            bool isParsedDouble = ParseHelper.TryParse(typeof(double), "123.45", out object parsedDouble);
            Assert.IsTrue(isParsedDouble);
            Assert.AreEqual((double)123.45, parsedDouble);
            isParsedDouble = ParseHelper.TryParse(typeof(double), "-123.45", out parsedDouble);
            Assert.IsTrue(isParsedDouble);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            bool isParsedDecimal = ParseHelper.TryParse(typeof(decimal), "123.45", out object parsedDecimal);
            Assert.IsTrue(isParsedDecimal);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            isParsedDecimal = ParseHelper.TryParse(typeof(decimal), "-123.45", out parsedDecimal);
            Assert.IsTrue(isParsedDecimal);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            bool isParsedByte = ParseHelper.TryParse(typeof(byte), "123", out object parsedByte);
            Assert.IsTrue(isParsedByte);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            bool isParsedSbyte = ParseHelper.TryParse(typeof(sbyte), "123", out object parsedSbyte);
            Assert.IsTrue(isParsedSbyte);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            isParsedSbyte = ParseHelper.TryParse(typeof(sbyte), "-123", out parsedSbyte);
            Assert.IsTrue(isParsedSbyte);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool(Boolean)
            bool isParsedBool = ParseHelper.TryParse(typeof(bool), "true", out object parsedBool);
            Assert.IsTrue(isParsedBool);
            Assert.AreEqual(true, parsedBool);
            isParsedBool = ParseHelper.TryParse(typeof(bool), "false", out parsedBool);
            Assert.IsTrue(isParsedBool);
            Assert.AreEqual(false, parsedBool);

            // char(Char)
            bool isParsedChar = ParseHelper.TryParse(typeof(char), "1", out object parsedChar);
            Assert.IsTrue(isParsedChar);
            Assert.AreEqual('1', parsedChar);
            isParsedChar = ParseHelper.TryParse(typeof(char), "-", out parsedChar);
            Assert.IsTrue(isParsedChar);
            Assert.AreEqual('-', parsedChar);

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            bool isParsedDateTime = ParseHelper.TryParse(typeof(DateTime), n1.ToString(), out object parsedDateTime);
            Assert.IsTrue(isParsedDateTime);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            bool isParsedDateTimeOffset = ParseHelper.TryParse(typeof(DateTimeOffset), offset.ToString(), out object parsedDateTimeOffset);
            Assert.IsTrue(isParsedDateTimeOffset);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParse(typeof(TimeSpan), span.ToString(), out object parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            var guid = Guid.NewGuid();
            bool isParsedGuid = ParseHelper.TryParse(typeof(Guid), guid.ToString(), out object parsedGuid);
            Assert.IsTrue(isParsedGuid);
            Assert.AreEqual(guid, parsedGuid);
        }

        /// <summary>
        /// public static bool TryParse(Type targetType, string input, NumberStyles numberStyles, IFormatProvider formatProvider, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse 02: bool TryParse(Type, string, NumberStyles, IFormatProvider, out object result)")]
        public void Test_TryParse_02()
        {
            // int(Int32)
            bool isParsedInt; int expectedInt;
            expectedInt = 123;
            isParsedInt = ParseHelper.TryParse(typeof(int), expectedInt.ToString(), NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out object parsedInt);
            Assert.IsTrue(isParsedInt);
            Assert.AreEqual(expectedInt, parsedInt);
            expectedInt = -123;
            isParsedInt = ParseHelper.TryParse(typeof(int), expectedInt.ToString(), NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out parsedInt);
            Assert.IsTrue(isParsedInt);
            Assert.AreEqual(expectedInt, parsedInt);

            // uint(UInt32)
            bool isParsedUint = ParseHelper.TryParse(typeof(uint), "123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out object parsedUint);
            Assert.IsTrue(isParsedUint);
            Assert.IsTrue(isParsedUint);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            bool isParsedShort = ParseHelper.TryParse(typeof(short), "123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out object parsedShort);
            Assert.IsTrue(isParsedShort);
            Assert.AreEqual((short)123, parsedShort);
            isParsedShort = ParseHelper.TryParse(typeof(short), "-123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out parsedShort);
            Assert.IsTrue(isParsedShort);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            bool isParsedUshort = ParseHelper.TryParse(typeof(ushort), "456", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out object parsedUshort);
            Assert.IsTrue(isParsedUshort);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            bool isParsedLong = ParseHelper.TryParse(typeof(long), "123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out object parsedLong);
            Assert.IsTrue(isParsedLong);
            Assert.AreEqual((long)123, parsedLong);
            isParsedLong = ParseHelper.TryParse(typeof(long), "-123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out parsedLong);
            Assert.IsTrue(isParsedLong);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            bool isParsedUlong = ParseHelper.TryParse(typeof(ulong), "123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out object parsedUlong);
            Assert.IsTrue(isParsedUlong);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            bool isParsedFloat = ParseHelper.TryParse(typeof(float), "123.45", NumberStyles.Float, NumberFormatInfo.CurrentInfo, out object parsedFloat);
            Assert.IsTrue(isParsedFloat);
            Assert.AreEqual((float)123.45, parsedFloat);
            isParsedFloat = ParseHelper.TryParse(typeof(float), "-123.45", NumberStyles.Float, NumberFormatInfo.CurrentInfo, out parsedFloat);
            Assert.IsTrue(isParsedFloat);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            bool isParsedDouble = ParseHelper.TryParse(typeof(double), "123.45", NumberStyles.Float, NumberFormatInfo.CurrentInfo, out object parsedDouble);
            Assert.IsTrue(isParsedDouble);
            Assert.AreEqual((double)123.45, parsedDouble);
            isParsedDouble = ParseHelper.TryParse(typeof(double), "-123.45", NumberStyles.Float, NumberFormatInfo.CurrentInfo, out parsedDouble);
            Assert.IsTrue(isParsedDouble);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            bool isParsedDecimal = ParseHelper.TryParse(typeof(decimal), "123.45", NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out object parsedDecimal);
            Assert.IsTrue(isParsedDecimal);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            isParsedDecimal = ParseHelper.TryParse(typeof(decimal), "-123.45", NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out parsedDecimal);
            Assert.IsTrue(isParsedDecimal);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            bool isParsedByte = ParseHelper.TryParse(typeof(byte), "123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out object parsedByte);
            Assert.IsTrue(isParsedByte);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            bool isParsedSbyte = ParseHelper.TryParse(typeof(sbyte), "123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out object parsedSbyte);
            Assert.IsTrue(isParsedSbyte);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            isParsedSbyte = ParseHelper.TryParse(typeof(sbyte), "-123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out parsedSbyte);
            Assert.IsTrue(isParsedSbyte);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(bool), "true", NumberStyles.Any, NumberFormatInfo.CurrentInfo, out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(char), "1", NumberStyles.Any, NumberFormatInfo.CurrentInfo, out object parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(DateTime), DateTime.Now.ToString(), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out object parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString(), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out object parsedDateTimeOffset));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(TimeSpan), (DateTime.Now - DateTime.Today).ToString(), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out object parsedTimeSpan));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(Guid), Guid.NewGuid().ToString(), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out object parsedGuid));
        }

        /// <summary>
        /// public static bool TryParse(Type targetType, string input, IFormatProvider formatProvider, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse 03: bool TryParse(Type, string, IFormatProvider, out object result)")]
        public void Test_TryParse_03()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(int), "123", NumberFormatInfo.CurrentInfo, out object parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(uint), "123", NumberFormatInfo.CurrentInfo, out object parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(short), "123", NumberFormatInfo.CurrentInfo, out object parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(ushort), "456", NumberFormatInfo.CurrentInfo, out object parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(long), "123", NumberFormatInfo.CurrentInfo, out object parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(ulong), "123", NumberFormatInfo.CurrentInfo, out object parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(float), "123.45", NumberFormatInfo.CurrentInfo, out object parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(double), "123.45", NumberFormatInfo.CurrentInfo, out object parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(decimal), "123.45", NumberFormatInfo.CurrentInfo, out object parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(byte), "123", NumberFormatInfo.CurrentInfo, out object parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(sbyte), "123", NumberFormatInfo.CurrentInfo, out object parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(bool), "true", NumberFormatInfo.CurrentInfo, out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(char), "1", NumberFormatInfo.CurrentInfo, out object parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(DateTime), DateTime.Now.ToString(), NumberFormatInfo.CurrentInfo, out object parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString(), NumberFormatInfo.CurrentInfo, out object parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParse(typeof(TimeSpan), span.ToString(), DateTimeFormatInfo.CurrentInfo, out object parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(Guid), Guid.NewGuid().ToString(), NumberFormatInfo.CurrentInfo, out object parsedGuid));
        }

        /// <summary>
        /// public static bool TryParse(Type targetType, string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse 03: bool TryParse(Type, string, IFormatProvider, out object result)")]
        public void Test_TryParse_04()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(int), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(uint), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(short), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(ushort), "456", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(long), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(ulong), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(float), "123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(double), "123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(decimal), "123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(byte), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(sbyte), "123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(bool), "true", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(char), "1", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedChar));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            bool isParsedDateTime = ParseHelper.TryParse(typeof(DateTime), n1.ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDateTime);
            Assert.IsTrue(isParsedDateTime);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            bool isParsedDateTimeOffset = ParseHelper.TryParse(typeof(DateTimeOffset), offset.ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDateTimeOffset);
            Assert.IsTrue(isParsedDateTimeOffset);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParse(typeof(TimeSpan), span.ToString(), DateTimeFormatInfo.CurrentInfo, out object parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse(typeof(Guid), Guid.NewGuid().ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedGuid));
        }

        #endregion // *** TryParse(Type, ... ) overloads ***

        #region *** TryParseExact(Type, ... ) overloads ***

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string format, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 01: bool TryParseExact(Type, string, string, out object)")]
        public void Test_TryParseExact_01()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(int), "123", "", out object parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(uint), "123", "", out object parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(short), "123", "", out object parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ushort), "456", "", out object parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(long), "123", "", out object parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ulong), "123", "", out object parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(float), "123.45", "", out object parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(double), "123.45", "", out object parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(decimal), "123.45", "", out object parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(byte), "123", "", out object parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(sbyte), "123", "", out object parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(bool), "true", "", out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(char), "1", "", out object parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTime), DateTime.Now.ToString(), "", out object parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString(), "", out object parsedDateTimeOffset));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(TimeSpan), (DateTime.Now - DateTime.Today).ToString(), "", out object parsedTimeSpan));

            // Guid
            bool isParsedGuid; Guid expectedGuid;
            expectedGuid = Guid.NewGuid();
            isParsedGuid = ParseHelper.TryParseExact(typeof(Guid), expectedGuid.ToString(), "D", out object parsedGuid);
            Assert.IsTrue(isParsedGuid);
            Assert.AreEqual(expectedGuid, parsedGuid);
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 02: bool TryParseExact(Type, string, string, IFormatProvider, out T result)")]
        public void Test_TryParseExact_02()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(int), "123", "", NumberFormatInfo.CurrentInfo, out object parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(uint), "123", "", NumberFormatInfo.CurrentInfo, out object parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(short), "123", "", NumberFormatInfo.CurrentInfo, out object parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ushort), "456", "", NumberFormatInfo.CurrentInfo, out object parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(long), "123", "", NumberFormatInfo.CurrentInfo, out object parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ulong), "123", "", NumberFormatInfo.CurrentInfo, out object parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(float), "123.45", "", NumberFormatInfo.CurrentInfo, out object parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(double), "123.45", "", NumberFormatInfo.CurrentInfo, out object parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(decimal), "123.45", "", NumberFormatInfo.CurrentInfo, out object parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(byte), "123", "", NumberFormatInfo.CurrentInfo, out object parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(sbyte), "123", "", NumberFormatInfo.CurrentInfo, out object parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(bool), "true", "", NumberFormatInfo.CurrentInfo, out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(char), "1", "", NumberFormatInfo.CurrentInfo, out object parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTime), DateTime.Now.ToString(), "", DateTimeFormatInfo.CurrentInfo, out object parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString(), "", DateTimeFormatInfo.CurrentInfo, out object parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParseExact(typeof(TimeSpan), span.ToString(), "c", DateTimeFormatInfo.CurrentInfo, out object parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(Guid), Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, out object parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 03: bool TryParseExact(Type, string, string, IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParseExact_03()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(int), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(uint), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(short), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ushort), "456", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(long), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ulong), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(float), "123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(double), "123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(decimal), "123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(byte), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(sbyte), "123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(bool), "true", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(char), "1", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedChar));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            bool isParsedDateTime = ParseHelper.TryParseExact(typeof(DateTime), n1.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDateTime);
            Assert.IsTrue(isParsedDateTime);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            bool isParsedDateTimeOffset = ParseHelper.TryParseExact(typeof(DateTimeOffset), offset.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDateTimeOffset);
            Assert.IsTrue(isParsedDateTimeOffset);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(TimeSpan), (DateTime.Now - DateTime.Today).ToString(), "c", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedTimeSpan));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(Guid), Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 04: bool TryParseExact(Type, string, string, IFormatProvider, TimeSpanStyles, out T result)")]
        public void Test_TryParseExact_04()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(int), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(uint), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(short), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ushort), "456", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(long), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ulong), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(float), "123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(double), "123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(decimal), "123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(byte), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(sbyte), "123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(bool), "true", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(char), "1", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTime), DateTime.Now.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParseExact(typeof(TimeSpan), span.ToString(), "c", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(Guid), Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 05: bool TryParseExact(Type, string, string[], IFormatProvider, out T result)")]
        public void Test_TryParseExact_05()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(int), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(uint), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(short), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ushort), "456", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(long), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ulong), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(float), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(double), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(decimal), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(byte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(sbyte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(bool), "true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(char), "1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out object parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTime), DateTime.Now.ToString(), new string[] { "", "" }, DateTimeFormatInfo.CurrentInfo, out object parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString(), new string[] { "", "" }, DateTimeFormatInfo.CurrentInfo, out object parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParseExact(typeof(TimeSpan), span.ToString(), new string[] { "c", "" }, DateTimeFormatInfo.CurrentInfo, out object parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(Guid), Guid.NewGuid().ToString(), new string[] { "D", "" }, NumberFormatInfo.CurrentInfo, out object parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 06: bool TryParseExact(Type, string, string[], IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParseExact_06()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(int), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(uint), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(short), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ushort), "456", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(long), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ulong), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(float), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(double), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(decimal), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(byte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(sbyte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(bool), "true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(char), "1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedChar));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            bool isParsedDateTime = ParseHelper.TryParseExact(typeof(DateTime), n1.ToString("F"), new string[] { "F", "" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDateTime);
            Assert.IsTrue(isParsedDateTime);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            bool isParsedDateTimeOffset = ParseHelper.TryParseExact(typeof(DateTimeOffset), offset.ToString("F"), new string[] { "F", "" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedDateTimeOffset);
            Assert.IsTrue(isParsedDateTimeOffset);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(TimeSpan), (DateTime.Now - DateTime.Today).ToString(), new string[] { "c", "" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedTimeSpan));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(Guid), Guid.NewGuid().ToString(), new string[] { "D", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out object parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 07: bool TryParseExact(Type, string, string[], IFormatProvider, TimeSpanStyles, out T result)")]
        public void Test_TryParseExact_07()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(int), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(uint), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(short), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ushort), "456", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(long), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(ulong), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(float), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(double), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(decimal), "123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(byte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(sbyte), "123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(bool), "true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(char), "1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTime), DateTime.Now.ToString("F"), new string[] { "F", "" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(DateTimeOffset), new DateTimeOffset(DateTime.Now).ToString("F"), new string[] { "F", "" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParseExact(typeof(TimeSpan), span.ToString(), new string[] { "c", "" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact(typeof(Guid), Guid.NewGuid().ToString(), new string[] { "D", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out object parsedGuid));
        }

        #endregion // *** TryParseExact(Type, ... ) overloads ***

        #endregion // *** non generic methods ***

        #region *** generic methods ***

        #region *** Parse<T>( ... ) overloads ***

        /// <summary>
        /// public static TTargetType Parse<TTargetType>(string input)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse Generic 01: Parse<T>(string)")]
        public void Test_Parse_Generic_01()
        {
            // int(Int32)
            int expectedInt; int parsedInt;
            expectedInt = 123;
            parsedInt = ParseHelper.Parse<int>(expectedInt.ToString());
            Assert.AreEqual(expectedInt, parsedInt);
            expectedInt = -123;
            parsedInt = ParseHelper.Parse<int>(expectedInt.ToString());
            Assert.AreEqual(expectedInt, parsedInt);
            PrivateAssert.ThrowsWrappedException<TargetInvocationException, FormatException>(() =>
                ParseHelper.Parse<int>(string.Empty));

            // uint(UInt32)
            uint parsedUint = ParseHelper.Parse<uint>("123");
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            short parsedShort = ParseHelper.Parse<short>("123");
            Assert.AreEqual((short)123, parsedShort);
            parsedShort = ParseHelper.Parse<short>("-123");
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            ushort parsedUshort = ParseHelper.Parse<ushort>("456");
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            long parsedLong = ParseHelper.Parse<long>("123");
            Assert.AreEqual((long)123, parsedLong);
            parsedLong = ParseHelper.Parse<long>("-123");
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            ulong parsedUlong = ParseHelper.Parse<ulong>("123");
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            float parsedFloat = ParseHelper.Parse<float>("123.45");
            Assert.AreEqual((float)123.45, parsedFloat);
            parsedFloat = ParseHelper.Parse<float>("-123.45");
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            double parsedDouble = ParseHelper.Parse<double>("123.45");
            Assert.AreEqual((double)123.45, parsedDouble);
            parsedDouble = ParseHelper.Parse<double>("-123.45");
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            decimal parsedDecimal = ParseHelper.Parse<decimal>("123.45");
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            parsedDecimal = ParseHelper.Parse<decimal>("-123.45");
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            byte parsedByte = ParseHelper.Parse<byte>("123");
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            sbyte parsedSbyte = ParseHelper.Parse<sbyte>("123");
            Assert.AreEqual((sbyte)123, parsedSbyte);
            parsedSbyte = ParseHelper.Parse<sbyte>("-123");
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool(Boolean)
            bool parsedBool = ParseHelper.Parse<bool>("true");
            Assert.AreEqual(true, parsedBool);
            parsedBool = ParseHelper.Parse<bool>("false");
            Assert.AreEqual(false, parsedBool);

            // char(Char)
            char parsedChar = ParseHelper.Parse<char>("1");
            Assert.AreEqual('1', parsedChar);
            parsedChar = ParseHelper.Parse<char>("-");
            Assert.AreEqual('-', parsedChar);

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = ParseHelper.Parse<DateTime>(n1.ToString());
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = ParseHelper.Parse<DateTimeOffset>(offset.ToString());
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = ParseHelper.Parse<TimeSpan>(span.ToString());
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            var guid = Guid.NewGuid();
            Guid parsedGuid = ParseHelper.Parse<Guid>(guid.ToString());
            Assert.AreEqual(guid, parsedGuid);
        }

        /// <summary>
        /// public static TTargetType Parse<TTargetType>(string input, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse Generic 02: Parse<T>(string, IFormatProvider)")]
        public void Test_Parse_Generic_02()
        {
            // int(Int32)
            int parsedInt = ParseHelper.Parse<int>("123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual(123, parsedInt);
            parsedInt = ParseHelper.Parse<int>("-123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual(-123, parsedInt);

            // uint(UInt32)
            uint parsedUint = ParseHelper.Parse<uint>("123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            short parsedShort = ParseHelper.Parse<short>("123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((short)123, parsedShort);
            parsedShort = ParseHelper.Parse<short>("-123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            ushort parsedUshort = ParseHelper.Parse<ushort>("456", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            long parsedLong = ParseHelper.Parse<long>("123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((long)123, parsedLong);
            parsedLong = ParseHelper.Parse<long>("-123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            ulong parsedUlong = ParseHelper.Parse<ulong>("123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            float parsedFloat = ParseHelper.Parse<float>("123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((float)123.45, parsedFloat);
            parsedFloat = ParseHelper.Parse<float>("-123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            double parsedDouble = ParseHelper.Parse<double>("123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((double)123.45, parsedDouble);
            parsedDouble = ParseHelper.Parse<double>("-123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            decimal parsedDecimal = ParseHelper.Parse<decimal>("123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            parsedDecimal = ParseHelper.Parse<decimal>("-123.45", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            byte parsedByte = ParseHelper.Parse<byte>("123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            sbyte parsedSbyte = ParseHelper.Parse<sbyte>("123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            parsedSbyte = ParseHelper.Parse<sbyte>("-123", NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<bool>("true", NumberFormatInfo.CurrentInfo));

            // char
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<char>("1", NumberFormatInfo.CurrentInfo));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = ParseHelper.Parse<DateTime>(n1.ToString(), DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = ParseHelper.Parse<DateTimeOffset>(offset.ToString(), DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = ParseHelper.Parse<TimeSpan>(span.ToString(), DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<Guid>(Guid.NewGuid().ToString(), NumberFormatInfo.CurrentInfo));
        }

        /// <summary>
        /// public static TTargetType Parse<TTargetType>(string input, NumberStyles numberStyles)
        /// </summary>
        [TestCategory("Test Parse Generic 03: Parse<T>(string, NumberStyles)")]
        [TestMethod]
        public void Test_Parse_Generic_03()
        {
            // int(Int32)
            int parsedInt = ParseHelper.Parse<int>("123", NumberStyles.Number);
            Assert.AreEqual(123, parsedInt);
            parsedInt = ParseHelper.Parse<int>("-123", NumberStyles.Number);
            Assert.AreEqual(-123, parsedInt);

            // uint(UInt32)
            uint parsedUint = ParseHelper.Parse<uint>("123", NumberStyles.Number);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            short parsedShort = ParseHelper.Parse<short>("123", NumberStyles.Number);
            Assert.AreEqual((short)123, parsedShort);
            parsedShort = ParseHelper.Parse<short>("-123", NumberStyles.Number);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            ushort parsedUshort = ParseHelper.Parse<ushort>("456", NumberStyles.Number);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            long parsedLong = ParseHelper.Parse<long>("123", NumberStyles.Number);
            Assert.AreEqual((long)123, parsedLong);
            parsedLong = ParseHelper.Parse<long>("-123", NumberStyles.Number);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            ulong parsedUlong = ParseHelper.Parse<ulong>("123", NumberStyles.Number);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            float parsedFloat = ParseHelper.Parse<float>("123.45", NumberStyles.Number);
            Assert.AreEqual((float)123.45, parsedFloat);
            parsedFloat = ParseHelper.Parse<float>("-123.45", NumberStyles.Number);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            double parsedDouble = ParseHelper.Parse<double>("123.45", NumberStyles.Number);
            Assert.AreEqual((double)123.45, parsedDouble);
            parsedDouble = ParseHelper.Parse<double>("-123.45", NumberStyles.Number);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            decimal parsedDecimal = ParseHelper.Parse<decimal>("123.45", NumberStyles.Number);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            parsedDecimal = ParseHelper.Parse<decimal>("-123.45", NumberStyles.Number);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            byte parsedByte = ParseHelper.Parse<byte>("123", NumberStyles.Number);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            sbyte parsedSbyte = ParseHelper.Parse<sbyte>("123", NumberStyles.Number);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            parsedSbyte = ParseHelper.Parse<sbyte>("-123", NumberStyles.Number);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<bool>("true", NumberStyles.Number));

            // char
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<char>("1", NumberStyles.Number));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<DateTime>(DateTime.Now.ToString(), NumberStyles.None));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString(), NumberStyles.None));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<TimeSpan>((DateTime.Now - DateTime.Today).ToString(), NumberStyles.None));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<Guid>(Guid.NewGuid().ToString(), NumberStyles.None));
        }

        /// <summary>
        /// public static TTargetType Parse<TTargetType>(string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse Generic 04: Parse<T>(string, NumberStyles, IFormatProvider)")]
        public void Test_Parse_Generic_04()
        {
            // int(Int32)
            int parsedInt = ParseHelper.Parse<int>("123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual(123, parsedInt);
            parsedInt = ParseHelper.Parse<int>("-123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual(-123, parsedInt);

            // uint(UInt32)
            uint parsedUint = ParseHelper.Parse<uint>("123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            short parsedShort = ParseHelper.Parse<short>("123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((short)123, parsedShort);
            parsedShort = ParseHelper.Parse<short>("-123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            ushort parsedUshort = ParseHelper.Parse<ushort>("456", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            long parsedLong = ParseHelper.Parse<long>("123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((long)123, parsedLong);
            parsedLong = ParseHelper.Parse<long>("-123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            ulong parsedUlong = ParseHelper.Parse<ulong>("123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            float parsedFloat = ParseHelper.Parse<float>("123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((float)123.45, parsedFloat);
            parsedFloat = ParseHelper.Parse<float>("-123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            double parsedDouble = ParseHelper.Parse<double>("123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((double)123.45, parsedDouble);
            parsedDouble = ParseHelper.Parse<double>("-123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            decimal parsedDecimal = ParseHelper.Parse<decimal>("123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            parsedDecimal = ParseHelper.Parse<decimal>("-123.45", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            byte parsedByte = ParseHelper.Parse<byte>("123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            sbyte parsedSbyte = ParseHelper.Parse<sbyte>("123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            parsedSbyte = ParseHelper.Parse<sbyte>("-123", NumberStyles.Number, NumberFormatInfo.CurrentInfo);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<bool>("true", NumberStyles.Number, NumberFormatInfo.CurrentInfo));

            // char
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<char>("1", NumberStyles.Number, NumberFormatInfo.CurrentInfo));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<DateTime>(DateTime.Now.ToString(), NumberStyles.None, NumberFormatInfo.CurrentInfo));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString(), NumberStyles.None, NumberFormatInfo.CurrentInfo));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<TimeSpan>((DateTime.Now - DateTime.Today).ToString(), NumberStyles.None, NumberFormatInfo.CurrentInfo));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<Guid>(Guid.NewGuid().ToString(), NumberStyles.None, NumberFormatInfo.CurrentInfo));
        }

        /// <summary>
        /// public static TTargetType Parse<TTargetType>(string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse Generic 05: Parse<T>(string, IFormatProvider, DateTimeStyles)")]
        public void Test_Parse_Generic_05()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<int>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<uint>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<short>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<ushort>("456", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<long>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<ulong>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<float>("123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<double>("123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<decimal>("123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<byte>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<sbyte>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // bool
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<bool>("true", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // char
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<char>("1", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = ParseHelper.Parse<DateTime>(n1.ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = ParseHelper.Parse<DateTimeOffset>(offset.ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<TimeSpan>(new TimeSpan(1, 0, 0).ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.Parse<Guid>(Guid.NewGuid().ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));
        }

        #endregion // *** Parse<T>( ... ) overloads ***

        #region *** ParseExact<T>( ... ) overloads ***

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string format)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 01: Parse<T>(string, string)")]
        public void Test_ParseExact_Generic_01()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<int>("123", ""));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<uint>("123", ""));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<short>("123", ""));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ushort>("123", ""));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<long>("123", ""));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ulong>("123", ""));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<float>("123.45", ""));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<double>("123.45", ""));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<decimal>("123.45", ""));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<byte>("123", ""));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<sbyte>("123", ""));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<bool>("true", ""));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<char>("1", ""));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<DateTime>(DateTime.Now.ToString(), ""));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString(), ""));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<TimeSpan>((DateTime.Now - DateTime.Today).ToString(), ""));

            // Guid
            var guid = Guid.NewGuid();
            Guid parsedGuid = ParseHelper.ParseExact<Guid>(guid.ToString(), "D");
            Assert.AreEqual(guid, parsedGuid);
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 02: Parse<T>(string, string, IFormatProvider)")]
        public void Test_ParseExact_Generic_02()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<int>("123", "", NumberFormatInfo.CurrentInfo));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<uint>("123", "", NumberFormatInfo.CurrentInfo));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<short>("123", "", NumberFormatInfo.CurrentInfo));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ushort>("123", "", NumberFormatInfo.CurrentInfo));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<long>("123", "", NumberFormatInfo.CurrentInfo));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ulong>("123", "", NumberFormatInfo.CurrentInfo));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<float>("123.45", "", NumberFormatInfo.CurrentInfo));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<double>("123.45", "", NumberFormatInfo.CurrentInfo));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<decimal>("123.45", "", NumberFormatInfo.CurrentInfo));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<byte>("123", "", NumberFormatInfo.CurrentInfo));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<sbyte>("123", "", NumberFormatInfo.CurrentInfo));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<bool>("true", "", NumberFormatInfo.CurrentInfo));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<char>("1", "", NumberFormatInfo.CurrentInfo));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = ParseHelper.ParseExact<DateTime>(n1.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = ParseHelper.ParseExact<DateTimeOffset>(offset.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = ParseHelper.ParseExact<TimeSpan>(span.ToString(), "c", DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<Guid>(Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo));
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 03: Parse<T>(string, string, IFormatProvider, DateTimeStyles)")]
        public void Test_ParseExact_Generic_03()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<int>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<uint>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<short>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ushort>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<long>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ulong>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<float>("123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<double>("123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<decimal>("123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<byte>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<sbyte>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<bool>("true", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<char>("1", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = ParseHelper.ParseExact<DateTime>(n1.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = ParseHelper.ParseExact<DateTimeOffset>(offset.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<TimeSpan>(new TimeSpan().ToString(), "c", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<Guid>(Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, DateTimeStyles.None));
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 04: Parse<T>(string, string, IFormatProvider, TimeSpanStyles)")]
        public void Test_ParseExact_Generic_04()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<int>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<uint>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<short>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ushort>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<long>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ulong>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<float>("123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<double>("123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<decimal>("123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<byte>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<sbyte>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<bool>("true", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<char>("1", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<DateTime>(DateTime.Now.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = ParseHelper.ParseExact<TimeSpan>(span.ToString(), "c", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<Guid>(Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 05: Parse<T>(string, string[], IFormatProvider)")]
        public void Test_ParseExact_Generic_05()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<int>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<uint>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<short>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ushort>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<long>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ulong>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<float>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<double>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<decimal>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<byte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<sbyte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<bool>("true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<char>("1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<DateTime>(DateTime.Now.ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = ParseHelper.ParseExact<TimeSpan>(span.ToString(), new string[] { "c", "c" }, DateTimeFormatInfo.CurrentInfo);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<Guid>(Guid.NewGuid().ToString(), new string[] { "D", "D" }, NumberFormatInfo.CurrentInfo));
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string [] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 06: Parse<T>(string, string[], IFormatProvider, DateTimeStyles)")]
        public void Test_ParseExact_Generic_06()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<int>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<uint>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<short>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ushort>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<long>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ulong>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<float>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<double>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<decimal>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<byte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<sbyte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<bool>("true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<char>("1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            DateTime parsedDateTime = ParseHelper.ParseExact<DateTime>(n1.ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            DateTimeOffset parsedDateTimeOffset = ParseHelper.ParseExact<DateTimeOffset>(offset.ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<TimeSpan>(new TimeSpan().ToString(), new string[] { "c", "c" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<Guid>(Guid.NewGuid().ToString(), new string[] { "D", "D" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None));
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 07: Parse<T>(string, string[], IFormatProvider, TimeSpanStyles)")]
        public void Test_ParseExact_Generic_07()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<int>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<uint>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<short>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ushort>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<long>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<ulong>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<float>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<double>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<decimal>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<byte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<sbyte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<bool>("true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<char>("1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<DateTime>(DateTime.Now.ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<DateTimeOffset>(new DateTimeOffset().ToString("F"), new string[] { "F", "F" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            TimeSpan parsedTimeSpan = ParseHelper.ParseExact<TimeSpan>(span.ToString(), new string[] { "c", "c" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.ParseExact<Guid>(Guid.NewGuid().ToString(), new string[] { "D", "D" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None));
        }

        #endregion // *** ParseExact<T>( ... ) overloads ***

        #region *** TryParse<T>( ... ) overloads ***

        /// <summary>
        /// public static bool TryParse<TTargetType>(string input, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse Generic 01: bool TryParse<T>(string, out T result)")]
        public void Test_TryParse_Generic_01()
        {
            // int(Int32)
            bool isParsedInt; int expectedInt;
            expectedInt = 123;
            isParsedInt = ParseHelper.TryParse(expectedInt.ToString(), out int parsedInt);
            Assert.IsTrue(isParsedInt);
            Assert.AreEqual(expectedInt, parsedInt);
            expectedInt = -123;
            isParsedInt = ParseHelper.TryParse(expectedInt.ToString(), out parsedInt);
            Assert.IsTrue(isParsedInt);
            Assert.AreEqual(expectedInt, parsedInt);

            // uint(UInt32)
            bool isParsedUint = ParseHelper.TryParse("123", out uint parsedUint);
            Assert.IsTrue(isParsedUint);
            Assert.IsTrue(isParsedUint);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            bool isParsedShort = ParseHelper.TryParse("123", out short parsedShort);
            Assert.IsTrue(isParsedShort);
            Assert.AreEqual((short)123, parsedShort);
            isParsedShort = ParseHelper.TryParse("-123", out parsedShort);
            Assert.IsTrue(isParsedShort);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            bool isParsedUshort = ParseHelper.TryParse<ushort>("456", out ushort parsedUshort);
            Assert.IsTrue(isParsedUshort);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            bool isParsedLong = ParseHelper.TryParse<long>("123", out long parsedLong);
            Assert.IsTrue(isParsedLong);
            Assert.AreEqual((long)123, parsedLong);
            isParsedLong = ParseHelper.TryParse<long>("-123", out parsedLong);
            Assert.IsTrue(isParsedLong);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            bool isParsedUlong = ParseHelper.TryParse<ulong>("123", out ulong parsedUlong);
            Assert.IsTrue(isParsedUlong);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            bool isParsedFloat = ParseHelper.TryParse<float>("123.45", out float parsedFloat);
            Assert.IsTrue(isParsedFloat);
            Assert.AreEqual((float)123.45, parsedFloat);
            isParsedFloat = ParseHelper.TryParse<float>("-123.45", out parsedFloat);
            Assert.IsTrue(isParsedFloat);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            bool isParsedDouble = ParseHelper.TryParse<double>("123.45", out double parsedDouble);
            Assert.IsTrue(isParsedDouble);
            Assert.AreEqual((double)123.45, parsedDouble);
            isParsedDouble = ParseHelper.TryParse<double>("-123.45", out parsedDouble);
            Assert.IsTrue(isParsedDouble);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            bool isParsedDecimal = ParseHelper.TryParse<decimal>("123.45", out decimal parsedDecimal);
            Assert.IsTrue(isParsedDecimal);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            isParsedDecimal = ParseHelper.TryParse<decimal>("-123.45", out parsedDecimal);
            Assert.IsTrue(isParsedDecimal);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            bool isParsedByte = ParseHelper.TryParse<byte>("123", out byte parsedByte);
            Assert.IsTrue(isParsedByte);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            bool isParsedSbyte = ParseHelper.TryParse<sbyte>("123", out sbyte parsedSbyte);
            Assert.IsTrue(isParsedSbyte);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            isParsedSbyte = ParseHelper.TryParse<sbyte>("-123", out parsedSbyte);
            Assert.IsTrue(isParsedSbyte);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool(Boolean)
            bool isParsedBool = ParseHelper.TryParse<bool>("true", out bool parsedBool);
            Assert.IsTrue(isParsedBool);
            Assert.AreEqual(true, parsedBool);
            isParsedBool = ParseHelper.TryParse<bool>("false", out parsedBool);
            Assert.IsTrue(isParsedBool);
            Assert.AreEqual(false, parsedBool);

            // char(Char)
            bool isParsedChar = ParseHelper.TryParse<char>("1", out char parsedChar);
            Assert.IsTrue(isParsedChar);
            Assert.AreEqual('1', parsedChar);
            isParsedChar = ParseHelper.TryParse<char>("-", out parsedChar);
            Assert.IsTrue(isParsedChar);
            Assert.AreEqual('-', parsedChar);

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            bool isParsedDateTime = ParseHelper.TryParse<DateTime>(n1.ToString(), out DateTime parsedDateTime);
            Assert.IsTrue(isParsedDateTime);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            bool isParsedDateTimeOffset = ParseHelper.TryParse<DateTimeOffset>(offset.ToString(), out DateTimeOffset parsedDateTimeOffset);
            Assert.IsTrue(isParsedDateTimeOffset);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParse<TimeSpan>(span.ToString(), out TimeSpan parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            var guid = Guid.NewGuid();
            bool isParsedGuid = ParseHelper.TryParse<Guid>(guid.ToString(), out Guid parsedGuid);
            Assert.IsTrue(isParsedGuid);
            Assert.AreEqual(guid, parsedGuid);
        }

        /// <summary>
        /// public static bool TryParse<TTargetType>(string input, NumberStyles numberStyles, IFormatProvider formatProvider, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse Generic 02: bool TryParse<T>(string, NumberStyles, IFormatProvider, out T result)")]
        public void Test_TryParse_Generic_02()
        {
            // int(Int32)
            bool isParsedInt; int expectedInt;
            expectedInt = 123;
            isParsedInt = ParseHelper.TryParse(expectedInt.ToString(), NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out int parsedInt);
            Assert.IsTrue(isParsedInt);
            Assert.AreEqual(expectedInt, parsedInt);
            expectedInt = -123;
            isParsedInt = ParseHelper.TryParse(expectedInt.ToString(), NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out parsedInt);
            Assert.IsTrue(isParsedInt);
            Assert.AreEqual(expectedInt, parsedInt);

            // uint(UInt32)
            bool isParsedUint = ParseHelper.TryParse("123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out uint parsedUint);
            Assert.IsTrue(isParsedUint);
            Assert.IsTrue(isParsedUint);
            Assert.AreEqual((uint)123, parsedUint);

            // short(Int16)
            bool isParsedShort = ParseHelper.TryParse("123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out short parsedShort);
            Assert.IsTrue(isParsedShort);
            Assert.AreEqual((short)123, parsedShort);
            isParsedShort = ParseHelper.TryParse("-123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out parsedShort);
            Assert.IsTrue(isParsedShort);
            Assert.AreEqual((short)-123, parsedShort);

            // ushort(UInt16)
            bool isParsedUshort = ParseHelper.TryParse<ushort>("456", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out ushort parsedUshort);
            Assert.IsTrue(isParsedUshort);
            Assert.AreEqual((ushort)456, parsedUshort);

            // long(Int64)
            bool isParsedLong = ParseHelper.TryParse<long>("123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out long parsedLong);
            Assert.IsTrue(isParsedLong);
            Assert.AreEqual((long)123, parsedLong);
            isParsedLong = ParseHelper.TryParse<long>("-123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out parsedLong);
            Assert.IsTrue(isParsedLong);
            Assert.AreEqual((long)-123, parsedLong);

            // ulong(Ulong)
            bool isParsedUlong = ParseHelper.TryParse<ulong>("123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out ulong parsedUlong);
            Assert.IsTrue(isParsedUlong);
            Assert.AreEqual((ulong)123, parsedUlong);

            // float(Single)
            bool isParsedFloat = ParseHelper.TryParse<float>("123.45", NumberStyles.Float, NumberFormatInfo.CurrentInfo, out float parsedFloat);
            Assert.IsTrue(isParsedFloat);
            Assert.AreEqual((float)123.45, parsedFloat);
            isParsedFloat = ParseHelper.TryParse<float>("-123.45", NumberStyles.Float, NumberFormatInfo.CurrentInfo, out parsedFloat);
            Assert.IsTrue(isParsedFloat);
            Assert.AreEqual((float)(-123.45), parsedFloat);

            // double(Double)
            bool isParsedDouble = ParseHelper.TryParse<double>("123.45", NumberStyles.Float, NumberFormatInfo.CurrentInfo, out double parsedDouble);
            Assert.IsTrue(isParsedDouble);
            Assert.AreEqual((double)123.45, parsedDouble);
            isParsedDouble = ParseHelper.TryParse<double>("-123.45", NumberStyles.Float, NumberFormatInfo.CurrentInfo, out parsedDouble);
            Assert.IsTrue(isParsedDouble);
            Assert.AreEqual((double)(-123.45), parsedDouble);

            // decimal(Decimal)
            bool isParsedDecimal = ParseHelper.TryParse<decimal>("123.45", NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out decimal parsedDecimal);
            Assert.IsTrue(isParsedDecimal);
            Assert.AreEqual((decimal)123.45, parsedDecimal);
            isParsedDecimal = ParseHelper.TryParse<decimal>("-123.45", NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out parsedDecimal);
            Assert.IsTrue(isParsedDecimal);
            Assert.AreEqual((decimal)(-123.45), parsedDecimal);

            // byte(Byte)
            bool isParsedByte = ParseHelper.TryParse<byte>("123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out byte parsedByte);
            Assert.IsTrue(isParsedByte);
            Assert.AreEqual((byte)123, parsedByte);

            // sbyte(SByte)
            bool isParsedSbyte = ParseHelper.TryParse<sbyte>("123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out sbyte parsedSbyte);
            Assert.IsTrue(isParsedSbyte);
            Assert.AreEqual((sbyte)123, parsedSbyte);
            isParsedSbyte = ParseHelper.TryParse<sbyte>("-123", NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out parsedSbyte);
            Assert.IsTrue(isParsedSbyte);
            Assert.AreEqual((sbyte)-123, parsedSbyte);

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<bool>("true", NumberStyles.Any, NumberFormatInfo.CurrentInfo, out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<char>("1", NumberStyles.Any, NumberFormatInfo.CurrentInfo, out char parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<DateTime>(DateTime.Now.ToString(), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out DateTime parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString(), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out DateTimeOffset parsedDateTimeOffset));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<TimeSpan>((DateTime.Now - DateTime.Today).ToString(), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out TimeSpan parsedTimeSpan));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<Guid>(Guid.NewGuid().ToString(), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out Guid parsedGuid));
        }

        /// <summary>
        /// public static bool TryParse<TTargetType>(string input, IFormatProvider formatProvider, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse Generic 03: bool TryParse<T>(string, IFormatProvider, out T result)")]
        public void Test_TryParse_Generic_03()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse("123", NumberFormatInfo.CurrentInfo, out int parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse("123", NumberFormatInfo.CurrentInfo, out uint parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse("123", NumberFormatInfo.CurrentInfo, out short parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<ushort>("456", NumberFormatInfo.CurrentInfo, out ushort parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<long>("123", NumberFormatInfo.CurrentInfo, out long parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<ulong>("123", NumberFormatInfo.CurrentInfo, out ulong parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<float>("123.45", NumberFormatInfo.CurrentInfo, out float parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<double>("123.45", NumberFormatInfo.CurrentInfo, out double parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<decimal>("123.45", NumberFormatInfo.CurrentInfo, out decimal parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<byte>("123", NumberFormatInfo.CurrentInfo, out byte parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<sbyte>("123", NumberFormatInfo.CurrentInfo, out sbyte parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<bool>("true", NumberFormatInfo.CurrentInfo, out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<char>("1", NumberFormatInfo.CurrentInfo, out char parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<DateTime>(DateTime.Now.ToString(), NumberFormatInfo.CurrentInfo, out DateTime parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString(), NumberFormatInfo.CurrentInfo, out DateTimeOffset parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParse<TimeSpan>(span.ToString(), DateTimeFormatInfo.CurrentInfo, out TimeSpan parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<Guid>(Guid.NewGuid().ToString(), NumberFormatInfo.CurrentInfo, out Guid parsedGuid));
        }

        /// <summary>
        /// public static bool TryParse<TTargetType>(string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse Generic 04: bool TryParse<T>(string, IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParse_Generic_04()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out int parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out uint parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out short parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<ushort>("456", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out ushort parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<long>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out long parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<ulong>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out ulong parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<float>("123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out float parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<double>("123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out double parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<decimal>("123.45", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out decimal parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<byte>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out byte parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<sbyte>("123", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out sbyte parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<bool>("true", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<char>("1", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out char parsedChar));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            bool isParsedDateTime = ParseHelper.TryParse<DateTime>(n1.ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out DateTime parsedDateTime);
            Assert.IsTrue(isParsedDateTime);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            bool isParsedDateTimeOffset = ParseHelper.TryParse<DateTimeOffset>(offset.ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out DateTimeOffset parsedDateTimeOffset);
            Assert.IsTrue(isParsedDateTimeOffset);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParse<TimeSpan>(span.ToString(), DateTimeFormatInfo.CurrentInfo, out TimeSpan parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParse<Guid>(Guid.NewGuid().ToString(), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out Guid parsedGuid));
        }

        #endregion // *** TryParse<T>( ... ) overloads ***

        #region *** TryParseExact<T>( ... ) overloads ***

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string format, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 01: bool TryParseExact<T>(string, string, out T result)")]
        public void Test_TryParseExact_Generic_01()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", out int parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", out uint parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", out short parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ushort>("456", "", out ushort parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<long>("123", "", out long parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ulong>("123", "", out ulong parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<float>("123.45", "", out float parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<double>("123.45", "", out double parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<decimal>("123.45", "", out decimal parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<byte>("123", "", out byte parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<sbyte>("123", "", out sbyte parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<bool>("true", "", out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<char>("1", "", out char parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTime>(DateTime.Now.ToString(), "", out DateTime parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString(), "", out DateTimeOffset parsedDateTimeOffset));

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<TimeSpan>((DateTime.Now - DateTime.Today).ToString(), "", out TimeSpan parsedTimeSpan));

            // Guid
            bool isParsedGuid; Guid expectedGuid;
            expectedGuid = Guid.NewGuid();
            isParsedGuid = ParseHelper.TryParseExact<Guid>(expectedGuid.ToString(), "D", out Guid parsedGuid);
            Assert.IsTrue(isParsedGuid);
            Assert.AreEqual(expectedGuid, parsedGuid);
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 02: bool TryParseExact<T>(string, string, IFormatProvider, out T result)")]
        public void Test_TryParseExact_Generic_02()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", NumberFormatInfo.CurrentInfo, out int parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", NumberFormatInfo.CurrentInfo, out uint parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", NumberFormatInfo.CurrentInfo, out short parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ushort>("456", "", NumberFormatInfo.CurrentInfo, out ushort parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<long>("123", "", NumberFormatInfo.CurrentInfo, out long parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ulong>("123", "", NumberFormatInfo.CurrentInfo, out ulong parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<float>("123.45", "", NumberFormatInfo.CurrentInfo, out float parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<double>("123.45", "", NumberFormatInfo.CurrentInfo, out double parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<decimal>("123.45", "", NumberFormatInfo.CurrentInfo, out decimal parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<byte>("123", "", NumberFormatInfo.CurrentInfo, out byte parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<sbyte>("123", "", NumberFormatInfo.CurrentInfo, out sbyte parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<bool>("true", "", NumberFormatInfo.CurrentInfo, out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<char>("1", "", NumberFormatInfo.CurrentInfo, out char parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTime>(DateTime.Now.ToString(), "", DateTimeFormatInfo.CurrentInfo, out DateTime parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString(), "", DateTimeFormatInfo.CurrentInfo, out DateTimeOffset parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParseExact<TimeSpan>(span.ToString(), "c", DateTimeFormatInfo.CurrentInfo, out TimeSpan parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<Guid>(Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, out Guid parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 03: bool TryParseExact<T>(string, string, IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParseExact_Generic_03()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out int parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out uint parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out short parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ushort>("456", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out ushort parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<long>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out long parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ulong>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out ulong parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<float>("123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out float parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<double>("123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out double parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<decimal>("123.45", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out decimal parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<byte>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out byte parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<sbyte>("123", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out sbyte parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<bool>("true", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<char>("1", "", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out char parsedChar));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            bool isParsedDateTime = ParseHelper.TryParseExact<DateTime>(n1.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out DateTime parsedDateTime);
            Assert.IsTrue(isParsedDateTime);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            bool isParsedDateTimeOffset = ParseHelper.TryParseExact<DateTimeOffset>(offset.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out DateTimeOffset parsedDateTimeOffset);
            Assert.IsTrue(isParsedDateTimeOffset);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<TimeSpan>((DateTime.Now - DateTime.Today).ToString(), "c", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out TimeSpan parsedTimeSpan));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<Guid>(Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out Guid parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 04: bool TryParseExact<T>(string, string, IFormatProvider, TimeSpanStyles, out T result)")]
        public void Test_TryParseExact_Generic_04()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out int parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out uint parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out short parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ushort>("456", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out ushort parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<long>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out long parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ulong>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out ulong parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<float>("123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out float parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<double>("123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out double parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<decimal>("123.45", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out decimal parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<byte>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out byte parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<sbyte>("123", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out sbyte parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<bool>("true", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<char>("1", "", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out char parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTime>(DateTime.Now.ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out DateTime parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString("F"), "F", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out DateTimeOffset parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParseExact<TimeSpan>(span.ToString(), "c", DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out TimeSpan parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<Guid>(Guid.NewGuid().ToString(), "D", NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out Guid parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 05: bool TryParseExact<T>(string, string[], IFormatProvider, out T result)")]
        public void Test_TryParseExact_Generic_05()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out int parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out uint parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out short parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ushort>("456", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out ushort parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<long>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out long parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ulong>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out ulong parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<float>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out float parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<double>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out double parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<decimal>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out decimal parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<byte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out byte parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<sbyte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out sbyte parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<bool>("true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<char>("1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, out char parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTime>(DateTime.Now.ToString(), new string[] { "", "" }, DateTimeFormatInfo.CurrentInfo, out DateTime parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString(), new string[] { "", "" }, DateTimeFormatInfo.CurrentInfo, out DateTimeOffset parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParseExact<TimeSpan>(span.ToString(), new string[] { "c", "" }, DateTimeFormatInfo.CurrentInfo, out TimeSpan parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<Guid>(Guid.NewGuid().ToString(), new string[] { "D", "" }, NumberFormatInfo.CurrentInfo, out Guid parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 06: bool TryParseExact<T>(string, string[], IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParseExact_Generic_06()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out int parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out uint parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out short parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ushort>("456", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out ushort parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<long>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out long parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ulong>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out ulong parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<float>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out float parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<double>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out double parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<decimal>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out decimal parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<byte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out byte parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<sbyte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out sbyte parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<bool>("true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<char>("1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out char parsedChar));

            // DateTime
            var n1 = DateTime.Now;
            n1 = new DateTime(n1.Year, n1.Month, n1.Day, n1.Hour, n1.Minute, n1.Second);
            bool isParsedDateTime = ParseHelper.TryParseExact<DateTime>(n1.ToString("F"), new string[] { "F", "" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out DateTime parsedDateTime);
            Assert.IsTrue(isParsedDateTime);
            Assert.AreEqual(n1, parsedDateTime);

            // DateTimeOffset
            var n2 = DateTime.Now;
            n2 = new DateTime(n2.Year, n2.Month, n2.Day, n2.Hour, n2.Minute, n2.Second);
            var offset = new DateTimeOffset(n2);
            bool isParsedDateTimeOffset = ParseHelper.TryParseExact<DateTimeOffset>(offset.ToString("F"), new string[] { "F", "" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out DateTimeOffset parsedDateTimeOffset);
            Assert.IsTrue(isParsedDateTimeOffset);
            Assert.AreEqual(offset, parsedDateTimeOffset);

            // TimeSpan
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<TimeSpan>((DateTime.Now - DateTime.Today).ToString(), new string[] { "c", "" }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out TimeSpan parsedTimeSpan));

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<Guid>(Guid.NewGuid().ToString(), new string[] { "D", "" }, NumberFormatInfo.CurrentInfo, DateTimeStyles.None, out Guid parsedGuid));
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 07: bool TryParseExact<T>(string, string[], IFormatProvider, TimeSpanStyles, out T result)")]
        public void Test_TryParseExact_Generic_07()
        {
            // int(Int32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out int parsedInt));

            // uint(UInt32)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out uint parsedUint));

            // short(Int16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out short parsedShort));

            // ushort(UInt16)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ushort>("456", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out ushort parsedUshort));

            // long(Int64)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<long>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out long parsedLong));

            // ulong(Ulong)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<ulong>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out ulong parsedUlong));

            // float(Single)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<float>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out float parsedFloat));

            // double(Double)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<double>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out double parsedDouble));

            // decimal(Decimal)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<decimal>("123.45", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out decimal parsedDecimal));

            // byte(Byte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<byte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out byte parsedByte));

            // sbyte(SByte)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<sbyte>("123", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out sbyte parsedSbyte));

            // bool(Boolean)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<bool>("true", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out bool parsedBool));

            // char(Char)
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<char>("1", new string[] { "", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out char parsedChar));

            // DateTime
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTime>(DateTime.Now.ToString("F"), new string[] { "F", "" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out DateTime parsedDateTime));

            // DateTimeOffset
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<DateTimeOffset>(new DateTimeOffset(DateTime.Now).ToString("F"), new string[] { "F", "" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out DateTimeOffset parsedDateTimeOffset));

            // TimeSpan
            var n3 = DateTime.Now;
            n3 = new DateTime(n3.Year, n3.Month, n3.Day, n3.Hour, n3.Minute, n3.Second);
            TimeSpan span = n3 - DateTime.Today;
            bool isParsedTimeSpan = ParseHelper.TryParseExact<TimeSpan>(span.ToString(), new string[] { "c", "" }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out TimeSpan parsedTimeSpan);
            Assert.IsTrue(isParsedTimeSpan);
            Assert.AreEqual(span, parsedTimeSpan);

            // Guid
            Assert.ThrowsException<InvalidOperationException>(() =>
                ParseHelper.TryParseExact<Guid>(Guid.NewGuid().ToString(), new string[] { "D", "" }, NumberFormatInfo.CurrentInfo, TimeSpanStyles.None, out Guid parsedGuid));
        }

        #endregion // *** TryParseExact<T>( ... ) overloads ***

        #endregion // *** generic methods ***
    }
}
