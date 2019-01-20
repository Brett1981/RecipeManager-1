using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using RecipeManager.GUI.Messages;
using System.Linq;
using Microsoft.Win32;

namespace RecipeManager.GUI.ViewModels
{
    public class RecipeViewModel : ObservableObject, IDisposable
    {
        public IRecipe Recipe { get; }

        public event EventHandler OnClose;
        
        private string title;
        private bool isOpen;
        private string description;
        private string imagePath;
        private ObservableCollection<IngredientViewModel> ingredients = new ObservableCollection<IngredientViewModel>(new List<IngredientViewModel>());
        private ObservableCollection<StepViewModel> steps = new ObservableCollection<StepViewModel>(new List<StepViewModel>());

        public string Title
        {
            get => title;
            set
            {
                if(Set(ref title, value))
                    Recipe.Title = value;
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (Set(ref description, value))
                    Recipe.Description = value;
            }
        }

        public string ImagePath
        {
            get => imagePath;
            set
            {
                if (Set(ref imagePath, value))
                    Recipe.ImagePath = value;
            }
        }

        public bool IsOpen
        {
            get => isOpen;
            set => Set(ref isOpen, value);
        }

        public ReadOnlyObservableCollection<StepViewModel> Steps { get; }
        public ReadOnlyObservableCollection<IngredientViewModel> Ingredients { get; }

        public ICommand Open { get; private set; }

        public ICommand Close { get; private set; }

        public ICommand AddIngredient { get; private set; }

        public ICommand AddStep { get; private set; }

        public ICommand SelectImage { get; private set; }

        private void OpenRecipe()
        {
            Messenger.Default.Send(new OpenRecipeMessage(this));
        }

        public RecipeViewModel(IRecipe recipe)
        {
            Recipe = recipe;
            Title = recipe.Title;
            Description = recipe.Description;
            ImagePath = recipe.ImagePath;
            Ingredients = new ReadOnlyObservableCollection<IngredientViewModel>(ingredients);
            Steps = new ReadOnlyObservableCollection<StepViewModel>(steps);

            SetupCommands(recipe);

            foreach (var ingredient in recipe.Ingredients)
            {
                OnRecipeOnIngredientAdded(this, ingredient);
            }

            foreach (var step in recipe.Steps)
            {
                OnRecipeOnStepAdded(this, step);
            }

            recipe.DataChanged += RecipeOnDataChanged;
            recipe.IngredientAdded += OnRecipeOnIngredientAdded;
            recipe.StepAdded += OnRecipeOnStepAdded;
            recipe.IngredientRemoved += OnRecipeOnIngredientRemoved;
            recipe.StepRemoved += OnRecipeOnStepRemoved;
        }

        private void SetupCommands(IRecipe recipe)
        {
            Open = new RelayCommand(OpenRecipe, true);
            Close = new RelayCommand(() =>
            {
                recipe.SaveRecipe();
                CloseRecipe();
            }, true);

            AddIngredient = new RelayCommand(() => { Recipe.AddIngredient(new Ingredient()); }, true);
            AddStep = new RelayCommand(() => Recipe.AddStep(new Step()), true);
            SelectImage = new RelayCommand(() =>
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter =
                    "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                if (openFileDialog.ShowDialog() == true)
                {
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "Recipes", "Images");
                    Directory.CreateDirectory(directory);
                    var newPath = Path.Combine(directory, Path.GetFileName(openFileDialog.FileName));
                    File.Copy(openFileDialog.FileName, newPath);
                    ImagePath = newPath;
                }
            }, true);
        }

        private void OnRecipeOnIngredientRemoved(object sender, IIngredient value)
        {
            var ingredient = ingredients.FirstOrDefault(x => x.Ingredient == value);
            if (ingredient != null)
            {
                ingredient.OnRemove -= IngredientViewModelOnRemove;
                ingredient.Dispose();
                ingredients.Remove(ingredient);
            }
        }

        private void OnRecipeOnIngredientAdded(object sender, IIngredient value)
        {
            var ingredientViewModel = new IngredientViewModel(value);
            ingredientViewModel.OnRemove += IngredientViewModelOnRemove;
            ingredients.Add(ingredientViewModel);
        }

        private void OnRecipeOnStepRemoved(object sender, IStep value)
        {
            var step = Steps.FirstOrDefault(x => x.Step == value);
            if (step != null)
            {
                step.OnRemove -= StepViewModelOnRemove;
                step.Dispose();
                steps.Remove(step);
            }
        }

        private void OnRecipeOnStepAdded(object sender, IStep value)
        {
            var stepViewModel = new StepViewModel(value);
            stepViewModel.OnRemove += StepViewModelOnRemove;
            steps.Add(stepViewModel);
        }

        private void StepViewModelOnRemove(object sender, EventArgs e)
        {
            if (sender is StepViewModel stepViewModel)
            {
                Recipe.RemoveStep(stepViewModel.Step);
            }
        }

        private void IngredientViewModelOnRemove(object sender, EventArgs e)
        {
            if (sender is IngredientViewModel ingredientViewModel)
            {
                Recipe.RemoveIngredient(ingredientViewModel.Ingredient);
            }
        }

        private void RecipeOnDataChanged(object sender, EventArgs e)
        {
            Title = Recipe.Title;
            Description = Recipe.Description;
            ImagePath = Recipe.ImagePath;
        }

        public void CloseRecipe()
        {
            IsOpen = false;
            OnClose?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            foreach (var ingredientViewModel in ingredients)
            {
                ingredientViewModel.OnRemove -= IngredientViewModelOnRemove;
                ingredientViewModel.Dispose();
            }

            foreach (var stepViewModel in steps)
            {
                stepViewModel.OnRemove -= StepViewModelOnRemove;
                stepViewModel.Dispose();
            }

            Recipe.DataChanged -= RecipeOnDataChanged;
            Recipe.IngredientAdded -= OnRecipeOnIngredientAdded;
            Recipe.StepAdded -= OnRecipeOnStepAdded;
            Recipe.IngredientRemoved -= OnRecipeOnIngredientRemoved;
            Recipe.StepRemoved -= OnRecipeOnStepRemoved;
        }
    }
}