// ExceptionWithContext.cs
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
using System.Runtime.Serialization;

namespace Pagansoft.Functional
{
    /// <summary>
    /// Represents an exception with additional context information
    /// </summary>
    [Serializable]
    public class ExceptionWithContext : Exception
    {
        private readonly Dictionary<string, object> _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pagansoft.Functional.ExceptionWithContext"/> class.
        /// </summary>
        public ExceptionWithContext() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pagansoft.Functional.ExceptionWithContext"/> class.
        /// </summary>
        /// <param name="context">The error context.</param>
        public ExceptionWithContext(Dictionary<string, object> context) : this(string.Empty, context) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pagansoft.Functional.ExceptionWithContext"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="context">The error context.</param>
        public ExceptionWithContext(string message, Dictionary<string, object> context) : this(message, null, context) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pagansoft.Functional.ExceptionWithContext"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="context">The error context.</param>
        public ExceptionWithContext(string message, Exception innerException, Dictionary<string, object> context)
            : base(message, innerException)
        {
            _context = context ?? new Dictionary<string, object>();
        }

        /// <inheritdoc />
        protected ExceptionWithContext(SerializationInfo info, StreamingContext streamingContext)
            : base(info, streamingContext)
        {
            _context = (Dictionary<string, object>)info.GetValue("Context", typeof(Dictionary<string, object>));
        }

        /// <summary>Gets a value from the exception context.</summary>
        /// <returns>
        /// An <see cref="Option.Some{T}"/> with the value, if a value of the given <typeparamref name="TResult">type</typeparamref> with the given <paramref name="key"/> was found,
        /// otherwise a <see cref="Option.None{T}"/>.
        /// </returns>
        /// <param name="key">The key.</param>
        /// <typeparam name="TResult">The expected result type.</typeparam>
        public Option<TResult> GetContextValue<TResult>(string key)
        {
            if (!_context.ContainsKey(key))
                return Option.None<TResult>();

            try
            {
                return ((TResult)_context[key]).AsOption();
            }
            catch (InvalidCastException)
            {
                return Option.None<TResult>();
            }
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Context", _context);
        }
    }
}