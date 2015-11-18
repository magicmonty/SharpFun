using System;
using System.Runtime.InteropServices;

namespace Pagansoft.Functional
{
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
        public static Action AndThen<T>(this Func<T> inner, Action<T> outer)
        {
            return () => outer (inner ());
        }

        /// <summary>
        /// Composes a new function, which executes the given <paramref name="inner"/> function
        /// first and then the given <paramref name="outer"/> function.
        /// </summary>
        /// <returns>The composed function.</returns>
        /// <param name="inner">The inner function.</param>
        /// <param name="outer">The outer function.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        public static Action<T1> AndThen<T1, T2>(this Func<T1, T2> inner, Action<T2> outer)
        {
            return x => outer (inner (x));
        }

        public static Func<T1, T3> AndThen<T1, T2, T3>(this Func<T1, T2> inner, Func<T2, T3> outer)
        {
            return x => outer (inner (x));
        }

        public static Func<T2> Compose<T1, T2>(this Func<T1, T2> outer, Func<T1> inner)
        {
            return () => outer (inner ());
        }

        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T2, T3> outer, Func<T1, T2> inner)
        {
            return inner.AndThen (outer);
        }

        public static Func<T2> Curry<T1, T2>(this Func<T1, T2> toCurry, T1 parameter)
        {
            return () => toCurry (parameter);
        }

        public static Func<T1, TResult> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> toCurry, T2 lastParameter)
        {
            return x => toCurry (x, lastParameter);
        }

        public static Func<T1, T2, TResult> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> toCurry, T3 lastParameter)
        {
            return (t1, t2) => toCurry (t1, t2, lastParameter);
        }

        public static Func<T1, T2, T3, TResult> Curry<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> toCurry, T4 lastParameter)
        {
            return (t1, t2, t3) => toCurry (t1, t2, t3, lastParameter);
        }
    }
}

