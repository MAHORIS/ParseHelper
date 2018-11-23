using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ParseHelper.UnitTest
{
    /// <summary>
    /// ParseHelperTest.cs
    /// </summary>
    /// <remarks>
    /// release 2018/11/11 ver 1.0.1.0
    /// release 2018/10/24 ver 1.0.0.3 (test only)
    /// release 2018/10/15 ver 1.0.0.2 (test only)
    /// release 2018/10/14 ver 1.0.0.1 (test only)
    /// release 2018/09/30 ver 1.0.0.0
    /// </remarks>
    [TestClass]
    public class ParseHelperTest
    {
        #region *** summary ***

        [TestMethod]
        [TestCategory("Summary")]
        public void _summary_of_ParseHelperTest()
        {
            var builder = new System.Text.StringBuilder();
            builder.AppendLine($"{GetType().Name}, unit test of {nameof(ParseHelper)}, needs following tests ...");
            builder.AppendLine($" - .Parse() method +4 overloads");
            builder.AppendLine($" - .ParseExact() method +6 overloads");
            builder.AppendLine($" - .TryParse() method +3 overloads");
            builder.AppendLine($" - .TryParseExact() method +6 overloads");
            builder.AppendLine($" - .Parse<T>() method +4 overloads");
            builder.AppendLine($" - .ParseExact<T>() method +6 overloads");
            builder.AppendLine($" - .TryParse<T>() method +3 overloads");
            builder.AppendLine($" - .TryParseExact<T>() method +6 overloads");
            Console.WriteLine(builder.ToString());
        }

        #endregion // *** summary ***

        #region *** unit test item template ***

        #region *** test context ***

        public TestContext TestContext { get; set; }

        #endregion // *** test context ***

        #region *** class initialize/ cleanup ***

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        #endregion // *** class initialize/ cleanup ***

        #region *** test initialize/ cleanup ***

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion // *** test initialize/ cleanup ***

        #endregion // *** unit test item template ***

        #region *** nested object types for unit test ***

        #endregion // *** nested object types for unit test ***

        #region *** unit test methods ***

        #region *** non generic methods ***

        #region *** Parse(Type, ... ) overloads ***

        /// <summary>
        /// public static object Parse(Type targetType, string input)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse 01: Parse(Type, string)")]
        public void Test_Parse_01()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = true, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = true, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = true, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.Parse(type, row.ParsingString);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.Parse(type, row.ParsingString));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.Parse(row.Type, row.ParsingString);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static object Parse(Type targetType, string input, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse 02: Parse(Type, string, IFormatProvider)")]
        public void Test_Parse_02()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.Parse(type, row.ParsingString, NumberFormatInfo.CurrentInfo);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.Parse(type, row.ParsingString, NumberFormatInfo.CurrentInfo));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.Parse(row.Type, row.ParsingString, NumberFormatInfo.CurrentInfo);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }

            //// IFormatProvider formatProvider accept null
            //ParseHelper.Parse(typeof(int), "123", null);
        }

        /// <summary>
        /// public static object Parse(Type targetType, string input, NumberStyles numberStyles)
        /// </summary>
        [TestCategory("Test Parse 03: Parse(Type, string, NumberStyles)")]
        [TestMethod]
        public void Test_Parse_03()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.Parse(type, row.ParsingString, NumberStyles.Any);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.Parse(type, row.ParsingString, NumberStyles.Any));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.Parse(row.Type, row.ParsingString, NumberStyles.Any);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static object Parse(Type targetType, string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse 04: Parse(Type, string, NumberStyles, IFormatProvider)")]
        public void Test_Parse_04()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.Parse(type, row.ParsingString, NumberStyles.Any, NumberFormatInfo.CurrentInfo);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.Parse(type, row.ParsingString, NumberStyles.Any, NumberFormatInfo.CurrentInfo));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.Parse(row.Type, row.ParsingString, NumberStyles.Any, NumberFormatInfo.CurrentInfo);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static object Parse(Type targetType, string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse 05: Parse(Type, string, IFormatProvider, DateTimeStyles)")]
        public void Test_Parse_05()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.Parse(type, row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.Parse(type, row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.Parse(row.Type, row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
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
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = true, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.ParseExact(type, row.ParsingString, row.Format);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.ParseExact(type, row.ParsingString, row.Format));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.ParseExact(row.Type, row.ParsingString, string.Empty);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string format, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 02: Parse(Type, string, string, IFormatProvider)")]
        public void Test_ParseExact_02()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.ParseExact(type, row.ParsingString, row.Format, NumberFormatInfo.CurrentInfo);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.ParseExact(type, row.ParsingString, row.Format, NumberFormatInfo.CurrentInfo));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.ParseExact(row.Type, row.ParsingString, string.Empty, NumberFormatInfo.CurrentInfo);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 03: Parse(Type, string, string, IFormatProvider, DateTimeStyles)")]
        public void Test_ParseExact_03()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.ParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.ParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.ParseExact(row.Type, row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 04: Parse(Type, string, string, IFormatProvider, TimeSpanStyles)")]
        public void Test_ParseExact_04()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.ParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.ParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.ParseExact(row.Type, row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 05: Parse(Type, string, string[], IFormatProvider)")]
        public void Test_ParseExact_05()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.ParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.ParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.ParseExact(row.Type, row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 06: Parse(Type, string, string[], IFormatProvider, DateTimeStyles)")]
        public void Test_ParseExact_06()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.ParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.ParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.ParseExact(row.Type, row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static object ParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact 07: Parse(Type,  string, string[], IFormatProvider, TimeSpanStyles)")]
        public void Test_ParseExact_07()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    var parsed = ParseHelper.ParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    Assert.ThrowsException<NotSupportedException>(() =>
                        ParseHelper.ParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                Exception exception = null;
                try
                {
                    ParseHelper.ParseExact(row.Type, row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Assert.IsNotNull(exception);
                Assert.IsInstanceOfType(exception, row.ExpectedExceptionType);
            }
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
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = true, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = true, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = true, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParse(type, row.ParsingString, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParse(type, row.ParsingString, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParse(row.Type, row.ParsingString, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParse(Type targetType, string input, NumberStyles numberStyles, IFormatProvider formatProvider, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse 02: bool TryParse(Type, string, NumberStyles, IFormatProvider, out object result)")]
        public void Test_TryParse_02()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParse(type, row.ParsingString, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParse(type, row.ParsingString, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParse(row.Type, row.ParsingString, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParse(Type targetType, string input, IFormatProvider formatProvider, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse 03: bool TryParse(Type, string, IFormatProvider, out object result)")]
        public void Test_TryParse_03()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParse(type, row.ParsingString, DateTimeFormatInfo.CurrentInfo, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParse(type, row.ParsingString, DateTimeFormatInfo.CurrentInfo, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParse(row.Type, row.ParsingString, DateTimeFormatInfo.CurrentInfo, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParse(Type targetType, string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse 04: bool TryParse(Type, string, IFormatProvider, DateTimeStyles, out object result)")]
        public void Test_TryParse_04()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParse(type, row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParse(type, row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParse(row.Type, row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
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
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = true, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, row.Format, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, row.Format, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParseExact(row.Type, row.ParsingString, string.Empty, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 02: bool TryParseExact(Type, string, string, IFormatProvider, out T result)")]
        public void Test_TryParseExact_02()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParseExact(row.Type, row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 03: bool TryParseExact(Type, string, string, IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParseExact_03()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParseExact(row.Type, row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 04: bool TryParseExact(Type, string, string, IFormatProvider, TimeSpanStyles, out T result)")]
        public void Test_TryParseExact_04()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParseExact(row.Type, row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 05: bool TryParseExact(Type, string, string[], IFormatProvider, out T result)")]
        public void Test_TryParseExact_05()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParseExact(row.Type, row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 06: bool TryParseExact(Type, string, string[], IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParseExact_06()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParseExact(row.Type, row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out object result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact 07: bool TryParseExact(Type, string, string[], IFormatProvider, TimeSpanStyles, out T result)")]
        public void Test_TryParseExact_07()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // variables
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                if (row.CanParse)
                {
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out parsedValue);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = ParseHelper.TryParseExact(type, row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out parsedValue);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                new { Type = (Type)null, ParsingString = "123" },  // null for type
                new { Type = typeof(object), ParsingString = "123" },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t" },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                wasParsed = ParseHelper.TryParseExact(row.Type, row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, out parsedValue);
                Assert.IsFalse(wasParsed);
            }
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
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = true, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = true, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = true, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.Parse);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType Parse<TTargetType>(string input, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse Generic 02: Parse<T>(string, IFormatProvider)")]
        public void Test_Parse_Generic_02()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.Parse);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(IFormatProvider) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, NumberFormatInfo.CurrentInfo };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, NumberFormatInfo.CurrentInfo };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType Parse<TTargetType>(string input, NumberStyles numberStyles)
        /// </summary>
        [TestCategory("Test Parse Generic 03: Parse<T>(string, NumberStyles)")]
        [TestMethod]
        public void Test_Parse_Generic_03()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.Parse);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(NumberStyles) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, NumberStyles.Any };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, NumberStyles.Any };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType Parse<TTargetType>(string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse Generic 04: Parse<T>(string, NumberStyles, IFormatProvider)")]
        public void Test_Parse_Generic_04()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.Parse);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, NumberStyles.Number, NumberFormatInfo.CurrentInfo };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, NumberStyles.Number, NumberFormatInfo.CurrentInfo };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType Parse<TTargetType>(string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test Parse Generic 05: Parse<T>(string, IFormatProvider, DateTimeStyles)")]
        public void Test_Parse_Generic_05()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.Parse);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(IFormatProvider), typeof(DateTimeStyles) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
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
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = true, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.ParseExact);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(string) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, row.Format };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, string.Empty };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 02: Parse<T>(string, string, IFormatProvider)")]
        public void Test_ParseExact_Generic_02()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.ParseExact);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(string), typeof(IFormatProvider) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 03: Parse<T>(string, string, IFormatProvider, DateTimeStyles)")]
        public void Test_ParseExact_Generic_03()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.ParseExact);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(string), typeof(IFormatProvider), typeof(DateTimeStyles) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 04: Parse<T>(string, string, IFormatProvider, TimeSpanStyles)")]
        public void Test_ParseExact_Generic_04()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.ParseExact);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(string), typeof(IFormatProvider), typeof(TimeSpanStyles) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 05: Parse<T>(string, string[], IFormatProvider)")]
        public void Test_ParseExact_Generic_05()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.ParseExact);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(string[]), typeof(IFormatProvider) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string [] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 06: Parse<T>(string, string[], IFormatProvider, DateTimeStyles)")]
        public void Test_ParseExact_Generic_06()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.ParseExact);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(string[]), typeof(IFormatProvider), typeof(DateTimeStyles) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
        }

        /// <summary>
        /// public static TTargetType ParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        /// </summary>
        [TestMethod]
        [TestCategory("Test ParseExact Generic 07: Parse<T>(string, string[], IFormatProvider, TimeSpanStyles)")]
        public void Test_ParseExact_Generic_07()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.ParseExact);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var argumentTypes = new Type[] { typeof(string), typeof(string[]), typeof(IFormatProvider), typeof(TimeSpanStyles) };
            var methodInfo = typeof(ParseHelper).GetMethod(methodName, flags, null, argumentTypes, null);

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None };
                if (row.CanParse)
                {
                    var parsed = genericInfo.Invoke(null, arguments);
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.AreEqual(row.Expected, parsed);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                        genericInfo.Invoke(null, arguments));
                    Assert.IsNotNull(exception.InnerException);
                    Assert.IsInstanceOfType(exception.InnerException, typeof(NotSupportedException));
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None };
                var exception = Assert.ThrowsException<TargetInvocationException>(() =>
                    genericInfo.Invoke(null, arguments));
                Assert.IsNotNull(exception.InnerException);
                Assert.IsInstanceOfType(exception.InnerException, row.ExpectedExceptionType);
            }
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
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = true, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = true, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = true, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParse);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(2))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParse<TTargetType>(string input, NumberStyles numberStyles, IFormatProvider formatProvider, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse Generic 02: bool TryParse<T>(string, NumberStyles, IFormatProvider, out T result)")]
        public void Test_TryParse_Generic_02()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = true, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = true, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = true, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = true, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = true, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = true, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = true, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = true, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = true, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = true, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = true, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = true, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = true, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = true, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = true, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = true, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = true, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParse);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(4))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(NumberStyles)))
                .Where(m => m.GetParameters().ElementAt(2).ParameterType.Equals(typeof(IFormatProvider)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, NumberStyles.Any, NumberFormatInfo.CurrentInfo, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, NumberStyles.Any, NumberFormatInfo.CurrentInfo, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParse<TTargetType>(string input, IFormatProvider formatProvider, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse Generic 03: bool TryParse<T>(string, IFormatProvider, out T result)")]
        public void Test_TryParse_Generic_03()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParse);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(3))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(IFormatProvider)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, DateTimeFormatInfo.CurrentInfo, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, DateTimeFormatInfo.CurrentInfo, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParse<TTargetType>(string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParse Generic 04: bool TryParse<T>(string, IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParse_Generic_04()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123" }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123" }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123" }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123" }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123" }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123" }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123" }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123" }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123" }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45" }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45" }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45" }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45" }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45" }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123" }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123" }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString() }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1" }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString() }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString() }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString() }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString() }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParse);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(4))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(IFormatProvider)))
                .Where(m => m.GetParameters().ElementAt(2).ParameterType.Equals(typeof(DateTimeStyles)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
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
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = true, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParseExact);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);            //var m1 = methods.Where(m => m.)
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(3))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(string)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, row.Format, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, string.Empty, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 02: bool TryParseExact<T>(string, string, IFormatProvider, out T result)")]
        public void Test_TryParseExact_Generic_02()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParseExact);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);            //var m1 = methods.Where(m => m.)
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(4))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(2).ParameterType.Equals(typeof(IFormatProvider)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 03: bool TryParseExact<T>(string, string, IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParseExact_Generic_03()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParseExact);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);            //var m1 = methods.Where(m => m.)
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(5))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(2).ParameterType.Equals(typeof(IFormatProvider)))
                .Where(m => m.GetParameters().ElementAt(3).ParameterType.Equals(typeof(DateTimeStyles)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 04: bool TryParseExact<T>(string, string, IFormatProvider, TimeSpanStyles, out T result)")]
        public void Test_TryParseExact_Generic_04()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParseExact);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);            //var m1 = methods.Where(m => m.)
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(5))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(2).ParameterType.Equals(typeof(IFormatProvider)))
                .Where(m => m.GetParameters().ElementAt(3).ParameterType.Equals(typeof(TimeSpanStyles)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, row.Format, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, string.Empty, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 05: bool TryParseExact<T>(string, string[], IFormatProvider, out T result)")]
        public void Test_TryParseExact_Generic_05()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParseExact);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);            //var m1 = methods.Where(m => m.)
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(4))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(string[])))
                .Where(m => m.GetParameters().ElementAt(2).ParameterType.Equals(typeof(IFormatProvider)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 06: bool TryParseExact<T>(string, string[], IFormatProvider, DateTimeStyles, out T result)")]
        public void Test_TryParseExact_Generic_06()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = true, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = true, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = false, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParseExact);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);            //var m1 = methods.Where(m => m.)
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(5))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(string[])))
                .Where(m => m.GetParameters().ElementAt(2).ParameterType.Equals(typeof(IFormatProvider)))
                .Where(m => m.GetParameters().ElementAt(3).ParameterType.Equals(typeof(DateTimeStyles)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        /// <summary>
        /// public static bool TryParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out TTargetType result)
        /// </summary>
        [TestMethod]
        [TestCategory("Test TryParseExact Generic 07: bool TryParseExact<T>(string, string[], IFormatProvider, TimeSpanStyles, out T result)")]
        public void Test_TryParseExact_Generic_07()
        {
            // create test data (do not use DataRowAttribute)
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var offset = new DateTimeOffset(now);
            var span = now - DateTime.Today;
            var guid = Guid.NewGuid();
            dynamic dataRows = new object[]
            {
                new { CanParse = false, Expected = (int)123, ParsingString = "123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (int)-123, ParsingString = "-123", Format = string.Empty }, // int(Int32)
                new { CanParse = false, Expected = (uint)123, ParsingString = "123", Format = string.Empty }, // uint(UInt32)
                new { CanParse = false, Expected = (short)123, ParsingString = "123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (short)-123, ParsingString = "-123", Format = string.Empty }, // short(Int16)
                new { CanParse = false, Expected = (ushort)123, ParsingString = "123", Format = string.Empty }, // ushort(UInt16)
                new { CanParse = false, Expected = (long)123, ParsingString = "123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (long)-123, ParsingString = "-123", Format = string.Empty }, // long(Int64)
                new { CanParse = false, Expected = (ulong)123, ParsingString = "123", Format = string.Empty }, // ulong(Ulong)
                new { CanParse = false, Expected = (float)123.45, ParsingString = "123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (float)-123.45, ParsingString = "-123.45", Format = string.Empty }, // float(Single)
                new { CanParse = false, Expected = (double)123.45, ParsingString = "123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (double)-123.45, ParsingString = "-123.45", Format = string.Empty }, // double(Double)
                new { CanParse = false, Expected = (decimal)123.45, ParsingString = "123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (decimal)-123.45, ParsingString = "-123.45", Format = string.Empty }, // decimal(Decimal)
                new { CanParse = false, Expected = (byte)123, ParsingString = "123", Format = string.Empty }, // byte(Byte)
                new { CanParse = false, Expected = (sbyte)123, ParsingString = "123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (sbyte)-123, ParsingString = "-123", Format = string.Empty }, // sbyte(SByte)
                new { CanParse = false, Expected = (bool)true, ParsingString = true.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (bool)false, ParsingString = false.ToString(), Format = string.Empty }, // bool(Boolean)
                new { CanParse = false, Expected = (char)'1', ParsingString = "1", Format = string.Empty }, // char(Char)
                new { CanParse = false, Expected = now, ParsingString = now.ToString("F"), Format = "F" }, // DateTime
                new { CanParse = false, Expected = offset, ParsingString = offset.ToString("F"), Format = "F" }, // DateTimeOffset
                new { CanParse = true, Expected = span, ParsingString = span.ToString(), Format = "c" }, // TimeSpan
                new { CanParse = false, Expected = guid, ParsingString = guid.ToString(), Format = "D" }, // Guid
            };

            // get invoke contexts
            var methodName = nameof(ParseHelper.TryParseExact);
            var methods = typeof(ParseHelper).GetMethods().Where(m => m.Name.Equals(methodName) && m.IsGenericMethod);            //var m1 = methods.Where(m => m.)
            var methodInfo = methods
                .Where(m => m.GetParameters().Count().Equals(5))
                .Where(m => m.GetParameters().ElementAt(0).ParameterType.Equals(typeof(string)))
                .Where(m => m.GetParameters().ElementAt(1).ParameterType.Equals(typeof(string[])))
                .Where(m => m.GetParameters().ElementAt(2).ParameterType.Equals(typeof(IFormatProvider)))
                .Where(m => m.GetParameters().ElementAt(3).ParameterType.Equals(typeof(TimeSpanStyles)))
                .Single();
            bool wasParsed = false; object parsedValue = null;

            // correct cases
            foreach (var row in dataRows)
            {
                var type = row.Expected.GetType();
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { row.Format, row.Format }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, null };
                if (row.CanParse)
                {
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    parsedValue = arguments.Last();
                    Console.WriteLine($"{type.Name}: \"{row.ParsingString}\" -> {row.Expected.ToString()}");
                    Assert.IsTrue(wasParsed);
                    Assert.AreEqual(row.Expected, parsedValue);
                }
                else
                {
                    Console.WriteLine($"{type.Name}: can not parse");
                    wasParsed = (bool)genericInfo.Invoke(null, arguments);
                    Assert.IsFalse(wasParsed);
                }
            }

            // invalid cases
            dynamic invalidRows = new object[]
            {
                //new { Type = (Type)null, ParsingString = "123", ExpectedExceptionType = typeof(ArgumentNullException) },  // null for type
                new { Type = typeof(object), ParsingString = "123", ExpectedExceptionType = typeof(NotSupportedException) },  // invalid type
                new { Type = typeof(int), ParsingString = (string)null, ExpectedExceptionType = typeof(ArgumentNullException) },  // null for parsing string
                new { Type = typeof(int), ParsingString = string.Empty, ExpectedExceptionType = typeof(ArgumentException) },  // string.Empty for parsing string
                new { Type = typeof(int), ParsingString = " \t", ExpectedExceptionType = typeof(ArgumentException) },  // whitespace for parsing string
            };
            foreach (var row in invalidRows)
            {
                var type = row.Type;
                var genericInfo = methodInfo.MakeGenericMethod(new Type[] { type });
                var arguments = new object[] { row.ParsingString, new string[] { string.Empty, string.Empty }, DateTimeFormatInfo.CurrentInfo, TimeSpanStyles.None, null };
                wasParsed = (bool)genericInfo.Invoke(null, arguments);
                Assert.IsFalse(wasParsed);
            }
        }

        #endregion // *** TryParseExact<T>( ... ) overloads ***

        #endregion // *** generic methods ***

        #endregion // *** unit test methods ***
    }
}
