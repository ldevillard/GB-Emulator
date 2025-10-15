using OpenTK.Windowing.Desktop;

namespace GB_DebugRender 
{
    class DebugRenderer
    {
        static void Main(string[] args) 
        {
            Emulator emulator = new Emulator();
            emulator.Init();
    
            NativeWindowSettings nativeSettings = new NativeWindowSettings()
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(1280, 720),
                Title = "GB-DebugRenderer",
                Icon = Window.CreateWindowIcon()
            };
    
            Window window = new Window(GameWindowSettings.Default, nativeSettings);
    
            window.Run();
        }
    }
}