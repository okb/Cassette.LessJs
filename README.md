Cassette.LessJs
===============

NOT MAINTAINED
==============

Version of the Less compiler for Cassette.Net using the original Js Libraries

Cassette.Less uses [dotless](https://github.com/dotless/dotless).  This is a great library and supports mono,
but is based on an older version of Less.  

This library implements a Less compiler using the [windows script host version of less.js for windows](https://github.com/duncansmart/less.js-windows/tree/windows-script-host).
This strictly limits usage to Windows OS, but has the advantage of matching the implementation of less compilation
in the [Web Essentials Visual Studio extension](https://github.com/madskristensen/WebEssentials2013).

OKB Note:
---------
The compiler needs file acces to write the resources to disc before running the compiler. this might pose a consern for some.

##Usage NancyFx:

Add this method to the project your bootstrapper resides in. It will add the assembly to cassettes assemblyscan.
wich is a separate container than Nancys container.

```cs
using Nancy.Bootstrapper;
namespace Web.Registrations
{
public class CassetteNancyPluginRegistrations : ApplicationRegistrations
    {
        public CassetteNancyPluginRegistrations()
        {
            AppDomainAssemblyTypeScanner.AddAssembliesToScan(assembly => assembly.FullName.Contains("Cassette.LessJs"));
        }
    }
}
```

##Usage MVC

To use it, add the DLL to your project and replace the Less configuration with the corresponding LessJs configuration

###Old
```cs
container.Register(Cassette.Stylesheets.ILessCompiler, Cassette.Stylesheets.LessCompiler).AsMultiInstance();
container.Register(typeof(Cassette.IFileSearchModifier<Cassette.Stylesheets.StylesheetBundle>),
                Cassette.Stylesheets.LessFileSearchModifier)).AsSingleton();
```
###New
```cs
container.Register(Cassette.Stylesheets.ILessJsCompiler, Cassette.Stylesheets.LessJsCompiler).AsMultiInstance();
container.Register(typeof(Cassette.IFileSearchModifier<Cassette.Stylesheets.StylesheetBundle>),
                Cassette.Stylesheets.LessJsFileSearchModifier)).AsSingleton();
```
