using System;

namespace AudioInBlender
{
    public class AudioAnalyzerConfig
    {
        public float BoostFactor { get; set; } = 3.0f;         
        public float SmoothingThreshold { get; set; } = 0.5f; 
        public float SmoothingFactor { get; set; } = 0.25f;    
        public bool EnableSmoothing { get; set; } = true;      
    }
}