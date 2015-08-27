using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace Pagansoft.Functional
{
    public static class OptionExtensions
    {
        /// <summary>
        /// Invokes the given <paramref name="someAction"/> with the value of the given <paramref name="option"/>
        /// if the <paramref name="option"/> has a value.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="someAction">The action to invoke.</param>
        /// <typeparam name="T">The type of the option value.</typeparam>
        public static Option<T> Do<T>(this Option<T> option, Action<T> someAction)
        {
            Contract.Requires(option != null);
            Contract.Requires(someAction != null);
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            if (option.IsSome)
                someAction(option.Value);

            return option;
        }

        /// <summary>
        /// Invokes the given <paramref name="noneAction"/>
        /// if the given <paramref name="option"/> has no value.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="noneAction">The action to invoke.</param>
        /// <param name="noneAction">Action.</param>
        public static Option<T> OtherwiseDo<T>(this Option<T> option, Action noneAction)
        {
            Contract.Requires(option != null);
            Contract.Requires(noneAction != null);
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            if (option.IsNone)
                noneAction();
            
            return option;
        }

        /// <summary>
        /// Runs the specified <paramref name="action"/> if the <paramref name="option"/> matches.
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="action">The action to run, if the option matches a Some.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Option<T> Match<T>(this Option<T> option, Action<T> action)
        {
            Contract.Requires(option != null);
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return option.Do(action);
        }

        /// <summary>
        /// Runs the specified <paramref name="action"/> if the <paramref name="option"/> matches.
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="action">The action to run, if the option matches a None.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Option<T> Match<T>(this Option<T> option, Action action)
        {
            Contract.Requires(option != null);
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return option.OtherwiseDo(action);
        }

        /// <summary>
        /// Runs the matching action for the given <paramref name="option"/>.
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="someAction">The action to run, if the option matches a Some.</param>
        /// <param name="noneAction">The action to run, if the option matches a None.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Option<T> Match<T>(this Option<T> option, Action<T> someAction, Action noneAction)
        {
            Contract.Requires(option != null);
            Contract.Requires(someAction != null);
            Contract.Requires(noneAction != null);
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return option
                .Do(someAction)
                .OtherwiseDo(noneAction);
        }

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of 
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        public static Option<TResult> Map<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, TResult> transformation)
        {
            Contract.Requires(option != null);
            Contract.Requires(transformation != null);
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

            return option.Bind(v => transformation(v).AsOption());
        }

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of 
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// (alias for <see cref="Map"/>
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        public static Option<TResult> Select<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, TResult> transformation)
        {
            Contract.Requires(option != null);
            Contract.Requires(transformation != null);
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

            return option.Map(transformation);
        }

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of 
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// If the transformation function throws an exception, a None will be returned
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        public static Option<TResult> TryMap<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, TResult> transformation)
        {
            Contract.Requires(option != null);
            Contract.Requires(transformation != null);
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

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
        /// (alias for <see cref="TryMap"/>)
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        public static Option<TResult> TrySelect<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, TResult> transformation)
        {
            Contract.Requires(option != null);
            Contract.Requires(transformation != null);
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

            return option.TryMap(transformation);
        }

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of 
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        public static Option<TResult> Bind<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, Option<TResult>> transformation)
        {
            Contract.Requires(option != null);
            Contract.Requires(transformation != null);
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

            return option.IsSome
                ? transformation(option.Value)
                : Option.None<TResult>();
        }

        /// <summary>
        /// Transforms the value of the input <paramref name="option"/> with the help of 
        /// the <paramref name="transformation"/> function into a new option of type <typeparamref name="TResult" />
        /// If the transformation function throws an exception, a None will be returned
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="transformation">The transformation function.</param>
        /// <typeparam name="TInput">The type of the input option.</typeparam>
        /// <typeparam name="TResult">The type of the result option.</typeparam>
        public static Option<TResult> TryBind<TInput, TResult>(
            this Option<TInput> option,
            Func<TInput, Option<TResult>> transformation)
        {
            Contract.Requires(option != null);
            Contract.Requires(transformation != null);
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

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
        /// If <typeparamref name="TResult"> is a reference type and the value is null,
        /// then a None will be returned, otherwise a Some
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="TResult">The type of the value.</typeparam>
        /// <returns>The option.</returns>
        public static Option<TResult> AsOption<TResult>(this TResult value)
        {
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

            return !typeof(TResult).IsValueType && value == null
                ? Option.None<TResult>()
                : Option.Some(value);
        }

        /// <summary>
        /// Transforms a nullable value into an option.
        /// If the value is null,
        /// then a None will be returned, otherwise a Some
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="TResult">The type of the value.</typeparam>
        /// <returns>The option.</returns>
        public static Option<TResult> AsOption<TResult>(this TResult? value) where TResult : struct
        {
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

            return value.HasValue
                ? Option.Some<TResult>((TResult)value)
                : Option.None<TResult>();
        }

        /// <summary>
        /// Transforms a value into an option.
        /// If <typeparamref name="TResult"> is a reference type and the value is null,
        /// then a None will be returned, otherwise a Some or a None is returned
        /// depending of the return value of the predicate
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="TResult">The type of the value.</typeparam>
        /// <returns>The option.</returns>
        public static Option<TResult> AsOption<TResult>(this TResult value, Func<TResult, bool> predicate)
        {
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

            return value.AsOption().Where(predicate);
        }

        /// <summary>
        /// Transforms a nullable value into an option.
        /// If the value is null,
        /// then a None will be returned, otherwise a Some or a None is returned
        /// depending of the return value of the predicate
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="TResult">The type of the value.</typeparam>
        /// <returns>The option.</returns>
        public static Option<TResult> AsOption<TResult>(this TResult? value, Func<TResult, bool> predicate) where TResult : struct
        {
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Option<TResult>>() != null);

            return value.AsOption().Where(predicate);
        }

        /// <summary>Returns the option if the predicate matches on the value</summary>
        /// <param name="option">Option.</param>
        /// <param name="predicate">Predicate.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate)
        {
            Contract.Requires(option != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return option.IsSome && predicate(option.Value)
                ? option
                : Option.None<T>();
        }

        /// <summary>Returns the option if the predicate matches on the value (alias for <see cref="Where"/>Where>)</summary>
        /// <param name="option">Option.</param>
        /// <param name="predicate">Predicate.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Option<T> If<T>(this Option<T> option, Func<T, bool> predicate)
        {
            Contract.Requires(option != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return option.Where(predicate);
        }

        /// <summary>Returns the option if the predicate didn't match on the value</summary>
        /// <param name="option">Option.</param>
        /// <param name="predicate">Predicate.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Option<T> WhereNot<T>(this Option<T> option, Func<T, bool> predicate)
        {
            Contract.Requires(option != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return option.IsSome && !predicate(option.Value)
                ? option
                : Option.None<T>();
        }

        /// <summary>Returns the option if the predicate didn't match on the value (alias for <see cref="WhereNot"/>)</summary>
        /// <param name="option">Option.</param>
        /// <param name="predicate">Predicate.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Option<T> Else<T>(this Option<T> option, Func<T, bool> predicate)
        {
            Contract.Requires(option != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return option.WhereNot(predicate);
        }
    }
}

