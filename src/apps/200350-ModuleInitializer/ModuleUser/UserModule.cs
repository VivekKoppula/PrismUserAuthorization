using ModuleUser.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SimplePrismShell.Core;

namespace ModuleUser
{
    [Roles("User")]
    // [Roles("User", "Admin")]
    public class UserModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public UserModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // The OnInitialized method will guarantee that the shell has been loaded and that the region has been created at this point
            // _regionManager.RegisterViewWithRegion("TabRegion", typeof(ViewA));
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(UserView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<UserView>();
            containerRegistry.Register<UserView>();
        }
    }
}
