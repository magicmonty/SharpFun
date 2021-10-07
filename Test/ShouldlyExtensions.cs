//
// ShouldlyExtensions.cs
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

using System.Diagnostics.CodeAnalysis;
using Shouldly;

namespace Pagansoft.Functional
{
    public static class ShouldlyExtensions
    {
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public static void ShouldBeStructuralEqual<T>(this T value, T otherValue) where T: class
        {
            if (value != null)
            {
                value.ShouldSatisfyAllConditions(
                    () => Equals(value, otherValue).ShouldBe(true, "Equals(value, otherValue)"),
                    () => value.Equals(otherValue).ShouldBe(true, "value.Equals(otherValue)"),
                    () => value.ShouldBe(otherValue),
                    () => value.GetHashCode().ShouldBe(otherValue.GetHashCode(), "GetHashCode"));
            }
            else
            {
                value.ShouldSatisfyAllConditions(
                    () => Equals(value, otherValue).ShouldBe(true, "Equals(value, otherValue)"),
                    () => value.ShouldBe(otherValue));
            }
        }

        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public static void ShouldNotBeStructuralEqual<T>(this T value, T otherValue) where T: class
        {
            if (value != null)
            {
                value.ShouldSatisfyAllConditions(
                    () => Equals(value, otherValue).ShouldBe(false, "Equals(value, otherValue)"),
                    () => value.Equals(otherValue).ShouldBe(false, "value.Equals(otherValue)"),
                    () => value.ShouldNotBe(otherValue));
            }
            else
            {
                value.ShouldSatisfyAllConditions(
                    () => Equals(value, otherValue).ShouldBe(false, "Equals(value, otherValue)"),
                    () => value.ShouldNotBe(otherValue));
            }
        }

        public static void ShouldBeSome<T>(this Option<T> actual, T expected)
        {
            actual.ShouldSatisfyAllConditions(
                () => actual.HasValue.ShouldBe(true),
                () => actual.Value.ShouldBe(expected));
        }

        public static void ShouldBeNone<T>(this Option<T> actual)
        {
            actual.HasValue.ShouldBe(false);
        }
    }
}

