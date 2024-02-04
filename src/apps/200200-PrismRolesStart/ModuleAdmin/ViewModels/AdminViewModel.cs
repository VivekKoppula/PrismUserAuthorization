using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleAdmin.ViewModels
{
    public class AdminViewModel : BindableBase
    {
        private string _welcomeMessage = "Admin Module Loaded";
        public string WelcomeMessage
        {
            get { return _welcomeMessage; }
            set { SetProperty(ref _welcomeMessage, value); }
        }

        public AdminViewModel()
        {
            
        }
    }
}
