using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Services
{
    public class InstanceTrackerService
    {
        public int Instance = 1;

        public async Task IncrimentInstance()
        {
            Instance++;
        }
    }
}
