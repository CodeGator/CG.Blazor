using CG.Blazor.ViewModels;
using System;
using System.Windows.Input;

namespace CG.Blazor.QuickStart.Plugin.ViewModels
{
public interface ISampleViewModel : IViewModel
{
    ICommand Command { get; }
    string A { get; set; }
    string B { get; set; }
}
}
