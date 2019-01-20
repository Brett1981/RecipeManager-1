using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace RecipeManager
{
    public class RecipeDatabase : IRecipeDatabase
    {
        private readonly List<IRecipe> recipes = new List<IRecipe>();
        private List<IMaterial> materials = new List<IMaterial>();

        public IEnumerable<IRecipe> Recipes => recipes;
        public IEnumerable<IMaterial> Materials => materials;
        
        public event EventHandler<IRecipe> RecipeAdded;
        public event EventHandler<IRecipe> RecipeRemoved;
        public event EventHandler<IMaterial> MaterialAdded;
        public event EventHandler<IMaterial> MaterialRemoved;
        public event EventHandler DataChanged;

        public RecipeDatabase()
        {
            Load();
        }
        
        public void AddRecipe(IRecipe recipe, bool save = true)
        {
            recipes.Add(recipe);
            
            if(save)
                recipe.SaveRecipe();
            
            DataChanged?.Invoke(this, EventArgs.Empty);
            RecipeAdded?.Invoke(this, recipe);
        }
        
        public bool RemoveRecipe(IRecipe recipe)
        {
            if (recipes.Remove(recipe))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Recipes", recipe.Id));
                DataChanged?.Invoke(this, EventArgs.Empty);
                RecipeRemoved?.Invoke(this, recipe);
                return true;
            }

            return false;
        }

        public bool RemoveMaterial(IMaterial material, bool save = true)
        {
            if (materials.Remove(material))
            {
                DataChanged?.Invoke(this, EventArgs.Empty);
                MaterialRemoved?.Invoke(this, material);
                SaveMaterials();
                return true;
            }

            return false;
        }

        public void AddMaterial(IMaterial material, bool save = true)
        {
            materials.Add(material);
            if(save)
                SaveMaterials();

            DataChanged?.Invoke(this, EventArgs.Empty);
            MaterialAdded?.Invoke(this, material);
        }

        public void Load()
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Recipes"));
            var recipeFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Recipes"));
            foreach (var recipeFile in recipeFiles)
            {
                string content = File.ReadAllText(recipeFile);
                var recipe = JsonConvert.DeserializeObject<Recipe>(content);
                AddRecipe(recipe, false);
            }

            LoadMaterials();
        }

        private void LoadMaterials()
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Materials.json")))
            {
                var text = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Materials.json"));
                materials = new List<IMaterial>(JsonConvert.DeserializeObject<List<Material>>(text));
            }
            else
            {
                materials = new List<IMaterial>();
            }
        }

        public void Save()
        {
            foreach (var recipe in Recipes)
            {
                recipe.SaveRecipe();
            }

            SaveMaterials();
        }

        private void SaveMaterials()
        {
            var serializedMaterials = JsonConvert.SerializeObject(materials);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "Materials.json"), serializedMaterials);
        }
    }
}