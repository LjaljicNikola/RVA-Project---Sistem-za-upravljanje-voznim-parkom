using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;

namespace Component1.InformationSystem.Commands
{
    public class DeleteVehicleCommand : IAppCommand
    {
        private readonly VehicleRepository _repository;
        private readonly Vehicle _vehicle;

        public DeleteVehicleCommand(VehicleRepository repository, Vehicle vehicle)
        {
            _repository = repository;
            _vehicle = vehicle.Clone();
        }

        public void Execute() => _repository.Remove(_vehicle.Id);

        public void Undo() => _repository.Add(_vehicle);
    }
}
