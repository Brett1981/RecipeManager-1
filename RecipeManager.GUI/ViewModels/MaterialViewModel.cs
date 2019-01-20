using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace RecipeManager.GUI.ViewModels
{
    public class MaterialViewModel : ObservableObject
    {
        public IMaterial Material { get; }

        public string Name
        {
            get => name;
        }

        public MaterialType Type
        {
            get => type;
        }

        private MaterialType type;
        private string name;

        public MaterialViewModel(IMaterial material)
        {
            Material = material;
            name = material.Name;
            type = material.Type;
        }
    }
}
