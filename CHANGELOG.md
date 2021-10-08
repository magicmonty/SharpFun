All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [5.1.0]
### Added
- Unit type
- Linq Comprehensions for Result
- A lot of helper extensions for Option and Result
- `Either.IsLeft`, `Either.IsRight`
- `Either.Match` which returns a value
- `Result.IsFailure`, `Result.IsSuccess`
- `Result.Match` which returns a value
- `Option.Match` which returns a value
- `Option.ValueEquals` to compare the underlying value
- lazy evaluated `Option.ReturnValueOr`
- Included StyleCop Analyzers

### Changed
- Renamed `Result.MatchSuccess` into `Result.DoOnSuccess`
- Renamed `Result.MatchFailure` into `Result.DoOnFailure`

### Fixed
- Re-added XML documentation in package

## [5.0.0]
### Changed
- Switch to .net 5.0

## [2.0.4]
### Fixed
- Fixes some issues with Option Equality

## [2.0.3]
### Added
- Added Documentation XMLs

### Changed
- The DLL's are now signed

## [2.0.2]
### Changed
- Additional platform .NET 45 as extra dependency

## [2.0.1.1]
### Fixed
- Fixes incompatibility with NuGet

## [2.0.1]
### Added
- Adds multi platform support for .Net 3.5, .net 4.0, Portable .net 4.5 + Windows 8.0 + Windows Phone 8.1

## [2.0.0]
### Added
- Adds Either type
- Adds Result type
- new Extension methods for IEnumerable
  - Option<T> FirstOrNone<T>(IEnumerable<T>)
  - Option<T> LastOrNone<T>(IEnumerable<T>)

### Changed
- Renames IsSome to HasValue and IsNone to HasNoValue as they fit better the intent
- Renames Option.Else to Option.Unless
- Extension method IEnumerable<T>.OptionValues is now null safe

## [1.0.0]
### Added
- Initial release with an Option type

[Unreleased]: https://github.com/magicmonty/SharpFun/compare/5.1.0...HEAD
[5.1.0]: https://github.com/magicmonty/SharpFun/compare/5.0.0...5.1.0
[5.0.0]: https://github.com/magicmonty/SharpFun/compare/2.0.1.0...5.0.0
[2.0.1]: https://github.com/magicmonty/SharpFun/compare/2.0.0.0...2.0.1.0
[2.0.0]: https://github.com/magicmonty/SharpFun/compare/1.0.0.0...2.0.0.0
[1.0.0]: https://github.com/magicmonty/SharpFun/tree/1.0.0.0
