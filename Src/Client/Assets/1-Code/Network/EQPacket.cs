using UnityEngine;
using System;
using System.Collections;

public class EQPacket
{
    #region Class Variables
    protected UInt32 _size = 0;    // sizeof packet
    protected byte[] _pBuffer = null;     // this should be an unsigned char*  what's c# equiv?
    protected EmuOpcode _emu_opcode = EmuOpcode.OP_Unknown;     //this is just a cache so we dont look it up several times on Get()

    public byte[] pBuffer
    {
        get { return _pBuffer; }
        set { _pBuffer = value; }
    }

    public EmuOpcode Emu_Opcode
    {
        get { return _emu_opcode; }
        set { _emu_opcode = value; }
    }

    public UInt32 Size
    {
        get { return _size + 2; } // not sure why +2? emu source
        set { _size = value; }
    }

    public EQPacket()
    {
    }
    #endregion

    public EQPacket(EmuOpcode op, byte[] pBuffer, UInt32 size)
    {
        this.pBuffer = pBuffer;
        this.Emu_Opcode = op;
        this.Size = size;
    }
}
