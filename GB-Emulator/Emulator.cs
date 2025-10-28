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

    bool isInitialized = false;
    
    int cycleCount = 0;
    const int cyclesPerFrame = 70224; // Number of cycles per frame (for 59.7 FPS)

    Memory memory = new Memory();

    Register AF;
    Register BC;
    Register DE;
    Register HL;
    Register SP;
    Register PC;

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

    public void Initialize()
    {
        Reset();
        isInitialized = true;
    }

    void Reset()
    {
        // Initialize registers to default values
        AF.value = 0x01B0;
        BC.value = 0x0013;
        DE.value = 0x00D8;
        HL.value = 0x014D;
        SP.value = 0xFFFE;
        PC.value = 0x0100;

        // Initialize memory-mapped IO registers to default values
        memory.Write(0xFF00, 0xCF);
        memory.Write(0xFF05, 0x00);
        memory.Write(0xFF06, 0x00);
        memory.Write(0xFF07, 0x00);
        memory.Write(0xFF10, 0x80);
        memory.Write(0xFF11, 0xBF);
        memory.Write(0xFF12, 0xF3);
        memory.Write(0xFF14, 0xBF);
        memory.Write(0xFF16, 0x3F);
        memory.Write(0xFF17, 0x00);
        memory.Write(0xFF19, 0xBF);
        memory.Write(0xFF1A, 0x7F);
        memory.Write(0xFF1B, 0xFF);
        memory.Write(0xFF1C, 0x9F);
        memory.Write(0xFF1E, 0xBF);
        memory.Write(0xFF20, 0xFF);
        memory.Write(0xFF21, 0x00);
        memory.Write(0xFF22, 0x00);
        memory.Write(0xFF23, 0xBF);
        memory.Write(0xFF24, 0x77);
        memory.Write(0xFF25, 0xF3);
        memory.Write(0xFF26, 0xF1);
        memory.Write(0xFF40, 0x91);
        memory.Write(0xFF42, 0x00);
        memory.Write(0xFF43, 0x00);
        memory.Write(0xFF45, 0x00);
        memory.Write(0xFF47, 0xFC);
        memory.Write(0xFF48, 0xFF);
        memory.Write(0xFF49, 0xFF);
        memory.Write(0xFF4A, 0x00);
        memory.Write(0xFF4B, 0x00);
        memory.Write(0xFFFF, 0x00);
    }

    public void Update()
    {
        if (!isInitialized)
        {
            Utility.LogError("Emulator not initialized. Call Initialize() before Emulate().");
            return;
        }

        cycleCount = 0;
        while (cycleCount < cyclesPerFrame)
        {
            // CPU emulation logic here
            int cycles = ExecuteNextOpcode();
            cycleCount += cycles;
        }
    }

    int ExecuteNextOpcode()
    {
        // Fetch the next opcode
        BYTE opcode = memory.Read(PC.value);
        Utility.Log($"Executed opcode: 0x{opcode:X2} at PC: 0x{PC.value - 1:X4}");
        
        PC.value++;

        return ExecuteOpcode(opcode);
    }

    int ExecuteOpcode(BYTE opcode)
    {
        // Placeholder for opcode execution logic
        return 4; // Assume each opcode takes 4 cycles for now
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