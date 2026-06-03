using System.Windows.Input;
using Component1.InformationSystem.Commands;
using Component1.InformationSystem.Helpers;
using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;
using Component1.InformationSystem.Services;
using AppCommandManager = Component1.InformationSystem.Commands.CommandManager;

namespace Component1.InformationSystem.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly VehicleRepository _vehicleRepository;
        private readonly TelemetryRepository _telemetryRepository;
        private readonly IPersistenceService<Vehicle> _vehiclePersistence;
        private readonly IPersistenceService<VehicleTelemetry> _telemetryPersistence;
        private readonly IActivityLogger _logger;

        public VehicleViewModel VehicleVM { get; }
        public TelemetryViewModel TelemetryVM { get; }
        public ChartViewModel ChartVM { get; }

        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }

        public MainViewModel(
            VehicleRepository vehicleRepository,
            TelemetryRepository telemetryRepository,
            AppCommandManager commandManager,
            IPersistenceService<Vehicle> vehiclePersistence,
            IPersistenceService<VehicleTelemetry> telemetryPersistence,
            IActivityLogger logger)
        {
            _vehicleRepository = vehicleRepository;
            _telemetryRepository = telemetryRepository;
            _vehiclePersistence = vehiclePersistence;
            _telemetryPersistence = telemetryPersistence;
            _logger = logger;

            VehicleVM = new VehicleViewModel(vehicleRepository, commandManager, logger);
            TelemetryVM = new TelemetryViewModel(telemetryRepository, vehicleRepository, commandManager, logger);
            ChartVM = new ChartViewModel(telemetryRepository);

            SaveCommand = new RelayCommand(_ => SaveData());
            LoadCommand = new RelayCommand(_ => LoadData());
        }

        private void SaveData()
        {
            _vehiclePersistence.Save(_vehicleRepository.GetAll(), "vehicles.xml");
            _telemetryPersistence.Save(_telemetryRepository.GetAll(), "telemetry.xml");
            _logger.Log("Data saved to XML.");
        }

        private void LoadData()
        {
            var vehicles = _vehiclePersistence.Load("vehicles.xml");
            _vehicleRepository.SetAll(vehicles);

            var telemetries = _telemetryPersistence.Load("telemetry.xml");
            _telemetryRepository.SetAll(telemetries);

            VehicleVM.Refresh();
            TelemetryVM.Refresh();
            ChartVM.Refresh();
            _logger.Log("Data loaded from XML.");
        }

        public void RefreshChart()
        {
            ChartVM.Refresh();
        }
    }
}