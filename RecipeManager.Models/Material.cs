namespace RecipeManager
{
    public class Material : IMaterial
    {
        public Material(string name, MaterialType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; private set; }

        public MaterialType Type { get; private set; }
    }
}