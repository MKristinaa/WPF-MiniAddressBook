using MiniAddressBook.ViewModels;
using System.Windows;

namespace MiniAddressBook
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }
    }
}