# PaganSoft.Functional
[![Build Status](https://travis-ci.org/magicmonty/SharpFun.svg?branch=master)](https://travis-ci.org/magicmonty/SharpFun)
[![NuGet Package](https://img.shields.io/nuget/v/Pagansoft.Functional.svg)](https://www.nuget.org/packages/Pagansoft.Functional/)

A functional datatype library for C#

The following types are currently provided
## Option

This is an implementation of the [Option monad](https://en.wikipedia.org/wiki/Option_type).

### Usage

#### Construction
```csharp
// Construction through factory methods
var someValue = Option.Some(53); // Option<int>
var noValue = Option.None<int>(); // Option<int>

// Explicit null content. Use this with care as this breaks the intent of an option type
var nullContent = Option.Some<string>(null);

// Construction through the simple extension method AsOption()
var someValue1 = "FOO".AsOption(); // Creates an Option.Some("FOO")

// Null values are considered as "No Value" on construction through "AsOption"
string nullString = null;
var noValue1 = nullString.AsOption(); // Creates an Option.None<string>)

// Construction through extension method with a predicate over the value
var someValue2 = "FOO".AsOption(s => s.StartsWith("F")); // Creates an Option.Some("FOO")
var noValue2 = "FOO".AsOption(s => s.StartsWith("K")); // Creates an Option.None<string>()
```

#### Check, if an option has a value or not
```csharp
var some = Option.Some("FOO");
Assert.Equals(true, some.HasValue);
Assert.Equals(false, some.HasNoValue);

var none = Option.None<string>();
Assert.Equals(false, none.HasValue);
Assert.Equals(true, none.HasNoValue);
```

#### Getting the value out of an option
```csharp
Assert.Equals("FOO", Option.Some("FOO").ResultValueOr("BAR"));
Assert.Equals("BAR", Option.None<string>().ResultValueOr("BAR"));
```

#### Extension methods

##### Do
If an option has a value, then the given action is executed.
The option is returned for chaining purposes.

```csharp
var some = Option.Some("FOO");
some.Do(Console.WriteLine); // Prints "FOO" on the console

var none = Option.None<string>();
none.Do(Console.WriteLine); // Does nothing
```

##### OtherwiseDo
If an option has **NO** value, then the given action is executed.
The option is returned for chaining purposes.

```csharp
var some = Option.Some("FOO");
some.OtherwiseDo(() => Console.WriteLine("I have no value")); // Does nothing

var none = Option.None<string>();
none.OtherwiseDo(() => Console.WriteLine("I have no value")); // Prints "I have no value" on the console

// Chaining:
ISomeService service = ....
var option = service.GetSomeValue(); // Returns an Option<string> for example
option
   .Do(v => Console.WriteLine("Got a value with content: {0}", v)
   .OtherwiseDo(() => Console.WriteLine("Got no value"));
```


##### Match
This comes in three overloads and matches either a Some or a None or has function parameters for both variants.
These are essentially aliases for `Do`, `OtherwiseDo` and `.Do(...).OtherwiseDo(...)`, so the above examples
could also be written like so:

```csharp
var some = Option.Some("FOO");
some.Match(() => Console.WriteLine("I have no value")); // Does nothing
some.Match(v => Console.WriteLine("I have the value {0}", v)); // Writes "I have the value FOO"

var none = Option.None<string>();
none.Match(() => Console.WriteLine("I have no value"));  // Writes "I have no value"
none.Match(v => Console.WriteLine("I have the value {0}", v)); // Does nothing

// Chaining:
ISomeService service = ....
var option = service.GetSomeValue(); // Returns an Option<string> for example
option.Match(
    v => Console.WriteLine("Got a value with content: {0}", v),
    () => Console.WriteLine("Got no value"));
```

##### Map / Select
Transforms the value of an option with the help of a transformation function.
`Select` is an alias for `Map`, for better compatibility with LINQ.

**Map catches no Exceptions, so you should use `TryMap` for unsafe transformation functions.**

```csharp
var some = Option.Some(42);
var none = Option.None<int>();

var someString = some.Map(v => v.ToString()); // returns an Option.Some("42");
var noString = none.Map(v => v.ToString()); // returns an Option.None<string>();
```

##### TryMap / TrySelect
Same as `Map`/`Select` but the transformation function is wrapped in a try-catch-block.
If the transformation function throws an exception, then a `None` is returned.

```csharp
var some = Option.Some(42);
var none = Option.None<int>();

var someString = some.TryMap(v => v.ToString()); // returns an Option.Some("42");
var someString = some.TryMap((string v) => throw new Exception("BLA")); // returns an Option.None<string>();
var noString = none.TryMap(v => v.ToString()); // returns an Option.None<string>();
```

##### Bind / TryBind
Similar to `Map`/`TryMap` `Bind` and `TryBind` transform the option into another type.
In this case the transformation function returns also an Option.

```csharp
var some = Option.Some(42);

var someString = some.Bind(v => Option.Some("FOO" + v)); // return Option.Some("FOO42"));
var noneString = some.Bind(_ => Option.None<string>()); // return Option.None<string>();
```

##### Where / If - WhereNot / Unless
Filters an option by its predicate, so for a `Some` a `Some` or a `None` is returned, depending if the predicate
matches or not. `If` is here an alias for `Where` and `Unless` is an alias for `WhereNot`.
```csharp
var some = Option.Some(42);

var none = some.Where(v => v < 40); // Option.None<int>()
var someOther = some.Where(v => v > 40); // Option.Some(42)
var stillSomeOther = some.WhereNot(v => v < 40); // Option.Some(42)
var otherNone = some.WhereNot(v => v > 40); // Option.None<int>()
```

## Either

TBD

## Result

TBD

# Building
- First build (gets all dependencies automatically)
    - Mac/Linux: `./updateAndBuild.sh`
    - Windows: `updateAndBuild.bat`
- Build without getting all dependecies:
    - Mac/Linux: `./build.sh`
    - Windows: `build.bat`

# Contributing
## Visual Studio
Please don't use NuGet for the Dependencies but Paket.
There is a Visual Studio plugin for Paket available to manage the dependencies.

## Unit tests
Currently all unit tests arw written with NUnit, as Xamarin studio doesn't support MSTest and
xUnit isn't currently working on Mono.
If the problems with xUnit are gone, the unit tests will be switched to xUnit.
