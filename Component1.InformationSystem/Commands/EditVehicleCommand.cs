using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;

namespace Component1.InformationSystem.Commands
{
    public class EditVehicleCommand : IAppCommand
    {
        private readonly VehicleRepository _repository;
        private readonly Vehicle _newState;
        private readonly Vehicle? _previousState;

        public EditVehicleCommand(VehicleRepository repository, Vehicle newState)
        {
            _repository = repository;
            _newState = newState;
            _previousState = repository.GetById(newState.Id)?.Clone();
        }

        public void Execute() => _repository.Update(_newState);

        public void Undo()
        {
            if (_previousState != null)
                _repository.Update(_previousState);
        }
    }
}
