# Common Scripts
This library contains scripts I commonly use in my games.

## Installation
Packaged using Unity's `Assets->Export Package` menu. So you can import the project into Unity using the latest *.unitypackage file on the Releases page using `Assets->Import Package`, or by using the Package Manager and this repo's git URL.

## Changes
You might notice that installing this package adds Analytics and Logging tabs to the Unity top menu bar. These are intended for use with the `AnalyticsWrapper` and `LogWrapper` classes, and there is currently no way to configure an installation without these changes.

## Uninstalling
The aforementioned Analytics and Logging menus are implemented using preprocessor defines. So, if uninstalling this package, you may want to deselect all options in these menus, else you may end up with some extraneous preprocessor defines left over.