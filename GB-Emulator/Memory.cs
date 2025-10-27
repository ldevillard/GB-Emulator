internal class Memory
{
    public BYTE[] Rom = new byte[0x8000];
    public BYTE[] Vram = new byte[0x2000];
    public BYTE[] ExternalRam = new byte[0x2000];
    public BYTE[] Wram = new byte[0x2000];
    public BYTE[] Oam = new byte[0xA0];
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
        if (addr < 0xFE00) return 0xFF;                           // Echo
        if (addr < 0xFEA0) return Oam[addr - 0xFE00];             // OAM
        if (addr == 0xFFFF) return IE;                            // IE
        if (addr >= 0xFF80) return Hram[addr - 0xFF80];           // HRAM

        Utility.LogError($"Read from unmapped address: 0x{addr:X4}");
        return 0xFF;
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
