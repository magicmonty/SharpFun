using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using Xunit;

namespace Pagansoft.Functional
{
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "This style is better for Tests")]
        public class OptionExtensionsForCollectionsTests
    {
        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> SON_0_Data =
            new[]
            {
                new object[]
                {
                    new[] { "one", "two", "three" }, Option.None<string>()
                },
                new object[]
                {
                    Array.Empty<string>(), Option.None<string>()
                },
                new object[]
                {
                    new[] { "one" }, Option.Some("one")
                },
                new object[] { null, Option.None<string>() },
                new object[]
                {
                    new[] { (string)null }, Option.None<string>()
                }
            };

        [Theory, MemberData(nameof(SON_0_Data))]
        public void SingleOrNone_0_Array_returns_expected_Option(
            string[] values,
            Option<string> expected) =>
            values.SingleOrNone().ShouldBe(expected);

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> SON_0_IE_Data =
            SON_0_Data.Skip(1);

        [Theory, MemberData(nameof(SON_0_IE_Data))]
        public void SingleOrNone_0_IE_returns_expected_Option(
            IEnumerable<string> values,
            Option<string> expected) =>
            values.SingleOrNone().ShouldBe(expected);

        private static readonly Func<string, bool> ToFalse = _ => false;

        private static readonly Func<string, bool> ToTrue = _ => true;

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> SON_1_Data =
            new[]
            {
                new object[]
                {
                    new[] { "one", "two", "three" },
                    ToFalse,
                    Option.None<string>()
                },
                new object[]
                {
                    new[] { "one", "two", "three" },
                    ToTrue,
                    Option.None<string>()
                },
                new object[]
                {
                    new[] { "one" }, ToFalse, Option.None<string>()
                },
                new object[]
                {
                    new[] { "one" }, ToTrue, Option.Some("one")
                },
                new object[] { null, ToFalse, Option.None<string>() },
                new object[] { null, ToTrue, Option.None<string>() }
            };

        [Theory, MemberData(nameof(SON_1_Data))]
        public void SingleOrNone_1_Array_returns_expected_Option(
            string[] values,
            [NotNull] Func<string, bool> f,
            Option<string> expected) =>
            values.SingleOrNone(f).ShouldBe(expected);

        [Theory, MemberData(nameof(SON_1_Data))]
        public void SingleOrNone_1_IE_returns_expected_Option(
            IEnumerable<string> values,
            [NotNull] Func<string, bool> f,
            Option<string> expected) =>
            values.SingleOrNone(f).ShouldBe(expected);

        private static readonly Func<Option<string>, bool> OptionToFalse = _ => false;
        private static readonly Func<Option<string>, bool> OptionToTrue = _ => true;

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> SON_1_Opt_Data =
            new[]
            {
                new object[]
                {
                    new[]
                    {
                        "one".AsOption(),
                        "two".AsOption(),
                        "three".AsOption()
                    },
                    OptionToFalse,
                    Option.None<string>()
                },
                new object[]
                {
                    new[]
                    {
                        "one".AsOption(),
                        "two".AsOption(),
                        "three".AsOption()
                    },
                    OptionToTrue,
                    Option.None<string>()
                },
                new object[]
                {
                    new[] { "one".AsOption() },
                    OptionToFalse,
                    Option.None<string>()
                },
                new object[]
                {
                    new[] { "one".AsOption() },
                    OptionToTrue,
                    Option.Some("one")
                },
                new object[] { null, OptionToFalse, Option.None<string>() },
                new object[] { null, OptionToTrue, Option.None<string>() }
            };

        [Theory, MemberData(nameof(SON_1_Opt_Data))]
        public void SingleOrNone_1_IE_Option_returns_expected_Option(
            [NotNull] IEnumerable<Option<string>> values,
            Func<Option<string>, bool> f,
            Option<string> expected) =>
            values.SingleOrNone(f).ShouldBe(expected);

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> FON_0_Data =
            new[]
            {
                new object[] { null, Option.None<string>() },
                new object[] { Array.Empty<string>(), Option.None<string>() },
                new object[]
                {
                    new[] { "one" }, Option.Some("one")
                },
                new object[]
                {
                    new[] { "one", "two", "three" },
                    Option.Some("one")
                }
            };

        [Theory, MemberData(nameof(FON_0_Data))]
        public void FirstOrNone_0_Array_returns_expected_Option(
            string[] values,
            Option<string> expected) =>
            values.FirstOrNone().ShouldBe(expected);

        private static readonly Func<string, bool> EqualsOne =
            s => s.Equals("one");

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> FON_1_Data =
            new[]
            {
                new object[] { null, ToFalse, Option.None<string>() },
                new object[] { null, ToTrue, Option.None<string>() },
                new object[]
                {
                    new[] { "one" }, ToTrue, Option.Some("one")
                },
                new object[]
                {
                    new[] { "1", "1", "1", "one", "1", "1" },
                    EqualsOne,
                    Option.Some("one")
                },
                new object[]
                {
                    new[] { "1", "1", "1", "one", "1", "1" },
                    ToTrue,
                    Option.Some("1")
                }
            };

        [Theory, MemberData(nameof(FON_1_Data))]
        public void FirstOrNone_1_Array_returns_expected_Option(
            string[] values,
            [NotNull] Func<string, bool> f,
            Option<string> expected) =>
            values.FirstOrNone(f).ShouldBe(expected);

        [Theory, MemberData(nameof(FON_1_Data))]
        public void FirstOrNone_1_IE_returns_expected_Option(
            IEnumerable<string> values,
            [NotNull] Func<string, bool> f,
            Option<string> expected) =>
            values.FirstOrNone(f).ShouldBe(expected);

        private static readonly Func<Option<string>, bool> OptionEqualsOne = o => o.ValueEquals("one");

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> FON_1_Opt_Data =
            new[]
            {
                new object[] { null, OptionToFalse, Option.None<string>() },
                new object[] { null, OptionToTrue, Option.None<string>() },
                new object[]
                {
                    new[] { "one".AsOption() },
                    OptionToTrue,
                    Option.Some("one")
                },
                new object[]
                {
                    new[]
                    {
                        "1".AsOption(),
                        "1".AsOption(),
                        "1".AsOption(),
                        "one".AsOption(),
                        "1".AsOption(),
                        "1".AsOption()
                    },
                    OptionEqualsOne,
                    Option.Some("one")
                },
                new object[]
                {
                    new[]
                    {
                        "1".AsOption(),
                        "1".AsOption(),
                        "1".AsOption(),
                        "one".AsOption(),
                        "1".AsOption(),
                        "1".AsOption()
                    },
                    OptionToTrue,
                    Option.Some("1")
                }
            };

        [Theory, MemberData(nameof(FON_1_Opt_Data))]
        public void FirstOrNone_1_IE_Option_returns_expected_Option(
            IEnumerable<Option<string>> values,
            Func<Option<string>, bool> f,
            Option<string> expected) =>
            values.FirstOrNone(f).ShouldBe(expected);

        [Theory, MemberData(nameof(FON_0_Data))]
        public void FirstOrNone_0_IE_returns_expected_Option(
            IEnumerable<string> values,
            Option<string> expected) =>
            values.FirstOrNone().ShouldBe(expected);

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> FON_0_Opt_Data =
            new[]
            {
                new object[] { null, Option.None<string>() },
                new object[] { null, Option.None<string>() },
                new object[]
                {
                    new[] { "one".AsOption() },
                    Option.Some("one")
                },
                new object[]
                {
                    new[]
                    {
                        "1".AsOption(),
                        "1".AsOption(),
                        "1".AsOption(),
                        "one".AsOption(),
                        "1".AsOption(),
                        "1".AsOption()
                    },
                    Option.Some("1")
                }
            };

        [Theory, MemberData(nameof(FON_0_Opt_Data))]
        public void FirstOrNone_0_Array_Option_returns_expected_Option(
            Option<string>[] values,
            Option<string> expected) =>
            values.FirstOrNone().ShouldBe(expected);

        [Theory, MemberData(nameof(FON_0_Opt_Data))]
        public void FirstOrNone_0_IE_Option_returns_expected_Option(
            IEnumerable<Option<string>> values,
            Option<string> expected) =>
            values.FirstOrNone().ShouldBe(expected);

        private static readonly Func<string, string> ToNull = _ => null;

        private static readonly Func<string, string> LengthLower3 = s =>
            s?.Length <= 3 ? null : s;

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> SNNV_1_Data =
            new[]
            {
                new object[]
                {
                    new[] { null, "one", "two", null, "three", null },
                    ToNull,
                    Array.Empty<string>()
                },
                new object[]
                {
                    new[] { null, "one", "two", null, "three", null },
                    LengthLower3,
                    new[] { "three" }
                },
                new object[]
                {
                    new[] { null, "012", "0123", "01", null, "01234" },
                    LengthLower3,
                    new[] { "0123", "01234" }
                },
                new object[] { null, ToNull, Array.Empty<string>() }
            };

        [Theory, MemberData(nameof(SNNV_1_Data))]
        public void SelectNonNullValues_1_Array_returns_expected(
            string[] source,
            [NotNull] Func<string, string> f,
            string[] expected) =>
            source.SelectNonNullValues(f).ShouldBe(expected);

        [Theory, MemberData(nameof(SNNV_1_Data))]
        public void SelectNonNullValues_1_IE_returns_expected(
            IEnumerable<string> source,
            [NotNull] Func<string, string> f,
            IEnumerable<string> expected) =>
            source.SelectNonNullValues(f).ShouldBe(expected);

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> SNNV_0_Data =
            new[]
            {
                new object[]
                {
                    new[] { null, "one", "two", null, "three", null },
                    new[] { "one", "two", "three" }
                },
                new object[] { null, Array.Empty<string>() },
                new object[]
                {
                    new[] { "one", "two", "three" },
                    new[] { "one", "two", "three" }
                }
            };

        [Theory, MemberData(nameof(SNNV_0_Data))]
        public void SelectNonNullValues_0_IE_returns_expected(
            IEnumerable<string> source,
            IEnumerable<string> expected) =>
            source.SelectNonNullValues().ShouldBe(expected);

        [Theory, MemberData(nameof(SNNV_0_Data))]
        public void SelectNonNullValues_0_Array_returns_expected(
            string[] source,
            string[] expected) =>
            source.SelectNonNullValues().ShouldBe(expected);

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> SV_0_Data =
            new[]
            {
                new object[] { null, Array.Empty<string>() },
                new object[]
                {
                    new[]
                    {
                        Option.Some("one"),
                        Option.None<string>(),
                        Option.Some("two"),
                        Option.None<string>(),
                        Option.Some("three"),
                        Option.None<string>()
                    },
                    new[] { "one", "two", "three" }
                }
            };

        [Theory, MemberData(nameof(SV_0_Data))]
        public void SelectValues_0_IE_returns_expected(
            IEnumerable<Option<string>> options,
            IEnumerable<string> expected) =>
            options.SelectValues().ShouldBe(expected);

        [Theory, MemberData(nameof(SV_0_Data))]
        public void SelectValues_0_Array_returns_expected(
            Option<string>[] options,
            string[] expected) =>
            options.SelectValues().ShouldBe(expected);

        private static readonly IEnumerable<string> OneTwoThree =
            new[] { "one", "two", "three" };

        private static readonly IEnumerable<string> Empty =
            Array.Empty<string>();

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> RVOE_0_IE_Data =
            new[]
            {
                new object[]
                {
                    Option.None<IEnumerable<string>>(),
                    Array.Empty<string>()
                },
                new object[]
                {
                    Empty.AsOption(),
                    Array.Empty<string>()
                },
                new object[] { OneTwoThree.AsOption(), OneTwoThree }
            };

        [Theory, MemberData(nameof(RVOE_0_IE_Data))]
        public void ReturnValueOrEmpty_0_IE_returns_expected(
            Option<IEnumerable<string>> values,
            IEnumerable<string> expected) =>
            values.ReturnValueOrEmpty().ShouldBe(expected);

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> RVOE_0_A_Data =
            new[]
            {
                new object[]
                {
                    Option.None<string[]>(),
                    Array.Empty<string>()
                },
                new object[]
                {
                    Option.Some(Array.Empty<string>()),
                    Array.Empty<string>()
                },
                new object[]
                {
                    new[] { "one", "two", "three" }.AsOption(),
                    new[] { "one", "two", "three" }
                }
            };

        [Theory, MemberData(nameof(RVOE_0_A_Data))]
        public void ReturnValueOrEmpty_0_Array_returns_expected(
            Option<string[]> values,
            string[] expected) =>
            values.ReturnValueOrEmpty().ShouldBe(expected);

        private static readonly Func<string, IEnumerable<char>> SplitOnSpace =
            s => s.Split(' ').Select(sc => sc[0]);

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> C_1_Opt_Data =
            new[]
            {
                new object[]
                {
                    Option.None<string>(),
                    SplitOnSpace,
                    Array.Empty<char>()
                },
                new object[]
                {
                    Option.Some("s t r i n g"),
                    SplitOnSpace,
                    new[] { 's', 't', 'r', 'i', 'n', 'g' }
                }
            };

        [Theory, MemberData(nameof(C_1_Opt_Data))]
        public void Collect_1_Opt(
            Option<string> option,
            [NotNull] Func<string, IEnumerable<char>> f,
            IEnumerable<char> expected) =>
            option.Collect(f).ShouldBe(expected);

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> C_1_Data =
            new[]
            {
                new object[]
                {
                    null,
                    SplitOnSpace,
                    Array.Empty<char>()
                },
                new object[]
                {
                    "s t r i n g",
                    SplitOnSpace,
                    new[] { 's', 't', 'r', 'i', 'n', 'g' }
                }
            };

        [Theory, MemberData(nameof(C_1_Data))]
        public void Collect_1_returns_expected(
            string obj,
            [NotNull] Func<string, IEnumerable<char>> f,
            IEnumerable<char> expected) =>
            obj.Collect(f).ShouldBe(expected);

        //// ReSharper disable once MemberCanBePrivate.Global
        public static readonly IEnumerable<object[]> GVON_1_Data =
            new[]
            {
                new object[] { null, 0, Option.None<string>() },
                new object[]
                {
                    new Dictionary<int, string>
                    {
                        { 1, "one" },
                        { 2, "two" },
                        { 3, "three" }
                    },
                    1,
                    Option.Some("one")
                },
                new object[]
                {
                    new Dictionary<int, string>
                    {
                        { 1, "one" },
                        { 2, "two" },
                        { 3, "three" }
                    },
                    4,
                    Option.None<string>()
                }
            };

        [Theory, MemberData(nameof(GVON_1_Data))]
        public void GetValueOrNone_1_returns_expected(
            IReadOnlyDictionary<int, string> dictionary,
            int key,
            Option<string> expected) =>
            dictionary.GetValueOrNone(key).ShouldBe(expected);
    }
}