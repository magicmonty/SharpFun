using NUnit.Framework;
using System;
using Shouldly;

namespace Pagansoft.Functional
{
    [TestFixture]
    public class OptionExtensionsTest
    {
        [Test]
        public void Do_Executes_If_Option_Contains_Value()
        {
            var wasExecuted = string.Empty;

            var option = Option.Some("FOO");

            option.Do(v => wasExecuted = string.Format("{0} was executed", v));

            wasExecuted.ShouldBe("FOO was executed");
        }

        [Test]
        public void Do_Does_Not_Execute_If_Option_Contains_No_Value()
        {
            var wasExecuted = string.Empty;
            var option = Option.None<string>();

            option.Do(v => wasExecuted = string.Format("{0} was executed", v));

            wasExecuted.ShouldBeEmpty();
        }

        [Test]
        public void OtherwiseDo_Executes_If_Option_Contains_No_Value()
        {
            var wasExecuted = false;
            var option = Option.None<string>();

            option.OtherwiseDo(() => wasExecuted = true);

            wasExecuted.ShouldBe(true);
        }

        [Test]
        public void OtherwiseDo_Does_Not_Execute_If_Option_Contains_Value()
        {
            var wasExecuted = false;
            var option = Option.Some("FOO");

            option.OtherwiseDo(() => wasExecuted = true);

            wasExecuted.ShouldBe(false);
        }

        [Test]
        public void Match_With_A_Parametrized_Action_Is_The_Same_As_Do()
        {
            var wasExecuted = string.Empty;

            var option = Option.Some("FOO");

            option.Match(v => wasExecuted = string.Format("{0} was executed", v));

            wasExecuted.ShouldBe("FOO was executed");
        }

        [Test]
        public void Match_With_An_Unparametrized_Action_Is_The_Same_As_OtherwiseDo()
        {
            var wasExecuted = string.Empty;
            var option = Option.None<string>();

            option.Match(() => wasExecuted = "was executed");

            wasExecuted.ShouldBe("was executed");
        }

        [Test]
        public void Match_With_Two_Actions_Is_Same_As_Do_And_Otherwise_Do_Combined_For_None_Option()
        {
            var wasExecuted = string.Empty;
            var option = Option.None<string>();

            option.Match(
                v => wasExecuted = string.Format("{0} was executed", v), 
                () => wasExecuted = "None was executed");

            wasExecuted.ShouldBe("None was executed");
        }

        [Test]
        public void Match_With_Two_Actions_Is_Same_As_Do_And_Otherwise_Do_Combined_For_Some_Option()
        {
            var wasExecuted = string.Empty;
            var option = Option.Some("FOO");

            option.Match(
                v => wasExecuted = string.Format("{0} was executed", v), 
                () => wasExecuted = "None was executed");

            wasExecuted.ShouldBe("FOO was executed");
        }
    }
}

