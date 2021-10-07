//
// EnumerableWithOptionExtensionsTests.cs
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
using Shouldly;
using Xunit;

namespace Pagansoft.Functional
{
    public class EnumerableWithOptionExtensionsTests
    {
        [Fact]
        public void OptionValues_is_empty_if_enumerable_is_empty() =>
            Array.Empty<Option<int>>()
                .OptionValues()
                .ShouldBeEmpty();

        [Fact]
        public void OptionValues_returns_only_values_from_SomeValues()
        {
            var options = new[]
            {
                Option.Some(10),
                Option.None<int>(),
                Option.Some(20)
            };

            options.OptionValues().ShouldBe(new [] { 10, 20 });
        }

        [Fact]
        public void OptionValues_with_default_value_returns_Nones_replaced_with_given_value()
        {
            var options = new[]
            {
                Option.Some(10),
                Option.None<int>(),
                Option.Some(20)
            };

            options.OptionValues(42).ShouldBe(new [] { 10, 42, 20 });
        }

        // ReSharper disable ExpressionIsAlwaysNull
        [Fact]
        public void OptionValues_on_null_returns_empty_Enumerable()
        {
            Option<int>[] options = null;

            options.ShouldSatisfyAllConditions(
                () => options.OptionValues().ShouldBeEmpty(),
                () => options.OptionValues(42).ShouldBeEmpty());
        }
        // ReSharper restore ExpressionIsAlwaysNull

        [Fact]
        public void FirstOrNone_returns_first_SomeValue_if_enumerable_contains_values() =>
            new[] { 42, 23 }
                .FirstOrNone()
                .ShouldBe(Option.Some(42));

        [Fact]
        public void FirstOrNone_returns_None_if_enumerable_contains_no_values() =>
            Array.Empty<int>()
                .FirstOrNone()
                .ShouldBe(Option.None<int>());

        [Fact]
        public void FirstOrNone_returns_none_if_enumerable_is_null() =>
            ((int[])null)
            .FirstOrNone()
            .ShouldBe(Option.None<int>());

        [Fact]
        public void LastOrNone_returns_last_SomeValue_if_enumerable_contains_values() =>
            new[] { 42, 23 }
                .LastOrNone()
                .ShouldBe(Option.Some(23));

        [Fact]
        public void LastOrNone_returns_None_if_enumerable_contains_no_values() =>
            Array.Empty<int>()
                .LastOrNone()
                .ShouldBe(Option.None<int>());

        [Fact]
        public void LastOrNone_returns_None_if_enumerable_is_null() =>
            ((int[])null)
            .LastOrNone()
            .ShouldBe(Option.None<int>());
    }
}
