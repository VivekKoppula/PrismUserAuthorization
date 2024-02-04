
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
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
            
        }

        protected override void InitializeShell(DependencyObject shell)
        {
            var identity = WindowsIdentity.GetCurrent();
            var p = new GenericPrincipal(identity, new string[] { "User", "Admin" });
            Thread.CurrentPrincipal = p;
            base.InitializeShell(shell);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
    }
}
