using System.Windows;

namespace AssemblyBrowser
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ModelView();
        }

    }
}
