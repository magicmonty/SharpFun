//
// EitherTests.cs
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
using Xunit;

namespace Pagansoft.Functional
{
    public class EitherTests
    {
        [Fact]
        public void Two_Either_are_equal_if_their_left_values_are_equal()
        {
            var value = Either.Left<int, string>(42);
            var otherValue = Either.Left<int, string>(42);

                value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Fact]
        public void Two_Either_are_equal_if_their_right_values_are_equal()
        {
            var value = Either.Right<int, string>("FOO");
            var otherValue = Either.Right<int, string>("FOO");

            value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Fact]
        public void Two_Either_are_not_equal_if_the_left_values_differ()
        {
            var value = Either.Left<int, string>(42);
            var otherValue = Either.Left<int, string>(1);

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Fact]
        public void Two_Either_are_not_equal_if_one_is_a_left_value_and_one_is_a_right_value()
        {
            var value = Either.Left<int, string>(42);
            var otherValue = Either.Right<int, string>("FOO");

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Fact]
        public void Two_Either_are_not_equal_if_the_right_values_differ()
        {
            var value = Either.Right<int, string>("FOO");
            var otherValue = Either.Right<int, string>("BAR");

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Fact]
        public void Two_Either_are_not_equal_if_one_instance_is_null()
        {
            var value = Either.Right<int, string>("FOO");
            Either<int, string> otherValue = null;

            // ReSharper disable ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [Fact]
        public void Two_Either_are_equal_if_both_instances_are_null()
        {
            Either<int, string> value = null;
            Either<int, string> otherValue = null;

            // ReSharper disable ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [Fact]
        public void ToString_returns_Either_value_if_is_left_value() =>
            Either.Left<int, string>(42).ToString().ShouldBe("42");

        [Fact]
        public void ToString_returns_Or_value_if_is_right_value() =>
            Either.Right<int, string>("FOO").ToString().ShouldBe("FOO");

        [Fact]
        public void ToString_returns_EmptyString_if_left_value_is_null() =>
            Either.Left<string, int>(null).ToString().ShouldBeEmpty();

        [Fact]
        public void ToString_returns_EmptyString_if_right_value_is_null() =>
            Either.Right<int, string>(null).ToString().ShouldBeEmpty();

        [Fact]
        public void IsLeft_returns_true_and_IsRight_returns_false_if_instance_is_a_left_value() =>
            Either.Left<int, string>(42)
                .ShouldSatisfyAllConditions(
                    e => e.IsLeft.ShouldBeTrue(),
                    e => e.IsRight.ShouldBeFalse());

        [Fact]
        public void IsLeft_returns_false_and_IsRight_returns_true_if_instance_is_a_right_value() =>
            Either.Right<int, string>("42")
                .ShouldSatisfyAllConditions(
                    e => e.IsLeft.ShouldBeFalse(),
                    e => e.IsRight.ShouldBeTrue());
        
        [Fact]
        public void Match_executes_on_left_value_if_instance_is_a_left_value()
        {
            var sut = Either.Left<int, string>(42);

            sut.Match(
                i => i.ShouldBe(42),
                _ => Assert.True(false, "Function called on or"));
        }

        [Fact]
        public void Match_executes_on_right_value_if_instance_is_a_right_value()
        {
            var sut = Either.Right<int, string>("FOO");

            sut.Match(
                _ => Assert.True(false, "Function called on either"),
                o => o.ShouldBe("FOO"));
        }

        [Fact]
        public void MatchLeft_calls_action_for_left_value()
        {
            var sut = Either.Left<int, string>(42);
            var actual = 0;

            sut.MatchLeft(i => actual = i);

            actual.ShouldBe(42);
        }

        [Fact]
        public void MatchRight_does_not_call_action_for_left_value()
        {
            var sut = Either.Left<int, string>(42);
            var actual = string.Empty;

            sut.MatchRight(o => actual = o);

            actual.ShouldBeEmpty();
        }

        [Fact]
        public void MatchRight_calls_action_for_right_value()
        {
            var sut = Either.Right<int, string>("FOO");
            var actual = string.Empty;

            sut.MatchRight(o => actual = o);

            actual.ShouldBe("FOO");
        }

        [Fact]
        public void MatchLeft_does_not_call_action_for_right_value()
        {
            var sut = Either.Right<int, string>("FOO");
            var actual = 0;

            sut.MatchLeft(o => actual = o);

            actual.ShouldBe(0);
        }

        [Fact]
        public void Case_returns_the_correct_value_on_left_value()
        {
            var sut = Either.Left<int, string>(42);

            sut.Case(
                l => "left " + l,
                r => "right" + r)
                .ShouldBe("left 42");
        }

        [Fact]
        public void Case_returns_the_correct_value_on_right_value()
        {
            var sut = Either.Right<int, string>("FOO");

            sut.Case(
                l => "left " + l,
                r => "right " + r)
                .ShouldBe("right FOO");
        }

        [Fact]
        public void Test_match_on_left_with_same_types()
        {
            Either.Left<int, int>(42).Match(
                i => i.ShouldBe(42),
                _ => Assert.True(false, "Function called on or"));
        }

        [Fact]
        public void Test_match_on_right_with_same_types()
        {
            Either.Right<int, int>(42).Match(
                _ => Assert.True(false, "Function called on or"),
                i => i.ShouldBe(42));
        }
    }
}

