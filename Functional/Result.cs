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
        /// <inheritdoc />
        public override void Match(Action<TSuccess> onLeft, Action<ExceptionWithContext> onRight)
        {
            MatchSuccess(onLeft);
            MatchFailure(onRight);
        }

        /// <inheritdoc />
        public override void MatchLeft(Action<TSuccess> onLeft)
        {
            /* intentionally left blank */
        }

        /// <inheritdoc />
        public override void MatchRight(Action<ExceptionWithContext> onRight)
        {
            /* intentionally left blank */
        }

        /// <summary>
        /// Executes the <paramref name="onSuccess"/> method, if the instance is a Success value.
        /// </summary>
        /// <param name="onSuccess">The action, which is called on success.</param>
        public void MatchSuccess(Action<TSuccess> onSuccess)
        {
            MatchLeft(onSuccess);
        }

        /// <summary>
        /// Executes the <paramref name="onFailure"/> method, if the instance is a Failure value.
        /// </summary>
        /// <param name="onFailure">The action, which is called on failure.</param>
        public void MatchFailure(Action<ExceptionWithContext> onFailure)
        {
            MatchRight(onFailure);
        }
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

            public override TResult Case<TResult>(Func<TSuccess, TResult> onLeft, Func<ExceptionWithContext, TResult> onRight)
            {
                return onLeft(_value);
            }

            public override void MatchLeft(Action<TSuccess> onLeft)
            {
                onLeft(_value);
            }

            protected override bool IsLeftValueEqual(Either<TSuccess, ExceptionWithContext> other)
            {
                if (ReferenceEquals(null, other))
                    return false;

                if (other.GetType() != GetType())
                    return false;

                return Equals(_value, ((SuccessResult<TSuccess>)other)._value);
            }

            protected override bool IsRightValueEqual(Either<TSuccess, ExceptionWithContext> other)
            {
                return !ReferenceEquals(null, other) && (other.GetType() == GetType());
            }

            #endregion

            public override string ToString()
            {
                return _value == null ? string.Empty : _value.ToString();
            }
        }

        private sealed class FailureResult<TSuccess> : Result<TSuccess>
        {
            private readonly ExceptionWithContext _failure;

            public FailureResult(ExceptionWithContext failure)
            {
                _failure = failure;
            }

            #region implemented abstract members of Either

            public override TResult Case<TResult>(Func<TSuccess, TResult> onLeft, Func<ExceptionWithContext, TResult> onRight)
            {
                return onRight(_failure);
            }

            public override void MatchRight(Action<ExceptionWithContext> onRight)
            {
                onRight(_failure);
            }

            protected override bool IsLeftValueEqual(Either<TSuccess, ExceptionWithContext> other)
            {
                return !ReferenceEquals(null, other) && (other.GetType() == GetType());
            }

            protected override bool IsRightValueEqual(Either<TSuccess, ExceptionWithContext> other)
            {
                if (ReferenceEquals(null, other))
                    return false;

                if (other.GetType() != GetType())
                    return false;

                return Equals(_failure, ((FailureResult<TSuccess>)other)._failure);
            }

            #endregion

            public override string ToString()
            {
                return _failure == null ? string.Empty : _failure.ToString();
            }
        }

        /// <summary>
        /// Creates a Success Result value.
        /// </summary>
        /// <param name="value">The success value.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Success Value</returns>
        public static Result<TSuccess> Success<TSuccess>(TSuccess value)
        {
            return new SuccessResult<TSuccess>(value);
        }

        /// <summary>
        /// Creates a Failure Result value.
        /// </summary>
        /// <param name="failure">The failure exception.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Failure Value</returns>
        public static Result<TSuccess> Failure<TSuccess>(ExceptionWithContext failure)
        {
            return new FailureResult<TSuccess>(failure);
        }

        /// <summary>
        /// Creates a Failure Result value.
        /// </summary>
        /// <param name="failureMessage">The failure message.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Failure Value</returns>
        public static Result<TSuccess> Failure<TSuccess>(string failureMessage)
        {
            return Failure<TSuccess>(new ExceptionWithContext(failureMessage, null));
        }

        /// <summary>
        /// Creates a Failure Result value.
        /// </summary>
        /// <param name="failureMessage">The failure message.</param>
        /// <param name="context">The context values.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Failure Value</returns>
        public static Result<TSuccess> Failure<TSuccess>(string failureMessage, Dictionary<string, object> context)
        {
            return Failure<TSuccess>(new ExceptionWithContext(failureMessage, context));
        }

        /// <summary>
        /// Creates a Failure Result value.
        /// </summary>
        /// <param name="failureMessage">The failure message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="context">The context values.</param>
        /// <typeparam name="TSuccess">The type of the success value.</typeparam>
        /// <returns>A new Failure Value</returns>
        public static Result<TSuccess> Failure<TSuccess>(string failureMessage, Exception innerException, Dictionary<string, object> context)
        {
            return Failure<TSuccess>(new ExceptionWithContext(failureMessage, innerException, context));
        }
    }
}