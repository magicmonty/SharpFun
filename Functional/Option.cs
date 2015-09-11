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
#if CONTRACTS
using System.Diagnostics.Contracts;
#endif

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
        public abstract bool Equals(Option other);

        /// <summary>
        /// Returns the value or.
        /// </summary>
        /// <returns>The value or.</returns>
        /// <param name="defaultValue">Default value.</param>
        public abstract T ReturnValueOr(T defaultValue);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Option{T}"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Option{T}"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Option);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Option{T}"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>Performs an implicit conversion from <see typeparamref="T"/> to <see cref="Option{T}"/>.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Option<T>(T value)
        {
#if CONTRACTS
            Contract.Ensures(Contract.Result<Option<T>>() != null);
#endif
            return ReferenceEquals(null, value)
                ? None<T>()
                : Some(value);
        }
    }

    /// <summary>Factory class for fluent construction of an option monad</summary>
    public abstract class Option
    {
        /// <summary>Gets a value indicating whether this instance represents some value.</summary>
        /// <value><c>true</c> if this instance represents some value; otherwise, <c>false</c>.</value>
        public abstract bool HasValue { get; }

        /// <summary>Gets a value indicating whether this instance represents no value.</summary>
        /// <value><c>true</c> if this instance represents no value; otherwise, <c>false</c>.</value>
        public bool HasNoValue { get { return !HasValue; } }

        private sealed class OptionSome<TResult> : Option<TResult>
        {
            public OptionSome(TResult value)
            {
                _value = value;
            }

            public override TResult Value { get { return _value; } }

            private readonly TResult _value;

            public override bool HasValue { get { return true; } }

            public override TResult ReturnValueOr(TResult defaultValue)
            {
                return _value;
            }

            public override bool Equals(Option other)
            {
                if (ReferenceEquals(null, other) || other.HasValue != HasValue)
                    return false;

                return ReferenceEquals(this, other) || Equals(_value, other.GetValue<TResult>());
            }

            public override int GetHashCode()
            {
                return _value != null ? _value.GetHashCode() : 0;
            }

            public override string ToString()
            {
                return _value == null ? string.Empty : _value.ToString();
            }

            #region Overrides of Option

            protected override object GetValue<T>()
            {
                if (typeof(T) == typeof(TResult))
                    return _value;

#if !PORTABLE
                if (typeof(TResult).IsSubclassOf(typeof(T)))
                    return _value;

                if (typeof(T).IsSubclassOf(typeof(TResult)))
                    return _value;
#endif
                return null;
            }

            #endregion
        }

        private sealed class OptionNone<TResult> : Option<TResult>
        {
            public override bool HasValue { get { return false; } }

            public override TResult Value
            {
                get
                {
                    throw new ArgumentException("A None Option has no value!");
                }
            }

            public override TResult ReturnValueOr(TResult defaultValue)
            {
                return defaultValue;
            }

            public override bool Equals(Option other)
            {
                return !ReferenceEquals(null, other)
                && HasValue == other.HasValue;
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public override string ToString()
            {
                return string.Empty;
            }
        }

        /// <summary>Creates an option which represents the specified value.</summary>
        /// <param name="value">The value, this option represent.</param>
        /// <typeparam name="T">The type of the represented value.</typeparam>
        /// <returns>An option with a value</returns>
        public static Option<T> Some<T>(T value)
        {
#if CONTRACTS
            Contract.Ensures(Contract.Result<Option<T>>() != null);
#endif
            return new OptionSome<T>(value);
        }

        /// <summary>Creates an option which represents no value of a given type.</summary>
        /// <typeparam name="T">The type of the represented value.</typeparam>
        /// <returns>An option without a value</returns>
        public static Option<T> None<T>()
        {
#if CONTRACTS
            Contract.Ensures(Contract.Result<Option<T>>() != null);
#endif
            return new OptionNone<T>();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Option{T}"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Option{T}"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return true;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Option{T}"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Option left, Option right)
        {
            return Equals(left, right);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Option left, Option right)
        {
            return !(left == right);
        }

        /// <summary>Gets the value with the given type.</summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <returns>The value or null, if the type does not match</returns>
        protected virtual object GetValue<T>()
        {
            return null;
        }
    }
}
