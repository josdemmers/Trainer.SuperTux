using CommunityToolkit.Mvvm.Messaging;
using Memory;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using Trainer.SuperTux.Messages;

namespace Trainer.SuperTux.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        private double _allCoins = 9999;
        private string _appStatus = "Not connected";
        private double _delayMonitoring = 100;
        private Mem _memLib = new();
        private string _gameStatus = "Not found";
        private bool _isGameFound = false;
        private bool _isInfiniteJumpsChecked = false;
        private bool _isLockFirePowerChecked = true;
        private bool _isToggleSwitchChecked = false;     

        // Start of Constructors region

        #region Constructors

        public DashboardViewModel()
        {
            // Init View commands
            ApplyAllCoinsCommand = new RelayCommand(ApplyAllCoinsExecute);

            // Init tasks
            _ = StartMonitoringTask();
        }

        #endregion

        // Start of Events region

        #region Events

        #endregion

        // Start of Properties region

        #region Properties

        public ICommand ApplyAllCoinsCommand { get; }

        public double AllCoins 
        {
            get => _allCoins;
            set => SetProperty(ref _allCoins, value);
        }

        public string AppStatus 
        { 
            get => _appStatus;
            set => SetProperty(ref _appStatus, value);
        }

        public string GameStatus
        { 
            get => _gameStatus;
            set => SetProperty(ref _gameStatus, value);
        }

        public bool IsInfiniteJumpsChecked
        {
            get => _isInfiniteJumpsChecked;
            set => SetProperty(ref _isInfiniteJumpsChecked, value);
        }

        public bool IsLockFirePowerChecked
        {
            get => _isLockFirePowerChecked;
            set => SetProperty(ref _isLockFirePowerChecked, value);
        }

        public bool IsToggleSwitchChecked
        {
            get => _isToggleSwitchChecked;
            set => SetProperty(ref _isToggleSwitchChecked, value);
        }

        #endregion

        // Start of Event handlers region

        #region Event handlers

        private void ApplyAllCoinsExecute()
        {
            if (_isGameFound && IsToggleSwitchChecked)
            {
                string coins = AllCoins.ToString();
                _memLib.WriteMemory("base+002AB50C,4,18,0", "int", coins);
            }
        }

        #endregion

        // Start of Methods region

        #region Methods

        private void ApplyCheats()
        {
            if (IsInfiniteJumpsChecked)
            {
                _memLib.WriteMemory("base+CAE61", "byte", "1");
            }
            else
            {
                _memLib.WriteMemory("base+CAE61", "byte", "0");
            }

            if (IsLockFirePowerChecked)
            {
                // Character State. 0=small, 1=big, 2=fire
                _memLib.WriteMemory("base+002AB50C,4,18,4", "int", "2");
                // Max fireballs
                _memLib.WriteMemory("base+002AB50C,4,18,8", "int", "99");
            }
        }

        private async Task StartMonitoringTask()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        _isGameFound = _memLib.OpenProcess("supertux2");
                        if (_isGameFound)
                        {
                            GameStatus = "Found";
                            if (IsToggleSwitchChecked)
                            {
                                AppStatus = "Connected";

                                // Apply activated cheats
                                ApplyCheats();
                            }
                            else
                            {
                                AppStatus = "Not connected";
                            }
                        }
                        else
                        {
                            IsToggleSwitchChecked = false;
                            GameStatus = "Not found";
                            AppStatus = "Not connected";
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                        _delayMonitoring = 1000;

                        IsToggleSwitchChecked = false;
                        GameStatus = "Not found";
                        AppStatus = "Not connected";
                    }
                });
                await Task.Delay(TimeSpan.FromMilliseconds(_delayMonitoring));
            }
        }

        #endregion




















    }
}
