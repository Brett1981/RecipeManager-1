using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace RecipeManager.GUI.ViewModels
{
    public class StepViewModel : ObservableObject, IDisposable
    {
        private string value;
        public IStep Step { get; }

        public string Value
        {
            get => value;
            set
            {
                if (Set(ref this.value, value))
                {
                    Step.Value = value;
                }
            }
        }

        public event EventHandler OnRemove;

        public StepViewModel(IStep step)
        {
            Step = step;
            step.DataChanged += StepOnDataChanged;
            Value = Step.Value;
            Remove = new RelayCommand(() => OnRemove?.Invoke(this, EventArgs.Empty), true);
        }

        public ICommand Remove { get; }

        private void StepOnDataChanged(object sender, EventArgs e)
        {
            Value = Step.Value;
        }

        public void Dispose()
        {
            Step.DataChanged -= StepOnDataChanged;
        }
    }
}
