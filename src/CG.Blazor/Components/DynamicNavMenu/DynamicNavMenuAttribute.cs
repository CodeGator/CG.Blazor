
namespace CG.Blazor.Components;

/// <summary>
/// This class is an attribute that denotes an associated dynamic navigation 
/// menu.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DynamicNavMenuAttribute : Attribute
{
    /// <summary>
    /// This property contains the title for the menu.
    /// </summary>
    public string MenuTitle { get; } = null!;

    /// <summary>
    /// This property contains the icon for the menu.
    /// </summary>
    public string? MenuIcon { get; set; }

    /// <summary>
    /// This property contains the color for the menu.
    /// </summary>
    public string? MenuColor { get; set; }

    /// <summary>
    /// This property contains the title for the group menu.
    /// </summary>
    public string? GroupTitle { get; set; }

    /// <summary>
    /// This property contains the icon for the group menu.
    /// </summary>
    public string? GroupIcon { get; set; }

    /// <summary>
    /// This property contains the color for the group menu.
    /// </summary>
    public string? GroupColor { get; set; }

    /// <summary>
    /// This property contains the route for the menu.
    /// </summary>
    public string? MenuRoute { get; set; }

    /// <summary>
    /// This property contains the match for the menu.
    /// </summary>
    public NavLinkMatch? MenuMatch { get; set; }

    /// <summary>
    /// This property indicates whether or not the menu is disabled.
    /// </summary>
    public bool MenuDisabled { get; set; }

    /// <summary>
    /// This property indicates the relative priority for the menu.
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// This constructor creates a new instance of the <see cref="DynamicNavMenuAttribute"/>
    /// </summary>
    /// <param name="menuTitle">The title for the menu.</param>
    public DynamicNavMenuAttribute(
        string menuTitle
        )
    {
        // Save the reference(s).
        MenuTitle = menuTitle;
    }
}
