using DearImGuiController;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;

namespace GB_DebugRender
{
    class Window : GameWindow
    {
        ImGuiController imguiController = null;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.1f, 0.2f, 0.3f, 1.0f);
            imguiController = new ImGuiController(ClientSize.X, ClientSize.Y);
            
            ImGuiIOPtr io = ImGui.GetIO();
            io.FontGlobalScale = 1.25f;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            imguiController.Update(this, (float)args.Time);

            SetupDockingSpace();

            ImGui.Begin("Debug Window");
            ImGui.Text("Hello ImGui + OpenTK !");
            ImGui.Text($"Window size: {ClientSize.X} x {ClientSize.Y}");
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
    }
}
