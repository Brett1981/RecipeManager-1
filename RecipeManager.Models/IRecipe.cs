using System;
using System.Collections.Generic;

namespace RecipeManager
{
    public interface IRecipe
    {
        string Id { get; }
        string Title { get; set; }
        string Description { get; set; }
        string ImagePath { get; set; }
        IEnumerable<IIngredient> Ingredients { get; }
        IEnumerable<IStep> Steps { get; }
        event EventHandler DataChanged;
        event EventHandler<IIngredient> IngredientAdded;
        event EventHandler<IIngredient> IngredientRemoved;
        event EventHandler<IStep> StepAdded;
        event EventHandler<IStep> StepRemoved;
        void AddIngredient(IIngredient ingredient);
        void AddStep(IStep step);
        bool RemoveIngredient(IIngredient ingredient);
        bool RemoveStep(IStep step);
        void SaveRecipe();
    }
}