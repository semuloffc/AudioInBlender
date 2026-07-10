using NAudio.Wave;

public class AudioCapture
{
    private WaveInEvent? waveIn;

    public void StartCapture(int deviceNumber)
    {
        waveIn = new WaveInEvent();
        waveIn.DeviceNumber = deviceNumber;

        waveIn.WaveFormat = new WaveFormat(16000, 16, 1);

        waveIn.DataAvailable += OnDataAvailable;

        waveIn.StartRecording();
        Console.WriteLine($"The sound from the {deviceNumber} device is being recorded...");
    }
    public void StopCapture()
    {
        if (waveIn != null)
        {
            waveIn.StopRecording();
            waveIn.Dispose();
            waveIn = null;
            Console.WriteLine("stop record");
        }
    }

    private void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        Console.WriteLine($"{e.BytesRecorded} bytes of audio data received");
    }
}