using DearImGuiController;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace GB_DebugRender
{
    class Window : GameWindow
    {
        ImGuiController imguiController = null;
        Emulator emulator;

        public Window(Emulator emulator, GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            this.emulator = emulator;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.1f, 0.2f, 0.3f, 1.0f);
            imguiController = new ImGuiController(ClientSize.X, ClientSize.Y);
            
            ImGuiIOPtr io = ImGui.GetIO();
            io.FontGlobalScale = 1.0f;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            imguiController.Update(this, (float)args.Time);

            SetupDockingSpace();

            ImGui.Begin("Debug Window");
            ImGui.Text($"Window size: {ClientSize.X} x {ClientSize.Y}");
            ImGui.Text($"ROM loaded: " + emulator.GetROMTitle());
            ImGui.End();

            imguiController.Render();

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            imguiController.WindowResized(e.Width, e.Height);
        }

        static void SetupDockingSpace()
        {
            ImGuiViewportPtr viewport = ImGui.GetMainViewport();
            Vector2 pos = new Vector2(viewport.Pos.X, viewport.Pos.Y);
            Vector2 size = new Vector2(viewport.Size.X, viewport.Size.Y);
            ImGui.SetNextWindowPos(pos);
            ImGui.SetNextWindowSize(size);

            ImGuiWindowFlags windowFlags =
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoBringToFrontOnFocus |
                ImGuiWindowFlags.NoNavFocus |
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoDocking;

            ImGui.Begin("DockSpace", windowFlags);

            uint dockspaceId = ImGui.GetID("MainDockSpace");
            ImGui.DockSpace(dockspaceId, System.Numerics.Vector2.Zero, ImGuiDockNodeFlags.None);

            ImGui.End();
        }

        public static WindowIcon CreateWindowIcon()
        {
            SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>("Resources/GB-Emulator.png");

            if (!image.DangerousTryGetSinglePixelMemory(out var memory))
                throw new InvalidOperationException("Impossible de lire les pixels de l'image.");

            byte[] imageBytes = MemoryMarshal.AsBytes(memory.Span).ToArray();

            Image otkImage = new Image(image.Width, image.Height, imageBytes);
            return new WindowIcon(otkImage);
        }
    }
}
