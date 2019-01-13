using System;

namespace RecipeManager
{
    public class Ingredient : IIngredient
    {
        private IMaterial material;
        private float grams;

        public IMaterial Material
        {
            get => material;
            set
            {
                material = value;
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public float Grams
        {
            get => grams;
            set
            {
                grams = value;
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler DataChanged;
    }
}