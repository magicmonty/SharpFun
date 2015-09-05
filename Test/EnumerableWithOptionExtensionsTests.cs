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

