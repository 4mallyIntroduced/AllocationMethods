using AllocationMethods.Model;

namespace AllocationMethods.Services
{
    public class SimulationModelBuilder
    {
        /// <summary>
        /// Get a new instance of a TimerModel
        /// </summary>
        /// <returns></returns>
        public static ISimulationModel GetNewTimer()
        {
            return new SimulationModel();
        }
    }
}
