
namespace CG.Blazor.Components
{
    /// <summary>
    /// This class is the code-behind for the <see cref="Wizard"/> razor 
    /// component.
    /// </summary>
    public partial class Wizard : MudComponentBase, IAsyncDisposable
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains the inner list of wizard panels.
        /// </summary>
        private readonly List<WizardPanel> _panels = new();

        /// <summary>
        /// This field indicate whether the component has been disposed.
        /// </summary>
        private bool _disposed;

        #endregion

        // *******************************************************************
        // Events.
        // *******************************************************************

        #region Events

        /// <summary>
        /// This event is raised whenever the index changes.
        /// </summary>
        [Parameter]
        public EventCallback<IndexChangedEventArgs> IndexChanged { get; set; }

        /// <summary>
        /// This event is raised whenever the wizard is finished.
        /// </summary>
        [Parameter]
        public EventCallback WizardFinished { get; set; }

        /// <summary>
        /// This event is raised whenever the wizard is cancelled.
        /// </summary>
        [Parameter]
        public EventCallback WizardCancelled { get; set; }

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties
        
        /// <summary>
        /// This property contains the action content for the component.
        /// </summary>
        [Parameter]
        public RenderFragment? ActionContent { get; set; }

        /// <summary>
        /// This property indicates whether or not to the timeline should respond
        /// to mouse clicks.
        /// </summary>
        [Parameter]
        public bool AllowActiveTimeline { get; set; }

        /// <summary>
        /// This property contains the variant for the wizard buttons.
        /// </summary>
        [Parameter]
        public Variant ButtonVariant { get; set; }

        /// <summary>
        /// This property contains the color for the cancel button.
        /// </summary>
        [Parameter]
        public Color CancelButtonColor { get; set; }

        /// <summary>
        /// This property contains the tooltip for the cancel button.
        /// </summary>
        [Parameter]
        public string CancelButtonTooltip { get; set; } = null!;

        /// <summary>
        /// This property indicates whether the wizard can skip ahead
        /// pages, or not.
        /// </summary>
        [Parameter]
        public bool CanSkipAhead { get; set; }

        /// <summary>
        /// This property contains the child content for the component.
        /// </summary>
        [Parameter]
        public RenderFragment? WizardContent { get; set; }

        /// <summary>
        /// This property contains the CSS class for the component.
        /// </summary>
        protected string Classname => new CssBuilder("mud-wizard")
            .AddClass(Class)
            .Build();

        /// <summary>
        /// This property indicates whether the next button should be disabled, or not. 
        /// </summary>
        [Parameter]
        public bool DisableNextButton { get; set; }

        /// <summary>
        /// This property indicates whether the previous button should be disabled, or not. 
        /// </summary>
        [Parameter]
        public bool DisablePreviousButton { get; set; }

        /// <summary>
        /// This property contains the description for the wizard.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = null!;

        /// <summary>
        /// This property contains the color for the wizard descriptions.
        /// </summary>
        [Parameter]
        public Color DescriptionColor { get; set; }

        /// <summary>
        /// This property contains the typography for the wizard descriptions.
        /// </summary>
        [Parameter]
        public Typo DescriptionTypo { get; set; }

        /// <summary>
        /// This property contains the color for timeline dots.
        /// </summary>
        [Parameter]
        public Color DotColor { get; set; }

        /// <summary>
        /// This property contains the elevation for the component.
        /// </summary>
        [Parameter]
        public int Elevation { set; get; }

        /// <summary>
        /// This property contains the color for the finish button.
        /// </summary>
        [Parameter]
        public Color FinishButtonColor { get; set; }

        /// <summary>
        /// This property contains the tooltip for the finish button.
        /// </summary>
        [Parameter]
        public string FinishButtonTooltip { get; set; } = null!;

        /// <summary>
        /// This property indicates whether to hide the actions area, or not.
        /// </summary>
        [Parameter]
        public bool HideActions { get; set; }

        /// <summary>
        /// This property indicates whether to show the cancel button, or not.
        /// </summary>
        [Parameter]
        public bool HideCancelButton { get; set; }

        /// <summary>
        /// This property indicates whether to show the finish button, or not.
        /// </summary>
        [Parameter]
        public bool HideFinishButton { get; set; }

        /// <summary>
        /// This property indicates whether to hide the header area, or not.
        /// </summary>
        [Parameter]
        public bool HideHeader { get; set; }

        /// <summary>
        /// This property indicates whether to hide the header timeline, or not.
        /// </summary>
        [Parameter]
        public bool HideTimeline { get; set; }

        /// <summary>
        /// This property indicates whether the previous button should be
        /// disabled, or not. True if it should be disabled; False otherwise.
        /// </summary>
        public bool IsPreviousDisabled => DisablePreviousButton ||
            !SelectedIndex.HasValue || SelectedIndex <= 0;

        /// <summary>
        /// This property indicates whether the next button should be
        /// disabled, or not. True if it should be disabled; False otherwise.
        /// </summary>
        public bool IsNextDisabled => DisableNextButton ||
            (!SelectedIndex.HasValue || SelectedIndex >= _panels.Count - 1);

        /// <summary>
        /// This property indicates whether the finish button should be
        /// hidden, or not. True if it should be hidden; False otherwise.
        /// </summary>
        public bool IsFinishVisible =>
            !HideFinishButton && (!SelectedIndex.HasValue || SelectedIndex >= _panels.Count - 1);

        /// <summary>
        /// This property contains the color for the next button.
        /// </summary>
        [Parameter]
        public Color NextButtonColor { get; set; }

        /// <summary>
        /// This property contains the tooltip for the next button.
        /// </summary>
        [Parameter]
        public string NextButtonTooltip { get; set; } = null!;

        /// <summary>
        /// This property contains the color for the previous button.
        /// </summary>
        [Parameter]
        public Color PreviousButtonColor { get; set; }

        /// <summary>
        /// This property contains the tooltip for the previous button.
        /// </summary>
        [Parameter]
        public string PreviousButtonTooltip { get; set; } = null!;

        /// <summary>
        /// This property contains the title for the wizard.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = null!;

        /// <summary>
        /// This property indicates whether the wizard should be outlined, 
        /// or not. True to outline; False otherwise.
        /// </summary>
        [Parameter]
        public bool Outlined { get; set; }

        /// <summary>
        /// This property contains a list of the current wizard panels.
        /// </summary>
        public IReadOnlyList<WizardPanel> Panels => _panels.ToList();

        /// <summary>
        /// This property contains the index of the currently selected panel.
        /// </summary>
        [Parameter]
        public int? SelectedIndex { get; set; }

        /// <summary>
        /// This property contains the currently selected panel.
        /// </summary>
        [Parameter]
        public WizardPanel? SelectedPanel { get; set; }

        /// <summary>
        /// This property contains the color for selected timeline dot.
        /// </summary>
        [Parameter]
        public Color SelectedDotColor { get; set; }

        /// <summary>
        /// This property indicates whether the border radius should be set
        /// to zero, or not. True to set the border radius to zero; False 
        /// otherwise.
        /// </summary>
        [Parameter]
        public bool Square { get; set; }

        /// <summary>
        /// This property contains the color for the wizard title.
        /// </summary>
        [Parameter]
        public Color TitleColor { get; set; }

        /// <summary>
        /// This property contains the typography for the wizard title.
        /// </summary>
        [Parameter]
        public Typo TitleTypo { get; set; }
                
        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc />
        public void Select(
            WizardPanel panel
            )
        {
            // Get the index for the panel.
            var index = _panels.IndexOf(panel);

            // Did we find the panel?
            if (-1 != index)
            {
                // Make the selection.
                Select(index);
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method causes the wizard to navigate to the specified panel index.
        /// </summary>
        /// <param name="index">The index to use for the operation.</param>
        public void Select(
            int? index
            )
        {
            // Sanity check the index first.
            if (index == SelectedIndex)
            {
                return; // Nothing to do.
            }

            // Create arguments for the event.
            var eventArgs = new IndexChangedEventArgs()
            {
                NewIndex = index,
                CurrentIndex = SelectedIndex
            };

            // Fire the event.
            IndexChanged.InvokeAsync(eventArgs);

            // Should we cancel the navigation?
            if (eventArgs.NewIndex == eventArgs.CurrentIndex)
            {
                return; // No change to the wizard.
            }

            // Is the index within a valid range?
            if (eventArgs.NewIndex >= 0 &&
                eventArgs.NewIndex < _panels.Count)
            {
                // Update the index.
                SelectedIndex = eventArgs.NewIndex;

                // Select the current panel.
                if (eventArgs.NewIndex.HasValue)
                {
                    SelectedPanel = _panels[eventArgs.NewIndex.Value];
                }
                else
                {
                    SelectedPanel = null;
                }
            }

            // Do we have a selected panel?
            if (SelectedPanel is not null)
            {
                // Should we carry the wizard title to this page?
                if (string.IsNullOrEmpty(SelectedPanel.Title))
                {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                    SelectedPanel.Title = Title;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
                }

                // Should we carry the wizard description to this page?
                if (string.IsNullOrEmpty(SelectedPanel.Description))
                {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                    SelectedPanel.Description = Description;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
                }
            }

            // Ensure the UI is updated.
            StateHasChanged();
        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This is called when the user clicks the timeline.
        /// </summary>
        /// <param name="index">The index to use for the operation.</param>
        protected void TimelineSelect(
            int? index
            )
        {
            // Are we skipping ahead?
            if (index > SelectedIndex)
            {
                // Can we skip ahead?
                if (CanSkipAhead)
                {
                    // Select the panel.
                    Select(index);
                }
            }
            else
            {
                // Select the panel.
                Select(index);
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method adds a new wizard panel to the component.
        /// </summary>
        /// <param name="panel">The wizard panel to add.</param>
        protected internal void AddPanel(
            WizardPanel panel
            )
        {
            // Add the panel to the collection.
            _panels.Add(panel);
        }

        // *******************************************************************

        /// <summary>
        /// This method removes a panel from the component.
        /// </summary>
        /// <param name="panel">The wizard panel to remove.</param>
        protected internal Task RemovePanel(
            WizardPanel panel
            )
        {
            // Remove the panel from the collection.
            _panels.Remove(panel);

            // Return the task.
            return Task.CompletedTask;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to select the next panel in the wizard.
        /// </summary>
        protected void OnNext()
        {
            // Select the next panel.
            Select(SelectedIndex + 1);
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to select the previous panel in the wizard.
        /// </summary>
        protected void OnPrevious()
        {
            // Select the previous panel.
            Select(SelectedIndex - 1);
        }

        // *******************************************************************

        /// <summary>
        /// This method cancels the wizard.
        /// </summary>
        /// <returns>A task to perform the operation.</returns>
        protected async Task OnCancel()
        {
            // Raise the event.
            await WizardCancelled.InvokeAsync();
        }

        // *******************************************************************

        /// <summary>
        /// This method finishes the wizard.
        /// </summary>
        /// <returns>A task to perform the operation.</returns>
        protected async Task OnFinish()
        {
            // Raise the event.
            await WizardFinished.InvokeAsync();
        }

        // *******************************************************************

        /// <summary>
        /// This method is called after a render operation.
        /// </summary>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                // Try to select the first page.
                Select(0);
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to set the parameter values.
        /// </summary>
        protected override void OnParametersSet()
        {
            // Set default values.
            AllowActiveTimeline = true;
            ButtonVariant = Variant.Filled;
            CancelButtonColor = Color.Inherit;
            CancelButtonTooltip = "";
            CanSkipAhead = true;
            HideActions = ActionContent is null;
            HideCancelButton = true;
            HideFinishButton = true;
            DescriptionColor = Color.Inherit;
            DescriptionTypo = Typo.caption;
            DotColor = Color.Default;
            Elevation = 1;
            FinishButtonColor = Color.Primary;
            FinishButtonTooltip = "";
            NextButtonColor = Color.Primary;
            NextButtonTooltip = "";
            PreviousButtonColor = Color.Inherit;
            PreviousButtonTooltip = "";
            SelectedDotColor = Color.Info;
            TitleColor = Color.Inherit;
            TitleTypo = Typo.h4;
            Title = "";
            Description = "";

            // Give the base class a chance.
            base.OnParametersSet();
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to dispose of the component.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> for the operation.</returns>
        public ValueTask DisposeAsync()
        {
            // Have we already been disposed?
            if (_disposed == true)
            {
                return ValueTask.CompletedTask;
            }

            // Mark that we've been disposed.
            _disposed = true;

            // Prevent derived types from having to implement IDisposable.
            GC.SuppressFinalize(this);

            return ValueTask.CompletedTask;
        }

        #endregion
    }
}