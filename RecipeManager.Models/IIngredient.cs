using System;

namespace RecipeManager
{
    public interface IIngredient
    {
        IMaterial Material { get; set; }
        float Grams { get; set; }
        event EventHandler DataChanged;
    }
}