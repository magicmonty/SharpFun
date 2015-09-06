using System;
using NUnit.Framework;
using Shouldly;

namespace Pagansoft.Functional
{
    [TestFixture]
    public class EitherTests
    {
        [Test]
        public void Two_Eithers_Are_Equal_If_Their_Left_Values_Are_Equal()
        {
            var value = Either.Left<int, string>(42);
            var otherValue = Either.Left<int, string>(42);

                value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Equal_If_Their_Right_Values_Are_Equal()
        {
            var value = Either.Right<int, string>("FOO");
            var otherValue = Either.Right<int, string>("FOO");

            value.ShouldSatisfyAllConditions(
                () => value.ShouldBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(true, "value == otherValue"),
                () => (value != otherValue).ShouldBe(false, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Not_Equal_If_The_Left_Values_Differ()
        {
            var value = Either.Left<int, string>(42);
            var otherValue = Either.Left<int, string>(1);

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Not_Equal_If_The_One_Is_a_Left_Value_And_One_Is_A_Right_value()
        {
            var value = Either.Left<int, string>(42);
            var otherValue = Either.Right<int, string>("FOO");

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Not_Equal_If_The_Right_Values_Differ()
        {
            var value = Either.Right<int, string>("FOO");
            var otherValue = Either.Right<int, string>("BAR");

            value.ShouldSatisfyAllConditions(
                () => value.ShouldNotBeStructuralEqual(otherValue),
                () => (value == otherValue).ShouldBe(false, "value == otherValue"),
                () => (value != otherValue).ShouldBe(true, "value != otherValue"));
        }

        [Test]
        public void Two_Eithers_Are_Not_Equal_If_One_Instance_Is_Null()
        {
            var value = Either.Right<int, string>("FOO");
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
        public void ToString_Returns_Either_Value_If_Is_Left_Value()
        {
            Either.Left<int, string>(42).ToString().ShouldBe("42");
        }

        [Test]
        public void ToString_Returns_Or_Value_If_Is_Right_Value()
        {
            Either.Right<int, string>("FOO").ToString().ShouldBe("FOO");
        }

        [Test]
        public void ToString_Returns_EmptyString_If_Left_Value_Is_Null()
        {
            Either.Left<string, int>(null).ToString().ShouldBeEmpty();
        }

        [Test]
        public void ToString_Returns_EmptyString_If_Right_Value_Is_Null()
        {
            Either.Right<int, string>(null).ToString().ShouldBeEmpty();
        }

        [Test]
        public void Match_Executes_On_Left_Value_If_Instance_Is_a_Left_Value()
        {
            var sut = Either.Left<int, string>(42);

            sut.Match(
                i => i.ShouldBe(42), 
                _ => Assert.Fail("Function called on or"));
        }

        [Test]
        public void Match_Executes_On_Right_Value_If_Instance_Is_a_Right_Value()
        {
            var sut = Either.Right<int, string>("FOO");

            sut.Match(
                _ => Assert.Fail("Function called on either"), 
                o => o.ShouldBe("FOO"));
        }

        [Test]
        public void MatchLeft_Calls_Action_For_Left_Value()
        {
            var sut = Either.Left<int, string>(42);
            int actual = 0;
            sut.MatchLeft(i => actual = i);
            actual.ShouldBe(42);
        }

        [Test]
        public void MatchRight_Does_Not_Call_Action_For_Left_Value()
        {
            var sut = Either.Left<int, string>(42);
            string actual = "";
            sut.MatchRight(o => actual = o);
            actual.ShouldBeEmpty();
        }

        [Test]
        public void MatchRight_Calls_Action_For_Right_Value()
        {
            var sut = Either.Right<int, string>("FOO");
            string actual = "";
            sut.MatchRight(o => actual = o);
            actual.ShouldBe("FOO");
        }

        [Test]
        public void MatchLeft_Does_Not_Call_Action_For_Right_Value()
        {
            var sut = Either.Right<int, string>("FOO");
            int actual = 0;
            sut.MatchLeft(o => actual = o);
            actual.ShouldBe(0);
        }

        [Test]
        public void Case_Returns_The_Correct_Value_On_Left_Value()
        {
            var sut = Either.Left<int, string>(42);

            sut.Case(l => "left " + l, r => "right" + r).ShouldBe("left 42");
        }

        [Test]
        public void Case_Returns_The_Correct_Value_On_Right_Value()
        {
            var sut = Either.Right<int, string>("FOO");

            sut.Case(l => "left " + l, r => "right " + r).ShouldBe("right FOO");
        }

        [Test]
        public void Test_Match_On_Left_With_Same_Types()
        {
            Either.Left<int, int>(42).Match(
                i => i.ShouldBe(42), 
                _ => Assert.Fail("Function called on or"));
        }

        [Test]
        public void Test_Match_On_Right_With_Same_Types()
        {
            Either.Right<int, int>(42).Match(
                _ => Assert.Fail("Function called on or"),
                i => i.ShouldBe(42));
        }
    }
}

