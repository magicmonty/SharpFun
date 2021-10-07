//
// ExceptionWithContextTests.cs
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

using Shouldly;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Pagansoft.Functional
{
    public class ExceptionWithContextTests
    {
        private readonly Dictionary<string, object> _context;

        public ExceptionWithContextTests()
        {
            _context = new Dictionary<string, object>
            {
                { "String", "StringValue" },
                { "Int", 42 }
            };
        }

        [Fact]
        public void Initializes_correctly_with_ctor_without_parameters() =>
            new ExceptionWithContext()
                .ShouldSatisfyAllConditions(
                    ex => ex.Message.ShouldBeEmpty(),
                    ex => ex.InnerException.ShouldBe(null));

        [Fact]
        public void Initializes_correctly_with_ctor_with_context_as_parameter() =>
            new ExceptionWithContext(_context)
                .ShouldSatisfyAllConditions(
                    ex => ex.Message.ShouldBeEmpty(),
                    ex => ex.InnerException.ShouldBe(null));

        [Fact]
        public void Initializes_correctly_with_ctor_with_message_and_context_as_parameter() =>
            new ExceptionWithContext("ErrorMessage", _context)
                .ShouldSatisfyAllConditions(
                    ex => ex.Message.ShouldBe("ErrorMessage"),
                    ex => ex.InnerException.ShouldBe(null));

        [Fact]
        public void Initializes_correctly_with_ctor_with_message_InnerException_and_context_as_parameter()
        {
            var inner = new Exception("BOO");
            new ExceptionWithContext("ErrorMessage", inner, _context)
                .ShouldSatisfyAllConditions(
                    ex => ex.Message.ShouldBe("ErrorMessage"),
                    ex => ex.InnerException.ShouldBeSameAs(inner));
        }

        [Fact]
        public void GetContextValue_returns_Some_value_if_key_and_type_match() =>
            new ExceptionWithContext(_context).ShouldSatisfyAllConditions(
                ex => ex.GetContextValue<string>("String").ShouldBeSome("StringValue"),
                ex => ex.GetContextValue<int>("Int").ShouldBeSome(42));

        [Fact]
        public void GetContextValue_returns_None_if_key_is_not_found() =>
            new ExceptionWithContext(_context)
                .GetContextValue<string>("FOO")
                .ShouldBeNone();

        [Fact]
        public void GetContextValue_returns_none_if_type_does_not_match() =>
            new ExceptionWithContext(_context)
                .ShouldSatisfyAllConditions(
                    ex => ex.GetContextValue<int>("String").ShouldBeNone(),
                    ex => ex.GetContextValue<string>("Int").ShouldBeNone());
    }
}

