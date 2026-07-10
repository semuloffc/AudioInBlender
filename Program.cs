class Program
{
    static void Main()
    {
        var audioDevice = new AudioInputDevice();
        int Selected = audioDevice.SelectDevice();
        
        Console.WriteLine($"You are cheos device №{Selected}");
        
        var capture = new AudioCapture();
        capture.StartCapture(Selected);

        Console.WriteLine("press any key to exit");
        Console.ReadKey();
    }
}
