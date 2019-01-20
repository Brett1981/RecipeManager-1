using System;
using Newtonsoft.Json;

namespace RecipeManager
{
    public class Material : IMaterial
    {
        public Material(string name, MaterialType type)
        {
            Name = name;
            Type = type;
            Id = Guid.NewGuid().ToString();
        }

        [JsonConstructor]
        public Material(string id, string name, MaterialType type)
        {
            Id = id;
            Name = name;
            Type = type;

            if (Id == null)
            {
                Id = Guid.NewGuid().ToString();
            }
        }

        public string Id { get; private set; }
        public string Name { get; private set; }

        public MaterialType Type { get; private set; }
    }
}