using System;

namespace AudioInBlender
{
    public class AudioRMSAnalyzer
    {
        private readonly AudioAnalyzerConfig _config;
        private float _previousSmoothed = 0f;

        public event Action<float> AmplitudeUpdated;


        public AudioRMSAnalyzer(AudioAnalyzerConfig config)
        {
            _config = config;
        }


        public void OnSamplesAvailable(float[] samples)
        {

            float sum = 0;
            foreach (var s in samples)
                sum += s * s;
            float rms = MathF.Sqrt(sum / samples.Length);

            float raw = MathF.Min(rms * _config.BoostFactor, 1.0f);

            if (_config.EnableSmoothing)
            {
                float diff = raw - _previousSmoothed;

                if (MathF.Abs(diff) > _config.SmoothingThreshold)
                {
                    float smoothed = _previousSmoothed + diff * _config.SmoothingFactor;
                    _previousSmoothed = Math.Clamp(smoothed, 0f, 1f);
                }
                else
                {
                    _previousSmoothed = raw;
                }
            }
            else
            {
                _previousSmoothed = raw;
            }

            AmplitudeUpdated?.Invoke(_previousSmoothed);
        }
    }
}