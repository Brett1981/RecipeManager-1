using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using RecipeManager.GUI.Messages;
using RecipeManager.GUI.Views;

namespace RecipeManager.GUI.ViewModels
{
    public class RecipeExplorerViewModel : ObservableObject, IDisposable
    {
        private readonly IRecipeDatabase database;
        private ObservableCollection<RecipeViewModel> recipeViewModels = new ObservableCollection<RecipeViewModel>();
        private RecipeViewModel selectedRecipe;

        private ObservableCollection<MaterialViewModel> materialViewModels = new ObservableCollection<MaterialViewModel>();
        
        public ReadOnlyObservableCollection<RecipeViewModel> Recipes { get; private set; }
        public static ReadOnlyObservableCollection<MaterialViewModel> Materials { get; private set; }

        public RecipeViewModel SelectedRecipe
        {
            get => selectedRecipe;
            set => Set(ref selectedRecipe, value);
        }

        public ICommand Add { get; }

        public RecipeExplorerViewModel(IRecipeDatabase database)
        {
            this.database = database;
            Recipes = new ReadOnlyObservableCollection<RecipeViewModel>(recipeViewModels);
            Materials = new ReadOnlyObservableCollection<MaterialViewModel>(materialViewModels);

            foreach (var material in database.Materials)
            {
                materialViewModels.Add(new MaterialViewModel(material));
            }

            foreach (var recipe in database.Recipes)
            {
                recipeViewModels.Add(new RecipeViewModel(recipe));
            }

            database.RecipeAdded += DatabaseOnRecipeAdded;
            database.RecipeRemoved += DatabaseOnRecipeRemoved;
            database.MaterialAdded += DatabaseOnMaterialAdded;
            database.MaterialRemoved += DatabaseOnMaterialRemoved;
            
            Messenger.Default.Register<OpenRecipeMessage>(this, x=>OpenRecipe(x.Recipe));
            Add = new RelayCommand(AddRecipe, true);
        }

        private void DatabaseOnMaterialRemoved(object sender, IMaterial material)
        {
            materialViewModels.Add(new MaterialViewModel(material));
        }

        private void DatabaseOnMaterialAdded(object sender, IMaterial material)
        {
            var materialVM = materialViewModels.FirstOrDefault(x => x.Material == material);
            materialViewModels.Remove(materialVM);
        }

        public void AddRecipe()
        {
            var recipe = new Recipe("New Recipe", "Recipe description");
            var recipeViewModel = new RecipeViewModel(recipe);
            recipeViewModels.Add(recipeViewModel);
            database.AddRecipe(recipe);
            OpenRecipe(recipeViewModel);
        }

        public void OpenRecipe(RecipeViewModel recipeViewModel)
        {
            if (SelectedRecipe != null && SelectedRecipe.IsOpen)
            {
                SelectedRecipe.CloseRecipe();
            }
            
            SelectedRecipe = recipeViewModel;
            SelectedRecipe.IsOpen = true;
        }

        public void Save()
        {
            database.Save();
        }

        private void DatabaseOnRecipeRemoved(object sender, IRecipe recipe)
        {
            var recipeVM = recipeViewModels.FirstOrDefault(x => x.Recipe == recipe);
            recipeVM?.Dispose();
            recipeViewModels.Remove(recipeVM);
        }

        private void DatabaseOnRecipeAdded(object sender, IRecipe recipe)
        {
            if (recipeViewModels.All(x => x.Recipe != recipe))
            {
                recipeViewModels.Add(new RecipeViewModel(recipe));
            }
        }

        public void Dispose()
        {
            foreach (var recipeViewModel in recipeViewModels)
            {
                recipeViewModel.Dispose();
            }

            database.RecipeAdded -= DatabaseOnRecipeAdded;
            database.RecipeRemoved -= DatabaseOnRecipeRemoved;
            database.MaterialAdded -= DatabaseOnMaterialAdded;
            database.MaterialRemoved -= DatabaseOnMaterialRemoved;
        }
    }
}