// Option.cs
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
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Pagansoft.Functional
{
    /// <summary>Base class for the Option Monad</summary>
    /// <typeparam name="T">The type of the contained value</typeparam>
    public abstract class Option<T> : Option, IEquatable<Option>
    {
        /// <summary>Gets the value.</summary>
        public abstract T Value { get; }

        /// <summary>
        /// Determines whether the specified <see cref="Option{T}"/> is equal to the current <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="Option{T}"/> to compare with the current <see cref="Option{T}"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Option{T}"/> is equal to the current
        /// <see cref="Option{T}"/>; otherwise, <c>false</c>.</returns>
        public abstract bool Equals(Option? other);

        /// <summary>
        /// Returns a function result, depending if the Value is a Some or a None
        /// </summary>
        /// <param name="ifSome">function to call if the value is a Some</param>
        /// <param name="ifNone">function to call if the value is a None</param>
        /// <typeparam name="TResult">Type of the result value</typeparam>
        /// <returns>The result value of the called function</returns>
        public abstract TResult Match<TResult>(Func<T, TResult> ifSome, Func<TResult> ifNone);

        /// <summary>
        /// Returns the value or its default
        /// </summary>
        /// <returns>The value if its Some, or its default if its None</returns>
        public T? ReturnValueOrDefault() =>
            //// ReSharper disable once RedundantTypeSpecificationInDefaultExpression
            ReturnValueOr(() => default(T));

        /// <summary>
        /// Returns the value or the default value (if none).
        /// </summary>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>The value or the default value (if none).</returns>
        public abstract T? ReturnValueOr(T? defaultValue);

        /// <summary>
        /// Returns the value or the default value (if none).
        /// Lazy evaluated
        /// </summary>
        /// <param name="defaultValue">A function, which returns the default value.</param>
        /// <returns>The value or the default value (if none).</returns>
        public abstract T? ReturnValueOr(Func<T?> defaultValue);

        /// <summary>
        /// Determines whether the specified <paramref name="otherValue"/> is equal to the internal value
        /// </summary>
        /// <param name="otherValue">The object to compare with the current internal value</param>
        /// <returns>
        /// <c>true</c> if the instance is a Some and the internal value equals the <paramref name="otherValue"/>,
        /// otherwise <c>false</c>
        /// </returns>
        [DebuggerStepThrough, Pure]
        public abstract bool ValueEquals(T? otherValue);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Option{T}"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Option{T}"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj) =>
            Equals(obj as Option);

        /// <summary>
        /// Serves as a hash function for a <see cref="Option{T}"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => 0;

        /// <summary>Performs an implicit conversion from <see typeparamref="T"/> to <see cref="Option{T}"/>.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Option<T>(T? value) =>
            value is null
                ? None<T>()
                : Some(value);
    }

    /// <summary>Factory class for fluent construction of an option monad</summary>
    public abstract class Option
    {
        /// <summary>Gets a value indicating whether this instance represents some value.</summary>
        /// <value><c>true</c> if this instance represents some value; otherwise, <c>false</c>.</value>
        public abstract bool HasValue { get; }

        /// <summary>Gets a value indicating whether this instance represents no value.</summary>
        /// <value><c>true</c> if this instance represents no value; otherwise, <c>false</c>.</value>
        public bool HasNoValue => !HasValue;

        private sealed class OptionSome<TResult> : Option<TResult>
        {
            public OptionSome(TResult value) => _value = value;

            public override TResult Value => _value;

            private readonly TResult _value;

            public override bool HasValue => true;

            public override TResult2 Match<TResult2>(Func<TResult, TResult2> ifSome, Func<TResult2> ifNone) =>
                ifSome(_value);

            public override TResult ReturnValueOr(TResult? defaultValue) => _value;

            public override TResult ReturnValueOr(Func<TResult?> defaultValue) => _value;

            [DebuggerStepThrough, Pure]
            public override bool ValueEquals(TResult? otherValue) =>
                Equals(_value, otherValue);

            public override bool Equals(Option? other)
            {
                if (other is null || other.HasValue != HasValue) { return false; }

                return ReferenceEquals(this, other)
                       || Equals(_value, other.GetValue<TResult>());
            }

            public override int GetHashCode() =>
                _value is not null
                    ? _value.GetHashCode()
                    : 0;

            public override string ToString() =>
                _value is null
                    ? string.Empty
                    : _value.ToString() ?? string.Empty;

            #region Overrides of Option

            protected override object? GetValue<T>()
            {
                if (typeof(T) == typeof(TResult))
                {
                    return _value;
                }

                if (typeof(TResult).IsSubclassOf(typeof(T)))
                {
                    return _value;
                }

                if (typeof(T).IsSubclassOf(typeof(TResult)))
                {
                    return _value;
                }

                return null;
            }

            #endregion
        }

        private sealed class OptionNone<TResult> : Option<TResult>
        {
            public override bool HasValue => false;

            public override TResult Value =>
                throw new ArgumentException("A None Option has no value!");

            public override TResult2 Match<TResult2>(Func<TResult, TResult2> ifSome, Func<TResult2> ifNone) =>
                ifNone();

            public override TResult? ReturnValueOr(TResult? defaultValue) =>
                defaultValue;

            public override TResult? ReturnValueOr(Func<TResult?> defaultValue) =>
                defaultValue();

            [DebuggerStepThrough, Pure]
            public override bool ValueEquals(TResult? otherValue) =>
                false;

            public override bool Equals(Option? other) =>
                other is not null
                && HasValue == other.HasValue;

            public override int GetHashCode() => 0;

            public override string ToString() => string.Empty;
        }

        /// <summary>Creates an option which represents the specified value.</summary>
        /// <param name="value">The value, this option represent.</param>
        /// <typeparam name="T">The type of the represented value.</typeparam>
        /// <returns>An option with a value</returns>
        public static Option<T> Some<T>(T value) => new OptionSome<T>(value);

        /// <summary>Creates an option which represents no value of a given type.</summary>
        /// <typeparam name="T">The type of the represented value.</typeparam>
        /// <returns>An option without a value</returns>
        public static Option<T> None<T>() => new OptionNone<T>();

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Option{T}"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Option{T}"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj) => true;

        /// <summary>
        /// Serves as a hash function for a <see cref="Option{T}"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => 0;

        /// <summary>Implements the operator ==.</summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Option left, Option right) =>
            Equals(left, right);

        /// <summary>Implements the operator !=.</summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Option left, Option right) =>
            !Equals(left, right);

        /// <summary>Gets the value with the given type.</summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <returns>The value or null, if the type does not match</returns>
        protected virtual object? GetValue<T>() => null;
    }
}
