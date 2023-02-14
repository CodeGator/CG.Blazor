
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
    /// <param name="caption">The caption to use for the operation.</param>
    /// <param name="delimiter">The delimiter to use for parsing embedded
    /// lists in the error message.</param>
    /// <returns>A task to perform the operation.</returns>
    public static async Task ShowErrorBox(
        this IDialogService dialogService,
        Exception exception,
        string title = "Something broke!",
        string caption = "",
        char delimiter = '|'
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(dialogService, nameof(dialogService))
            .ThrowIfNull(exception, nameof(exception));

        // Create the body of the message.
        var sb = new StringBuilder();

        // Was a title specified?
        if (!string.IsNullOrEmpty(title))
        {
            sb.Append($"<b>{title}</b><br /><br />");
        }
        sb.Append($"<ul><li>{exception.Message}</li>");

        // Does the base error contain data?
        if (exception.GetBaseException().Data.Count > 0)
        {
            // Loop through the data.
            foreach (var obj in exception.GetBaseException().Data)
            {
                // Add each bit of data.
                sb.Append($"<li>{obj}</li>");
            }

            // Close the list and start a new one.
            sb.Append($"</ul><br /><ul>");
        }

        // Get the exception message.
        var msg = exception.GetBaseException().Message;

        // Look for delimiters in the message.
        var parts = msg.Split(delimiter);

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

        // End the list.
        sb.Append($"</ul>");

        // Display the formatted error message.
        await dialogService.ShowMessageBox(
                title: caption,
                markupMessage: (MarkupString)sb.ToString()
                );
    }

    #endregion
}
