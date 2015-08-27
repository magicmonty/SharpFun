using System;

namespace Pagansoft.Functional
{
    /// <summary>Base class for the Option Monad</summary>
    public abstract class Option<T> : IEquatable<Option<T>>
    {
        /// <summary>Gets a value indicating whether this instance represents some value.</summary>
        /// <value><c>true</c> if this instance represents some value; otherwise, <c>false</c>.</value>
        public abstract bool IsSome { get; }

        /// <summary>Gets a value indicating whether this instance represents no value.</summary>
        /// <value><c>true</c> if this instance represents no value; otherwise, <c>false</c>.</value>
        public bool IsNone { get { return !IsSome; } }

        /// <summary>Gets the value.</summary>
        public abstract T Value { get; }

        /// <summary>
        /// Determines whether the specified <see cref="Pagansoft.Functional.Option`1[[`0]]"/> is equal to the current <see cref="Pagansoft.Functional.Option`1"/>.
        /// </summary>
        /// <param name="other">The <see cref="Pagansoft.Functional.Option`1[[`0]]"/> to compare with the current <see cref="Pagansoft.Functional.Option`1"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Pagansoft.Functional.Option`1[[`0]]"/> is equal to the current
        /// <see cref="Pagansoft.Functional.Option`1"/>; otherwise, <c>false</c>.</returns>
        public abstract bool Equals(Option<T> other);

        /// <summary>
        /// Returns the value or.
        /// </summary>
        /// <returns>The value or.</returns>
        /// <param name="defaultValue">Default value.</param>
        public abstract T ReturnValueOr(T defaultValue);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Pagansoft.Functional.Option`1"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Pagansoft.Functional.Option`1"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Pagansoft.Functional.Option`1"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Option<T>);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Pagansoft.Functional.Option`1"/> object.
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
            return ReferenceEquals(null, value)
                ? Option.None<T>()
                : Option.Some(value);
        }
    }

    /// <summary>Factory class for fluent construction of an option monad</summary>
    public static class Option
    {
        /// <summary>Represents some result</summary>
        private sealed class Some<TResult> : Option<TResult>
        {
            public Some(TResult value)
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

        private sealed class None<TResult> : Option<TResult>
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
            return new Some<T>(value);
        }

        /// <summary>Creates an option which represents no value of a given type.</summary>
        /// <typeparam name="T">The type of the represented value.</typeparam>
        public static Option<T> None<T>()
        {
            return new None<T>();
        }
    }

}

