using System;
using Shouldly;
using Xunit;

namespace Pagansoft.Functional
{
    public class FunctionCompositionExtensionTests
    {
        [Fact]
        public void AndThen_Executes_Functions_In_Correct_Order()
        {
            Func<int, int> add1 = x => x + 1;
            Func<int, int> doubleValue = x => x * 2;
            Func<int, int> add1AndDoubleValue = add1.AndThen(doubleValue);

            add1AndDoubleValue(5).ShouldBe(12);
        }

        [Fact]
        public void Compose_Executes_Functions_In_Correct_Order()
        {
            Func<int, int> add1 = x => x + 1;
            Func<int, int> doubleValue = x => x * 2;
            Func<int, int> doubleValueAndAdd1 = add1.Compose(doubleValue);

            doubleValueAndAdd1(5).ShouldBe(11);
        }

        [Fact]
        public void Curry_Allows_Curring_of_Functions()
        {
            Func<int, int, int> adder = (x, y) => x + y;
            Func<int, int> add1 = adder.Curry(1);
            Func<int> add1And5 = add1.Curry(5);
            Func<int> add2And5 = adder.Curry(2).Curry(5);

            add1(5).ShouldBe(6);
            add1And5().ShouldBe(6);
            add2And5().ShouldBe(7);
        }

        [Fact]
        public void Curry_With_Two_Parameters()
        {
            Func<int, int, int> adder = (x, y) => x + y;

            var add3 = adder.Curry(3);
            add3(1).ShouldBe(4);
        }

        [Fact]
        public void Curry_With_Three_Parameters()
        {
            Func<int, int, int, int> adder = (x, y, z) => x + y + z;

            var add3 = adder.Curry(3);
            add3(1, 2).ShouldBe(6);
        }
    }
}
