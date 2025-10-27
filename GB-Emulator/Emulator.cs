using System.Runtime.InteropServices;

public class Emulator() 
{
    [StructLayout(LayoutKind.Explicit)]
    struct Register
    {
        [FieldOffset(0)] public WORD value;

        [FieldOffset(0)] public BYTE low;
        [FieldOffset(1)] public BYTE high;
    }

    Memory memory = new Memory();

    public bool LoadROM(string path)
    {
        Utility.Log($"Loading ROM {path}");
        
        if (!File.Exists(path)) 
        {
            Utility.LogError($"ROM file not found: {path}");
            return false;
        }

        memory.LoadROM(System.IO.File.ReadAllBytes(path));

        Utility.LogSuccess($"ROM loaded successfully: {path}");

        return true;
    }

    public string GetROMTitle() 
    {
        const WORD titleAddr = 0x0134;
        const int titleLength = 16;

        char[] title = new char[titleLength];
        for (int i = 0; i < 16; i++)
        {
            BYTE b = memory.Read((WORD)(titleAddr + i));
            
            if (b == 0)
                break;

            title[i] = (char)b;
        }
        return new string(title).TrimEnd('\0');
    }
}