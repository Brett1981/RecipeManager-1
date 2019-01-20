using System;
using System.Collections.Generic;

namespace RecipeManager
{
    public interface IRecipeDatabase
    {
        IEnumerable<IRecipe> Recipes { get; }
        IEnumerable<IMaterial> Materials { get; }
        event EventHandler<IRecipe> RecipeAdded;
        event EventHandler<IRecipe> RecipeRemoved;
        event EventHandler<IMaterial> MaterialAdded;
        event EventHandler<IMaterial> MaterialRemoved;
        event EventHandler DataChanged;
        void AddRecipe(IRecipe recipe, bool save = true);
        bool RemoveRecipe(IRecipe recipe);
        void AddMaterial(IMaterial material, bool save = true);
        bool RemoveMaterial(IMaterial material, bool save = true);
        void Load();
        void Save();
    }
}