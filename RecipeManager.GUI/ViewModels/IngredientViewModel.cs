using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace RecipeManager.GUI.ViewModels
{
    public class IngredientViewModel : ObservableObject, IDisposable
    {
        private int amount;
        private MaterialViewModel material;
        public IIngredient Ingredient { get; }

        public ICommand Remove { get; }

        public event EventHandler OnRemove;

        public int Amount
        {
            get => amount;
            set
            {
                if (Set(ref amount, value))
                    Ingredient.Amount = value;
            }
        }

        public MaterialViewModel Material
        {
            get => material;
            set
            {
                if(Set(ref material, value))
                    if(value != null)
                        Ingredient.MaterialId = value.Material.Id;
            }
        }

        public ReadOnlyObservableCollection<MaterialViewModel> AllMaterials
        {
            get => RecipeExplorerViewModel.Materials;
        }

        public ICommand PreviewTextInput { get; }

        public IngredientViewModel(IIngredient ingredient)
        {
            Ingredient = ingredient;
            ingredient.DataChanged += IngredientOnDataChanged;
            IngredientOnDataChanged(this, EventArgs.Empty);

            if (ingredient.MaterialId != null)
            {
                Material = AllMaterials.FirstOrDefault(x => x.Material.Id == ingredient.MaterialId);
            }

            if (ingredient.MaterialId == null)
            {
                Material = AllMaterials.FirstOrDefault();
            }

            Remove = new RelayCommand(() => OnRemove?.Invoke(this, EventArgs.Empty), true);
            PreviewTextInput = new RelayCommand<TextCompositionEventArgs>(args =>
            {
                if (int.TryParse(args.Text, out int result))
                {
                    if (result > 0)
                    {
                        return;
                    }
                }

                args.Handled = true;
            }, true);
        }

        private void IngredientOnDataChanged(object sender, EventArgs e)
        {
            Amount = Ingredient.Amount;
        }

        public void Dispose()
        {
            Ingredient.DataChanged -= IngredientOnDataChanged;
        }
    }
}
