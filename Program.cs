class Program
{
    
    static void Main()
    {
        var audioDevice = new AudioInputDevice();
        int Selected = audioDevice.SelectDevice();
        
        Console.WriteLine($"You are cheos device №{Selected}");

        Console.ReadKey();
        Console.Clear();

        Console.ReadKey();
    }
}
