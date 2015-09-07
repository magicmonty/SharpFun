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
using NUnit.Framework;
using Shouldly;
using System;

namespace Pagansoft.Functional
{
    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void Two_Results_Are_Equal_If_Their_Success_Values_Are_Equal()
        {
            var value = Result.Success(42);
            var otherValue = Result.Success(42);

                value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Test]
        public void Two_Results_Are_Equal_If_Their_Failure_Values_Are_Equal()
        {
            var ex = new ExceptionWithContext();
            var value = Result.Failure<int>(ex);
            var otherValue = Result.Failure<int>(ex);

            value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Test]
        public void Two_Results_Are_Not_Equal_If_The_Success_Values_Differ()
        {
            var value = Result.Success(42);
            var otherValue = Result.Success(1);

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Results_Are_Not_Equal_If_The_One_Is_a_Success_And_One_Is_A_Failure()
        {
            var value = Result.Success(42);
            var otherValue = Result.Failure<int>(new ExceptionWithContext());

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Results_Are_Not_Equal_If_The_Failure_Values_Differ()
        {
            var value = Result.Failure<int>(new ExceptionWithContext("FOO", null));
            var otherValue = Result.Failure<int>(new ExceptionWithContext("BAR", null));

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Results_Are_Not_Equal_If_One_Instance_Is_Null()
        {
            var value = Result.Success("FOO");
            Result<string> otherValue = null;

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Results_Are_Equal_If_Both_Instances_Are_Null()
        {
            Result<string> value = null;
            Result<string> otherValue = null;

            value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Test]
        public void ToString_Returns_Success_Value_On_Success()
        {
            Result.Success(42).ToString().ShouldBe("42");
        }

        [Test]
        public void ToString_Returns_Failure_Value_On_Failure()
        {
            Result.Failure<int>(new ExceptionWithContext("Message", null)).ToString().ShouldBe("Pagansoft.Functional.ExceptionWithContext: Message");
        }

        [Test]
        public void ToString_Returns_EmptyString_If_Success_Value_Is_Null()
        {
            Result.Success<string>(null).ToString().ShouldBeEmpty();
        }

        [Test]
        public void ToString_Returns_EmptyString_If_Failure_Value_Is_Null()
        {
            Result.Failure<string>(null).ToString().ShouldBeEmpty();
        }

        [Test]
        public void Match_Executes_On_Success_Value_If_Instance_Is_a_Success()
        {
            var sut = Result.Success(42);

            sut.Match(
                i => i.ShouldBe(42), 
                _ => Assert.Fail("Function called on or"));
        }

        [Test]
        public void Match_Executes_On_Failure_Value_If_Instance_Is_a_Failure()
        {
            var ex = new ExceptionWithContext();
            var sut = Result.Failure<string>(ex);

            sut.Match(
                _ => Assert.Fail("Function called on either"), 
                o => o.ShouldBeSameAs(ex));
        }

        [Test]
        public void MatchSuccess_Calls_Action_For_Success()
        {
            var sut = Result.Success(42);
            int actual = 0;
            sut.MatchSuccess(i => actual = i);
            actual.ShouldBe(42);
        }

        [Test]
        public void MatchFailure_Does_Not_Call_Action_For_Success()
        {
            var sut = Result.Success(42);
            string actual = "";
            sut.MatchFailure(o => actual = o.ToString());
            actual.ShouldBeEmpty();
        }

        [Test]
        public void MatchFailure_Calls_Action_For_Failure()
        {
            var ex = new ExceptionWithContext();
            var sut = Result.Failure<int>(ex);
            object actual = null;
            sut.MatchFailure(o => actual = o);
            actual.ShouldBeSameAs(ex);
        }

        [Test]
        public void MatchSuccess_Does_Not_Call_Action_For_Failure()
        {
            var sut = Result.Failure<int>(new ExceptionWithContext());
            int actual = 0;
            sut.MatchLeft(o => actual = o);
            actual.ShouldBe(0);
        }

        [Test]
        public void Case_Returns_The_Correct_Value_On_Success()
        {
            var sut = Result.Success<int>(42);

            sut.Case(l => "success " + l, r => "failure " + r).ShouldBe("success 42");
        }

        [Test]
        public void Case_Returns_The_Correct_Value_On_Right_Value()
        {
            var sut = Result.Failure<int>(new ExceptionWithContext("ErrorMessage", null));

            sut.Case(l => "success " + l, r => "failure " + r).ShouldBe("failure Pagansoft.Functional.ExceptionWithContext: ErrorMessage");
        }
    }
}

