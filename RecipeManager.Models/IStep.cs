using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager
{
    public interface IStep
    {
        string Value { get; set; }
        event EventHandler DataChanged;
    }
}
