// OptionExtensions.cs
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

namespace Pagansoft.Functional
{
    /// <summary>
    /// Extension methods for the <see cref="Option{T}"/> type
    /// </summary>
    public static class OptionExtensions
    {
        /// <summary>
        /// Invokes the given <paramref name="someAction"/> with the value of the given <paramref name="option"/>
        /// if the <paramref name="option"/> has a value.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="someAction">The action to invoke.</param>
        /// <typeparam name="T">The type of the option value.</typeparam>
        /// <returns>The option itself for chaining calls</returns>
        public static Option<T> Do<T>(this Option<T> option, Action<T> someAction)
        {
            if (option.HasValue)
            {
                someAction.Invoke(option.Value);
            }

            return option;
        }

        /// <summary>
        /// Invokes the given <paramref name="noneAction"/>
        /// if the given <paramref name="option"/> has no value.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="noneAction">The action to invoke.</param>
        /// <typeparam name="T">The type of the option value.</typeparam>
        /// <returns>The option itself for chaining calls</returns>
        public static Option<T> OtherwiseDo<T>(this Option<T> option, Action noneAction)
        {
            if (option.HasNoValue)
            {
                noneAction();
            }

            return option;
        }

        /// <summary>
        /// Runs the specified <paramref name="action"/> if the <paramref name="option"/> matches.
        /// </summary>
        /// <param name="option">The Option to match on.</param>
        /// <param name="action">The action to run, if the option matches a Some.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>The option itself for chaining calls</returns>
        public static Option<T> Match<T>(this Option<T> option, Action<T> action) =>
            option.Do(action);

        /// <summary>
        /// Runs the specified <paramref name="action"/> if the <paramref name="option"/> matches.
        /// </summary>
        /// <param name="option">The Option to match on.</param>
        /// <param name="action">The action to run, if the option matches a None.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>The option itself for chaining calls</returns>
        public static Option<T> Match<T>(this Option<T> option, Action action) =>
            option.OtherwiseDo(action);

        /// <summary>
        /// Runs the matching action for the given <paramref name="option"/>.
        /// </summary>
        /// <param name="option">The Option to match on.</param>
        /// <param name="someAction">The action to run, if the option matches a Some.</param>
        /// <param name="noneAction">The action to run, if the option matches a None.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>The option itself for chaining calls</returns>
        public static Option<T> Match<T>(this Option<T> option, Action<T> someAction, Action noneAction) =>
            option
                .Do(someAction)
                .OtherwiseDo(noneAction);

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// </summary>
        /// <param name="option">The Option to transform.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        /// <returns>An option with the containing type <typeparamref name="TResult" /></returns>
        public static Option<TResult> Map<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, TResult> transformation) =>
            option.Bind(v => transformation(v).AsOption());

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// (alias for <see cref="OptionExtensions.Map{TInput, TResult}"/>
        /// </summary>
        /// <param name="option">The Option to transform.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        /// <returns>An option with the containing type <typeparamref name="TResult" /></returns>
        public static Option<TResult> Select<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, TResult> transformation) =>
            option.Map(transformation);

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// If the transformation function throws an exception, a None will be returned
        /// </summary>
        /// <param name="option">The Option to transform.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        /// <returns>An option with the containing type <typeparamref name="TResult" /></returns>
        public static Option<TResult> TryMap<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, TResult> transformation)
        {
            try
            {
                return option.Map(transformation);
            }
            catch
            {
                return Option.None<TResult>();
            }
        }

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// If the transformation function throws an exception, a None will be returned
        /// (alias for <see cref="OptionExtensions.TryMap{TInput, TResult}"/>)
        /// </summary>
        /// <param name="option">The Option to transform.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        /// <returns>An option with the containing type <typeparamref name="TResult" /></returns>
        public static Option<TResult> TrySelect<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, TResult> transformation) =>
            option.TryMap(transformation);

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// </summary>
        /// <param name="option">The Option to transform.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        /// <returns>An option with the containing type <typeparamref name="TResult" /></returns>
        public static Option<TResult> Bind<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, Option<TResult>> transformation) =>
            option.HasValue
                ? transformation(option.Value)
                : Option.None<TResult>();

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// If the transformation function throws an exception, a None will be returned
        /// </summary>
        /// <param name="option">The Option to transform.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        /// <returns>An option with the containing type <typeparamref name="TResult" /></returns>
        public static Option<TResult> TryBind<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, Option<TResult>> transformation)
        {
            try
            {
                return option.Bind(transformation);
            }
            catch
            {
                return Option.None<TResult>();
            }
        }

        /// <summary>
        /// Transforms a value into an option.
        /// If <typeparamref name="TResult" /> is a reference type and the value is null,
        /// then a None will be returned, otherwise a Some
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="TResult">The type of the value.</typeparam>
        /// <returns>The option.</returns>
        public static Option<TResult> AsOption<TResult>(this TResult? value) =>
            value == null
                ? Option.None<TResult>()
                : Option.Some(value);

        /// <summary>
        /// Transforms a <see cref="Nullable"/> value into an option.
        /// If the value is null,
        /// then a None will be returned, otherwise a Some
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="TResult">The type of the value.</typeparam>
        /// <returns>The option.</returns>
        public static Option<TResult> AsOption<TResult>(this TResult? value) where TResult : struct =>
            value.HasValue
                ? Option.Some((TResult)value)
                : Option.None<TResult>();

        /// <summary>
        /// Transforms a value into an option.
        /// If <typeparamref name="TResult" /> is a reference type and the value is null,
        /// then a None will be returned, otherwise a Some or a None is returned
        /// depending of the return value of the <paramref name="predicate" />
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="predicate">Predicate function which is evaluated</param>
        /// <typeparam name="TResult">The type of the value.</typeparam>
        /// <returns>The option.</returns>
        public static Option<TResult> AsOption<TResult>(this TResult? value, Func<TResult, bool> predicate) =>
            value.AsOption().Where(predicate);

        /// <summary>
        /// Transforms a <see cref="Nullable"/> value into an option.
        /// If the value is null,
        /// then a None will be returned, otherwise a Some or a None is returned
        /// depending of the return value of the predicate
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="predicate">Predicate function which is evaluated</param>
        /// <typeparam name="TResult">The type of the value.</typeparam>
        /// <returns>The option.</returns>
        public static Option<TResult> AsOption<TResult>(this TResult? value, Func<TResult, bool> predicate) where TResult : struct =>
            value.AsOption().Where(predicate);

        /// <summary>Returns the option if the predicate matches on the value</summary>
        /// <param name="option">The Option to filter.</param>
        /// <param name="predicate">Predicate function which is evaluated</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>
        /// <see cref="Option.Some{T}">Some</see> value, if the predicate matches,
        /// otherwise <see cref="Option.None{T}">no value</see>
        /// </returns>
        public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate) =>
            option.HasValue && predicate(option.Value)
                ? option
                : Option.None<T>();

        /// <summary>Returns the option if the predicate matches on the value (alias for <see cref="OptionExtensions.Where{T}"/>)</summary>
        /// <param name="option">The Option to filter.</param>
        /// <param name="predicate">Predicate function which is evaluated</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>
        /// <see cref="Option.Some{T}">Some</see> value, if the predicate matches,
        /// otherwise <see cref="Option.None{T}">no value</see>
        /// </returns>
        public static Option<T> If<T>(this Option<T> option, Func<T, bool> predicate) =>
            option.Where(predicate);

        /// <summary>Returns the option if the predicate didn't match on the value</summary>
        /// <param name="option">The Option to filter.</param>
        /// <param name="predicate">The filter predicate.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>
        /// <see cref="Option.None{T}">No value</see>, if the predicate matches,
        /// otherwise <see cref="Option.Some{T}">some value</see>
        /// </returns>
        public static Option<T> WhereNot<T>(this Option<T> option, Func<T, bool> predicate) =>
            option.HasValue && !predicate(option.Value)
                ? option
                : Option.None<T>();

        /// <summary>Returns the option if the predicate didn't match on the value (alias for <see cref="OptionExtensions.WhereNot{T}"/>)</summary>
        /// <param name="option">The Option to filter.</param>
        /// <param name="predicate">The filter predicate.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>
        /// <see cref="Option.None{T}">No value</see>, if the predicate matches,
        /// otherwise <see cref="Option.Some{T}">some value</see>
        /// </returns>
        public static Option<T> Unless<T>(this Option<T> option, Func<T, bool> predicate) =>
            option.WhereNot(predicate);
    }
}
