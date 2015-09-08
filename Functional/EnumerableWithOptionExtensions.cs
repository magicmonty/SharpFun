// EnumerableWithOptionExtensions.cs
//
// Author:
//       Martin Gondermann <magicmonty@pagansoft.de>
//
// Copyright (c) 2015 (c) 2015 by Martin Gondermann
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
#if CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace Pagansoft.Functional
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/> with <see cref="Option{T}"/>
    /// elements.
    /// </summary>
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
#if CONTRACTS
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
#endif
            if (options == null)
                yield break;

            foreach (var option in options.Where(o => o.HasValue))
                yield return option.Value;
        }

        /// <summary>
        /// Returns the values from all elements with all <see cref="Option.None{T}"/> replaced by the given <paramref name="defaultValue"/>,
        /// </summary>
        /// <returns>The values of all elements with a value.</returns>
        /// <param name="options">The list of options.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> OptionValues<T>(this IEnumerable<Option<T>> options, T defaultValue)
        {
#if CONTRACTS
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
#endif
            if (options == null)
                yield break;

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
#if CONTRACTS
            Contract.Ensures(Contract.Result<Option<T>>() != null);
#endif
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
#if CONTRACTS
            Contract.Ensures(Contract.Result<Option<T>>() != null);
#endif
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