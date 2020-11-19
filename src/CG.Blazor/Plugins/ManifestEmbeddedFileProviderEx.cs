using CG.Validations;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CG.Blazor.Plugins
{
    /// <summary>
    /// This class is an internal wrapper around the <see cref="IFileProvider"/>
    /// interface.
    /// </summary>
    internal class ManifestEmbeddedFileProviderEx : IFileProvider
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the inner provider.
        /// </summary>
        protected ManifestEmbeddedFileProvider InnerProvider { get; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="ManifestEmbeddedFileProviderEx"/>
        /// class.
        /// </summary>
        /// <param name="assembly">The assembly to use with the provider.</param>
        public ManifestEmbeddedFileProviderEx(
            Assembly assembly
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(assembly, nameof(assembly));

            // Create the innder provider.
            InnerProvider = new ManifestEmbeddedFileProvider(
                assembly
                );
        }

        // *******************************************************************

        /// <summary>
        /// This constructor creates a new instance of the <see cref="ManifestEmbeddedFileProviderEx"/>
        /// class.
        /// </summary>
        /// <param name="assembly">The assembly to use with the provider.</param>
        /// <param name="root">The root file path to use with the provider.</param>
        public ManifestEmbeddedFileProviderEx(
            Assembly assembly, 
            string root
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(assembly, nameof(assembly))
                .ThrowIfNullOrEmpty(root, nameof(root));

            // Create the innder provider.
            InnerProvider = new ManifestEmbeddedFileProvider(
                assembly, 
                root
                );
        }

        // *******************************************************************

        /// <summary>
        /// This constructor creates a new instance of the <see cref="ManifestEmbeddedFileProviderEx"/>
        /// class.
        /// </summary>
        /// <param name="assembly">The assembly to use with the provider.</param>
        /// <param name="root">The root file path to use with the provider.</param>
        /// <param name="lastModified">The last modified date to use with the provider.</param>
        public ManifestEmbeddedFileProviderEx(
            Assembly assembly, 
            string root, 
            DateTimeOffset lastModified
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(assembly, nameof(assembly))
                .ThrowIfNullOrEmpty(root, nameof(root));

            // Create the innder provider.
            InnerProvider = new ManifestEmbeddedFileProvider(
                assembly, 
                root, 
                lastModified
                );
        }

        // *******************************************************************

        /// <summary>
        /// This constructor creates a new instance of the <see cref="ManifestEmbeddedFileProviderEx"/>
        /// class.
        /// </summary>
        /// <param name="assembly">The assembly to use with the provider.</param>
        /// <param name="root">The root file path to use with the provider.</param>
        /// <param name="manifestName">The manifest name to use with the provider.</param>
        /// <param name="lastModified">The last modified date to use with the provider.</param>
        public ManifestEmbeddedFileProviderEx(
            Assembly assembly, 
            string root, 
            string manifestName, 
            DateTimeOffset lastModified
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(assembly, nameof(assembly))
                .ThrowIfNullOrEmpty(root, nameof(root))
                .ThrowIfNullOrEmpty(manifestName, nameof(manifestName));

            // Create the innder provider.
            InnerProvider = new ManifestEmbeddedFileProvider(
                assembly, 
                root, 
                manifestName, 
                lastModified
                );
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc />
        public IDirectoryContents GetDirectoryContents(
            string subpath
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNullOrEmpty(subpath, nameof(subpath));

            // Defer to the inner provider.
            return InnerProvider.GetDirectoryContents(subpath);
        }

        // *******************************************************************

        /// <inheritdoc />
        public IFileInfo GetFileInfo(
            string subpath
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNullOrEmpty(subpath, nameof(subpath));
                        
            // Watch for paths that start with '/_content/', those are special
            //   and require some extra handling.
            if (subpath.StartsWith("/_content/"))
            {
                // If we get here the caller is looking for what it probably thinks
                //   is a static web resource. However, if the file in question exists,
                //   in this provider, then it's actually an embedded resource. 
                // Unfortunately, there seems to be an impedence mismatch between the paths 
                //   for static web resources and those for embedded resources. Because of 
                //   that mismatch, and the fact that the caller (in this case ASP.NET Core) 
                //   doesn't realize it's actually asking for an embedded resource, means that
                //   we'll need to step in and give the 'subpath' parameter a bit of a haircut 
                //   before we send it on, to the inner provider.

                // First, trim off the _content part,
                subpath = subpath.Substring("/_content/".Length);

                // Get the name of the assembly.
                var asmName = InnerProvider.Assembly.GetName().Name;

                // Is the assembly name part of the path?
                if (subpath.StartsWith($"{asmName}"))
                {
                    // Trim off the assembly name.
                    subpath = subpath.Substring(asmName.Length);
                }
            }

            // Defer to the inner provider.
            return InnerProvider.GetFileInfo(subpath);
        }

        // *******************************************************************

        /// <inheritdoc />
        public IChangeToken Watch(
            string filter
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNullOrEmpty(filter, nameof(filter));

            // Defer to the inner provider.
            return InnerProvider.Watch(filter);
        }

        #endregion
    }
}
