//
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
using System.Diagnostics.Contracts;

namespace Pagansoft.Functional
{
    /// <summary>Base class for the Option Monad</summary>
    public abstract class Option<T> : IEquatable<Option<T>>
    {
        /// <summary>Gets a value indicating whether this instance represents some value.</summary>
        /// <value><c>true</c> if this instance represents some value; otherwise, <c>false</c>.</value>
        public abstract bool IsSome { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has a value
        /// </summary>
        /// <value><c>true</c> if this instance has value; otherwise, <c>false</c>.</value>
        public bool HasValue { get { return IsSome; } }

        /// <summary>Gets a value indicating whether this instance represents no value.</summary>
        /// <value><c>true</c> if this instance represents no value; otherwise, <c>false</c>.</value>
        public bool IsNone { get { return !IsSome; } }

        /// <summary>
        /// Gets a value indicating whether this instance has NO value
        /// </summary>
        /// <value><c>true</c> if this instance has NO value; otherwise, <c>false</c>.</value>
        public bool HasNoValue { get { return IsNone; } }

        /// <summary>Gets the value.</summary>
        public abstract T Value { get; }

        /// <summary>
        /// Determines whether the specified <see cref="Option{T}"/> is equal to the current <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="Option{T}"/> to compare with the current <see cref="Option{T}"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Option{T}"/> is equal to the current
        /// <see cref="Option{T}"/>; otherwise, <c>false</c>.</returns>
        public abstract bool Equals(Option<T> other);

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
            return Equals(obj as Option<T>);
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

        /// <param name="value">Value.</param>
        public static implicit operator Option<T>(T value)
        {
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return ReferenceEquals(null, value)
                ? Option.None<T>()
                : Option.Some(value);
        }
    }

    /// <summary>Factory class for fluent construction of an option monad</summary>
    public static class Option
    {
        /// <summary>Represents some result</summary>
        private sealed class OptionSome<TResult> : Option<TResult>
        {
            public OptionSome(TResult value)
            {
                _value = value;
            }

            public override TResult Value { get { return _value; } }

            private readonly TResult _value;

            public override bool IsSome { get { return true; } }

            public override TResult ReturnValueOr(TResult defaultValue)
            {
                return _value;
            }

            public override bool Equals(Option<TResult> other)
            {
                if (ReferenceEquals(null, other) || other.GetType() != this.GetType())
                    return false;
                
                if (ReferenceEquals(this, other))
                    return true;

                return Equals(_value, other.Value);
            }

            public override int GetHashCode()
            {
                return _value != null ? _value.GetHashCode() : 0;
            }

            public override string ToString()
            {
                return _value == null ? "null" : _value.ToString();
            }
        }

        /// <summary>Represents no result</summary>
        private sealed class OptionNone<TResult> : Option<TResult>
        {
            public override bool IsSome { get { return false; } }

            public override TResult Value
            {
                get {
                    throw new ArgumentException("A None Option has no value!");
                }
            }

            public override TResult ReturnValueOr(TResult defaultValue)
            {
                return defaultValue;
            }

            public override bool Equals(Option<TResult> other)
            {
                return !ReferenceEquals(null, other)
                && other.GetType() == this.GetType();
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public override string ToString()
            {
                return "None";
            }
        }

        /// <summary>Creates an option which represents the specified value.</summary>
        /// <param name="value">The value, this option represent.</param>
        /// <typeparam name="T">The type of the represented value.</typeparam>
        public static Option<T> Some<T>(T value)
        {
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return new OptionSome<T>(value);
        }

        /// <summary>Creates an option which represents no value of a given type.</summary>
        /// <typeparam name="T">The type of the represented value.</typeparam>
        public static Option<T> None<T>()
        {
            Contract.Ensures(Contract.Result<Option<T>>() != null);

            return new OptionNone<T>();
        }
    }

}

