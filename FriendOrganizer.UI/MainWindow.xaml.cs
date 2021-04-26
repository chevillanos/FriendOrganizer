using FriendOrganizer.UI.ViewModel;
using System;
using System.Windows;

namespace FriendOrganizer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        // See Bootstrapper.cs for xaml to accept a ViewModel parameter
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            // Do not put Load() in constructor
            // Constructor should just initialize the View object
            // and not make a call to DB (Load - calls an EF service)
            DataContext = _viewModel; 
            Loaded += MainWindow_Loaded;
        }

        // Add this function to the Loaded delegate to call on 
        // a view model's Load

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
