using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EQBrowser;

class EQApplicationPacket : EQPacket
{
    #region Constructors
    EQApplicationPacket(EmuOpcode op, byte[] pBuffer, UInt32 size)
        : base()
    {
    }

    EQApplicationPacket(EmuOpcode op)
        : base(op, null, 0)
    {
    }

    EQApplicationPacket(EmuOpcode op, UInt32 size)
        : base(op, null, size)
    {
    }

    EQApplicationPacket(EmuOpcode op, byte[] pBuffer, UInt32 size)
        : base(op, pBuffer, size)
    {
    }
    #endregion
}
