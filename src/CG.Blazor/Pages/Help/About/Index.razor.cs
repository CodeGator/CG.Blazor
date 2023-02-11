
using System.Globalization;
using System.Runtime.InteropServices;

namespace CG.Blazor.Pages.Help.About;

/// <summary>
/// This class is the code-behind for the <see cref="Index"/> page.
/// </summary>
public partial class Index
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains a reference to breadcrumbs for the view.
    /// </summary>
    internal protected readonly List<BreadcrumbItem> _crumbs = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Help", href: "/help", disabled: true),
        new BreadcrumbItem("About", href: "/help/about")
    };

    /// <summary>
    ///  This field contains the list of assemblies.
    /// </summary>
    internal protected IEnumerable<AssemblyModel>? _assemblies;

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called by the framework to initialize the page.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        // Build the list of assemblies.
        _assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.IsDynamic)
            .Select(x => new AssemblyModel()
            {
                Name = !string.IsNullOrEmpty(x.GetName().Name ?? "unknown") ? x.GetName().Name ?? "unknown" : "unknown",
                Version = !string.IsNullOrEmpty(x.ReadInformationalVersion()) ? x.ReadInformationalVersion() : "unknown",
                Company = !string.IsNullOrEmpty(x.ReadCompany()) ? x.ReadCompany() : "unknown"
            }).OrderBy(x => x.Company).ThenBy(x => x.Name)
            .ToList();

        // Give the base class a chance.
        await base.OnInitializedAsync();
    }

    #endregion

}

/// <summary>
/// This class represents the model for a .NET assembly.
/// </summary>
public class AssemblyModel
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the name of the assembly.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the version of the assembly.
    /// </summary>
    public string Version { get; set; } = null!;

    /// <summary>
    /// This property contains the company of the assembly.
    /// </summary>
    public string Company { get; set; } = null!;

    /// <summary>
    /// This property contains the creation date of the assembly.
    /// </summary>
    public DateTime CreationDate { get; set; }

    #endregion
}
