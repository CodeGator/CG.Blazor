
using MudBlazor;
using System.Collections;
using static MudBlazor.CategoryTypes;

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

    /// <summary>
    /// This field contains the selected assembly model. 
    /// </summary>
    internal protected AssemblyModel? _selectedItem;

    /// <summary>
    /// This field contains the selcted row number, for the table.
    /// </summary>
    internal protected int _selectedRowNumber = -1;

    /// <summary>
    /// This field contains a reference to the mud table.
    /// </summary>
    internal protected MudTable<AssemblyModel>? _mudTable;

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
                Company = !string.IsNullOrEmpty(x.ReadCompany()) ? x.ReadCompany() : "unknown",
                Description = !string.IsNullOrEmpty(x.ReadDescription()) ? x.ReadDescription() : "unknown",
                Copyright = !string.IsNullOrEmpty(x.ReadCopyright()) ? x.ReadCopyright() : "unknown"
            }).OrderBy(x => x.Company).ThenBy(x => x.Name)
            .ToList();

        // Give the base class a chance.
        await base.OnInitializedAsync();
    }

    // *******************************************************************

    /// <summary>
    /// This method calculates a dynamic class for each row in the table.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <param name="rowNumber">The row number for the associated row.</param>
    /// <returns>A CSS class string for the given table row.</returns>
    protected string SelectedRowClassFunc(
        AssemblyModel? element, 
        int rowNumber
        ) 
    {
        if (_selectedRowNumber == rowNumber) 
        {
            _selectedRowNumber = -1;
            return string.Empty;
        }

        else if (_mudTable.SelectedItem != null && 
            _mudTable.SelectedItem.Equals(element)
            ) 
        {
            _selectedRowNumber = rowNumber;
            return "selected";
        }

        return string.Empty;
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
    /// This property contains the description of the assembly.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// This property contains the copyright of the assembly.
    /// </summary>
    public string Copyright { get; set; } = null!;

    /// <summary>
    /// This property contains the creation date of the assembly.
    /// </summary>
    public DateTime CreationDate { get; set; }

    #endregion
}
