using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;

namespace Component1.InformationSystem.Commands
{
    public class AddVehicleCommand : IAppCommand
    {
        private readonly VehicleRepository _repository;
        private readonly Vehicle _vehicle;

        public AddVehicleCommand(VehicleRepository repository, Vehicle vehicle)
        {
            _repository = repository;
            _vehicle = vehicle;
        }

        public void Execute() => _repository.Add(_vehicle);

        public void Undo() => _repository.Remove(_vehicle.Id);
    }
}
