﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoPro.Hero
{
    class CapabilityAttribute:Attribute
    {
        public int LocalIndex { get; set; }
        public int Mask { get; set; }
        public int Position { get; set; }
    }
}
