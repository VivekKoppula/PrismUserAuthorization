using ModuleAdmin.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleAdmin
{
    public class AdminModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public AdminModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // The OnInitialized method will guarantee that the shell has been loaded and that the region has been created at this point
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(AdminView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<AdminView>();
            containerRegistry.Register<AdminView>();
        }
    }
}
