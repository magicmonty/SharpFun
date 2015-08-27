using NUnit.Framework;
using System;
using Shouldly;

namespace Pagansoft.Functional
{
    [TestFixture]
    public class OptionTest
    {
        [Test]
        public void IsSome_Is_True_For_Option_Some()
        {
            var sut = Option.Some(1);
            sut.IsSome.ShouldBe(true);
        }

        [Test]
        public void IsNone_Is_False_For_Option_Some()
        {
            var sut = Option.Some(1);
            sut.IsNone.ShouldBe(false);
        }

        [Test]
        public void Value_Of_Some_Should_Be_Value_From_Constructor()
        {
            var sut = Option.Some(1);
            sut.Value.ShouldBe(1);
        }

        [Test]
        public void IsSome_Is_False_For_Option_None()
        {
            var sut = Option.None<int>();
            sut.IsSome.ShouldBe(false);
        }

        [Test]
        public void IsNone_Is_True_For_Option_None()
        {
            var sut = Option.None<int>();
            sut.IsNone.ShouldBe(true);
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
    