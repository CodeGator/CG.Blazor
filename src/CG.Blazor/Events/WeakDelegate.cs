using CG.Validations;
using System;
using System.Reflection;

namespace CG.Blazor.Events
{
    /// <summary>
    /// This class is a default implementation of the <see cref="IWeakDelegate"/>
    /// interface.
    /// </summary>
    internal sealed class WeakDelegate : IWeakDelegate
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains the raw delegate.
        /// </summary>
        private readonly Delegate _delegate;

        /// <summary>
        /// This field contains reflection informaiton about the delegate.
        /// </summary>
        private readonly MethodInfo _method;

        /// <summary>
        /// This field contains the type of the delegate.
        /// </summary>
        private readonly Type _delegateType;

        /// <summary>
        /// This field contains a weak reference to the target.
        /// </summary>
        private readonly WeakReference _weakReference;

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the target of the delegate.
        /// </summary>
        public Delegate Target
        {
            get
            {
                // Do we have a delegate reference?
                if (null != _delegate)
                {
                    // Return the delegate.
                    return _delegate;
                }
                else
                {
                    // If we get here then we need to re-create a reference 
                    //   to the original delete target.

                    // Is the target a static method?
                    if (_method.IsStatic)
                    {
                        // Return the delegate.
                        return _method.CreateDelegate(
                            _delegateType,
                            null
                            );
                    }

                    // Is the target still alive?
                    if (null != _weakReference.Target)
                    {
                        // Return the delegate.
                        return _method.CreateDelegate(
                            _delegateType,
                            _weakReference.Target
                            );
                    }

                    // Can't make the delegate.
                    return null;
                }
            }
        }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// this cosntructor creates a new instance of the <see cref="WeakDelegate"/>
        /// class.
        /// </summary>
        /// <param name="delegate">The delegate to use with the weak delegate.</param>
        /// <param name="keepAlive">True to keep the target alive; false otherwise.</param>
        public WeakDelegate(
            Delegate @delegate, 
            bool keepAlive
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(@delegate, nameof(@delegate));

            // What sort of reference should we create?
            if (keepAlive)
            {
                // Don't bother wrapping the delegate.
                _delegate = @delegate;
            }
            else
            {
                // Create a weak reference for the target.
                _weakReference = new WeakReference(
                    @delegate.Target
                    );

                // Get information about the delegate method.
                _method = @delegate.GetMethodInfo();

                //  Get the type of the delegate.
                _delegateType = @delegate.GetType();
            }
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method determines if the target matches the specified delegate.
        /// </summary>
        /// <param name="delegate">The delegate to use for the operation.</param>
        /// <returns>True if the delegates match; false otherwise.</returns>
        public bool TargetEquals(
            Delegate @delegate
            )
        {
            // Is the internal delegate missing?
            if (null != _delegate)
            {
                // Return the compare results.
                return _delegate == @delegate;
            }

            // Are we comparing against a null reference?
            if (null == @delegate)
            {
                // Return the compare results.
                return !_method.IsStatic && !_weakReference.IsAlive;
            }

            // Return the compare results.
            return _weakReference.Target.Equals(@delegate.Target) && 
                Equals(_method, @delegate.GetMethodInfo());
        }

        #endregion
    }
}
