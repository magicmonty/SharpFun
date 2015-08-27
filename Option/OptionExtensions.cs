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
            return option.IsSome
                ? Option.Some<TResult>(transformation(option.Value))    
                : Option.None<TResult>();
        }
    }
}

