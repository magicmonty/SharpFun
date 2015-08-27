using NUnit.Framework;
using Shouldly;

namespace Pagansoft.Functional
{
    [TestFixture]
    public class EnumerableWithOptionExtensionsTest
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
    }
}

