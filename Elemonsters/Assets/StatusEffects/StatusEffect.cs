﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.StatusEffects
{
    public class StatusEffect
    {
        public string Name { get; set; }
        public EffectTypes Type { get; set; }
        public int Duration { get; set; }
        public int Value { get; set; }
    }
}
