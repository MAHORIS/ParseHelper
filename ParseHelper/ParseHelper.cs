using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ParseHelper
{
    /// <summary>
    /// ParseHelper.cs
    /// </summary>
    /// <remarks>
    /// release 2018/11/11 ver 1.0.1.0
    /// release 2018/09/30 ver 1.0.0.0
    /// </remarks>
    public class ParseHelper
    {
        #region *** private members ***

        private static MethodInfo GetParseMethod(Type targetType, string methodName, Type[] argumentTypes)
        {
            var flags = BindingFlags.Static | BindingFlags.Public;
            var methodInfo = targetType.GetMethod(methodName, flags, null, argumentTypes, null);
            return methodInfo;
        }

        private static void ThrowsExceptionIfNotParseMethodFound(Type targetType, MethodInfo methodInfo, string methodName, Type[] argumentTypes)
        {
            var canInvoke = methodInfo != null;
            if (!canInvoke)
            {
                var argTypeNames = string.Join(", ", argumentTypes.Select(t => t.Name).ToArray());
                var targetTypeName = targetType.FullName;
                var exceptionMessage = $"\"{methodName}({argTypeNames})\" method does not exist in the specified type [{targetTypeName}].";
                throw new NotSupportedException(exceptionMessage);
            }
        }

        private static object Invoke(Type targetType, string methodName, Type[] argumentTypes, object[] arguments)
        {
            var methodInfo = GetParseMethod(targetType, methodName, argumentTypes);
            ThrowsExceptionIfNotParseMethodFound(targetType, methodInfo, methodName, argumentTypes);
            var returnValue = methodInfo.Invoke(null, arguments);
            return returnValue;
        }

        [Obsolete()]
        private static TReturnType Invoke<TTargetType, TReturnType>(string methodName, Type[] argumentTypes, object[] arguments)
        {
            var returnValue = Invoke(typeof(TTargetType), methodName, argumentTypes, arguments);
            return (TReturnType)returnValue;
        }

        #endregion // *** private members ***

        #region *** non generic methods ***

        #region *** Parse(Type, ... ) overloads ***

        public static object Parse(Type targetType, string input)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(Parse);
            var arguments = new object[] { input };
            var argumentTypes = new Type[] { typeof(string) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object Parse(Type targetType, string input, IFormatProvider formatProvider)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(Parse);
            var arguments = new object[] { input, formatProvider };
            var argumentTypes = new Type[] { typeof(string), typeof(IFormatProvider) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object Parse(Type targetType, string input, NumberStyles numberStyles)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(Parse);
            var arguments = new object[] { input, numberStyles };
            var argumentTypes = new Type[] { typeof(string), typeof(NumberStyles) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object Parse(Type targetType, string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(Parse);
            var arguments = new object[] { input, numberStyles, formatProvider };
            var argumentTypes = new Type[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object Parse(Type targetType, string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(Parse);
            var arguments = new object[] { input, formatProvider, dateTimeStyles };
            var argumentTypes = new Type[] { typeof(string), typeof(IFormatProvider), typeof(DateTimeStyles) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        #endregion // *** *** Parse(Type, ... ) overloads ***

        #region *** ParseExact(Type, ... ) overloads ***

        public static object ParseExact(Type targetType, string input, string format)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(ParseExact);
            var arguments = new object[] { input, format };
            var argumentTypes = new Type[] { typeof(string), typeof(string) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object ParseExact(Type targetType, string input, string format, IFormatProvider formatProvider)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(ParseExact);
            var arguments = new object[] { input, format, formatProvider };
            var argumentTypes = new Type[] { typeof(string), typeof(string), typeof(IFormatProvider) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object ParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(ParseExact);
            var arguments = new object[] { input, format, formatProvider, dateTimeStyles };
            var argumentTypes = new Type[] { typeof(string), typeof(string), typeof(IFormatProvider), typeof(DateTimeStyles) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object ParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(ParseExact);
            var arguments = new object[] { input, format, formatProvider, timeSpanStyles };
            var argumentTypes = new Type[] { typeof(string), typeof(string), typeof(IFormatProvider), typeof(TimeSpanStyles) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object ParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(ParseExact);
            var arguments = new object[] { input, formats, formatProvider };
            var argumentTypes = new Type[] { typeof(string), typeof(string[]), typeof(IFormatProvider) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object ParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(ParseExact);
            var arguments = new object[] { input, formats, formatProvider, dateTimeStyles };
            var argumentTypes = new Type[] { typeof(string), typeof(string[]), typeof(IFormatProvider), typeof(DateTimeStyles) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        public static object ParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        {
            var a1 = targetType ?? throw new ArgumentNullException(nameof(targetType));
            var a2 = input ?? throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
            var methodName = nameof(ParseExact);
            var arguments = new object[] { input, formats, formatProvider, timeSpanStyles };
            var argumentTypes = new Type[] { typeof(string), typeof(string[]), typeof(IFormatProvider), typeof(TimeSpanStyles) };
            return Invoke(targetType, methodName, argumentTypes, arguments);
        }

        #endregion // *** ParseExact(Type, ... ) overloads ***

        #region *** TryParse(Type, ... ) overloads ***

        public static bool TryParse(Type targetType, string input, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParse);
            var arguments = new object[] { input, null };
            var argumentTypes = new Type[] { typeof(string), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        public static bool TryParse(Type targetType, string input, NumberStyles numberStyles, IFormatProvider formatProvider, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParse);
            var arguments = new object[] { input, numberStyles, formatProvider, null };
            var argumentTypes = new Type[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        public static bool TryParse(Type targetType, string input, IFormatProvider formatProvider, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParse);
            var arguments = new object[] { input, formatProvider, null };
            var argumentTypes = new Type[] { typeof(string), typeof(IFormatProvider), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        public static bool TryParse(Type targetType, string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParse);
            var arguments = new object[] { input, formatProvider, dateTimeStyles, null };
            var argumentTypes = new Type[] { typeof(string), typeof(IFormatProvider), typeof(DateTimeStyles), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        #endregion // *** TryParse(Type, ... ) overloads ***

        #region *** TryParseExact(Type, ... ) overloads ***

        public static bool TryParseExact(Type targetType, string input, string format, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParseExact);
            var arguments = new object[] { input, format, null };
            var argumentTypes = new Type[] { typeof(string), typeof(string), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        public static bool TryParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParseExact);
            var arguments = new object[] { input, format, formatProvider, null };
            var argumentTypes = new Type[] { typeof(string), typeof(string), typeof(IFormatProvider), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        public static bool TryParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParseExact);
            var arguments = new object[] { input, format, formatProvider, dateTimeStyles, null };
            var argumentTypes = new Type[] { typeof(string), typeof(string), typeof(IFormatProvider), typeof(DateTimeStyles), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        public static bool TryParseExact(Type targetType, string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParseExact);
            var arguments = new object[] { input, format, formatProvider, timeSpanStyles, null };
            var argumentTypes = new Type[] { typeof(string), typeof(string), typeof(IFormatProvider), typeof(TimeSpanStyles), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        public static bool TryParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParseExact);
            var arguments = new object[] { input, formats, formatProvider, null };
            var argumentTypes = new Type[] { typeof(string), typeof(string[]), typeof(IFormatProvider), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        public static bool TryParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParseExact);
            var arguments = new object[] { input, formats, formatProvider, dateTimeStyles, null };
            var argumentTypes = new Type[] { typeof(string), typeof(string[]), typeof(IFormatProvider), typeof(DateTimeStyles), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        public static bool TryParseExact(Type targetType, string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out object result)
        {
            var returnValue = false;
            result = default(object);
            if (targetType == null) return returnValue;
            var methodName = nameof(TryParseExact);
            var arguments = new object[] { input, formats, formatProvider, timeSpanStyles, null };
            var argumentTypes = new Type[] { typeof(string), typeof(string[]), typeof(IFormatProvider), typeof(TimeSpanStyles), targetType.MakeByRefType() };
            try
            {
                returnValue = (bool)Invoke(targetType, methodName, argumentTypes, arguments);
                if (returnValue)
                {
                    result = arguments.Last();
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        #endregion // *** TryParseExact(Type, ... ) overloads ***

        #endregion // *** non generic methods ***

        #region *** generic methods ***

        #region *** Parse<T>( ... ) overloads ***

        public static TTargetType Parse<TTargetType>(string input)
        {
            return (TTargetType)Parse(typeof(TTargetType), input);
        }

        public static TTargetType Parse<TTargetType>(string input, IFormatProvider formatProvider)
        {
            return (TTargetType)Parse(typeof(TTargetType), input, formatProvider);
        }

        public static TTargetType Parse<TTargetType>(string input, NumberStyles numberStyles)
        {
            return (TTargetType)Parse(typeof(TTargetType), input, numberStyles);
        }

        public static TTargetType Parse<TTargetType>(string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return (TTargetType)Parse(typeof(TTargetType), input, numberStyles, formatProvider);
        }

        public static TTargetType Parse<TTargetType>(string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return (TTargetType)Parse(typeof(TTargetType), input, formatProvider, dateTimeStyles);
        }

        #endregion // *** Parse<T>( ... ) overloads ***

        #region *** ParseExact<T>( ... ) overloads ***

        public static TTargetType ParseExact<TTargetType>(string input, string format)
        {
            return (TTargetType)ParseExact(typeof(TTargetType), input, format);
        }

        public static TTargetType ParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider)
        {
            return (TTargetType)ParseExact(typeof(TTargetType), input, format, formatProvider);
        }

        public static TTargetType ParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return (TTargetType)ParseExact(typeof(TTargetType), input, format, formatProvider, dateTimeStyles);
        }

        public static TTargetType ParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        {
            return (TTargetType)ParseExact(typeof(TTargetType), input, format, formatProvider, timeSpanStyles);
        }

        public static TTargetType ParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider)
        {
            return (TTargetType)ParseExact(typeof(TTargetType), input, formats, formatProvider);
        }

        public static TTargetType ParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return (TTargetType)ParseExact(typeof(TTargetType), input, formats, formatProvider, dateTimeStyles);
        }

        public static TTargetType ParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles)
        {
            return (TTargetType)ParseExact(typeof(TTargetType), input, formats, formatProvider, timeSpanStyles);
        }

        #endregion // *** ParseExact<T>( ... ) overloads ***

        #region *** TryParse<T>( ... ) overloads ***

        public static bool TryParse<TTargetType>(string input, out TTargetType result)
        {
            var canParse = TryParse(typeof(TTargetType), input, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        public static bool TryParse<TTargetType>(string input, NumberStyles numberStyles, IFormatProvider formatProvider, out TTargetType result)
        {
            var canParse = TryParse(typeof(TTargetType), input, numberStyles, formatProvider, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        public static bool TryParse<TTargetType>(string input, IFormatProvider formatProvider, out TTargetType result)
        {
            var canParse = TryParse(typeof(TTargetType), input, formatProvider, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        public static bool TryParse<TTargetType>(string input, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out TTargetType result)
        {
            var canParse = TryParse(typeof(TTargetType), input, formatProvider, dateTimeStyles, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        #endregion // *** TryParse<T>( ... ) overloads ***

        #region *** TryParseExact<T>( ... ) overloads ***

        public static bool TryParseExact<TTargetType>(string input, string format, out TTargetType result)
        {
            var canParse = TryParseExact(typeof(TTargetType), input, format, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        public static bool TryParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, out TTargetType result)
        {
            var canParse = TryParseExact(typeof(TTargetType), input, format, formatProvider, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        public static bool TryParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out TTargetType result)
        {
            var canParse = TryParseExact(typeof(TTargetType), input, format, formatProvider, dateTimeStyles, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        public static bool TryParseExact<TTargetType>(string input, string format, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out TTargetType result)
        {
            var canParse = TryParseExact(typeof(TTargetType), input, format, formatProvider, timeSpanStyles, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        public static bool TryParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, out TTargetType result)
        {
            var canParse = TryParseExact(typeof(TTargetType), input, formats, formatProvider, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        public static bool TryParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out TTargetType result)
        {
            var canParse = TryParseExact(typeof(TTargetType), input, formats, formatProvider, dateTimeStyles, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        public static bool TryParseExact<TTargetType>(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out TTargetType result)
        {
            var canParse = TryParseExact(typeof(TTargetType), input, formats, formatProvider, timeSpanStyles, out object parsed);
            result = canParse ? (TTargetType)parsed : default(TTargetType);
            return canParse;
        }

        #endregion // *** TryParseExact<T>( ... ) overloads ***

        #endregion // *** generic methods ***
    }
}
