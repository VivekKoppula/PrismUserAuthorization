using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleUser.ViewModels
{
    public class UserViewModel : BindableBase
    {
        private string _welcomeMessage = "User Module Loaded";
        public string WelcomeMessage
        {
            get { return _welcomeMessage; }
            set { SetProperty(ref _welcomeMessage, value); }
        }

        public UserViewModel()
        {

        }
    }
}
