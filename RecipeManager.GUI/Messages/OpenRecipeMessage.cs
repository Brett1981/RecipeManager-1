using GalaSoft.MvvmLight.Messaging;
using RecipeManager.GUI.ViewModels;

namespace RecipeManager.GUI.Messages
{
    public class OpenRecipeMessage : MessageBase
    {
        public RecipeViewModel Recipe { get; }

        public OpenRecipeMessage(RecipeViewModel recipe)
        {
            Recipe = recipe;
        }
    }
}