using NAudio.Wave;

public class AudioInputDevice
{
    public int SelectDevice()
    {
        Console.WriteLine("input device:");

        int deviceCount = WaveInEvent.DeviceCount;

        for (int i = 0; i < deviceCount; i++)
        {
            var caps = WaveInEvent.GetCapabilities(i);
            Console.WriteLine($"{i}: {caps.ProductName}");
        }
        
        Console.Write("Select device");
        string input = Console.ReadLine();
        int SelectedDevice = int.Parse(input);

        return SelectedDevice;
    }
}
