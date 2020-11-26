using CG.Blazor.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CG.Blazor.Views
{
    /// <summary>
    /// This class is a base implementation of a Blazor view.
    /// </summary>
    /// <typeparam name="T">The type of associated view-model.</typeparam>
    /// <remarks>
    /// <para>
    /// The idea, with this class, is to create a simple MVVM integration for
    /// use with Blazor. As we all know, Blazor was deliberately designed not 
    /// favor any one paradigm over another, so, if we want to use MVVM with 
    /// Blazor, which we do, then we're on our own for that part. That's fine 
    /// though because, the only parts that are really missing are property change 
    /// notifications, and we'll handle those here.
    /// </para>
    /// </remarks>
    /// <example>
    /// All that's required to use this class is to add the following tag to your
    /// razor page markup:
    /// <code>
    /// @inherits ViewBase<IMyViewModel></IMyViewModel>
    /// </code>
    /// Where <c>IMyViewModel</c> 
    /// </example>
    public abstract class ViewBase<T> : ComponentBase, IDisposable
        where T : class, IViewModel
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains an associated view-model.
        /// </summary>
        [Inject]
        protected T ViewModel { get; set; }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method performs application-defined tasks associated with 
        /// freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
#pragma warning disable CS1998 
            ViewModel.PropertyChanged -= async (sender, e) => { };
#pragma warning restore CS1998 
        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method is invoked when the component is ready to start, having 
        /// received its initial parameters from its parent in the render tree.
        /// </summary>
        /// <returns>A task to perform the operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            // Do we have a view-model instance?
            if (null != ViewModel)
            {
                // Wire up a handler for any view-model property changes.
                ViewModel.PropertyChanged += async (sender, e) =>
                {
                    // Tell Blazor whenever something changes.
                    await InvokeAsync(() => StateHasChanged());
                };
            }

            // Give the base class a chance.
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
