using CG.Blazor.Commands;
using CG.Blazor.QuickStart.Plugin.Services;
using CG.Blazor.ViewModels;
using System;
using System.Windows.Input;

namespace CG.Blazor.QuickStart.Plugin.ViewModels
{
    public class SampleViewModel : ViewModelBase, ISampleViewModel
    {
        protected SampleService _sampleService;
        protected string _a;
        protected string _b;

        protected ICommand _command;

        public string A
        {
            get { return _a; }
            set
            {
                SetValue(ref _a, value);
            }
        }

        public string B 
        { 
            get { return _b; }
            set
            {
                SetValue(ref _b, value);
            } 
        }

        public ICommand Command { get; }

        public SampleViewModel(
            SampleService sampleService
            )
        {
            _sampleService = sampleService;

            A = "press me!";
            Command = new DelegateCommand(() => 
            {
                A = _sampleService.GetTimeNow();
                B = "<-- The time came from a service!";
            });
        }
    }
}
