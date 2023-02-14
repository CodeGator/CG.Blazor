
namespace CG.Blazor.Components
{
    /// <summary>
    /// This class is the code-behind for the <see cref="WizardPanel"/> 
    /// razor component.
    /// </summary>
    public partial class WizardPanel : MudComponentBase, IAsyncDisposable
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field indicate whether the component has been disposed.
        /// </summary>
        internal protected bool _disposed;

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the child content for the panel.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// This property contains the description to display when this panel is active.
        /// This value overrides the <see cref="Wizard.Description"/> property, when
        /// this panel is selected.
        /// </summary>
        [Parameter]
        public string? Description { get; set; }

        /// <summary>
        /// This property contains the parent of the panel. 
        /// </summary>
        [CascadingParameter]
        internal protected Wizard? Parent { get; set; }

        /// <summary>
        /// This property contains the title to display when this panel is active.
        /// This value overrides the <see cref="Wizard.Title"/> property, when
        /// this panel is selected.
        /// </summary>
        [Parameter]
        public string? Title { get; set; }

        /// <summary>
        /// This property indicates whether the header, on the parent, should
        /// be hidden when this panel is active. This value overrides the 
        /// <see cref="Wizard.HideHeader"/> property, when this panel is selected.
        /// </summary>
        [Parameter]
        public bool HideHeader { get; set; }

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
            // Add ourselves to the parent wizard.
            Parent?.AddPanel(this);

            // Give the base class a chance.
            base.OnInitialized();
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to dispose of the component.
        /// </summary>
        /// <returns>A task to perform the operation.</returns>
        public async ValueTask DisposeAsync()
        {
            // Have we already been disposed?
            if (_disposed == true) 
            { 
                return; // Nothing to do.
            }

            // Mark that we've been disposed.
            _disposed = true;

            // Do we have a parent reference?
            if (Parent is not null)
            {
                // Cleanup the reference.
                await Parent.RemovePanel(this);
            }

            // Prevent derived types from having to implement IDisposable.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
