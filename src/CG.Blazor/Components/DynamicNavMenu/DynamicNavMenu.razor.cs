
namespace CG.Blazor.Components;

/// <summary>
/// This class is the code-behind for the <see cref="DynamicNavMenu"/> component.
/// </summary>
public partial class DynamicNavMenu : MudNavMenu
{
    // *******************************************************************
    // Types.
    // *******************************************************************

    #region Types

    /// <summary>
    /// This class contains properties for a menu item.
    /// </summary>
    public class _MenuItem
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the title for the group menu 
        /// </summary>
        public string? GroupTitle { get; set; }

        /// <summary>
        /// This property contains the icon for the group menu 
        /// </summary>
        public string? GroupIcon { get; set; }

        /// <summary>
        /// This property contains the color for the group menu 
        /// </summary>
        public Color? GroupColor { get; set; }

        /// <summary>
        /// This property contains the title for the menu 
        /// </summary>
        public string? MenuTitle { get; set; }

        /// <summary>
        /// This property contains the color for the menu 
        /// </summary>
        public Color? MenuColor { get; set; }

        /// <summary>
        /// This property contains the icon for the menu 
        /// </summary>
        public string? MenuIcon { get; set; }

        /// <summary>
        /// This property contains the route for the menu 
        /// </summary>
        public string? MenuRoute { get; set; }

        /// <summary>
        /// This property indicates whether or not the menu is disabled.
        /// </summary>
        public bool MenuDisabled { get; set; }

        /// <summary>
        /// This property contains the match for the menu 
        /// </summary>
        public NavLinkMatch? MenuMatch { get; set; }

        /// <summary>
        /// This property indicates the relative priority for the menu.
        /// </summary>
        public int? Priority { get; set; }

        #endregion
    }

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the list of menu items.
    /// </summary>
    internal protected readonly List<_MenuItem> _menuItems = new();

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property indicates whether or not to render any child content
    /// before the dynamic menu elements. If <c>false</c>, child content
    /// is rendered after dynamic menu elements.
    /// </summary>
    public bool ChildContentOnTop { get; set; }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called to initialize the component.
    /// </summary>
    protected override void OnInitialized()
    {
        // Log what we are about to do.
        Logger.LogDebug(
            "Searching for assemblies with razor pages decorated with a {name} attribute",
            nameof(DynamicNavMenuAttribute)
            );

        // Look for any assemblies with decorated razor pages.
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x =>
            x.GetTypes().Any(y => y.CustomAttributes.Any(y => y.AttributeType == typeof(DynamicNavMenuAttribute)))
            );

        // Did we find any?
        if (assemblies.Any())
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Looping through {count} assemblies with razor pages decorated with a {name} attribute",
                nameof(DynamicNavMenuAttribute)
                );

            // Loop through the assemblies.
            foreach (var assembly in assemblies)
            {
                // Log what we are about to do.
                Logger.LogDebug(
                    "Searching for razor pages decorated with a {name} attribute",
                    nameof(DynamicNavMenuAttribute)
                    );

                // Look for any classes decorated with the plugin page attribute.
                var decoratedTypes = assembly.ExportedTypes.Where(x =>
                    x.CustomAttributes.Any(y => y.AttributeType == typeof(DynamicNavMenuAttribute))
                    );

                // Did we find any?
                if (decoratedTypes.Any())
                {
                    // Log what we are about to do.
                    Logger.LogDebug(
                        "Looping through {count} types of razor pages decorated with a {name} attribute",
                        nameof(DynamicNavMenuAttribute)
                        );

                    // Loop though the types.
                    foreach (var type in decoratedTypes)
                    {
                        // Log what we are about to do.
                        Logger.LogDebug(
                            "Searching for a {name} attribute for page: {page}",
                            nameof(DynamicNavMenuAttribute),
                            type.Name
                            );

                        // Look for a plugin page attribute.
                        var pluginPageAttribute = type.CustomAttributes.FirstOrDefault(y =>
                            y.AttributeType == typeof(DynamicNavMenuAttribute)
                            );

                        // Can we take a shortcut?
                        if (pluginPageAttribute is null)
                        {
                            // Log what happened.
                            Logger.LogWarning(
                                "Unable to find a {attr} attribute for razor page: {type}",
                                nameof(DynamicNavMenuAttribute),
                                type.Name
                                );

                            continue; // Nothing to do!
                        }

                        // Log what we are about to do.
                        Logger.LogDebug(
                            "Recovering menu properties from a {name} attribute",
                            nameof(DynamicNavMenuAttribute)
                            );

                        // Look for the menu group title.
                        var menuGroupTitle = pluginPageAttribute.NamedArguments.FirstOrDefault(x =>
                            x.MemberName == nameof(DynamicNavMenuAttribute.GroupTitle)
                            );

                        // Look for the menu group icon.
                        var menuGroupIcon = pluginPageAttribute.NamedArguments.FirstOrDefault(x =>
                            x.MemberName == nameof(DynamicNavMenuAttribute.GroupIcon)
                            );

                        // Look for the menu group color.
                        var menuGroupColor = pluginPageAttribute.NamedArguments.FirstOrDefault(x =>
                            x.MemberName == nameof(DynamicNavMenuAttribute.GroupColor)
                            );

                        // Look for the menu title.
                        var menuTitle = $"{pluginPageAttribute.ConstructorArguments.FirstOrDefault()}"
                            .Trim('"');

                        // Look for the menu icon.
                        var menuIcon = pluginPageAttribute.NamedArguments.FirstOrDefault(x =>
                            x.MemberName == nameof(DynamicNavMenuAttribute.MenuIcon)
                            );

                        // Look for the menu color.
                        var menuColor = pluginPageAttribute.NamedArguments.FirstOrDefault(x =>
                            x.MemberName == nameof(DynamicNavMenuAttribute.MenuColor)
                            );

                        // Look for the menu match.
                        var menuMatch = pluginPageAttribute.NamedArguments.FirstOrDefault(x =>
                            x.MemberName == nameof(DynamicNavMenuAttribute.MenuMatch)
                            );

                        // Look for the menu disabled flag.
                        var menuDisabled = pluginPageAttribute.NamedArguments.FirstOrDefault(x =>
                            x.MemberName == nameof(DynamicNavMenuAttribute.MenuDisabled)
                            );

                        // Look for the menu route.
                        var menuRoute = pluginPageAttribute.NamedArguments.FirstOrDefault(x =>
                            x.MemberName == nameof(DynamicNavMenuAttribute.MenuRoute)
                            );

                        // Are we missing a route?
                        var route = "";
                        if (menuRoute.TypedValue.Value is null)
                        {
                            // Log what we are about to do.
                            Logger.LogDebug(
                                "No route specified in the {name} attribute - looking in the page: {page}",
                                nameof(DynamicNavMenuAttribute),
                                type.Name
                                );

                            // Look for a route attribute.
                            var routeAttribute = type.CustomAttributes.FirstOrDefault(y =>
                                y.AttributeType == typeof(RouteAttribute)
                                );

                            // Are we missing a route attribute?
                            if (routeAttribute is null)
                            {
                                // Log what happened.
                                Logger.LogWarning(
                                    "Unable to find a route for menu group: {group}, item: {item}",
                                    $"{menuGroupTitle.TypedValue.Value}",
                                    $"{menuTitle}"
                                    );

                                continue; // Nothing to do!
                            }

                            // Log what we are about to do.
                            Logger.LogDebug(
                                "Assigning route: {route} to menu group: {group}, title: {title}",
                                $"{routeAttribute.ConstructorArguments.First().Value}",
                                $"{menuGroupTitle.TypedValue.Value}",
                                $"{menuTitle}"
                                );

                            // Assign the route from the attribute.
                            route = $"{routeAttribute.ConstructorArguments.First().Value}";
                        }
                        else
                        {
                            // Log what we are about to do.
                            Logger.LogDebug(
                                "Assigning route: {route} to menu group: {group}, title: {title}",
                                $"{menuRoute.TypedValue.Value}",
                                $"{menuGroupTitle.TypedValue.Value}",
                                $"{menuTitle}"
                                );

                            // Assign the route from the attribute.
                            route = $"{menuRoute.TypedValue.Value}";
                        }

                        // Log what we are about to do.
                        Logger.LogDebug(
                            "Parsing enum values"
                            );

                        // Try to parse the group color.
                        if (!Enum.TryParse<Color>($"{menuGroupColor.TypedValue.Value}", true, out var gColor))
                        {
                            gColor = Color.Inherit;
                        }

                        // Try to parse the menu color.
                        if (!Enum.TryParse<Color>($"{menuColor.TypedValue.Value}", true, out var mColor))
                        {
                            mColor = Color.Inherit;
                        }

                        // Try to parse the match.
                        if (!Enum.TryParse<NavLinkMatch>($"{menuMatch}", true, out var nlMatch))
                        {
                            nlMatch = NavLinkMatch.Prefix;
                        }

                        // Try to parse the disabled flag.
                        if (!bool.TryParse($"{menuDisabled}", out var disabled))
                        {
                            disabled = false;
                        }

                        // Log what we are about to do.
                        Logger.LogDebug(
                            "Creating menu item group: {group}, title: {title}, route: {route}",
                            $"{menuGroupTitle.TypedValue.Value}",
                            $"{menuTitle}",
                            route
                            );

                        // Create the menu item.
                        var menuItem = new _MenuItem()
                        {
                            GroupTitle = $"{menuGroupTitle.TypedValue.Value}",
                            GroupIcon = $"{menuGroupIcon.TypedValue.Value}",
                            GroupColor = gColor,
                            MenuTitle = $"{menuTitle}",
                            MenuIcon = $"{menuIcon.TypedValue.Value}",
                            MenuColor = mColor,
                            MenuMatch = nlMatch,
                            MenuDisabled = disabled,
                            MenuRoute = route,
                        };

                        // Add the menu item.
                        _menuItems.Add( menuItem );
                    }
                }
                else
                {
                    // Log what happened.
                    Logger.LogDebug(
                        "No razor pages decorated with a {name} attribute were found",
                        nameof(DynamicNavMenuAttribute)
                        );
                }
            }
        }
        else
        {
            // Log what happened.
            Logger.LogDebug(
                "No assemblies with razor pages decorated with a {name} attribute were found",
                nameof(DynamicNavMenuAttribute)
                );
        }

        // Give the base class a chance.
        base.OnInitialized();
    }

    #endregion
}
