
namespace CG.Blazor.Components;

/// <summary>
/// This class is the code-behind for the <see cref="Footer"/> component.
/// </summary>
public partial class Footer
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains any child content.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.General.Behavior)]
    public RenderFragment ChildContent { get; set; } = null!;

    #endregion
}
