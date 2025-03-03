using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;
using System;

public class BluetoothService
{
    public async Task<List<DeviceInformation>> DiscoverDevicesAsync()
    {
        // Use the BluetoothDevice API to get a selector string.
        string selector = BluetoothDevice.GetDeviceSelector();
        var devices = await DeviceInformation.FindAllAsync(selector);

        // Filter devices based on your criteria (e.g., device name contains "WashingMachine")
        var washingMachines = devices.Where(d => d.Name.Contains("WashingMachine")).ToList();
        return washingMachines;
    }

    public async Task<bool> PairDeviceAsync(DeviceInformation device)
    {
        if (!device.Pairing.IsPaired)
        {
            var result = await device.Pairing.PairAsync();
            return result.Status == DevicePairingResultStatus.Paired;
        }
        return true;
    }
}



