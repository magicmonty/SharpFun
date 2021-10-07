//
// OptionExtensionsTests.cs
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

using System;
using Shouldly;
using Xunit;

namespace Pagansoft.Functional
{
    public class OptionExtensionsTests
    {
        [Fact]
        public void Do_Executes_If_Option_Contains_Value()
        {
            var wasExecuted = string.Empty;

            var option = Option.Some("FOO");

            option.Do(v => wasExecuted = string.Format("{0} was executed", v));

            wasExecuted.ShouldBe("FOO was executed");
        }

        [Fact]
        public void Do_Does_Not_Execute_If_Option_Contains_No_Value()
        {
            var wasExecuted = string.Empty;
            var option = Option.None<string>();

            option.Do(v => wasExecuted = string.Format("{0} was executed", v));

            wasExecuted.ShouldBeEmpty();
        }

        [Fact]
        public void OtherwiseDo_Executes_If_Option_Contains_No_Value()
        {
            var wasExecuted = false;
            var option = Option.None<string>();

            option.OtherwiseDo(() => wasExecuted = true);

            wasExecuted.ShouldBe(true);
        }

        [Fact]
        public void OtherwiseDo_Does_Not_Execute_If_Option_Contains_Value()
        {
            var wasExecuted = false;
            var option = Option.Some("FOO");

            option.OtherwiseDo(() => wasExecuted = true);

            wasExecuted.ShouldBe(false);
        }

        [Fact]
        public void Match_With_A_Parametrized_Action_Is_The_Same_As_Do()
        {
            var wasExecuted = string.Empty;

            var option = Option.Some("FOO");

            option.Match(v => wasExecuted = string.Format("{0} was executed", v));

            wasExecuted.ShouldBe("FOO was executed");
        }

        [Fact]
        public void Match_With_An_Unparametrized_Action_Is_The_Same_As_OtherwiseDo()
        {
            var wasExecuted = string.Empty;
            var option = Option.None<string>();

            option.Match(() => wasExecuted = "was executed");

            wasExecuted.ShouldBe("was executed");
        }

        [Fact]
        public void Match_With_Two_Actions_Is_Same_As_Do_And_Otherwise_Do_Combined_For_None_Option()
        {
            var wasExecuted = string.Empty;
            var option = Option.None<string>();

            option.Match(
                v => wasExecuted = string.Format("{0} was executed", v),
                () => wasExecuted = "None was executed");

            wasExecuted.ShouldBe("None was executed");
        }

        [Fact]
        public void Match_With_Two_Actions_Is_Same_As_Do_And_Otherwise_Do_Combined_For_Some_Option()
        {
            var wasExecuted = string.Empty;
            var option = Option.Some("FOO");

            option.Match(
                v => wasExecuted = string.Format("{0} was executed", v),
                () => wasExecuted = "None was executed");

            wasExecuted.ShouldBe("FOO was executed");
        }

        [Fact]
        public void Map_Returns_Some_Transformed_Value_If_Option_Has_Value()
        {
            var option = Option.Some(1);

            option.Map(v => v + "FOO").ShouldBe(Option.Some("1FOO"));
        }

        [Fact]
        public void Map_Returns_None_If_Option_Has_No_Value()
        {
            var option = Option.None<int>();

            option.Map(v => v + "BAR").ShouldBe(Option.None<string>());
        }

        [Fact]
        public void Bind_Returns_Some_Transformed_Value_If_Option_Has_Value_And_Transformation_Function_Returns_Some()
        {
            var option = Option.Some(1);

            option.Bind(_ => Option.Some("FOO")).ShouldBe(Option.Some("FOO"));
        }

        [Fact]
        public void Bind_Returns_None_If_Option_Has_Value_And_Transformation_Function_Returns_None()
        {
            var option = Option.Some(1);

            option.Bind(_ => Option.None<string>()).ShouldBe(Option.None<string>());
        }

        [Fact]
        public void Bind_Returns_None_If_Option_Has_No_Value()
        {
            var option = Option.None<int>();

            option.Bind(_ => Option.Some("FOO")).ShouldBe(Option.None<string>());
        }

        [Fact]
        public void AsOption_Returns_None_If_Value_Is_Null() =>
            ((object)null)
            .AsOption()
            .ShouldBe(Option.None<object>());

        [Fact]
        public void AsOption_Returns_Some_With_Value_If_Value_Is_Not_Null()
        {
            const string value = "FOO";
            value.AsOption().ShouldBe(Option.Some("FOO"));
        }

        [Fact]
        public void AsOption_Returns_None_If_Nullable_Value_Is_Null() =>
            ((int?)null)
            .AsOption()
            .ShouldBe(Option.None<int>());

        [Fact]
        public void AsOption_Returns_Some_Value_If_Nullable_Value_Is_Not_Null()
        {
            int? value = 10;

            value.AsOption().ShouldBe(Option.Some(10));
        }

        [Fact]
        public void AsOption_Returns_Some_Value_Is_Value_Type()
        {
            const int value = 10;

            value.AsOption().ShouldBe(Option.Some(10));
        }

        [Fact]
        public void Where_Returns_Some_For_Option_Some_And_Predicate_True()
        {
            Option.Some(10).Where(v => v == 10).ShouldBe(Option.Some(10));
        }

        [Fact]
        public void Where_Returns_None_For_Option_Some_And_Predicate_False()
        {
            Option.Some(10).Where(v => v > 10).ShouldBe(Option.None<int>());
        }

        [Fact]
        public void Where_Returns_None_For_Option_None()
        {
            Option.None<int>().Where(_ => true).ShouldBe(Option.None<int>());
        }

        [Fact]
        public void If_Returns_Some_For_Option_Some_And_Predicate_True()
        {
            Option.Some(10).If(v => v == 10).ShouldBe(Option.Some(10));
        }

        [Fact]
        public void If_Returns_None_For_Option_Some_And_Predicate_False()
        {
            Option.Some(10).If(v => v > 10).ShouldBe(Option.None<int>());
        }

        [Fact]
        public void If_Returns_None_For_Option_None()
        {
            Option.None<int>().If(_ => true).ShouldBe(Option.None<int>());
        }

        [Fact]
        public void WhereNot_Returns_Some_For_Option_Some_And_Predicate_False()
        {
            Option.Some(5).WhereNot(v => v > 10).ShouldBe(Option.Some(5));
        }

        [Fact]
        public void WhereNot_Returns_None_For_Option_Some_And_Predicate_True()
        {
            Option.Some(11).WhereNot(v => v > 10).ShouldBe(Option.None<int>());
        }

        [Fact]
        public void WhereNot_Returns_None_For_Option_None()
        {
            Option.None<int>().WhereNot(_ => true).ShouldBe(Option.None<int>());
        }

        [Fact]
        public void Unless_Returns_Some_For_Option_Some_And_Predicate_False()
        {
            Option.Some(5).Unless(v => v > 10).ShouldBe(Option.Some(5));
        }

        [Fact]
        public void Unless_Returns_None_For_Option_Some_And_Predicate_True()
        {
            Option.Some(11).Unless(v => v > 10).ShouldBe(Option.None<int>());
        }

        [Fact]
        public void Unless_Returns_None_For_Option_None()
        {
            Option.None<int>().Unless(_ => true).ShouldBe(Option.None<int>());
        }

        [Fact]
        public void TryMap_Returns_None_If_Transformation_Function_Throws_Exception()
        {
            Option.Some(10).TryMap<int, string>(_ => {
                throw new Exception();
            }).ShouldBe(Option.None<string>());
        }

        [Fact]
        public void TryMap_Returns_Some_If_Transformation_Function_Does_Not_Throw_Exception()
        {
            Option
                .Some(10)
                .TryMap(_ => "FOO")
                .ShouldBe(Option.Some("FOO"));
        }

        [Fact]
        public void TryBind_Returns_None_If_Transformation_Function_Throws_Exception()
        {
            Option
                .Some(10)
                .TryBind<int, string>(_ => throw new Exception())
                .ShouldBe(Option.None<string>());
        }

        [Fact]
        public void TryBind_Returns_Some_If_Transformation_Function_Does_Not_Throw_Exception()
        {
            Option
                .Some(10)
                .TryBind(_ => "FOO".AsOption())
                .ShouldBe(Option.Some("FOO"));
        }

        [Fact]
        public void AsOption_With_Predicate_Is_Composed_Of_AsOption_And_Where()
        {
            const int intValue = 10;
            int? nullableIntValue = 10;
            const string referenceValue = "FOO";

            intValue.AsOption(v => v == 10).ShouldBe(Option.Some(10));
            intValue.AsOption(v => v != 10).ShouldBe(Option.None<int>());
            nullableIntValue.AsOption<int>(v => v == 10).ShouldBe(Option.Some(10));
            nullableIntValue.AsOption<int>(v => v != 10).ShouldBe(Option.None<int>());
            ((int?)null).AsOption<int>(_ => true).ShouldBe(Option.None<int>());
            ((int?)null).AsOption<int>(_ => false).ShouldBe(Option.None<int>());
            referenceValue.AsOption(_ => true).ShouldBe(Option.Some("FOO"));
            referenceValue.AsOption(_ => false).ShouldBe(Option.None<string>());
            ((string)null).AsOption(_ => true).ShouldBe(Option.None<string>());
            ((string)null).AsOption(_ => false).ShouldBe(Option.None<string>());
        }

    }
}

