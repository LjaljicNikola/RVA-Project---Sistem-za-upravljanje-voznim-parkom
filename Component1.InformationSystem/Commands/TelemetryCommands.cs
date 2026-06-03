using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;

namespace Component1.InformationSystem.Commands
{
    public class AddTelemetryCommand : IAppCommand
    {
        private readonly TelemetryRepository _repository;
        private readonly VehicleTelemetry _telemetry;

        public AddTelemetryCommand(TelemetryRepository repository, VehicleTelemetry telemetry)
        {
            _repository = repository;
            _telemetry = telemetry;
        }

        public void Execute() => _repository.Add(_telemetry);
        public void Undo() => _repository.Remove(_telemetry.Id);
    }

    public class EditTelemetryCommand : IAppCommand
    {
        private readonly TelemetryRepository _repository;
        private readonly VehicleTelemetry _newState;
        private readonly VehicleTelemetry _previousState;

        public EditTelemetryCommand(TelemetryRepository repository, VehicleTelemetry newState)
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

    public class DeleteTelemetryCommand : IAppCommand
    {
        private readonly TelemetryRepository _repository;
        private readonly VehicleTelemetry _telemetry;

        public DeleteTelemetryCommand(TelemetryRepository repository, VehicleTelemetry telemetry)
        {
            _repository = repository;
            _telemetry = telemetry.Clone();
        }

        public void Execute() => _repository.Remove(_telemetry.Id);
        public void Undo() => _repository.Add(_telemetry);
    }
}
