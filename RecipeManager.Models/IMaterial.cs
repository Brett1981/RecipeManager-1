namespace RecipeManager
{
    public interface IMaterial
    {
        string Id { get; }
        string Name { get; }
        MaterialType Type { get; }
    }
}