class Program
{
    static void Main()
    {
        var audioDevice = new AudioInputDevice();
        int Selected = audioDevice.SelectDevice();
        
        Console.WriteLine($"You are cheos device №{Selected}");
        
        var capture = new AudioCapture();
        var rmsAnalyzer = new AudioRMSAnalyzer(capture);

        rmsAnalyzer.AmplitudeUpdated += SendToConsole;

        capture.StartCapture(Selected);

        Console.WriteLine("press any key to exit");
        Console.ReadKey();
    }

    static void SendToConsole(float value)
    {
        Console.WriteLine($"send: {value:F3}");
    }
}   
