using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;

namespace SimplePrismShell.Core
{
    /// <summary>
    /// This is picked from https://github.com/PrismLibrary/Prism/blob/master/src/Wpf/Prism.Wpf/Modularity/ModuleInitializer.cs
    /// Implements the <see cref="IModuleInitializer"/> interface. Handles loading of a module based on a type.
    /// </summary>
    public class RoleBasedModuleInitializer : IModuleInitializer
    {
        private readonly IContainerExtension _containerExtension;

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleInitializer"/>.
        /// </summary>
        /// <param name="containerExtension">The container that will be used to resolve the modules by specifying its type.</param>
        public RoleBasedModuleInitializer(IContainerExtension containerExtension)
        {
            this._containerExtension = containerExtension ?? throw new ArgumentNullException(nameof(containerExtension));
        }

        /// <summary>
        /// Initializes the specified module.
        /// </summary>
        /// <param name="moduleInfo">The module to initialize</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Catches Exception to handle any exception thrown during the initialization process with the HandleModuleInitializationError method.")]
        public void Initialize(IModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            IModule moduleInstance = null!;
            try
            {
                if (ModuleIsInUserRole(moduleInfo))
                {
                    moduleInstance = this.CreateModule(moduleInfo);
                    if (moduleInstance != null)
                    {
                        moduleInstance.RegisterTypes(_containerExtension);
                        moduleInstance.OnInitialized(_containerExtension);
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleModuleInitializationError(
                    moduleInfo, moduleInstance?.GetType().Assembly.FullName!, ex);
            }
        }

        private bool ModuleIsInUserRole(IModuleInfo moduleInfo)
        {
            bool isInRole = false;

            var roles = GetModuleRoles(moduleInfo);

            foreach (var role in roles)
            {
                if (WindowsPrincipal.Current!.IsInRole(role))
                {
                    isInRole = true;
                    break;
                }
            }

            return isInRole;
        }

        private IEnumerable<string> GetModuleRoles(IModuleInfo moduleInfo)
        {
            var type = Type.GetType(moduleInfo.ModuleType);

            if (type == null)
                return null!;

            foreach (var attr in GetCustomAttribute<RolesAttribute>(type!))
                return attr.Roles.AsEnumerable();
            

            return null!;
        }

        private IEnumerable<T> GetCustomAttribute<T>(Type type)
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>();
        }

        /// <summary>
        /// Handles any exception occurred in the module Initialization process,
        /// This method can be overridden to provide a different behavior.
        /// </summary>
        /// <param name="moduleInfo">The module metadata where the error happened.</param>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="exception">The exception thrown that is the cause of the current error.</param>
        /// <exception cref="ModuleInitializeException"></exception>
        public virtual void HandleModuleInitializationError(IModuleInfo moduleInfo, string assemblyName, Exception exception)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            Exception moduleException;

            if (exception is ModuleInitializeException)
            {
                moduleException = exception;
            }
            else
            {
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, assemblyName, exception.Message, exception);
                }
                else
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, exception.Message, exception);
                }
            }

            throw moduleException;
        }

        /// <summary>
        /// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
        /// </summary>
        /// <param name="moduleInfo">The module to create.</param>
        /// <returns>A new instance of the module specified by <paramref name="moduleInfo"/>.</returns>
        protected virtual IModule CreateModule(IModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            return this.CreateModule(moduleInfo.ModuleType);
        }

        /// <summary>
        /// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The type name to resolve. This type must implement <see cref="IModule"/>.</param>
        /// <returns>A new instance of <paramref name="typeName"/>.</returns>
        protected virtual IModule CreateModule(string typeName)
        {
            Type moduleType = Type.GetType(typeName)!;

            if (moduleType == null)
            {
                throw new ModuleInitializeException(string.Format(CultureInfo.CurrentCulture, "Failed To Get Type", typeName));
            }

            return (IModule)_containerExtension.Resolve(moduleType);
        }
    }
}
