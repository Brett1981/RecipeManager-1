using System;
using System.Collections.Generic;

namespace RecipeManager
{
    public interface IRecipeDatabase
    {
        IEnumerable<IRecipe> Recipes { get; }
        event EventHandler<IRecipe> RecipeAdded;
        event EventHandler<IRecipe> RecipeRemoved;
        event EventHandler DataChanged;
        void AddRecipe(IRecipe recipe);
        bool RemoveRecipe(IRecipe recipe);
    }
}