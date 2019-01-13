namespace RecipeManager
{
    public interface IMaterial
    {
        string Name { get; }
        MaterialType Type { get; }
    }
}