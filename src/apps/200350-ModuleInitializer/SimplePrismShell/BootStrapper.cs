
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SimplePrismShell.Core;
using SimplePrismShell.Views;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace SimplePrismShell
{
    public class BootStrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IModuleInitializer, RoleBasedModuleInitializer>();
        }

        protected override void InitializeShell(DependencyObject shell)
        {
            var identity = WindowsIdentity.GetCurrent();
            // var p = new GenericPrincipal(identity, new string[] { "User", "Admin" });
            // var p = new GenericPrincipal(identity, new string[] { "User" });
            var p = new GenericPrincipal(identity, new string[] { "Admin" });
            Thread.CurrentPrincipal = p;
            base.InitializeShell(shell);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
    }
}
