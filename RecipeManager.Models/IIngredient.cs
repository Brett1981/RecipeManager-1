using System;

namespace RecipeManager
{
    public interface IIngredient
    {
        string MaterialId { get; set; }
        int Amount { get; set; }
        event EventHandler DataChanged;
    }
}