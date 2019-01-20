using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager
{
    public class Step : IStep
    {
        private string value;

        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler DataChanged;


    }
}
