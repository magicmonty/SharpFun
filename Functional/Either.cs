using System;
using System.Diagnostics.Contracts;

namespace Pagansoft.Functional
{
    /// <summary>
    /// Represents a type, which can have either one of two given types
    /// </summary>
    /// <example>
    /// <code>
    /// // Prints: "Success: 42"
    /// var success = new Either<int, System.Exception>(42);
    /// success.Match(
    ///   r => Console.WriteLine("Success: {0}", r),
    ///   ex => Console.WriteLine(ex.Message));
    /// </code>
    ///
    /// <code>
    /// // Prints: "An error happened!"
    /// var error = new Either<int, System.Exception>(new System.Exception("An error happened!"));
    /// success.Match(
    ///   r => Console.WriteLine("Success: {0}", r),
    ///   ex => Console.WriteLine(ex.Message));
    /// </code>
    /// </example>
    public class Either<TEither, TOr>
    {
        private readonly bool _isEither;

        private readonly TEither _either;
        private readonly TOr _or;

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TEither, TOr}"/> class with a
        /// value of type <typeparamref name="TEither" />
        /// </summary>
        /// <param name="value">The value (must not be null).</param>
        public Either(TEither value)
        {
            Contract.Requires(either != null);

            _or = default(TOr);
            _either = value;
            _isEither = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TEither, TOr}"/> class with a
        /// value of type <typeparamref name="TOr" />
        /// </summary>
        /// <param name="value">The value (must not be null).</param>
        public Either(TOr value)
        {
            Contract.Requires(or != null);

            _or = value;
            _either = default(TEither);
            _isEither = false;
        }

        /// <summary>
        /// Executes the given Action, depending of the internal type.
        /// </summary>
        /// <param name="ifEither">This action will be executed, if this instance is a <typeparamref name="TEither" /></param>
        /// <param name="ifOr">This action will be executed, if this instance is a <typeparamref name="TOr" />.</param>
        public void Match(Action<TEither> ifEither, Action<TOr> ifOr)
        {
            if (_isEither)
                ifEither(_either);
            else
                ifOr(_or);
        }
    }
}

