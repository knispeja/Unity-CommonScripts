# Changelog
All notable changes to this project will be documented in this file.

## [4.0.0]
### Added
- Additional logging configuration to ComponentPool<T>
- ComponentPool<T> can now instantiate prefabs even after the maximum is reached, if desired

### Changed
- ComponentPool<T> prefabs are now required to be of type T rather than GameObjects, this is a breaking change

## [3.1.0]
### Added
- Additional methods in `LogWrapper` that allow setting log level at runtime

### Changed
- Some methods now have optional log level parameters
- Clarified that some editor-only error messages will not show up in release builds

## [3.0.0]
### Added
- `LogWrapper` class for easily configuring logging levels in a way that completely compiles away slow logging statements
- A new Logging menu in the menu bar that sets preprocessor directives to control log levels

### Changed
- All logging internal to this library now use `LogWrapper`

## [2.0.0]
### Added
- Event listener design pattern for easy inspector driven event listening
- Player preference wrappers designed for interacting with prefs through scriptable objects

### Changed
- Many breaking namespace changes, moved classes into the `KnispelCommon` namespace

## [1.2.1]
### Fixed
- `AudioUtilities` can now handle decibel values below -80

## [1.2.0]
### Added
- `UnityUtilities` for finding inactive objects
- `IsAnyPointerOverGameObject()` takes an optional default return value

### Fixed
- `IsAnyPointerOverGameObject()` no longer throws when no event system is set

## [1.1.1]
### Added
- PlayMode tests for `ComponentPool`

### Changed
- `LayerUtilities` to `LayerExtensions`

## [1.1.0]
### Added
- Analytics menu for configuring Analytics wrapper preprocessor defines
- Utility methods for preprocessor defines / directives

### Removed
- Android utilities

### Fixed
- Empty folders causing warnings when the package is consumed

## [1.0.0]
### Added
- Many basic utility methods for everyday use
- Unity Analytics wrapper
- Object/Component pool