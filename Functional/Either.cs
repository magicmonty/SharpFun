using System;
using System.Diagnostics.Contracts;

namespace Pagansoft.Functional
{
    /// <summary>
    /// Represents a type, which can have either one of two given types
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// // Prints: "Success: 42"
    /// var success = new Either<int, Exception>(42);
    /// success.Match(
    ///   r => Console.WriteLine("Success: {0}", r),
    ///   ex => Console.WriteLine(ex.Message));
    /// </code>
    ///
    /// <code>
    /// // Prints: "An error happened!"
    /// var error = new Either<int, Exception>(new System.Exception("An error happened!"));
    /// success.Match(
    ///   r => Console.WriteLine("Success: {0}", r),
    ///   ex => Console.WriteLine(ex.Message));
    /// ]]></code>
    /// </example>
    public class Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
    {
        private readonly bool _isLeft;

        private readonly TLeft _leftValue;
        private readonly TRight _rightValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TLeft, TRight}"/> class with a
        /// value of type <typeparamref name="TLeft" />
        /// </summary>
        /// <param name="leftValue">The value (must not be null).</param>
        public Either(TLeft leftValue)
        {
            Contract.Requires(leftValue != null);

            _rightValue = default(TRight);
            _leftValue = leftValue;
            _isLeft = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TLeft, TRight}"/> class with a
        /// value of type <typeparamref name="TRight" />
        /// </summary>
        /// <param name="rightValue">The value (must not be null).</param>
        public Either(TRight rightValue)
        {
            Contract.Requires(rightValue != null);

            _rightValue = rightValue;
            _leftValue = default(TLeft);
            _isLeft = false;
        }

        /// <summary>
        /// Executes the given Action, depending of the internal type.
        /// </summary>
        /// <param name="onLeft">This action will be executed, if this instance is a <typeparamref name="TLeft" /></param>
        /// <param name="onRight">This action will be executed, if this instance is a <typeparamref name="TRight" />.</param>
        public void Match(Action<TLeft> onLeft, Action<TRight> onRight)
        {
            if (_isLeft)
                onLeft(_leftValue);
            else
                onRight(_rightValue);
        }

        /// <summary>
        /// Executes the given Action, if the internal value is an either.
        /// </summary>
        /// <param name="onLeft">This action will be executed, if this instance is a <typeparamref name="TLeft" /></param>
        public void Match(Action<TLeft> onLeft)
        {
            Match(onLeft, _ => {});
        }

        /// <summary>
        /// Executes the given Action, if the internal value is an or.
        /// </summary>
        /// <param name="onRight">This action will be executed, if this instance is a <typeparamref name="TRight" /></param>
        public void Match(Action<TRight> onRight)
        {
            Match(_ => {}, onRight);
        }

        /// <summary>
        /// Returns the value of the specified function based on the internal state.
        /// </summary>
        /// <param name="onLeft">This function is called, if the internal value is a <typeparamref name="TLeft" />.</param>
        /// <param name="onRight">This function is called, if the internal value is a <typeparamref name="TRight" />.</param>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public TResult Case<TResult>(Func<TLeft, TResult> onLeft, Func<TRight, TResult> onRight)
        {
            return _isLeft 
                ? onLeft(_leftValue)
                : onRight(_rightValue);
        }

        #region Equality members

        /// <inheritdoc/>
        public bool Equals(Either<TLeft, TRight> other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return _isLeft
                ? (other._isLeft && Equals(_leftValue, other._leftValue))
                : (!other._isLeft && Equals(_rightValue, other._rightValue));
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
                var hashCode = _isLeft.GetHashCode();
                hashCode = (hashCode * 397) ^ (_isLeft && _leftValue != null ? _leftValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (!_isLeft && _rightValue != null ? _rightValue.GetHashCode() : 0);
                return hashCode;
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

        /// <inheritdoc/>
        public override string ToString()
        {
            return _isLeft 
                ? _leftValue == null ? string.Empty : _leftValue.ToString()
                : _rightValue == null ? string.Empty : _rightValue.ToString();
        }
    }
}

