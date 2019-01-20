using System;
using Newtonsoft.Json;

namespace RecipeManager
{
    public class Ingredient : IIngredient
    {
        private string materialId;
        private int amount;

        public Ingredient()
        {
        }

        [JsonConstructor]
        public Ingredient(string materialId, int amount)
        {
            this.materialId = materialId;
            this.amount = amount;
        }

        public string MaterialId
        {
            get => materialId;
            set
            {
                materialId = value;
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int Amount
        {
            get => amount;
            set
            {
                amount = value;
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler DataChanged;
    }
}