﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Models.Combat
{
    public class ActiveResults
    {
        public List<DamageResults> DamageResults { get; set; } = new List<DamageResults>();
    }
}