internal class Memory
{
    public BYTE[] Rom = new byte[0x8000];
    public BYTE[] Vram = new byte[0x2000];
    public BYTE[] ExternalRam = new byte[0x2000];
    public BYTE[] Wram = new byte[0x2000];
    public BYTE[] Oam = new byte[0xA0];
    public BYTE[] IoPorts = new byte[0x80];
    public BYTE[] Hram = new byte[0x7F];
    public BYTE IE;

    public void LoadROM(BYTE[] romData)
    {
        if (romData.Length > Rom.Length)
        {
            throw new ArgumentException("ROM data is too large.");
        }

        Array.Copy(romData, Rom, romData.Length);
    }

    public byte Read(WORD addr)
    {
        if (addr < 0x8000) return Rom[addr];                      // ROM
        if (addr < 0xA000) return Vram[addr - 0x8000];            // VRAM
        if (addr < 0xC000) return ExternalRam[addr - 0xA000];     // Cart RAM
        if (addr < 0xE000) return Wram[addr - 0xC000];            // WRAM
        if (addr < 0xFE00) return Wram[addr - 0xE000];            // Echo
        if (addr < 0xFEA0) return Oam[addr - 0xFE00];             // OAM
        if (addr == 0xFFFF) return IE;                            // IE
        if (addr < 0xFF80) return IoPorts[addr - 0xFF00];         // IO Ports
        if (addr >= 0xFF80) return Hram[addr - 0xFF80];           // HRAM

        Utility.LogError($"Read from unmapped address: 0x{addr:X4}");
        return 0xFF;
    }

    public void Write(WORD addr, BYTE value)
    {
        if (addr < 0x8000) 
        {
            Utility.LogError($"Attempt to write to ROM address: 0x{addr:X4}");
            return; // ROM is read-only
        }
        if (addr < 0xA000) { Vram[addr - 0x8000] = value; return; }         // VRAM
        if (addr < 0xC000) { ExternalRam[addr - 0xA000] = value; return; }  // Cart RAM
        if (addr < 0xE000) { Wram[addr - 0xC000] = value; return; }         // WRAM
        if (addr < 0xFE00) 
        {
            Utility.LogError($"Attempt to write to Echo address: 0x{addr:X4}");
            return;                                                         // Echo
        } 
        if (addr < 0xFEA0) { Oam[addr - 0xFE00] = value; return; }          // OAM
        if (addr == 0xFFFF) { IE = value; return; }                         // IE
        if (addr < 0xFF80) { IoPorts[addr - 0xFF00] = value; return; }      // IO Ports
        if (addr >= 0xFF80) { Hram[addr - 0xFF80] = value; return; }        // HRAM
        
        Utility.LogError($"Write to unmapped address: 0x{addr:X4}");
    }

    public byte[] ReadBytes(WORD startAddr, int length)
    {
        byte[] result = new byte[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = Read((WORD)(startAddr + i));
        }
        return result;
    }
}
