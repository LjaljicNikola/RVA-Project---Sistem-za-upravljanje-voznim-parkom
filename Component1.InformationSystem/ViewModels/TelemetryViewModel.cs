using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Component1.InformationSystem.Commands;
using Component1.InformationSystem.Helpers;
using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;
using AppCommandManager = Component1.InformationSystem.Commands.CommandManager;

namespace Component1.InformationSystem.ViewModels
{
    public class TelemetryViewModel : ViewModelBase
    {
        private readonly TelemetryRepository _repository;
        private readonly VehicleRepository _vehicleRepository;
        private readonly AppCommandManager _commandManager;
        private readonly IActivityLogger _logger;

        private Guid _selectedVehicleId;
        private DateTime _readingTime = DateTime.Now;
        private double _distanceTraveled;
        private double _fuelConsumption;
        private VehicleTelemetry? _selectedTelemetry;
        private string? _searchText;

        public ObservableCollection<VehicleTelemetry> Telemetries { get; } = new();
        public ObservableCollection<VehicleTelemetry> FilteredTelemetries { get; } = new();
        public ObservableCollection<Vehicle> Vehicles { get; } = new();

        public VehicleTelemetry? SelectedTelemetry
        {
            get => _selectedTelemetry;
            set { SetProperty(ref _selectedTelemetry, value); if (value != null) LoadSelected(value); }
        }

        public Guid SelectedVehicleId { get => _selectedVehicleId; set => SetProperty(ref _selectedVehicleId, value); }
        public DateTime ReadingTime { get => _readingTime; set => SetProperty(ref _readingTime, value); }
        public double DistanceTraveled { get => _distanceTraveled; set => SetProperty(ref _distanceTraveled, value); }
        public double FuelConsumption { get => _fuelConsumption; set => SetProperty(ref _fuelConsumption, value); }

        public string? SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ApplySearch(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public bool CanUndo => _commandManager.CanUndo;
        public bool CanRedo => _commandManager.CanRedo;

        public TelemetryViewModel(TelemetryRepository repository, VehicleRepository vehicleRepository, AppCommandManager commandManager, IActivityLogger logger)
        {
            _repository = repository;
            _vehicleRepository = vehicleRepository;
            _commandManager = commandManager;
            _logger = logger;
            _commandManager.StackChanged += () => { OnPropertyChanged(nameof(CanUndo)); OnPropertyChanged(nameof(CanRedo)); };

            AddCommand = new RelayCommand(_ => AddTelemetry(), _ => CanAdd());
            EditCommand = new RelayCommand(_ => EditTelemetry(), _ => SelectedTelemetry != null);
            DeleteCommand = new RelayCommand(_ => DeleteTelemetry(), _ => SelectedTelemetry != null);
            UndoCommand = new RelayCommand(_ => { _commandManager.Undo(); Refresh(); });
            RedoCommand = new RelayCommand(_ => { _commandManager.Redo(); Refresh(); });

            Refresh();
        }

        private bool CanAdd() =>
            SelectedVehicleId != Guid.Empty &&
            ValidationHelper.IsPositive(DistanceTraveled) &&
            ValidationHelper.IsPositive(FuelConsumption) &&
            ValidationHelper.IsNotFutureDate(ReadingTime);

        private void AddTelemetry()
        {
            var t = new VehicleTelemetry
            {
                VehicleId = SelectedVehicleId,
                ReadingTime = ReadingTime,
                DistanceTraveled = DistanceTraveled,
                FuelConsumption = FuelConsumption,
                Status = VehicleStatus.Normal
            };
            _commandManager.Execute(new AddTelemetryCommand(_repository, t));
            _logger.Log($"Telemetry added for vehicle {SelectedVehicleId}");
            Refresh();
        }

        private void EditTelemetry()
        {
            if (SelectedTelemetry == null) return;
            var updated = SelectedTelemetry.Clone();
            updated.VehicleId = SelectedVehicleId;
            updated.ReadingTime = ReadingTime;
            updated.DistanceTraveled = DistanceTraveled;
            updated.FuelConsumption = FuelConsumption;
            _commandManager.Execute(new EditTelemetryCommand(_repository, updated));
            _logger.Log($"Telemetry edited: {updated.Id}");
            Refresh();
        }

        private void DeleteTelemetry()
        {
            if (SelectedTelemetry == null) return;
            _commandManager.Execute(new DeleteTelemetryCommand(_repository, SelectedTelemetry));
            _logger.Log($"Telemetry deleted: {SelectedTelemetry.Id}");
            Refresh();
        }

        public void Refresh()
        {
            Vehicles.Clear();
            foreach (var v in _vehicleRepository.GetAll())
                Vehicles.Add(v);

            Telemetries.Clear();
            foreach (var t in _repository.GetAll())
                Telemetries.Add(t);

            ApplySearch();
        }

        private void ApplySearch()
        {
            FilteredTelemetries.Clear();
            var source = string.IsNullOrWhiteSpace(SearchText)
                ? Telemetries
                : new ObservableCollection<VehicleTelemetry>(Telemetries.Where(t =>
                    t.VehicleId.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    t.Status.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            foreach (var t in source)
                FilteredTelemetries.Add(t);
        }

        private void LoadSelected(VehicleTelemetry t)
        {
            SelectedVehicleId = t.VehicleId;
            ReadingTime = t.ReadingTime;
            DistanceTraveled = t.DistanceTraveled;
            FuelConsumption = t.FuelConsumption;
        }
    }
}