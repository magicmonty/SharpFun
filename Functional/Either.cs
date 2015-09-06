//
// Either.cs
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
    /// Represents a type, which can have either one of two given types
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// // Prints: "Success: 42"
    /// var success = Either.Left<int, Exception>(42);
    /// success.Match(
    ///   r => Console.WriteLine("Success: {0}", r),
    ///   ex => Console.WriteLine(ex.Message));
    /// </code>
    ///
    /// <code>
    /// // Prints: "An error happened!"
    /// var error = Either.Right<int, Exception>(new System.Exception("An error happened!"));
    /// success.Match(
    ///   r => Console.WriteLine("Success: {0}", r),
    ///   ex => Console.WriteLine(ex.Message));
    /// ]]></code>
    /// </example>
    public abstract class Either<TLeft, TRight>
    {
        /// <summary>
        /// Returns the value of the specified function based on the internal state.
        /// </summary>
        /// <param name="onLeft">This function is called, if the internal value is a <typeparamref name="TLeft" />.</param>
        /// <param name="onRight">This function is called, if the internal value is a <typeparamref name="TRight" />.</param>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public abstract TResult Case<TResult>(Func<TLeft, TResult> onLeft, Func<TRight, TResult> onRight);

        /// <summary>
        /// Executes the given Action, depending of the internal type.
        /// </summary>
        /// <param name="onLeft">This action will be executed, if this instance is a <typeparamref name="TLeft" /></param>
        /// <param name="onRight">This action will be executed, if this instance is a <typeparamref name="TRight" />.</param>
        public abstract void Match(Action<TLeft> onLeft, Action<TRight> onRight);

        /// <summary>
        /// Executes the given Action, if this instance is a <typeparamref name="TLeft" />.
        /// </summary>
        /// <param name="onLeft">This action will be executed, if this instance is a <typeparamref name="TLeft" /></param>
        public abstract void MatchLeft(Action<TLeft> onLeft);

        /// <summary>
        /// Executes the given Action, if this instance is a <typeparamref name="TRight" />.
        /// </summary>
        /// <param name="onRight">This action will be executed, if this instance is a <typeparamref name="TRight" /></param>
        public abstract void MatchRight(Action<TRight> onRight);

        #region Equality members

        /// <inheritdoc/>
        public bool Equals(Either<TLeft, TRight> other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return ReferenceEquals(this, other) 
                || (IsLeftValueEqual(other) && IsRightValueEqual(other));
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as Either<TLeft, TRight>);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return 0;
            }
        }

        /// <param name="leftValue">Left value.</param>
        /// <param name="rightValue">Right value.</param>
        public static bool operator ==(Either<TLeft, TRight> leftValue, Either<TLeft, TRight> rightValue)
        {
            return Equals(leftValue, rightValue);
        }

        /// <param name="leftValue">Left value.</param>
        /// <param name="rightValue">Right value.</param>
        public static bool operator !=(Either<TLeft, TRight> leftValue, Either<TLeft, TRight> rightValue)
        {
            return !(leftValue == rightValue);
        }

        #endregion

        protected abstract bool IsLeftValueEqual(Either<TLeft, TRight> other);

        protected abstract bool IsRightValueEqual(Either<TLeft, TRight> other);
    }

    /// <summary>
    /// Helper class for creating Either values
    /// </summary>
    public static class Either
    {
        private sealed class EitherLeft<TLeft, TRight> : Either<TLeft, TRight>
        {
            private readonly TLeft _value;
                        
            public EitherLeft(TLeft value)
            {
                _value = value;
            }

            #region Either implementation

            public override TResult Case<TResult>(Func<TLeft, TResult> onLeft, Func<TRight, TResult> onRight)
            {
                return onLeft(_value);
            }

            public override void Match(Action<TLeft> onLeft, Action<TRight> onRight)
            {
                onLeft(_value);
            }

            public override void MatchLeft(Action<TLeft> onLeft)
            {
                onLeft(_value);
            }

            public override void MatchRight(Action<TRight> onRight)
            {
                /* intentionally left blank */
            }

            protected override bool IsLeftValueEqual(Either<TLeft, TRight> other)
            {
                if (ReferenceEquals(null, other))
                    return false;

                if (other.GetType() != GetType())
                    return false;
                
                return Equals(_value, ((EitherLeft<TLeft, TRight>)other)._value);
            }

            protected override bool IsRightValueEqual(Either<TLeft, TRight> other)
            {
                return !ReferenceEquals(null, other) && (other.GetType() == GetType());
            }

            #endregion

            #region Equality members

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                unchecked
                {
                    return _value != null ? _value.GetHashCode() : 0;
                }
            }
                
            #endregion

            public override string ToString()
            {
                return _value == null ? string.Empty : _value.ToString();
            }
        }

        private sealed class EitherRight<TLeft, TRight> : Either<TLeft, TRight>
        {
            private readonly TRight _value;

            public EitherRight(TRight value)
            {
                _value = value;
            }

            #region IEither implementation

            public override TResult Case<TResult>(Func<TLeft, TResult> onLeft, Func<TRight, TResult> onRight)
            {
                return onRight(_value);
            }

            public override void Match(Action<TLeft> onLeft, Action<TRight> onRight)
            {
                onRight(_value);
            }

            public override void MatchLeft(Action<TLeft> onLeft)
            {
                /* intentionally left blank */
            }

            public override void MatchRight(Action<TRight> onRight)
            {
                onRight(_value);
            }
                
            protected override bool IsLeftValueEqual(Either<TLeft, TRight> other)
            {
                return !ReferenceEquals(null, other) && (other.GetType() == GetType());
            }

            protected override bool IsRightValueEqual(Either<TLeft, TRight> other)
            {
                if (ReferenceEquals(null, other))
                    return false;

                if (other.GetType() != GetType())
                    return false;

                return Equals(_value, ((EitherRight<TLeft, TRight>)other)._value);
            }

            #endregion

            #region Equality members

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                unchecked
                {
                    return _value != null ? _value.GetHashCode() : 0;
                }
            }

            #endregion

            public override string ToString()
            {
                return _value == null ? string.Empty : _value.ToString();
            }
        }

        /// <summary>
        /// Creates a left value
        /// </summary>
        /// <param name="value">Value.</param>
        /// <typeparam name="TLeft">The 1st type parameter.</typeparam>
        /// <typeparam name="TRight">The 2nd type parameter.</typeparam>
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value) 
        {
            return new EitherLeft<TLeft, TRight>(value);
        }

        /// <summary>
        /// Creates a right value
        /// </summary>
        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value) 
        {
            return new EitherRight<TLeft, TRight>(value);
        }
    }
}

