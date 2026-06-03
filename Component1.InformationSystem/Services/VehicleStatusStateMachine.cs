using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;

namespace Component1.InformationSystem.Services
{
    public class VehicleStatusStateMachine : IStateMachine<VehicleStatus>
    {
        public VehicleStatus Advance(VehicleStatus currentState)
        {
            return currentState switch
            {
                VehicleStatus.Normal => VehicleStatus.HighConsumption,
                VehicleStatus.HighConsumption => VehicleStatus.Malfunction,
                VehicleStatus.Malfunction => VehicleStatus.Inactive,
                VehicleStatus.Inactive => VehicleStatus.Normal,
                _ => VehicleStatus.Normal
            };
        }
    }
}
