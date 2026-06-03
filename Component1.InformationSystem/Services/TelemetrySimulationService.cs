using System;
using System.Linq;
using System.Threading;
using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;

namespace Component1.InformationSystem.Services
{
    public class TelemetrySimulationService
    {
        private readonly TelemetryRepository _repository;
        private readonly IStateMachine<VehicleStatus> _stateMachine;
        private readonly IActivityLogger _logger;
        private Timer? _timer;

        public event Action? TelemetryUpdated;

        public TelemetrySimulationService(
            TelemetryRepository repository,
            IStateMachine<VehicleStatus> stateMachine,
            IActivityLogger logger)
        {
            _repository = repository;
            _stateMachine = stateMachine;
            _logger = logger;
        }

        public void Start(int intervalSeconds = 5)
        {
            _timer = new Timer(_ => AdvanceAllStates(), null, TimeSpan.Zero, TimeSpan.FromSeconds(intervalSeconds));
        }

        public void Stop() => _timer?.Dispose();

        private void AdvanceAllStates()
        {
            foreach (var telemetry in _repository.GetAll())
            {
                var oldStatus = telemetry.Status;
                telemetry.Status = _stateMachine.Advance(telemetry.Status);
                _repository.Update(telemetry);
                _logger.Log($"Telemetry {telemetry.Id} status changed: {oldStatus} -> {telemetry.Status}");
            }
            TelemetryUpdated?.Invoke();
        }
    }
}