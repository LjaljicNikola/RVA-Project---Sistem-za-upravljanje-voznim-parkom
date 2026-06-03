using System.Windows;
using Component1.InformationSystem.Commands;
using Component1.InformationSystem.Helpers;
using Component1.InformationSystem.Repositories;
using Component1.InformationSystem.Services;
using Component1.InformationSystem.ViewModels;
using Component1.InformationSystem.Views;
using Component1.InformationSystem.WCF;
using AppCommandManager = Component1.InformationSystem.Commands.CommandManager;

namespace Component1.InformationSystem
{
    public partial class App : Application
    {
        private FleetServiceHost? _serviceHost;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var vehicleRepo = new VehicleRepository();
            var telemetryRepo = new TelemetryRepository();
            var commandManager = new AppCommandManager();
            var logger = new ActivityLogger();
            var vehiclePersistence = new VehicleXmlPersistenceService();
            var telemetryPersistence = new TelemetryXmlPersistenceService();
            var stateMachine = new VehicleStatusStateMachine();

            DataSeeder.Seed(vehicleRepo, telemetryRepo);

            var simulation = new TelemetrySimulationService(telemetryRepo, stateMachine, logger);

            var mainVM = new MainViewModel(vehicleRepo, telemetryRepo, commandManager, vehiclePersistence, telemetryPersistence, logger);

            simulation.TelemetryUpdated += () =>
            {
                Dispatcher.Invoke(() => mainVM.RefreshChart());
            };
            simulation.Start(5);

            var fleetService = new FleetService(vehicleRepo, telemetryRepo);
            _serviceHost = new FleetServiceHost();
            _serviceHost.Start(fleetService);

            var window = new MainWindow { DataContext = mainVM };
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceHost?.Stop();
            base.OnExit(e);
        }
    }
}