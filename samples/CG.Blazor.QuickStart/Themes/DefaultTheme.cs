
using MudBlazor;

namespace CG.Blazor.QuickStart.Themes;

/// <summary>
/// This class demonstrates deriving from <see cref="BaseTheme{T}"/>
/// </summary>
public class DefaultTheme : BaseTheme<DefaultTheme> 
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="DefaultTheme"/>
    /// class
    /// </summary>
    public DefaultTheme()
    {
        // Create the default palette
        Palette.Primary = Colors.BlueGrey.Default;
        Palette.Secondary = Colors.Amber.Default;
        Palette.Tertiary = Colors.Green.Lighten2;
        Palette.AppbarBackground = Colors.Teal.Default;
    }

    #endregion
}
