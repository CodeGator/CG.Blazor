using System;

namespace CG.Blazor.QuickStart.Plugin.Services
{
public class SampleService
{
    public string GetTimeNow()
    {
        return $"{DateTime.Now}";
    }
}
}
