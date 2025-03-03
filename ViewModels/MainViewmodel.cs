using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using BluetoothAssistant.Models;
using CommunityToolkit.Mvvm.Input;
using Windows.Devices.Enumeration;

namespace BluetoothAssistant.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly BluetoothService _bluetoothService = new BluetoothService();

        public ObservableCollection<DeviceInfoModel> Devices { get; set; } = new ObservableCollection<DeviceInfoModel>();

        // Commands for Refresh and Connect
        public ICommand RefreshCommand { get; }
        public ICommand ConnectCommand { get; }

        // For simplicity, we store the selected device here.
        private DeviceInfoModel _selectedDevice;
        public DeviceInfoModel SelectedDevice
        {
            get => _selectedDevice;
            set { _selectedDevice = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            // RelayCommand is a common implementation for ICommand. 
            // You can implement your own or use one from a library like CommunityToolkit.Mvvm.
            RefreshCommand = new RelayCommand(async () => await RefreshDevicesAsync());
            ConnectCommand = new RelayCommand(async () => await ConnectToDeviceAsync(), () => SelectedDevice != null);

            // Load devices on startup
            _ = RefreshDevicesAsync();
        }

        private async Task RefreshDevicesAsync()
        {
            Devices.Clear();
            var deviceInfos = await _bluetoothService.DiscoverDevicesAsync();
            foreach (var device in deviceInfos)
            {
                // Here we assume COM port information is either provided or set to a default value.
                Devices.Add(new DeviceInfoModel
                {
                    Name = device.Name,
                    Id = device.Id,
                    ComPort = -1  // Default, update this as needed.
                });
            }
        }

        private async Task ConnectToDeviceAsync()
        {
            if (SelectedDevice == null)
                return;

            // Pairing with the device (you can extend this logic to check COM port validity).
            var deviceInfo = await DeviceInformation.CreateFromIdAsync(SelectedDevice.Id);
            bool paired = await _bluetoothService.PairDeviceAsync(deviceInfo);

            if (paired)
            {
                // TODO: Add logic to validate or update the COM port assignment.
                // For example, re-read the device properties to get the updated COM port.
            }
            else
            {
                // Notify user pairing failed.
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
public class RelayCommand : ICommand
{
    private readonly Func<bool> _canExecute;
    private readonly Action _execute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

    public void Execute(object parameter) => _execute();
}