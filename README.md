# CG.Blazor: 

---
[![Build Status](https://dev.azure.com/codegator/CG.Blazor/_apis/build/status/CodeGator.CG.Blazor?branchName=master)](https://dev.azure.com/codegator/CG.Blazor/_build/latest?definitionId=4&branchName=master)
[![Github docs](https://img.shields.io/static/v1?label=Documentation&message=online&color=blue)](https://codegator.github.io/CG.Blazor/index.html)
[![NuGet downloads](https://img.shields.io/nuget/dt/CG.Blazor.svg?style=flat)](https://nuget.org/packages/CG.Blazor)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/codegator/CG.Blazor/4)

#### What does it do?
The package contains Blazor related abstractions used by other CodeGator packages.

#### Commonly used types:
* Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions
* CG.Blazor.Views.ViewBase
* CG.Blazor.ViewModels.IViewModel
* CG.Blazor.ViewModels.ViewModelBase
* CG.Blazor.Plugins.IPluginModule
* CG.Blazor.Options.PluginOptions
* CG.Blazor.Options.ModuleOptions

#### What platform(s) does it support?
* .NET Standard 2.x or higher

#### How do I install it?
The binary is hosted on [NuGet](https://www.nuget.org/packages/CG.Blazor). To install the package using the NuGet package manager:

PM> Install-Package CG.Blazor

#### How do I contact you?
If you've spotted a bug in the code please use the project Issues [HERE](https://github.com/CodeGator/CG.Blazor/issues)

---

#### Quick Start
We have a small sample [HERE](https://github.com/CodeGator/CG.Blazor/samples/QuickStart) that demonstrated how to use the CG.Blazor library.

Otherwise, here are the steps for integrating CG.Blazor into your project:

- Open a new, or existing server-side Blazor project. (Sorry, haven't tested anything with client-side Blazor yet)
- Add a reference to the CG.Blazor nuget package in your project.
- If you want to play with plugins, add a new, or existing Razor Class Library project to your solution. Then, either add a reference to that project, in your main Blazor project, or, otherwise arrange for your Razor Class Library to be available 




