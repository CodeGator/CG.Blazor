using CG.Mvvm.ViewModels;
using System.Threading.Tasks;

namespace CG.Blazor.ViewModels
{
    /// <summary>
    /// This class is a Blazor specific, base implementation of the <see cref="IViewModel"/>
    /// interface.
    /// </summary>
    public abstract class BlazorViewModelBase : ViewModelBase, IViewModel
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method is called to initialize the view-model.
        /// </summary>
        /// <returns>A task to perform the operation.</returns>
        public virtual Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
