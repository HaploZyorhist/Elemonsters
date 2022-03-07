using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Services.Interfaces
{
    public interface IInstanceTrackerService
    {
        /// <summary>
        /// method for incrimenting the instance
        /// </summary>
        Task IncrimentInstance();

        /// <summary>
        /// method for getting the instance
        /// </summary>
        /// <returns>int indicating the instance</returns>
        Task<int> GetInstance();
    }
}
