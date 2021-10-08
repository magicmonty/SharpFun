using System.Diagnostics;

namespace Pagansoft.Functional
{
    /// <summary>
    /// Extension methods for the <see cref="Result"/> class
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if the value is true and a success
        /// </summary>
        /// <param name="result">the result value to check</param>
        /// <returns>Returns <c>true</c> if the value is false and a success, otherwise <c>false</c></returns>
        [DebuggerStepThrough]
        public static bool IsTrue(this Result<bool> result) =>
            result.Match(v => v, _ => false);

        /// <summary>
        /// Returns <c>true</c> if the value is false and a success
        /// </summary>
        /// <param name="result">the result value to check</param>
        /// <returns>Returns <c>true</c> if the value is false and a success, otherwise <c>false</c></returns>
        [DebuggerStepThrough]
        public static bool IsFalse(this Result<bool> result) =>
            result.Match(v => !v, _ => false);

        /// <summary>Converts a value into an <see cref="Result{TSuccess}"/> with the success value set.</summary>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <param name="input">The input.</param>
        /// <returns>The left value encapsulated as a successful result (<see cref="Result{TSuccess}" />)</returns>
        /// <example>
        /// <code language="cs">
        /// <![CDATA[
        /// var success = "The universe".ToResult<string>();
        /// ]]>
        /// </code>
        /// </example>
        [DebuggerStepThrough]
        public static Result<TSuccess> ToResult<TSuccess>(this TSuccess input) =>
            Result.Success(input);

        /// <summary>Converts a value into an <see cref="Result{TSuccess}"/> with an error set.</summary>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <param name="input">The input.</param>
        /// <returns>The left value encapsulated as a successful result (<see cref="Result{TSuccess}" />)</returns>
        /// <example>
        /// <code language="cs">
        /// <![CDATA[
        /// var failure = ExceptionErrorInfo.WithMessage("The Universe is broken").ToResult<string>();
        /// ]]>
        /// </code>
        /// </example>
        [DebuggerStepThrough]
        public static Result<TSuccess> ToResult<TSuccess>(this ExceptionWithContext input) =>
            Result.Failure<TSuccess>(input);

        /// <summary>
        /// Returns itself, made for convenience
        /// </summary>
        /// <typeparam name="TSuccess">The type of the left value.</typeparam>
        /// <param name="value">The result value.</param>
        /// <returns>the instance itself</returns>
        [DebuggerStepThrough]
        public static Result<TSuccess> ToResult<TSuccess>(this Result<TSuccess> value) =>
            value;
    }
}