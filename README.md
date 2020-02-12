# XDWS Libraries - .NET Platform
This repository contains common code files used by other XDWS projects in .NET platforms. It contains the following components:

* **Xlfdll.Core**: Contains data structures and methods to extend basic data types and I/O operations in .NET platforms, provided as the abstracted base of other XDWS components and projects
* **Xlfdll.Data.Access**: Contains data operator and system constant classes for accessing Microsoft Access databases
* **Xlfdll.Data.MySQL**: Contains data operator and system constant classes for accessing MySQL databases. Requires MySQL Connector/NET 8.0+ to work (lower versions can also be used by minor modifications)
* **Xlfdll.Data.SQLite**: Contains data operator and system constant classes for accessing SQLite databases. Requires the official System.Data.SQLite ADO.NET provider
* **Xlfdll.Network.SecureShell**: Contains classes for Secure Shell (SSH) operations. Requires Renci's SSH.NET library
* **Xlfdll.Windows**: Contains methods and data structures for encapsulating OS facilities specific to Windows
* **Xlfdll.Windows.Forms**: Contains extensions and additional dialogs for use in .NET Windows Forms UI framework
* **Xlfdll.Windows.Presentation**: Contains extensions and additional dialogs, converters, and styles in resource dictionaries for use in Windows Presentation Foundation (WPF) UI framework
* **Xlfdll.Xamarin.Forms**: Contains support extensions and helpers for use in Xamarin.Forms framework
* **Xlfdll.Xamarin.Android**: Contains specific support components (localization, etc.) for use in Xamarin.iOS platform
* **Xlfdll.Xamarin.iOS**: Contains specific support components (localization, etc.) for use in Xamarin.Android platform

## Development Prerequisites
For most of the .NET components, the following platforms are recommended:

* **.NET Framework 4.8**
* **Visual Studio 2015+**

Most of the methods and data types should be able to work in latest .NET Core platform, though this is not tested yet. The majority of the code files should also be able to use in older versions of .NET Framework, with no or minor modifications (possibly except the ones that use latest C# language features, such as ObservableObject class in Xlfdll.Windows.Presentation component). 

Before the build, generate-build-number.sh needs to be executed in a Git / Bash shell to generate build information code file (BuildInfo.cs).
