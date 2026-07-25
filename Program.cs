using System;
using System.IO;
using System.Text.Json;   
using NAudio.Wave;

namespace AudioInBlender
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = LoadConfig();   

            Console.WriteLine("Available audio input devices:");
            for (int i = 0; i < WaveInEvent.DeviceCount; i++)
            {
                var caps = WaveInEvent.GetCapabilities(i);
                Console.WriteLine($"{i}: {caps.ProductName}");
            }
            Console.Write("Select device number: ");
            int deviceNumber = int.Parse(Console.ReadLine());

            var capture = new AudioCapture();  
            var analyzer = new AudioRMSAnalyzer(config);  
            var sender = new UDPSender("127.0.0.1", 9000);

            capture.SamplesAvailable += analyzer.OnSamplesAvailable;
            analyzer.AmplitudeUpdated += sender.Send;

            capture.StartCapture(deviceNumber);
            Console.WriteLine("Capturing audio... Press any key to stop.");
            Console.ReadKey();

            capture.StopCapture();
        }

        private static AudioAnalyzerConfig LoadConfig()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(configPath))
            {
                try
                {
                    string json = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<AudioAnalyzerConfig>(json);
                    if (config != null)
                        return config;
                }
                catch
                {
                    Console.WriteLine("Ошибка чтения конфига. Использую значения по умолчанию.");
                }
            }
            return new AudioAnalyzerConfig();
        }
    }
}
