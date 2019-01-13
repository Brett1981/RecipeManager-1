using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RecipeManager
{
    public class Recipe : IRecipe
    {
        private readonly List<IIngredient> ingredients = new List<IIngredient>();
        private readonly List<string> steps = new List<string>();
        private string title;
        private string description;

        public Recipe(string title, string description)
        {
            this.title = title;
            this.description = description;
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public IEnumerable<IIngredient> Ingredients => ingredients;

        public IEnumerable<string> Steps => steps;

        public event EventHandler DataChanged;
        public event EventHandler<IIngredient> IngredientAdded;
        public event EventHandler<IIngredient> IngredientRemoved;
        public event EventHandler<string> StepAdded;
        public event EventHandler<string> StepRemoved;

        public void AddIngredient(IIngredient ingredient)
        {
            ingredients.Add(ingredient);
            DataChanged?.Invoke(this, EventArgs.Empty);
            IngredientAdded?.Invoke(this, ingredient);
        }

        public void AddStep(string step)
        {
            steps.Add(step);
            DataChanged?.Invoke(this, EventArgs.Empty);
            StepAdded?.Invoke(this, step);
        }

        public bool RemoveIngredient(IIngredient ingredient)
        {
            if (ingredients.Remove(ingredient))
            {
                DataChanged?.Invoke(this, EventArgs.Empty);
                IngredientRemoved?.Invoke(this, ingredient);
                return true;
            }

            return false;
        }

        public bool RemoveStep(string step)
        {
            if (steps.Remove(step))
            {
                DataChanged?.Invoke(this, EventArgs.Empty);
                StepRemoved?.Invoke(this, step);

                return true;
            }

            return false;
        }
    }
}