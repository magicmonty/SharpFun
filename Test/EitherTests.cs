using System;
using NUnit.Framework;
using Shouldly;

namespace Pagansoft.Functional
{
    [TestFixture]
    public class EitherTests
    {
        [Test]
        public void Two_Eithers_Are_Equal_If_Their_Either_Values_Are_Equal()
        {
            var value = new Either<int, string>(42);
            var otherValue = new Either<int, string>(42);

            value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Equal_If_Their_Or_Values_Are_Equal()
        {
            var value = new Either<int, string>("FOO");
            var otherValue = new Either<int, string>("FOO");

            value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Not_Equal_If_The_Either_Values_Differ()
        {
            var value = new Either<int, string>(42);
            var otherValue = new Either<int, string>(1);

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Not_Equal_If_The_One_Is_An_Either_And_One_Is_An_Or()
        {
            var value = new Either<int, string>(42);
            var otherValue = new Either<int, string>("FOO");

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Not_Equal_If_The_Or_Values_Differ()
        {
            var value = new Either<int, string>("FOO");
            var otherValue = new Either<int, string>("BAR");

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Not_Equal_If_One_Instance_Is_Null()
        {
            var value = new Either<int, string>("FOO");
            Either<int, string> otherValue = null;

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Equal_If_Both_Instances_Are_Null()
        {
            Either<int, string> value = null;
            Either<int, string> otherValue = null;

            value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Test]
        public void ToString_Returns_Either_Value_If_IsEither()
        {
            new Either<int, string>(42).ToString().ShouldBe("42");
        }

        [Test]
        public void ToString_Returns_Or_Value_If_IsNotEither()
        {
            new Either<int, string>("FOO").ToString().ShouldBe("FOO");
        }

        [Test]
        public void ToString_Returns_EmptyString_If_Either_Value_Is_Null()
        {
            new Either<string, int>(null).ToString().ShouldBeEmpty();
        }

        [Test]
        public void ToString_Returns_EmptyString_If_Or_Value_Is_Null()
        {
            new Either<int, string>(null).ToString().ShouldBeEmpty();
        }

        [Test]
        public void Match_Executes_On_Either_Value_If_Instance_Is_an_Either()
        {
            var sut = new Either<int, string>(42);

            sut.Match(
                i => i.ShouldBe(42), 
                _ => Assert.Fail("Function called on or"));
        }

        [Test]
        public void Match_Executes_On_Or_Value_If_Instance_Is_an_Or()
        {
            var sut = new Either<int, string>("FOO");

            sut.Match(
                _ => Assert.Fail("Function called on either"), 
                o => o.ShouldBe("FOO"));
        }

        [Test]
        public void Match_With_Single_Action_Calls_Action_For_Either_If_Types_Match()
        {
            var sut = new Either<int, string>(42);
            int actual = 0;
            sut.Match(i => actual = i);
            actual.ShouldBe(42);
        }

        [Test]
        public void Match_With_Single_Action_Does_Not_Call_Action_If_Types_Dont_Match_For_Either()
        {
            var sut = new Either<int, string>(42);
            string actual = "";
            sut.Match(o => actual = o);
            actual.ShouldBeEmpty();
        }

        [Test]
        public void Match_With_Single_Action_Calls_Action_For_Or_If_Types_Match()
        {
            var sut = new Either<int, string>("FOO");
            string actual = "";
            sut.Match(o => actual = o);
            actual.ShouldBe("FOO");
        }

        [Test]
        public void Match_With_Single_Action_Does_Not_Call_Action_If_Types_Dont_Match_For_Or()
        {
            var sut = new Either<int, string>("FOO");
            int actual = 0;
            sut.Match(o => actual = o);
            actual.ShouldBe(0);
        }

        [Test]
        public void Case_Returns_The_Correct_Value_On_Left_Value()
        {
            var sut = new Either<int, string>(42);

            sut.Case(l => "left " + l, r => "right" + r).ShouldBe("left 42");
        }

        [Test]
        public void Case_Returns_The_Correct_Value_On_Right_Value()
        {
            var sut = new Either<int, string>("FOO");

            sut.Case(l => "left " + l, r => "right " + r).ShouldBe("right FOO");
        }
    }
}

