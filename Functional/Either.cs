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
    /// <summary>Represents a type, which can have either one of two given types</summary>
    /// <example>
    /// <para><code><![CDATA[
    /// // Prints: "Success: 42"
    /// var success = Either.Left<int, Exception>(42);
    /// success.Match(
    ///   r => Console.WriteLine("Success: {0}", r),
    ///   ex => Console.WriteLine(ex.Message));
    /// </code></para>
    /// <para><code>
    /// // Prints: "An error happened!"
    /// var error = Either.Right<int, Exception>(new System.Exception("An error happened!"));
    /// success.Match(
    ///   r => Console.WriteLine("Success: {0}", r),
    ///   ex => Console.WriteLine(ex.Message));
    /// ]]></code></para>
    /// </example>
    /// <typeparam name="TLeft">The type of the left value</typeparam>
    /// <typeparam name="TRight">The type of the right value</typeparam>
    public abstract class Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
    {
        /// <summary>
        /// Convenience helper, that indicates, that the value is a Left value
        /// </summary>
        public abstract bool IsLeft { get; }

        /// <summary>
        /// Convenience helper, that indicates, that the value is a Right value
        /// </summary>
        public bool IsRight => !IsLeft;

        /// <summary>
        /// Returns the value of the specified function based on the internal state.
        /// </summary>
        /// <param name="onLeft">This function is called, if the internal value is a <typeparamref name="TLeft" />.</param>
        /// <param name="onRight">This function is called, if the internal value is a <typeparamref name="TRight" />.</param>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <returns>A value of type <typeparamref name="TResult"/></returns>
        public abstract TResult Case<TResult>(Func<TLeft?, TResult> onLeft, Func<TRight?, TResult> onRight);

        /// <summary>
        /// Executes the given Action, depending of the internal type.
        /// </summary>
        /// <param name="onLeft">This action will be executed, if this instance is a <typeparamref name="TLeft" /></param>
        /// <param name="onRight">This action will be executed, if this instance is a <typeparamref name="TRight" />.</param>
        public abstract void Match(Action<TLeft?> onLeft, Action<TRight?> onRight);

        /// <summary>
        /// Executes a given function and returns it result value, depending of the internal type
        /// </summary>
        /// <param name="onLeft">This function will be executed, if this instance is a <typeparamref name="TLeft" /></param>
        /// <param name="onRight">This function will be executed, if this instance is a <typeparamref name="TLeft" />.</param>
        /// <typeparam name="TResult">The type of the function result</typeparam>
        /// <returns>The result of the chosen function</returns>
        public abstract TResult Match<TResult>(Func<TLeft, TResult> onLeft, Func<TRight?, TResult> onRight);

        /// <summary>
        /// Executes the given Action, if this instance is a <typeparamref name="TLeft" />.
        /// </summary>
        /// <param name="onLeft">This action will be executed, if this instance is a <typeparamref name="TLeft" /></param>
        public abstract void MatchLeft(Action<TLeft?> onLeft);

        /// <summary>
        /// Executes the given Action, if this instance is a <typeparamref name="TRight" />.
        /// </summary>
        /// <param name="onRight">This action will be executed, if this instance is a <typeparamref name="TRight" /></param>
        public abstract void MatchRight(Action<TRight?> onRight);

        #region Equality members

        /// <inheritdoc />
        public bool Equals(Either<TLeft, TRight>? other) =>
            other is not null &&
            (ReferenceEquals(this, other)
             || (IsLeftValueEqual(other)
                 && IsRightValueEqual(other)));

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            Equals(obj as Either<TLeft, TRight>);

        /// <inheritdoc/>
        public override int GetHashCode() => 0;

        /// <summary>Implements the operator ==.</summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="rightValue">The right value.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Either<TLeft, TRight>? leftValue, Either<TLeft, TRight>? rightValue) =>
            Equals(leftValue, rightValue);

        /// <summary>Implements the operator !=.</summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="rightValue">The right value.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Either<TLeft, TRight>? leftValue, Either<TLeft, TRight>? rightValue) =>
            !Equals(leftValue, rightValue);

        #endregion

        /// <summary>
        /// Determines whether the left value of this instance is equal to the specified other value.
        /// </summary>
        /// <returns><c>true</c> if the left value of this instance is equal to the specified other value; otherwise, <c>false</c>.</returns>
        /// <param name="other">The other value.</param>
        protected abstract bool IsLeftValueEqual(Either<TLeft, TRight>? other);

        /// <summary>
        /// Determines whether the right value of this instance is equal to the specified other value.
        /// </summary>
        /// <returns><c>true</c> if the right value of this instance is equal to the specified other value; otherwise, <c>false</c>.</returns>
        /// <param name="other">The other value.</param>
        protected abstract bool IsRightValueEqual(Either<TLeft, TRight>? other);
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

            public override bool IsLeft => true;

            public override TResult Case<TResult>(Func<TLeft, TResult> onLeft, Func<TRight, TResult> onRight) =>
                onLeft(_value);

            public override void Match(Action<TLeft> onLeft, Action<TRight> onRight) =>
                onLeft(_value);

            public override TResult Match<TResult>(Func<TLeft, TResult> onLeft, Func<TRight?, TResult> onRight) =>
                onLeft(_value);

            public override void MatchLeft(Action<TLeft> onLeft) =>
                onLeft(_value);

            public override void MatchRight(Action<TRight> onRight)
            {
                /* intentionally left blank */
            }

            protected override bool IsLeftValueEqual(Either<TLeft, TRight>? other) =>
                other is not null
                && other.GetType() == GetType()
                && Equals(_value, ((EitherLeft<TLeft, TRight>)other)._value);

            protected override bool IsRightValueEqual(Either<TLeft, TRight>? other) =>
                other is not null && other.GetType() == GetType();

            #endregion

            #region Equality members

            /// <inheritdoc/>
            public override int GetHashCode() =>
                _value != null
                    ? _value.GetHashCode()
                    : 0;

            #endregion

            public override string ToString() =>
                _value == null
                    ? string.Empty
                    : _value.ToString() ?? string.Empty;
        }

        private sealed class EitherRight<TLeft, TRight> : Either<TLeft, TRight>
        {
            private readonly TRight _value;

            public EitherRight(TRight value)
            {
                _value = value;
            }

            #region Either implementation

            public override bool IsLeft => false;

            public override TResult Case<TResult>(Func<TLeft, TResult> onLeft, Func<TRight, TResult> onRight) =>
                onRight(_value);

            public override void Match(Action<TLeft> onLeft, Action<TRight> onRight) =>
                onRight(_value);

            public override TResult Match<TResult>(Func<TLeft, TResult> onLeft, Func<TRight?, TResult> onRight) =>
                onRight(_value);

            public override void MatchLeft(Action<TLeft> onLeft)
            {
                /* intentionally left blank */
            }

            public override void MatchRight(Action<TRight> onRight) =>
                onRight(_value);

            protected override bool IsLeftValueEqual(Either<TLeft, TRight>? other) =>
                other is not null && other.GetType() == GetType();

            protected override bool IsRightValueEqual(Either<TLeft, TRight>? other) =>
                other is not null
                && other.GetType() == GetType()
                && Equals(_value, ((EitherRight<TLeft, TRight>)other)._value);

            #endregion

            #region Equality members

            /// <inheritdoc/>
            public override int GetHashCode() =>
                _value != null
                    ? _value.GetHashCode()
                    : 0;

            #endregion

            public override string ToString() =>
                _value == null
                    ? string.Empty
                    : _value.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Creates a left value
        /// </summary>
        /// <param name="value">The left value.</param>
        /// <typeparam name="TLeft">The type of the left value.</typeparam>
        /// <typeparam name="TRight">The type of the right value.</typeparam>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> containing a left value</returns>
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value) =>
            new EitherLeft<TLeft, TRight>(value);

        /// <summary>
        /// Creates a right value
        /// </summary>
        /// <param name="value">The right value.</param>
        /// <typeparam name="TLeft">The type of the left value.</typeparam>
        /// <typeparam name="TRight">The type of the right value.</typeparam>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> containing a right value</returns>
        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value) =>
            new EitherRight<TLeft, TRight>(value);
    }
}
