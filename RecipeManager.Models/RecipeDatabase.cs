using System;
using System.Collections.Generic;

namespace RecipeManager
{
    public class RecipeDatabase : IRecipeDatabase
    {
        private readonly List<IRecipe> recipes = new List<IRecipe>();

        public IEnumerable<IRecipe> Recipes => recipes;
        
        public event EventHandler<IRecipe> RecipeAdded;
        public event EventHandler<IRecipe> RecipeRemoved;
        public event EventHandler DataChanged;

        public void AddRecipe(IRecipe recipe)
        {
            recipes.Add(recipe);
            DataChanged?.Invoke(this, EventArgs.Empty);
            RecipeAdded?.Invoke(this, recipe);
        }
        
        public bool RemoveRecipe(IRecipe recipe)
        {
            if (recipes.Remove(recipe))
            {
                DataChanged?.Invoke(this, EventArgs.Empty);
                RecipeRemoved?.Invoke(this, recipe);
                return true;
            }

            return false;
        }
    }
}