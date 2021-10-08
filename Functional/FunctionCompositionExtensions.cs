using System;

namespace Pagansoft.Functional
{
    /// <summary>
    /// Extensions for function composition
    /// </summary>
    public static class FunctionCompositionExtensions
    {
        /// <summary>
        /// Composes a new function, which executes the given <paramref name="inner"/> function
        /// first and then the given <paramref name="outer"/> function.
        /// </summary>
        /// <returns>The composed function.</returns>
        /// <param name="inner">The inner function.</param>
        /// <param name="outer">The outer function.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Action AndThen<T>(this Func<T> inner, Action<T> outer) =>
            () => outer (inner ());

        /// <summary>
        /// Composes a new function, which executes the given <paramref name="inner"/> function
        /// first and then the given <paramref name="outer"/> function.
        /// </summary>
        /// <returns>The composed function.</returns>
        /// <param name="inner">The inner function.</param>
        /// <param name="outer">The outer function.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        public static Action<T1> AndThen<T1, T2>(this Func<T1, T2> inner, Action<T2> outer) =>
            x => outer (inner (x));

        /// <summary>
        /// Composes a new function, which executes the given <paramref name="inner"/> function
        /// first and then the given <paramref name="outer"/> function.
        /// </summary>
        /// <returns>The composed function.</returns>
        /// <param name="inner">The inner function.</param>
        /// <param name="outer">The outer function.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
        public static Func<T1, T3> AndThen<T1, T2, T3>(this Func<T1, T2> inner, Func<T2, T3> outer) =>
            x => outer (inner (x));

        /// <summary>
        /// Composes two functions together.
        /// (Outer with the value of the result of inner will be called, if the resulting function will be called)
        /// </summary>
        /// <param name="outer">The outer function</param>
        /// <param name="inner">The inner function</param>
        /// <typeparam name="T1">Type of the input parameter</typeparam>
        /// <typeparam name="T2">The result type of the resulting function</typeparam>
        /// <returns>The composed function</returns>
        public static Func<T2> Compose<T1, T2>(this Func<T1, T2> outer, Func<T1> inner) =>
            () => outer (inner ());

        /// <summary>
        /// Composes two functions together.
        /// (Outer with the value of the result of inner will be called, if the resulting function will be called)
        /// </summary>
        /// <param name="outer">The outer function</param>
        /// <param name="inner">The inner function</param>
        /// <typeparam name="T1">Type of the first input parameter</typeparam>
        /// <typeparam name="T2">Type of the second input parameter</typeparam>
        /// <typeparam name="T3">The result type of the resulting function</typeparam>
        /// <returns>The composed function</returns>
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T2, T3> outer, Func<T1, T2> inner) =>
            inner.AndThen (outer);

        /// <summary>
        /// Creates a new function with the last parameter already set (currying) out of a given function
        /// </summary>
        /// <param name="toCurry">The function to curry</param>
        /// <param name="parameter">The value of the parameter to set</param>
        /// <typeparam name="TParam">Type of the input parameter</typeparam>
        /// <typeparam name="TResult">The return type of the function</typeparam>
        /// <returns>The curried function</returns>
        public static Func<TResult> Curry<TParam, TResult>(this Func<TParam, TResult> toCurry, TParam parameter) =>
            () => toCurry (parameter);

        /// <summary>
        /// Creates a new function with the last parameter already set (currying) out of a given function
        /// </summary>
        /// <param name="toCurry">The function to curry</param>
        /// <param name="lastParameter">The value of the parameter to set</param>
        /// <typeparam name="T1">Type of the first input parameter</typeparam>
        /// <typeparam name="TParam">Type of the last input parameter</typeparam>
        /// <typeparam name="TResult">The return type of the function</typeparam>
        /// <returns>The curried function</returns>
        public static Func<T1, TResult> Curry<T1, TParam, TResult>(this Func<T1, TParam, TResult> toCurry, TParam lastParameter) =>
            x => toCurry (x, lastParameter);

        /// <summary>
        /// Creates a new function with the last parameter already set (currying) out of a given function
        /// </summary>
        /// <param name="toCurry">The function to curry</param>
        /// <param name="lastParameter">The value of the parameter to set</param>
        /// <typeparam name="T1">Type of the first input parameter</typeparam>
        /// <typeparam name="T2">Type of the second input parameter</typeparam>
        /// <typeparam name="TParam">Type of the last input parameter</typeparam>
        /// <typeparam name="TResult">The return type of the function</typeparam>
        /// <returns>The curried function</returns>
        public static Func<T1, T2, TResult> Curry<T1, T2, TParam, TResult>(this Func<T1, T2, TParam, TResult> toCurry, TParam lastParameter) =>
            (t1, t2) => toCurry (t1, t2, lastParameter);

        /// <summary>
        /// Creates a new function with the last parameter already set (currying) out of a given function
        /// </summary>
        /// <param name="toCurry">The function to curry</param>
        /// <param name="lastParameter">The value of the parameter to set</param>
        /// <typeparam name="T1">Type of the first input parameter</typeparam>
        /// <typeparam name="T2">Type of the second input parameter</typeparam>
        /// <typeparam name="T3">Type of the third input parameter</typeparam>
        /// <typeparam name="TParam">Type of the last input parameter</typeparam>
        /// <typeparam name="TResult">The return type of the function</typeparam>
        /// <returns>The curried function</returns>
        public static Func<T1, T2, T3, TResult> Curry<T1, T2, T3, TParam, TResult>(this Func<T1, T2, T3, TParam, TResult> toCurry, TParam lastParameter) =>
            (t1, t2, t3) => toCurry (t1, t2, t3, lastParameter);
    }
}
