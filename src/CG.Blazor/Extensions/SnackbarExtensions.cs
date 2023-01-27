
namespace MudBlazor;

/// <summary>
/// This class contains extension methods related to the <see cref="IDialogService"/>
/// type.
/// </summary>
public static class DialogServiceExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method shows an error message, formatted from the given exception.
    /// </summary>
    /// <param name="dialogService">The dialog service to use for the operation.</param>
    /// <param name="ex">The exception to use for the operation.</param>
    /// <param name="title">The title to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    public static async Task ShowErrorBox(
        this IDialogService dialogService,
        Exception ex,
        string title = "Something broke!"
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(dialogService, nameof(dialogService))
            .ThrowIfNull(ex, nameof(ex));

        // Should we supply a default title?
        if (string.IsNullOrWhiteSpace(title)) 
        {
            title = "Something broke!";
        }

        // Create the body of the message.
        var sb = new StringBuilder();
        sb.Append($"<b>{title}</b><br /><br />");
        sb.Append($"<ul>");
        sb.Append($"<li>{ex.Message}</li>");

        // Does the base error contain data?
        if (ex.GetBaseException().Data.Count > 0)
        {
            // Loop through the data.
            foreach (var obj in ex.GetBaseException().Data)
            {
                // Add each bit of data.
                sb.Append($"<li>{obj}</li>");
            }            
        }
        else
        {
            // Otherwise, just add the message from the base error.
            sb.Append($"<li>{ex.GetBaseException().Message}</li>");
        }
        sb.Append($"</ul>");

        // Display the formatted error message.
        await dialogService.ShowMessageBox(
                title: title,
                markupMessage: (MarkupString)sb.ToString()
                );
    }

    #endregion
}
