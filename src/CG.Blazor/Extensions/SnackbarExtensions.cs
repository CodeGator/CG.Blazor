
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
    /// <param name="exception">The exception to use for the operation.</param>
    /// <param name="title">The title to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    public static async Task ShowErrorBox(
        this IDialogService dialogService,
        Exception exception,
        string title = "Something broke!"
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(dialogService, nameof(dialogService))
            .ThrowIfNull(exception, nameof(exception));

        // Should we supply a default title?
        if (string.IsNullOrWhiteSpace(title)) 
        {
            title = "Something broke!";
        }

        // Create the body of the message.
        var sb = new StringBuilder();
        sb.Append($"<b>{title}</b><br /><br />");
        sb.Append($"<ul>");
        sb.Append($"<li>{exception.Message}</li>");

        // Does the base error contain data?
        if (exception.GetBaseException().Data.Count > 0)
        {
            // Loop through the data.
            foreach (var obj in exception.GetBaseException().Data)
            {
                // Add each bit of data.
                sb.Append($"<li>{obj}</li>");
            }

            sb.Append($"</ul><br /><ul>");
        }

        // Get the exception message.
        var msg = exception.GetBaseException().Message;

        // Look for delimiters in the message.
        var parts = msg.Split('|');

        // Did wee find any?
        if (parts.Length > 1)
        {
            // Loop through the parts.
            foreach (var part in parts)
            {
                // Add each part.
                sb.Append($"<li>{part}</li>");
            }
        }
        else 
        {
            // Otherwise, just add the message verbatim.
            sb.Append($"<li>{msg}</li>");
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
