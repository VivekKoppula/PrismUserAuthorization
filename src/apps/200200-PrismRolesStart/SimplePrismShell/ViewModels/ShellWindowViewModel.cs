using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace SimplePrismShell.ViewModels
{
    public class ShellWindowViewModel : BindableBase
    {

        private string _windowTitle = default!;
        public string WindowTitle
        {
            get { return _windowTitle; }
            set { SetProperty(ref _windowTitle, value); }
        }

        public ShellWindowViewModel()
        {
            WindowTitle = "Main Shell Window";
        }
    }
}
