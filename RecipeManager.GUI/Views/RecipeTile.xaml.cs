using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RecipeManager.GUI.Views
{
    public partial class RecipeTile : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(RecipeTile));
        public static readonly DependencyProperty ClickProperty =
            DependencyProperty.Register("Click", typeof(ICommand), typeof(RecipeTile));
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(RecipeTile));

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public ICommand Click
        {
            get => (ICommand) GetValue(ClickProperty);
            set => SetValue(ClickProperty, value);
        }

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public RecipeTile()
        {
            InitializeComponent();
        }
    }
}
