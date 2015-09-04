using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics.Contracts;

namespace Pagansoft.Functional
{
    public static class EnumerableWithOptionExtensions
    {
        /// <summary>
        /// Returns the values from all elements with a value
        /// </summary>
        /// <returns>The values of all elements with a value.</returns>
        /// <param name="options">The list of options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> OptionValues<T>(this IEnumerable<Option<T>> options)
        {
            foreach (var option in options.Where(o => o.IsSome))
                yield return option.Value;
        }

        /// <summary>
        /// Returns the values from all elements with all nones replaced by the given <paramref name="defaultValue"/>,
        /// </summary>
        /// <returns>The values of all elements with a value.</returns>
        /// <param name="options">The list of options.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> OptionValues<T>(this IEnumerable<Option<T>> options, T defaultValue)
        {
            foreach (var option in options)
                yield return option.ReturnValueOr(defaultValue);
        }

        /// <summary>
        /// Gets the first element of the specified enumerable as Option or a <see cref="Option.None{T}"/>.
        /// </summary>
        /// <returns>The first element of the enumeration as option or a <see cref="Option.None{T}"/> if no element exists.</returns>
        /// <param name="enumerable">The enumerable.</param>
        /// <typeparam name="T">The type of the elements of the enumerable.</typeparam>
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> enumerable)
        {
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            if (enumerable == null)
                return Option.None<T>();
            
            try
            {
                return enumerable.First().AsOption();
            }
            catch (InvalidOperationException)
            {
                return Option.None<T>();
            }
        }

        /// <summary>
        /// Gets the last element of the specified enumerable as Option or a <see cref="Option.None{T}"/>.
        /// </summary>
        /// <returns>The last element of the enumeration as option or a <see cref="Option.None{T}"/> if no element exists.</returns>
        /// <param name="enumerable">The enumerable.</param>
        /// <typeparam name="T">The type of the elements of the enumerable.</typeparam>
        public static Option<T> LastOrNone<T>(this IEnumerable<T> enumerable)
        {
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            if (enumerable == null)
                return Option.None<T>();

            try
            {
                return enumerable.Last().AsOption();
            }
            catch (InvalidOperationException)
            {
                return Option.None<T>();
            }
        }
    }
}

