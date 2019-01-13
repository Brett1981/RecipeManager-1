using System;
using System.Collections.Generic;

namespace RecipeManager
{
    public interface IRecipe
    {
        string Title { get; set; }
        string Description { get; set; }
        IEnumerable<IIngredient> Ingredients { get; }
        IEnumerable<string> Steps { get; }
        event EventHandler DataChanged;
        void AddIngredient(IIngredient ingredient);
        void AddStep(string step);
        bool RemoveIngredient(IIngredient ingredient);
        bool RemoveStep(string step);
    }
}