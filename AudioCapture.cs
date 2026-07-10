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

    public event Action<float[]>? SamplesAvailable;
    private void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        float[] samples = new float[e.BytesRecorded / 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = BitConverter.ToInt16(e.Buffer, i * 2);
            samples[i] = sample / 32768f;
        }

        SamplesAvailable?.Invoke(samples);
    }
}