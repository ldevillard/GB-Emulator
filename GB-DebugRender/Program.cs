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

            string ROMTitle = emulator.GetROMTitle();

            NativeWindowSettings nativeSettings = new NativeWindowSettings()
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(1280, 720),
                Title = "GB-DebugRenderer",
                Icon = Window.CreateWindowIcon()
            };
    
            Window window = new Window(emulator, GameWindowSettings.Default, nativeSettings);
    
            window.Run();
        }
    }
}