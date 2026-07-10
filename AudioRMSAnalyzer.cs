public class AudioRMSAnalyzer
{
    private float currentRms = 0f;

    public event Action<float>? AmplitudeUpdated;

    public AudioRMSAnalyzer(AudioCapture capture)
    {
        capture.SamplesAvailable += OnSamplesAvailable;
    }
    
    private void OnSamplesAvailable(float[] samples)
    {
        float sum = 0f;
        foreach (float s in samples)
        {
            sum += s * s;
        }
        float rms = MathF.Sqrt(sum / samples.Length);

        currentRms = MathF.Min(rms * 3f, 1f);

        AmplitudeUpdated?.Invoke(currentRms);
    }
}