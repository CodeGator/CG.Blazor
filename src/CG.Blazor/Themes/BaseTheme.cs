
namespace CG.Blazor.Themes;

/// <summary>
/// This class is a base implementation of a default MudBlazor UI theme.
/// </summary>
/// <typeparam name="T">The type of associated concrete theme</typeparam>
public abstract class BaseTheme<T> : MudTheme 
    where T : BaseTheme<T>, new()
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the singleton theme instance.
    /// </summary>
    private static BaseTheme<T>? _instance;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="BaseTheme{T}"/>
    /// class.
    /// </summary>
    protected BaseTheme()
    {
        // Create the default palette
        Palette = new Palette()
        {
            Primary = "#594AE2",
            Secondary = "#ff4081ff",
            Tertiary = "#1ec8a5ff",
            Background = "#ffffffff",
            AppbarBackground = "#594ae2ff",
            DrawerBackground = "#FFF",
            DrawerText = "rgba(0,0,0, 0.7)",
            Success = "#06d79c"
        };

        // Create the default layout.
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "6px",
        };

        // Create the default typography.
        Typography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            H1 = new H1()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "6rem",
                FontWeight = 300,
                LineHeight = 1.167,
                LetterSpacing = "-.01562em"
            },
            H2 = new H2()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "3.75rem",
                FontWeight = 300,
                LineHeight = 1.2,
                LetterSpacing = "-.00833em"
            },
            H3 = new H3()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "3rem",
                FontWeight = 400,
                LineHeight = 1.167,
                LetterSpacing = "0"
            },
            H4 = new H4()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "2.125rem",
                FontWeight = 400,
                LineHeight = 1.235,
                LetterSpacing = ".00735em"
            },
            H5 = new H5()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "1.5rem",
                FontWeight = 400,
                LineHeight = 1.334,
                LetterSpacing = "0"
            },
            H6 = new H6()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "1.25rem",
                FontWeight = 400,
                LineHeight = 1.6,
                LetterSpacing = ".0075em"
            },
            Button = new Button()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 500,
                LineHeight = 1.75,
                LetterSpacing = ".02857em"
            },
            Body1 = new Body1()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "1rem",
                FontWeight = 400,
                LineHeight = 1.5,
                LetterSpacing = ".00938em"
            },
            Body2 = new Body2()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            Caption = new Caption()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.66,
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2()
            {
                FontFamily = new[] { "Poppins", "Roboto", "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 500,
                LineHeight = 1.57,
                LetterSpacing = ".00714em"
            }
        };

        // Create the default shadows.
        Shadows = new Shadow();

        // Create the default z-index.
        ZIndex = new ZIndex();
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a singleton instance of <see cref="BaseTheme{T}"/>
    /// </summary>
    /// <returns>The singleton instance of <see cref="BaseTheme{T}"/></returns>
    public static BaseTheme<T> Instance()
    {
        // Should we create the instance?
        if (null == _instance)
        {
            // Yup, create it.
            _instance = new T();
        }

        // Return the instance.
        return _instance;
    }

    #endregion
}
