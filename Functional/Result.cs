// Result.cs
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
using System.Diagnostics;

namespace Pagansoft.Functional
{
    /// <summary>
    /// Base class for a Result.
    /// A result is a special case of an <see cref="Either{TLeft, TRight}"/>, where the
    /// left value is a success value and the right value is an <see cref="ExceptionWithContext"/>.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value</typeparam>
    public abstract class Result<TSuccess> : Either<TSuccess, ExceptionWithContext>
    {
        /// <summary>
        /// Convenience helper, which indicates, that the result is a success
        /// </summary>
        public bool IsSuccess => IsLeft;

        /// <summary>
        /// Convenience helper, which indicates, that the result is a failure
        /// </summary>
        public bool IsFailure => IsRight;

        /// <inheritdoc />
        public override void Match(Action<TSuccess?> onLeft, Action<ExceptionWithContext?> onRight)
        {
            DoOnSuccess(onLeft);
            DoOnFailure(onRight);
        }

        /// <inheritdoc />
        public override void MatchLeft(Action<TSuccess> onLeft)
        {
            /* intentionally left blank */
        }

        /// <inheritdoc />
        public override void MatchRight(Action<ExceptionWithContext?> onRight)
        {
            /* intentionally left blank */
        }

        /// <summary>
        /// Executes the <paramref name="onSuccess"/> method, if the instance is a Success value.
        /// </summary>
        /// <param name="onSuccess">The action, which is called on success.</param>
        public void DoOnSuccess(Action<TSuccess> onSuccess) =>
            MatchLeft(onSuccess);

        /// <summary>
        /// Executes the <paramref name="onFailure"/> method, if the instance is a Failure value.
        /// </summary>
        /// <param name="onFailure">The action, which is called on failure.</param>
        public void DoOnFailure(Action<ExceptionWithContext?> onFailure) =>
            MatchRight(onFailure);

        /// <summary>
        /// If the Result is a failure, try to rescue the execution with an alternate result
        /// </summary>
        /// <param name="alternateAction">The function to execute in case of a failure</param>
        /// <returns>The result itself on success or the result of the alternate action in case of a failure</returns>
        [DebuggerStepThrough]
        public Result<TSuccess> Rescue(Func<ExceptionWithContext?, Result<TSuccess>> alternateAction) =>
            Match(_ => this, alternateAction);

        #region Linq query methods

        /// <summary>
        /// Projects the success value of a result into a new result of a different type
        /// </summary>
        /// <param name="selector">A transform function to apply to the success value</param>
        /// <typeparam name="TOut">The result type of the <paramref name="selector"/> function</typeparam>
        /// <returns>The result of the selector function if the result is a success or a failure</returns>
        [DebuggerStepThrough]
        public Result<TOut> Select<TOut>(Func<TSuccess, TOut> selector)
        {
            try
            {
                return Match(
                    success => Result.Success(selector(success)),
                    Result.Failure<TOut>);
            }
            catch (ExceptionWithContext e)
            {
                return Result.Failure<TOut>(e);
            }
            catch (Exception e)
            {
                return Result.Failure<TOut>(new ExceptionWithContext(e.Message, e, null));
            }
        }

        /// <summary>
        /// Projects the success value of a result into a new result of a different type
        /// and flattens the result
        /// </summary>
        /// <param name="selectorB">A transform function to apply to the success value</param>
        /// <typeparam name="TResult">The result type of the <paramref name="selectorB"/> function</typeparam>
        /// <returns>The result of the selector function if the result is a success or a failure</returns>
        [DebuggerStepThrough]
        public Result<TResult> SelectMany<TResult>(Func<TSuccess, TResult> selectorB)
        {
            try
            {
                return Match(
                    success => Result.Success(selectorB(success)),
                    Result.Failure<TResult>);
            }
            catch (ExceptionWithContext e)
            {
                return Result.Failure<TResult>(e);
            }
            catch (Exception e)
            {
                return Result.Failure<TResult>(new ExceptionWithContext(e.Message, e, null));
            }
        }

        /// <summary>
        /// Projects the success value of a result into a new result of a different type
        /// and flattens the result
        /// </summary>
        /// <param name="selectorB">A transform function to apply to the success value</param>
        /// <typeparam name="TResult">The result type of the <paramref name="selectorB"/> function</typeparam>
        /// <returns>The result of the selector function if the result is a success or a failure</returns>
        [DebuggerStepThrough]
        public Result<TResult> SelectMany<TResult>(Func<TSuccess, Result<TResult>> selectorB)
        {
            try
            {
                return Match(
                    selectorB,
                    Result.Failure<TResult>);
            }
            catch (ExceptionWithContext e)
            {
                return Result.Failure<TResult>(e);
            }
            catch (Exception e)
            {
                return Result.Failure<TResult>(new ExceptionWithContext(e.Message, e, null));
            }
        }

        /// <summary>
        /// Projects the success value of a result into a new result of a different type
        /// and flattens the result
        /// </summary>
        /// <param name="selectorA">A transform function to apply to the success value of the first result</param>
        /// <param name="selectorB">A transform function to apply to the success value of the result of <paramref name="selectorA"/></param>
        /// <typeparam name="TProp">The result type of the <paramref name="selectorA"/> function</typeparam>
        /// <typeparam name="TResult">The result type of the <paramref name="selectorB"/> function</typeparam>
        /// <returns>The result of the selector function if the result is a success or a failure</returns>
        [DebuggerStepThrough]
        public Result<TResult> SelectMany<TProp, TResult>(
            Func<TSuccess, Result<TProp>> selectorA,
            Func<TSuccess, TProp, TResult> selectorB)
        {
            try
            {
                return Match(
                    success =>
                        selectorA(success)
                            .Match(
                                successA => Result.Success(selectorB(success, successA)),
                                Result.Failure<TResult>),
                    Result.Failure<TResult>);
            }
            catch (ExceptionWithContext e)
            {
                return Result.Failure<TResult>(e);
            }
            catch (Exception e)
            {
                return Result.Failure<TResult>(new ExceptionWithContext(e.Message, e, null));
            }
        }

        #endregion
    }

    /// <summary>
    /// Helper class for creating results
    /// </summary>
    public static class Result
    {
        private sealed class SuccessResult<TSuccess> : Result<TSuccess>
        {
            private readonly TSuccess _value;

            public SuccessResult(TSuccess value)
            {
                _value = value;
            }

            #region implemented abstract members of Either

            public override bool IsLeft => true;

            public override TResult Match<TResult>(Func<TSuccess, TResult> onLeft, Func<ExceptionWithContext?, TResult> onRight) =>
                onLeft(_value);

            public override TResult Case<TResult>(Func<TSuccess, TResult> onLeft, Func<ExceptionWithContext, TResult> onRight) =>
                onLeft(_value);

            public override void MatchLeft(Action<TSuccess> onLeft) =>
                onLeft(_value);

            protected override bool IsLeftValueEqual(Either<TSuccess, ExceptionWithContext>? other) =>
                other is not null
                && other.GetType() == GetType()
                && Equals(_value, ((SuccessResult<TSuccess>)other)._value);

            protected override bool IsRightValueEqual(Either<TSuccess, ExceptionWithContext>? other) =>
                other is not null
                && other.GetType() == GetType();

            #endregion

            public override string ToString() =>
                _value is null
                    ? string.Empty
                    : _value.ToString() ?? string.Empty;
        }

        private sealed class FailureResult<TSuccess> : Result<TSuccess>
        {
            private readonly ExceptionWithContext? _failure;

            public FailureResult(ExceptionWithContext? failure)
            {
                _failure = failure;
            }

            #region implemented abstract members of Either

            public override bool IsLeft => false;

            public override TResult Case<TResult>(Func<TSuccess?, TResult> onLeft, Func<ExceptionWithContext?, TResult> onRight) =>
                onRight(_failure);

            public override TResult Match<TResult>(Func<TSuccess, TResult> onLeft, Func<ExceptionWithContext?, TResult> onRight) =>
                onRight(_failure);

            public override void MatchRight(Action<ExceptionWithContext?> onRight) =>
                onRight(_failure);

            protected override bool IsLeftValueEqual(Either<TSuccess, ExceptionWithContext>? other) =>
                other is not null
                && other.GetType() == GetType();

            protected override bool IsRightValueEqual(Either<TSuccess, ExceptionWithContext>? other) =>
                other is not null
                && other.GetType() == GetType()
                && Equals(_failure, ((FailureResult<TSuccess>)other)._failure);

            #endregion

            public override string ToString() => _failure?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Creates a Success Result value.
        /// </summary>
        /// <param name="value">The success value.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Success Value</returns>
        public static Result<TSuccess> Success<TSuccess>(TSuccess value) =>
            new SuccessResult<TSuccess>(value);

        /// <summary>
        /// Creates a Failure Result value.
        /// </summary>
        /// <param name="failure">The failure exception.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Failure Value</returns>
        public static Result<TSuccess> Failure<TSuccess>(ExceptionWithContext? failure) =>
            new FailureResult<TSuccess>(failure);

        /// <summary>
        /// Creates a Failure Result value.
        /// </summary>
        /// <param name="failureMessage">The failure message.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Failure Value</returns>
        public static Result<TSuccess> Failure<TSuccess>(string failureMessage) =>
            Failure<TSuccess>(new ExceptionWithContext(failureMessage, null));

        /// <summary>
        /// Creates a Failure Result value.
        /// </summary>
        /// <param name="failureMessage">The failure message.</param>
        /// <param name="context">The context values.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Failure Value</returns>
        public static Result<TSuccess> Failure<TSuccess>(string failureMessage, Dictionary<string, object> context) =>
            Failure<TSuccess>(new ExceptionWithContext(failureMessage, context));

        /// <summary>
        /// Creates a Failure Result value.
        /// </summary>
        /// <param name="failureMessage">The failure message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="context">The context values.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Failure Value</returns>
        public static Result<TSuccess> Failure<TSuccess>(string failureMessage, Exception innerException, Dictionary<string, object> context) =>
            Failure<TSuccess>(new ExceptionWithContext(failureMessage, innerException, context));
    }
}
