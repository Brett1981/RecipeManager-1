using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace RecipeManager
{
    public class Recipe : IRecipe
    {
        private List<IIngredient> ingredients = new List<IIngredient>();
        private List<IStep> steps = new List<IStep>();
        private string title;
        private string description;
        private string imagePath;

        public Recipe(string title, string description)
        {
            this.title = title;
            this.description = description;
            Id = Guid.NewGuid().ToString();
        }

        [JsonConstructor]
        public Recipe(string title, string description, string imagePath, List<Ingredient> ingredients, List<Step> steps)
        {
            this.title = title;
            this.description = description;
            this.imagePath = imagePath;
            this.ingredients = new List<IIngredient>(ingredients);
            this.steps = new List<IStep>(steps);
        }
        

        public string Id { get; set; }

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

        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value; 
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public IEnumerable<IIngredient> Ingredients => ingredients;

        public IEnumerable<IStep> Steps => steps;

        public event EventHandler DataChanged;
        public event EventHandler<IIngredient> IngredientAdded;
        public event EventHandler<IIngredient> IngredientRemoved;
        public event EventHandler<IStep> StepAdded;
        public event EventHandler<IStep> StepRemoved;

        public void AddIngredient(IIngredient ingredient)
        {
            ingredients.Add(ingredient);
            DataChanged?.Invoke(this, EventArgs.Empty);
            IngredientAdded?.Invoke(this, ingredient);
        }

        public void AddStep(IStep step)
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

        public bool RemoveStep(IStep step)
        {
            if (steps.Remove(step))
            {
                DataChanged?.Invoke(this, EventArgs.Empty);
                StepRemoved?.Invoke(this, step);

                return true;
            }

            return false;
        }

        public void SaveRecipe()
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Recipes"));
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "Recipes", $"{Id}.json"), JsonConvert.SerializeObject(this));
        }
    }
}