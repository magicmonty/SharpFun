using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Pagansoft.Functional
{
    /// <summary>
    /// Helper extensions for handling different collections
    /// </summary>
    public static class OptionExtensionsForCollections
    {
        /// <summary>
        /// Returns Some value if the array has exactly one element, otherwise None
        /// </summary>
        /// <param name="values">The array of values to check</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>Some value if the array has exactly one element, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> SingleOrNone<T>(this T?[]? values) =>
            values?.Length != 1 ? Option.None<T>() : values[0].AsOption();

        /// <summary>
        /// Returns Some value if the array has exactly one element witch matches the predicate, otherwise None
        /// </summary>
        /// <param name="values">The array of values to check</param>
        /// <param name="predicate">A Predicate function</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>Some value if the array has exactly one element which matches the predicate, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> SingleOrNone<T>(this T[]? values, Func<T, bool> predicate) =>
            values?.Length != 1
                ? Option.None<T>()
                : predicate(values[0])
                    ? values[0].AsOption()
                    : Option.None<T>();

        /// <summary>
        /// Returns Some value if the enumerable has exactly one element, otherwise None
        /// </summary>
        /// <param name="values">The enumerable of values to check</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>Some value if the enumerable has exactly one element, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> SingleOrNone<T>(this IEnumerable<T>? values)
        {
            try
            {
                return values!.Single().AsOption();
            }
            catch
            {
                return Option.None<T>();
            }
        }

        /// <summary>
        /// Returns Some value if the enumerable has exactly one element witch matches the predicate, otherwise None
        /// </summary>
        /// <param name="values">The enumerable of values to check</param>
        /// <param name="predicate">A Predicate function</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>Some value if the enumerable has exactly one element which matches the predicate, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> SingleOrNone<T>(this IEnumerable<T>? values, Func<T, bool> predicate)
        {
            IEnumerable<T>? enumerable = values as T[] ?? values?.ToArray();

            if (enumerable?.Count() != 1) { return Option.None<T>(); }

            var value = enumerable.First();
            return predicate(value) ? value.AsOption() : Option.None<T>();
        }

        /// <summary>
        /// Returns Some value if the enumerable has exactly one element witch matches the predicate, otherwise None
        /// </summary>
        /// <param name="values">The enumerable of values to check</param>
        /// <param name="predicate">A Predicate function</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>Some value if the enumerable has exactly one element which matches the predicate, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> SingleOrNone<T>(this IEnumerable<Option<T>>? values, Func<Option<T>, bool> predicate)
        {
            IEnumerable<Option<T>>? enumerable = values as Option<T>[] ?? values?.ToArray();

            if (enumerable?.Count() != 1) { return Option.None<T>(); }

            var value = enumerable.First();
            return predicate(value) ? value : Option.None<T>();
        }

        /// <summary>
        /// Returns the first value of the array if it has at least one element, otherwise None
        /// </summary>
        /// <param name="values">The array of values to check</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>The first value of the array if it has at least one element, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> FirstOrNone<T>(this T?[]? values) =>
            values switch
            {
                null => Option.None<T>(),
                var x when x.Length > 0 => x[0].AsOption(),
                _ => Option.None<T>()
            };

        /// <summary>
        /// Returns the first value of the array which matches the <paramref name="predicate"/>, otherwise None
        /// </summary>
        /// <param name="values">The array of values to check</param>
        /// <param name="predicate">A predicate function</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>The first value of the array which matches the predicate, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> FirstOrNone<T>(this T[]? values, Func<T, bool> predicate)
        {
            if (values is null) { return Option.None<T>(); }

            foreach (var element in values.Where(predicate))
            {
                return element.AsOption();
            }

            return Option.None<T>();
        }

        /// <summary>
        /// Returns the first value of the enumerable which matches the <paramref name="predicate"/>, otherwise None
        /// </summary>
        /// <param name="values">The enumerable of values to check</param>
        /// <param name="predicate">A predicate function</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>The first value of the enumerable which matches the predicate, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> FirstOrNone<T>(this IEnumerable<T>? values, Func<T, bool> predicate)
        {
            if (values is null) { return Option.None<T>(); }

            foreach (var element in values.Where(predicate))
            {
                return element.AsOption();
            }

            return Option.None<T>();
        }

        /// <summary>
        /// Returns the first value of the enumerable which matches the <paramref name="predicate"/>, otherwise None
        /// </summary>
        /// <param name="values">The enumerable of values to check</param>
        /// <param name="predicate">A predicate function</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>The first value of the enumerable which matches the predicate, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> FirstOrNone<T>(this IEnumerable<Option<T>>? values, Func<Option<T>, bool> predicate)
        {
            if (values is null) { return Option.None<T>(); }

            foreach (var element in values.Where(predicate))
            {
                return element;
            }

            return Option.None<T>();
        }

        /// <summary>
        /// Returns the first value of the array if it has at least one element, otherwise None
        /// </summary>
        /// <param name="values">The array of values to check</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>The first value of the array if it has at least one element, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> FirstOrNone<T>(this Option<T>[]? values) =>
            values is null
                ? Option.None<T>()
                : values.Length > 0
                    ? values[0]
                    : Option.None<T>();

        /// <summary>
        /// Returns the first value of the enumerable if it has at least one element, otherwise None
        /// </summary>
        /// <param name="values">The enumerable of values to check</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>The first value of the enumerable if it has at least one element, otherwise None</returns>
        [Pure, DebuggerStepThrough]
        public static Option<T> FirstOrNone<T>(this IEnumerable<Option<T>>? values) =>
            values
                ?.Take(1)
                .ToArray()
                .FirstOrNone() ?? Option.None<T>();

        /// <summary>
        /// Gets all values from the array, which are not null
        /// </summary>
        /// <param name="source">The source array</param>
        /// <param name="selector">A selector function to project the source value to the result value</param>
        /// <typeparam name="TSource">Type of the source values</typeparam>
        /// <typeparam name="TResult">Type of the result values</typeparam>
        /// <returns>An array with all result values, which are not null</returns>
        [Pure, DebuggerStepThrough]
        public static TResult[] SelectNonNullValues<TSource, TResult>(
            this TSource?[]? source,
            Func<TSource?, TResult> selector) where TResult : class =>
            source
                ?.Select(selector)
                .SelectNonNullValues()
                .ToArray() ?? Array.Empty<TResult>();

        /// <summary>
        /// Gets all values from the enumerable, which are not null
        /// </summary>
        /// <param name="source">The source enumerable</param>
        /// <param name="selector">A selector function to project the source value to the result value</param>
        /// <typeparam name="TSource">Type of the source values</typeparam>
        /// <typeparam name="TResult">Type of the result values</typeparam>
        /// <returns>An enumerable with all result values, which are not null</returns>
        [Pure, DebuggerStepThrough]
        public static IEnumerable<TResult> SelectNonNullValues<TSource, TResult>(
            this IEnumerable<TSource?>? source,
            Func<TSource?, TResult> selector) where TResult : class =>
            source
                ?.Select(selector)
                .SelectNonNullValues() ?? Enumerable.Empty<TResult>();

        /// <summary>
        /// Gets all values from the enumerable, which are not null
        /// </summary>
        /// <param name="values">The source enumerable</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>An enumerable with all values, which are not null</returns>
        [Pure, DebuggerStepThrough]
        public static IEnumerable<T> SelectNonNullValues<T>(this IEnumerable<T?>? values) where T : class =>
            values?.Where(v => v is not null).Select(v => v!) ?? Enumerable.Empty<T>();

        /// <summary>
        /// Gets all values from the array, which are not null
        /// </summary>
        /// <param name="values">The source array</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>An array with all values, which are not null</returns>
        [Pure, DebuggerStepThrough]
        public static T[] SelectNonNullValues<T>(this T?[]? values) where T : class =>
            values?.Where(v => v is not null).Select(v => v!).ToArray() ?? Array.Empty<T>();

        /// <summary>
        /// Gets all values from the enumerable, which are not None
        /// </summary>
        /// <param name="options">The source enumerable</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>An enumerable with all values, which are not None</returns>
        [Pure, DebuggerStepThrough]
        public static IEnumerable<T> SelectValues<T>(this IEnumerable<Option<T>>? options)
        {
            if (options is null) { yield break; }

            foreach (var option in options.Where(o => o.HasValue))
            {
                // Item is never null, as w check HasValue beforehand and an option value can never be null if set
                //// ReSharper disable once AssignNullToNotNullAttribute
                yield return option.ReturnValueOrDefault() !;
            }
        }

        /// <summary>
        /// Gets all values from the array, which are not None
        /// </summary>
        /// <param name="options">The source array</param>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <returns>An array with all values, which are not None</returns>
        [Pure, DebuggerStepThrough]
        public static T[] SelectValues<T>(this Option<T>[]? options) =>
            options
                ?.Aggregate(
                    Array.Empty<T>(),
                    (c, n) => n.Match(v => c.Append(v).ToArray(), () => c))
                .ToArray()
            ?? Array.Empty<T>();

        /// <summary>
        /// Returns the enumerable or an empty one, if none
        /// </summary>
        /// <param name="values">The enumerable option</param>
        /// <typeparam name="T">Type of the values in the enumerable</typeparam>
        /// <returns>the enumerable or an empty one, if none</returns>
        [Pure, DebuggerStepThrough]
        public static IEnumerable<T> ReturnValueOrEmpty<T>(this Option<IEnumerable<T>> values) =>
            values.ReturnValueOr(Enumerable.Empty<T>) !;

        /// <summary>
        /// Returns the collection or an empty one, if none
        /// </summary>
        /// <param name="values">The collection option</param>
        /// <typeparam name="T">Type of the values in the collection</typeparam>
        /// <returns>the collection or an empty one, if none</returns>
        [Pure, DebuggerStepThrough]
        public static IReadOnlyCollection<T> ReturnValueOrEmpty<T>(this Option<IReadOnlyCollection<T>> values) =>
            values.ReturnValueOr(Array.Empty<T>()) !;

        /// <summary>
        /// Returns the array or an empty one, if none
        /// </summary>
        /// <param name="values">The array option</param>
        /// <typeparam name="T">Type of the values in the array</typeparam>
        /// <returns>the array or an empty one, if none</returns>
        [Pure, DebuggerStepThrough]
        public static T[] ReturnValueOrEmpty<T>(this Option<T[]> values) =>
            values.ReturnValueOr(Array.Empty<T>()) !;

        /// <summary>
        /// Gets a collection from an object in an option
        /// and returns all non null/none values
        /// </summary>
        /// <param name="option">The option to work on</param>
        /// <param name="getter">A getter function to get the enumerable for the option</param>
        /// <typeparam name="T">The option type</typeparam>
        /// <typeparam name="TEnumerable">The type of the enumerable values</typeparam>
        /// <returns>An enumerable with all non null/none values</returns>
        [Pure, DebuggerStepThrough]
        public static IEnumerable<TEnumerable> Collect<T, TEnumerable>(
            this Option<T> option,
            Func<T, IEnumerable<TEnumerable>> getter) =>
            option
                .Select(getter)
                .ReturnValueOrEmpty()
                .Select(v => v.AsOption())
                .SelectValues();

        /// <summary>
        /// Gets a collection from an object
        /// and returns all non null/none values
        /// </summary>
        /// <param name="obj">The object to work on</param>
        /// <param name="getter">A getter function to get the enumerable for the option</param>
        /// <typeparam name="T">The option type</typeparam>
        /// <typeparam name="TEnumerable">The type of the enumerable values</typeparam>
        /// <returns>An enumerable with all non null/none values</returns>
        [Pure, DebuggerStepThrough]
        public static IEnumerable<TEnumerable> Collect<T, TEnumerable>(
            this T? obj,
            Func<T, IEnumerable<TEnumerable>> getter) =>
            obj.AsOption().Collect(getter);

        /// <summary>
        /// Gets an array from an object in an option
        /// and returns all non null/none values
        /// </summary>
        /// <param name="option">The option to work on</param>
        /// <param name="getter">A getter function to get the array for the option</param>
        /// <typeparam name="T">The option type</typeparam>
        /// <typeparam name="TEnumerable">The type of the array values</typeparam>
        /// <returns>An array with all non null/none values</returns>
        [Pure, DebuggerStepThrough]
        public static TEnumerable[] Collect<T, TEnumerable>(
            this Option<T> option,
            Func<T, TEnumerable[]> getter) =>
            option
                .Select(getter)
                .ReturnValueOrEmpty()
                .Select(v => v.AsOption())
                .SelectValues()
                .ToArray();

        /// <summary>
        /// Gets a value for the given <paramref name="key"/> from the <paramref name="dictionary"/>.
        /// If the key is not found, a none will be returned
        /// </summary>
        /// <param name="dictionary">The dictionary to check</param>
        /// <param name="key">The key of the value to get</param>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <returns>Some value, or none if not found</returns>
        [Pure, DebuggerStepThrough]
        public static Option<TValue> GetValueOrNone<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue>? dictionary, TKey key) =>
            dictionary is null || !dictionary.TryGetValue(key, out var value)
                ? Option.None<TValue>()
                : Option.Some(value);
    }
}