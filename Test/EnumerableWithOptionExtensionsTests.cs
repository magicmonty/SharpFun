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
using NUnit.Framework;
using Shouldly;

namespace Pagansoft.Functional
{
    [TestFixture]
    public class EnumerableWithOptionExtensionsTests
    {
        [Test]
        public void OptionValues_Is_Empty_If_Enumerable_Is_Empty()
        {
            var options = new Option<int>[] { };

            options.OptionValues().ShouldBeEmpty();
        }

        [Test]
        public void OptionValues_Returns_Only_Values_From_Somes()
        {
            var options = new Option<int>[] { Option.Some(10), Option.None<int>(), Option.Some(20) };

            options.OptionValues().ShouldBe(new [] { 10, 20 });
        }

        [Test]
        public void OptionValues_With_Default_Value_Returns_Nones_Replaced_With_Given_Value()
        {
            var options = new Option<int>[] { Option.Some(10), Option.None<int>(), Option.Some(20) };

            options.OptionValues(42).ShouldBe(new [] { 10, 42, 20 });
        }

        [Test]
        public void OptionValues_On_Null_Returns_Empty_Enumerable()
        {
            Option<int>[] options = null;

            options.ShouldSatisfyAllConditions(
                () => options.OptionValues().ShouldBeEmpty(),
                () => options.OptionValues(42).ShouldBeEmpty());
        }

        [Test]
        public void FirstOrNone_Returns_Some_Value_If_Enumerable_Contains_Values()
        {
            new[] { 42, 23 }.FirstOrNone().ShouldBe(Option.Some(42));
        }

        [Test]
        public void FirstOrNone_Returns_None_If_Enumerable_Contains_No_Values()
        {
            new int[] { }.FirstOrNone().ShouldBe(Option.None<int>());
        }

        [Test]
        public void FirstOrNone_Returns_None_If_Enumerable_Is_Null()
        {
            ((int[])null).FirstOrNone().ShouldBe(Option.None<int>());
        }

        [Test]
        public void LastOrNone_Returns_Some_Value_If_Enumerable_Contains_Values()
        {
            new[] { 42, 23 }.LastOrNone().ShouldBe(Option.Some(23));
        }

        [Test]
        public void LastOrNone_Returns_None_If_Enumerable_Contains_No_Values()
        {
            new int[] { }.LastOrNone().ShouldBe(Option.None<int>());
        }

        [Test]
        public void LastOrNone_Returns_None_If_Enumerable_Is_Null()
        {
            ((int[])null).LastOrNone().ShouldBe(Option.None<int>());
        }
    }
}

