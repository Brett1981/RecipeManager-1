using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace RecipeManager.GUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RecipeExplorerViewModel RecipeExplorer { get; set; }

        public ICommand Closing { get; }

        public MainViewModel()
        {
            Closing = new RelayCommand<CancelEventArgs>(args =>
            {
                RecipeExplorer.Save();
            }, true);
            RecipeExplorer = new RecipeExplorerViewModel(new RecipeDatabase());
        }
    }
}