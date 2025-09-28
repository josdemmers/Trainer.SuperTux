using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Trainer.SuperTux.Messages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Trainer.SuperTux.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private ObservableCollection<object> _menuItems = new();
        private ObservableCollection<object> _footerMenuItems = new();

        private string _applicationTitle = "Trainer.SuperTux";

        // Start of Constructors region

        #region Constructors

        public MainWindowViewModel()
        {
            // Init View commands
            ApplicationClosingCommand = new RelayCommand(ApplicationClosingExecute);
            ApplicationLoadedCommand = new RelayCommand(ApplicationLoadedExecute);

            // Init menu items
            InitMenuItems();
            InitFooterMenuItems();
        }

        #endregion

        // Start of Events region

        #region Events

        #endregion

        // Start of Properties region

        #region Properties

        public ICommand ApplicationClosingCommand { get; }
        public ICommand ApplicationLoadedCommand { get; }

        public ObservableCollection<object> MenuItems { get => _menuItems; set => _menuItems = value; }
        public ObservableCollection<object> FooterMenuItems { get => _footerMenuItems; set => _footerMenuItems = value; }

        public string ApplicationTitle
        {
            get => _applicationTitle;
            set => SetProperty(ref _applicationTitle, value);
        }

        #endregion

        // Start of Event handlers region

        #region Event handlers

        private void ApplicationClosingExecute()
        {
        }

        private void ApplicationLoadedExecute()
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Dark);

            WeakReferenceMessenger.Default.Send(new ApplicationLoadedMessage(new ApplicationLoadedMessageParams()));
        }

        #endregion

        // Start of Methods region

        #region Methods

        private void InitMenuItems()
        {
            MenuItems.Add(new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            });
        }

        private void InitFooterMenuItems()
        {
            FooterMenuItems.Add(new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            });
        }

        #endregion
    }
}
