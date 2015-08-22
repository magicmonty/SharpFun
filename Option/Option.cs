using System;

namespace Pagansoft.Functional
{
    public static class Option
    {
        public static Option<T> Some<T>(T value)
        {
            return new Option<T>.Some<T>(value);
        }

        public static Option<T> None<T>()
        {
            return new Option<T>.None<T>();
        }
    }

    public abstract class Option<T>
    {
        public abstract bool IsSome { get; }

        public bool IsNone { get { return !IsSome; } }

        public abstract T Value { get; }

        internal sealed class Some<V> : Option<V>
        {
            public Some(V value)
            {
                _value = value;
            }

            public override V Value { get { return _value; } }
            private readonly V _value;

            public override bool IsSome { get { return true; } }
        }

        internal sealed class None<V> : Option<V>
        {
            public override bool IsSome { get { return false; } }

            public override V Value
            {
                get {
                    throw new ArgumentException("A None Option has no value!");
                }
            }
        }
    }

}

