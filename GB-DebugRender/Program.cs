using OpenTK.Windowing.Desktop;

namespace GB_DebugRender 
{
    class DebugRenderer
    {
        static void Main(string[] args) 
        {
            Emulator emulator = new Emulator();
            if (!emulator.LoadROM("Resources/tetris.gb"))
            {
                return;
            }

            NativeWindowSettings nativeSettings = new NativeWindowSettings()
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(1280, 720),
                Title = "GB-DebugRenderer",
                Icon = Window.CreateWindowIcon()
            };
    
            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.UpdateFrequency = 59.7275; // Approximation of GameBoy refresh rate

            Window window = new Window(emulator, gameWindowSettings, nativeSettings);
    
            window.Run();
        }
    }
}