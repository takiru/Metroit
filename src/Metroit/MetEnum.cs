using System;
using System.Linq;

namespace Metroit
{
    /// <summary>
    /// <see cref="Enum"/> の変換を提供します。
    /// </summary>
    public static class MetEnum
    {
        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, byte? value)
        {
            return Parse(type, (object)value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, int? value)
        {
            return Parse(type, (object)value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, long? value)
        {
            return Parse(type, (object)value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, sbyte? value)
        {
            return Parse(type, (object)value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, short? value)
        {
            return Parse(type, (object)value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, uint? value)
        {
            return Parse(type, (object)value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, ulong? value)
        {
            return Parse(type, (object)value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, ushort? value)
        {
            return Parse(type, (object)value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。<br/>
        /// <paramref name="value"/> が <see cref="Enum"/> 列挙型の型へ変換できないとき、<see cref="Enum"/> 列挙型の定義名から求めようとします。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!type.IsEnum)
            {
                throw new ArgumentException("Type provided must be an Enum.", nameof(type));
            }

            var enumType = Enum.GetUnderlyingType(type);
            object convertedValue;
            try
            {
                convertedValue = Convert.ChangeType(value, enumType);
                if (!Enum.IsDefined(type, convertedValue))
                {
                    throw new ArgumentException($"{value} は {type.Name} に定義されていません。");
                }
            }
            catch
            {
                var enumNames = Enum.GetNames(type);
                if (!enumNames.Contains(value))
                {
                    throw new ArgumentException($"{value} は {type.Name} に定義されていません。");
                }
            }

            return Enum.Parse(type, value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> が <see cref="Enum"/> 列挙型ではありません。 または<br/>
        /// <paramref name="value"/> が <typeparamref name="T"/> に定義されていません。
        /// </exception>
        public static object Parse(Type type, char? value)
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!type.IsEnum)
            {
                throw new ArgumentException("Type provided must be an Enum.", nameof(type));
            }

            var enumType = Enum.GetUnderlyingType(type);
            if (!Enum.IsDefined(type, Convert.ChangeType(value, enumType)))
            {
                throw new ArgumentException($"{value} は {type.Name} に定義されていません。");
            }

            return Enum.ToObject(type, value.Value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(byte? value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(int? value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(long? value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(sbyte? value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(short? value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(uint? value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(ulong? value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(ushort? value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。<br/>
        /// <paramref name="value"/> が <see cref="Enum"/> 列挙型の型へ変換できないとき、<see cref="Enum"/> 列挙型の定義名から求めようとします。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(string value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        public static T Parse<T>(char? value) where T : Enum
        {
            return (T)Parse(typeof(T), value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, byte? value, out object result)
        {
            return TryParse<byte>(type, value, out result);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, int? value, out object result)
        {
            return TryParse<int>(type, value, out result);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, long? value, out object result)
        {
            return TryParse<long>(type, value, out result);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, sbyte? value, out object result)
        {
            return TryParse<sbyte>(type, value, out result);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, short? value, out object result)
        {
            return TryParse<short>(type, value, out result);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, uint? value, out object result)
        {
            return TryParse<uint>(type, value, out result);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, ulong? value, out object result)
        {
            return TryParse<ulong>(type, value, out result);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, ushort? value, out object result)
        {
            return TryParse<ushort>(type, value, out result);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。<br/>
        /// <paramref name="value"/> は <see cref="Enum"/> 列挙型の定義名も検証対象とします。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, string value, out object result)
        {
            try
            {
                result = Parse(type, value);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse(Type type, char? value, out object result)
        {
            try
            {
                result = Parse(type, value);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(byte? value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(int? value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(long? value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(sbyte? value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(short? value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(uint? value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(ulong? value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(ushort? value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。<br/>
        /// <paramref name="value"/> は <see cref="Enum"/> 列挙型の定義名も検証対象とします。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(string value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証します。
        /// </summary>
        /// <typeparam name="T">検証を行う <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        public static bool TryParse<T>(char? value, out T result) where T : Enum
        {
            var success = TryParse(typeof(T), value, out object resultObj);
            result = (T)resultObj;
            return success;
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換します。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="value">変換を行う値。</param>
        /// <returns>変換された <see cref="Enum"/> 列挙型。</returns>
        /// <exception cref="ArgumentNullException"><see cref="value"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> が <typeparamref name="T"/> に定義されていません。</exception>
        private static object Parse(Type type, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!type.IsEnum)
            {
                throw new ArgumentException("Type provided must be an Enum.", nameof(type));
            }

            if (!Enum.IsDefined(type, value))
            {
                throw new ArgumentException($"{value} は {type.Name} に定義されていません。");
            }

            return Enum.ToObject(type, value);
        }

        /// <summary>
        /// <see cref="Enum"/> 列挙型へ変換を検証する。
        /// </summary>
        /// <typeparam name="T">変換結果となる <see cref="Enum"/> 列挙型。</typeparam>
        /// <param name="type">検証を行う <see cref="Enum"/> 列挙型。</param>
        /// <param name="value">検証を行う値。</param>
        /// <param name="result">変換された <see cref="Enum"/> 列挙型。</param>
        /// <returns>変換が成功したときは <see langword="true"/>, それ以外は <see langword="false"/> を返却します。</returns>
        private static bool TryParse<T>(Type type, T? value, out object result) where T : struct
        {
            try
            {
                result = Parse(type, value);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
