# XDWS Libraries
This repository contains common code files used by other XDWS projects in .NET platforms. It contains the following components:

* **Xlfdll.Core**: Contains data structures and methods to extend basic data types and I/O operations in .NET platforms, provided as the abstracted base of other XDWS components and projects
* **Xlfdll.Windows**: Contains methods and data structures for encapsulating OS facilities specific to Windows
* **Xlfdll.Windows.Forms**: Contains extensions and additional dialogs for use in .NET Windows Forms UI framework
* **Xlfdll.Windows.Presentation**: Contains extensions and additional dialogs, converters, and styles in resource dictionaries for use in Windows Presentation Foundation (WPF) UI framework
* **Xlfdll.Web**: Contains extensions and additional resources for Web development

## Development Prerequisites
For most of the .NET components, the following platforms are recommended:

* **.NET Framework 4.6.2+**
* **Visual Studio 2015+**

Most of the methods and data types should be able to work in latest .NET Core platform, though this is not tested yet. The majority of the code files should also be able to use in older versions of .NET Framework, with no or minor modifications (possibly except the ones that use latest C# language features such as ObservableObject class in Xlfdll.Windows.Presentation component). 