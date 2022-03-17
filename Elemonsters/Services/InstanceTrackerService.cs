using Elemonsters.Services.Interfaces;

namespace Elemonsters.Services
{
    /// <summary>
    /// class for tracking and handling the instance number for events
    /// </summary>
    public class InstanceTrackerService : IInstanceTrackerService
    {
        public int Instance = 1;

        /// <inheritdoc />
        public async Task IncrimentInstance()
        {
            Instance++;
        }

        /// <inheritdoc />
        public async Task<int> GetInstance()
        {
            try
            {
                return Instance;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
