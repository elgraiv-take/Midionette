﻿using System;

namespace Elgraiv.Midionette
{
    public enum MidiControlChangeType
    {
        Undefined=0,
        Modulation=1,
    }

    public class MidiControlChangeTypeUtil
    {
        public bool IsDefinedType(byte data0)
        {
            if (data0 == 0)
            {
                return false;
            }
            return Enum.IsDefined(typeof(MidiControlChangeType), data0);
        } 
    }
}
