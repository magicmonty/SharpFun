//
// OptionTests.cs
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
using System;
using Shouldly;

namespace Pagansoft.Functional
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void HasValue_Is_True_For_Option_Some()
        {
            var sut = Option.Some(1);
            sut.HasValue.ShouldBe(true);
        }

        [Test]
        public void HasNoValue_Is_False_For_Option_Some()
        {
            var sut = Option.Some(1);
            sut.HasNoValue.ShouldBe(false);
        }

        [Test]
        public void Value_Of_Some_Should_Be_Value_From_Constructor()
        {
            var sut = Option.Some(1);
            sut.Value.ShouldBe(1);
        }

        [Test]
        public void HasValue_Is_False_For_Option_None()
        {
            var sut = Option.None<int>();
            sut.HasValue.ShouldBe(false);
        }

        [Test]
        public void HasNoValue_Is_True_For_Option_None()
        {
            var sut = Option.None<int>();
            sut.HasNoValue.ShouldBe(true);
        }

        [Test]
        public void Value_Of_None_Should_Throw_Exception()
        {
            var sut = Option.None<int>();
            Should.Throw<ArgumentException>(() => {
                var x = sut.Value;
            }).Message.ShouldBe("A None Option has no value!");
        }

        [Test]
        public void Option_Some_Are_Equal_If_Content_Is_Equal()
        {
            var option1 = Option.Some("FOO");
            var option2 = Option.Some("FOO");

            Equals(option1, option2).ShouldBe(true);
            option1.Equals(option2).ShouldBe(true);
            option2.Equals(option1).ShouldBe(true);
            option1.ShouldBe(option2);
            option1.GetHashCode().ShouldBe(option2.GetHashCode());
        }

        [Test]
        public void Option_Some_Are_Not_Equal_If_Content_Is_Not_Equal()
        {
            var option1 = Option.Some("FOO");
            var option2 = Option.Some("BAR");

            Equals(option1, option2).ShouldBe(false);
            option1.Equals(option2).ShouldBe(false);
            option2.Equals(option1).ShouldBe(false);
            option1.ShouldNotBe(option2);
        }

        [Test]
        public void Option_None_Are_Equal_If_Containing_Type_Is_Same()
        {
            var option1 = Option.None<int>();
            var option2 = Option.None<int>();

            Equals(option1, option2).ShouldBe(true);
            option1.Equals(option2).ShouldBe(true);
            option2.Equals(option1).ShouldBe(true);
            option1.ShouldBe(option2);
            option1.GetHashCode().ShouldBe(option2.GetHashCode());
        }

        [Test]
        public void Option_None_And_Option_Some_Are_Not_Equal()
        {
            var option1 = Option.Some<int>(1);
            var option2 = Option.None<int>();

            option1.ShouldNotBe(option2);
            option2.ShouldNotBe(option1);
        }

        [Test]
        public void Implicit_Conversion_Converts_To_A_Some_Option()
        {
            Option<int> actual = 1;

            actual.ShouldBe(Option.Some(1));
        }

        [Test]
        public void Implicit_Conversion_Of_Null_Converts_To_A_None_Option()
        {
            Option<string> actual = (string)null;

            actual.ShouldBe(Option.None<string>());
        }

        [Test]
        public void ReturnValueOr_Of_A_Some_Returns_Its_Value()
        {
            Option.Some("FOO").ReturnValueOr("BAR").ShouldBe("FOO");
        }

        [Test]
        public void ReturnValueOr_Of_A_None_Returns_Its_Value()
        {
            Option.None<string>().ReturnValueOr("BAR").ShouldBe("BAR");
        }

        [Test]
        public void ToString_Returns_ToString_Of_Value()
        {
            Option.Some("FOO").ShouldBe("FOO");
        }

        [Test]
        public void ToString_Of_None_Returns_None()
        {
            Option.None<int>().ToString().ShouldBe("None");
        }
    }
}
    