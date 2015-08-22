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
            var sut = Option.Some<int>(1);
            sut.IsSome.ShouldBe(true);
		}

        [Test]
        public void IsNone_Is_False_For_Option_Some()
        {
            var sut = Option.Some<int>(1);
            sut.IsNone.ShouldBe(false);
        }

        [Test]
        public void Value_Of_Some_Should_Be_Value_From_Constructor()
        {
            var sut = Option.Some<int>(1);
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
            Should.Throw<ArgumentException>(() => { var x = sut.Value; }).Message.ShouldBe("A None Option has no value!");
        }
	}
}
    