﻿#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
#if !WINDOWS_PHONE
using System.Globalization;
using System.Xml;
#endif
#if NETSTANDARD1_3 || WINDOWS_UWP
using System.Reflection;
#endif
using System.Text.RegularExpressions;

namespace Serialize.Linq.Internals
{
    public static class ValueConverter
    {
        private static readonly IDictionary<Type, Func<object, Type, object>> _userDefinedConverters = new Dictionary<Type, Func<object, Type, object>>();
        private static readonly Regex _dateRegex = new Regex(@"^/Date\((?<date>-?\d+)((?<offsign>[-+])((?<offhours>\d{2})(?<offminutes>\d{2})))?\)/$",
#if !NETSTANDARD
            RegexOptions.Compiled | RegexOptions.ExplicitCapture
#else
            RegexOptions.ExplicitCapture
#endif
            );
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

        /// <summary>
        /// Adds the custom converter.
        /// </summary>
        /// <param name="convertTo">The convert to.</param>
        /// <param name="converter">The converter.</param>
        public static void AddCustomConverter(Type convertTo, Func<object, object> converter)
        {
            AddCustomConverter(convertTo, (v, t) => converter(v));
        }

        /// <summary>
        /// Adds the custom converter.
        /// </summary>
        /// <param name="converter">The converter.</param>
        public static void AddCustomConverter(Func<object, Type, object> converter)
        {
            AddCustomConverter(typeof(void), converter);
        }

        /// <summary>
        /// Adds the custom converter.
        /// </summary>
        /// <param name="convertTo">The convert to.</param>
        /// <param name="converter">The converter.</param>
        /// <exception cref="System.ArgumentNullException">
        /// convertTo
        /// or
        /// converter
        /// </exception>
        /// <exception cref="System.ApplicationException">Failed to add converter.</exception>
        public static void AddCustomConverter(Type convertTo, Func<object, Type, object> converter)
        {
            if (convertTo == null)
                throw new ArgumentNullException(nameof(convertTo));

            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            _userDefinedConverters.Add(convertTo, converter);
        }

        /// <summary>
        /// Clears the custom converters.
        /// </summary>
        public static void ClearCustomConverters()
        {
            _userDefinedConverters.Clear();
        }

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="convertTo">The convert to.</param>
        /// <returns></returns>
        public static object Convert(object value, Type convertTo)
        {
            if (value == null)
                return convertTo.IsValueType() ? Activator.CreateInstance(convertTo) : null;

            if (convertTo.IsInstanceOfType(value))
                return value;

            if (TryCustomConvert(value, convertTo, out var retval))
                return retval;

            // "general" checks should run first
            if (TryConvertToArray(value, convertTo, out var retArray))
                return retArray;

            if (TryConvertToList(value, convertTo, out var retList))
                return retList;

            if (TryConvertToNullable(value, convertTo, out var retNullable))
                return retNullable;

            if (TryConvertToDateTime(value, convertTo, out var retDateTime))
                return retDateTime;

            if (TryConvertToTimeSpan(value, convertTo, out var retTimeSpan))
                return retTimeSpan;

            if (TryConvertToEnum(value, convertTo, out var retEnum))
                return Enum.ToObject(convertTo, retEnum);

            if (TryConvertToGuid(value, convertTo, out var retGuid))
                return retGuid;

            if (TryConvertibleConversion(value, convertTo, out var retConvertible))
                return retConvertible;

            // fallback
            return Activator.CreateInstance(convertTo, value);
        }

        /// <summary>
        /// Tries the custom convert.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="convertTo">The convert to.</param>
        /// <param name="convertedValue">The converted value.</param>
        /// <returns></returns>
        private static bool TryCustomConvert(object value, Type convertTo, out object convertedValue)
        {
            if (_userDefinedConverters.TryGetValue(convertTo, out var converter) || _userDefinedConverters.TryGetValue(typeof(void), out converter))
            {
                convertedValue = converter(value, convertTo);
                return true;
            }
            convertedValue = null;
            return false;
        }

        /// <summary>
        /// Tries the convert to date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        private static bool TryConvertToDateTime(object value, Type convertTo, out DateTime convertedValue)
        {
            if (convertTo == typeof(DateTime))
            {
                var stringValue = value.ToString();
                if (DateTime.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out convertedValue) ||
                    DateTime.TryParse(stringValue, CultureInfo.CurrentCulture, DateTimeStyles.None, out convertedValue))
                    return true;
                else
                {
                    var match = _dateRegex.Match(stringValue);
                    if (match.Success && Int64.TryParse(match.Groups["date"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var msFromEpoch))
                    {
                        // Improvement: conversion takes account of local and UTC time
                        if (match.Groups["offsign"].Success)
                        {
                            var hours = Int32.Parse(match.Groups["offhours"].Value, NumberStyles.None, CultureInfo.InvariantCulture);
                            var minutes = Int32.Parse(match.Groups["offminutes"].Value, NumberStyles.None, CultureInfo.InvariantCulture);
                            var sign = match.Groups["offsign"].Value == "-" ? -1 : 1;
                            convertedValue = _epoch.AddMilliseconds(msFromEpoch).AddHours(hours * sign).AddMinutes(minutes * sign);
                            convertedValue = DateTime.SpecifyKind(new DateTimeOffset(convertedValue,
                                                                                     new TimeSpan(sign * hours, sign * minutes, 0)).DateTime,
                                                                  DateTimeKind.Local);
                        }
                        else
                        {
                            convertedValue = _epoch.AddMilliseconds(msFromEpoch);
                            convertedValue = DateTime.SpecifyKind(new DateTimeOffset(convertedValue,
                                                                                     TimeSpan.Zero).DateTime, 
                                                                  DateTimeKind.Utc);
                        }
                        return true;
                    }
                }
            }
            convertedValue = default(DateTime);
            return false;
        }

        private static bool TryConvertToTimeSpan(object value, Type convertTo, out TimeSpan convertedValue)
        {
            if (convertTo == typeof(TimeSpan))
            {
                var sourceSpanString = value.ToString();
                if (String.IsNullOrEmpty(sourceSpanString))
                {
                    convertedValue = TimeSpan.Zero;
                    return true;
                }
                else if (TimeSpan.TryParse(sourceSpanString, CultureInfo.InvariantCulture, out convertedValue))
                    return true;
                else
                    try
                    {
                        convertedValue = XmlConvert.ToTimeSpan(sourceSpanString);
                        return true;
                    }
                    catch { }
            }
            convertedValue = default(TimeSpan);
            return false;
        }

        private static bool TryConvertToEnum(object value, Type convertTo, out object convertedValue)
        {
            if (convertTo.IsEnum())
            {
                try
                {
                    convertedValue = Enum.ToObject(convertTo, value);
                    return true;
                }
                catch { }
            }
            convertedValue = null;
            return false;
        }

        private static bool TryConvertToArray(object value, Type convertTo, out Array convertedValue)
        {
            if (convertTo.IsArray && value.GetType().IsArray)
            {
                var elementType = convertTo.GetElementType();
                if (elementType == null)
                    throw new InvalidOperationException("Cannot build array with an unkown element type.");
                var valArray = (Array)value;
                convertedValue = Array.CreateInstance(elementType, valArray.Length);
                for (var i = 0; i < valArray.Length; i++)
                {
                    convertedValue.SetValue(Convert(valArray.GetValue(i), elementType), i);
                }
                return true;
            }
            convertedValue = null;
            return false;
        }

        private static bool TryConvertToList(object value, Type convertTo, out IList convertedValue)
        {
            if (convertTo.IsGenericType() && value is IEnumerable items && convertTo.GetGenericTypeDefinition() == typeof(List<>))
            {
                convertedValue = (IList)Activator.CreateInstance(convertTo);
                var argumentType = convertTo.GetGenericArguments()[0];
                foreach (var item in items)
                {
                    convertedValue.Add(Convert(item, argumentType));
                }
                return true;
            }
            convertedValue = null;
            return false;
        }

        private static bool TryConvertToNullable(object value, Type convertTo, out object convertedValue)
        {
            if (convertTo.IsGenericType() && convertTo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var argumentTypes = convertTo.GetGenericArguments();
                if (argumentTypes.Length == 1)
                {
                    convertedValue = Convert(value, argumentTypes[0]);
                    return true;
                }
            }
            convertedValue = null;
            return false;
        }

        // handels issue #138, thanks to Oleg Nadymov
        private static bool TryConvertToGuid(object value, Type convertTo, out Guid convertedValue)
        {
            if (convertTo.IsAssignableFrom(typeof(Guid)) && Guid.TryParse(value.ToString(), out var guid))
            {
                convertedValue = guid;
                return true;
            }
            convertedValue = default(Guid);
            return false;
        }

        private static bool TryConvertibleConversion(object value, Type convertTo, out IConvertible convertedValue)
        {
            if (value is IConvertible)
            {
                try
                {
                    convertedValue = (IConvertible)System.Convert.ChangeType(value, convertTo, CultureInfo.InvariantCulture);
                    return true;
                }
                catch { }
            }
            convertedValue = null;
            return false;
        }
    }
}
