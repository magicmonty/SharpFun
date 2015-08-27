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

        [Test]
        public void Map_Returns_Some_Transformed_Value_If_Option_Has_Value()
        {
            var option = Option.Some(1);

            option.Map(v => v + "FOO").ShouldBe(Option.Some("1FOO"));
        }

        [Test]
        public void Map_Returns_None_If_Option_Has_No_Value()
        {
            var option = Option.None<int>();

            option.Map(v => v + "BAR").ShouldBe(Option.None<string>());
        }

        [Test]
        public void Bind_Returns_Some_Transformed_Value_If_Option_Has_Value_And_Transformation_Function_Returns_Some()
        {
            var option = Option.Some(1);

            option.Bind(v => Option.Some("FOO")).ShouldBe(Option.Some("FOO"));
        }

        [Test]
        public void Bind_Returns_None_If_Option_Has_Value_And_Transformation_Function_Returns_None()
        {
            var option = Option.Some(1);

            option.Bind(v => Option.None<string>()).ShouldBe(Option.None<string>());
        }

        [Test]
        public void Bind_Returns_None_If_Option_Has_No_Value()
        {
            var option = Option.None<int>();

            option.Bind(v => Option.Some("FOO")).ShouldBe(Option.None<string>());
        }

        [Test]
        public void AsOption_Returns_None_If_Value_Is_Null()
        {
            object value = null;

            value.AsOption().ShouldBe(Option.None<object>());
        }

        [Test]
        public void AsOption_Returns_Some_With_Value_If_Value_Is_Not_Null()
        {
            const string value = "FOO";
            value.AsOption().ShouldBe(Option.Some("FOO"));
        }

        [Test]
        public void AsOption_Returns_None_If_Nullable_Value_Is_Null()
        {
            int? value = null;

            value.AsOption().ShouldBe(Option.None<int>());
        }

        [Test]
        public void AsOption_Returns_Some_Value_If_Nullable_Value_Is_Not_Null()
        {
            int? value = 10;

            value.AsOption().ShouldBe(Option.Some(10));
        }

        [Test]
        public void AsOption_Returns_Some_Value_Is_Value_Type()
        {
            const int value = 10;

            value.AsOption().ShouldBe(Option.Some(10));
        }

        [Test]
        public void Where_Returns_Some_For_Option_Some_And_Predicate_True()
        {
            Option.Some(10).Where(v => v == 10).ShouldBe(Option.Some(10));
        }

        [Test]
        public void Where_Returns_None_For_Option_Some_And_Predicate_False()
        {
            Option.Some(10).Where(v => v > 10).ShouldBe(Option.None<int>());
        }

        [Test]
        public void Where_Returns_None_For_Option_None()
        {
            Option.None<int>().Where(_ => true).ShouldBe(Option.None<int>());
        }
    }
}

