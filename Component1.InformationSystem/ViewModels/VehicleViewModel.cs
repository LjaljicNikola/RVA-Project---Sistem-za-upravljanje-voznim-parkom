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
    public class VehicleViewModel : ViewModelBase
    {
        private readonly VehicleRepository _repository;
        private readonly AppCommandManager _commandManager;
        private readonly IActivityLogger _logger;

        private string _licensePlate = string.Empty;
        private string _manufacturer = string.Empty;
        private string _model = string.Empty;
        private int _yearOfManufacture;
        private Vehicle? _selectedVehicle;
        private string? _searchText;

        public ObservableCollection<Vehicle> Vehicles { get; } = new();
        public ObservableCollection<Vehicle> FilteredVehicles { get; } = new();

        public Vehicle? SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                SetProperty(ref _selectedVehicle, value);
                if (value != null) LoadSelectedVehicle(value);
            }
        }

        public string LicensePlate { get => _licensePlate; set => SetProperty(ref _licensePlate, value); }
        public string Manufacturer { get => _manufacturer; set => SetProperty(ref _manufacturer, value); }
        public string Model { get => _model; set => SetProperty(ref _model, value); }
        public int YearOfManufacture { get => _yearOfManufacture; set => SetProperty(ref _yearOfManufacture, value); }

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

        public VehicleViewModel(VehicleRepository repository, AppCommandManager commandManager, IActivityLogger logger)
        {
            _repository = repository;
            _commandManager = commandManager;
            _logger = logger;
            _commandManager.StackChanged += () => { OnPropertyChanged(nameof(CanUndo)); OnPropertyChanged(nameof(CanRedo)); };

            AddCommand = new RelayCommand(_ => AddVehicle(), _ => CanAdd());
            EditCommand = new RelayCommand(_ => EditVehicle(), _ => SelectedVehicle != null);
            DeleteCommand = new RelayCommand(_ => DeleteVehicle(), _ => SelectedVehicle != null);
            UndoCommand = new RelayCommand(_ => { _commandManager.Undo(); Refresh(); });
            RedoCommand = new RelayCommand(_ => { _commandManager.Redo(); Refresh(); });

            Refresh();
        }

        public bool CanUndo => _commandManager.CanUndo;
        public bool CanRedo => _commandManager.CanRedo;

        private bool CanAdd() =>
            ValidationHelper.IsValidLicensePlate(LicensePlate) &&
            ValidationHelper.IsLettersOnly(Manufacturer) &&
            !string.IsNullOrWhiteSpace(Model) &&
            ValidationHelper.IsValidYear(YearOfManufacture);

        private void AddVehicle()
        {
            var vehicle = new Vehicle
            {
                LicensePlate = LicensePlate,
                Manufacturer = Manufacturer,
                Model = Model,
                YearOfManufacture = YearOfManufacture
            };
            _commandManager.Execute(new AddVehicleCommand(_repository, vehicle));
            _logger.Log($"Vehicle added: {vehicle.LicensePlate}");
            Refresh();
            ClearForm();
        }

        private void EditVehicle()
        {
            if (SelectedVehicle == null) return;
            var updated = SelectedVehicle.Clone();
            updated.LicensePlate = LicensePlate;
            updated.Manufacturer = Manufacturer;
            updated.Model = Model;
            updated.YearOfManufacture = YearOfManufacture;
            _commandManager.Execute(new EditVehicleCommand(_repository, updated));
            _logger.Log($"Vehicle edited: {updated.LicensePlate}");
            Refresh();
        }

        private void DeleteVehicle()
        {
            if (SelectedVehicle == null) return;
            _commandManager.Execute(new DeleteVehicleCommand(_repository, SelectedVehicle));
            _logger.Log($"Vehicle deleted: {SelectedVehicle.LicensePlate}");
            Refresh();
        }

        public void Refresh()
        {
            Vehicles.Clear();
            foreach (var v in _repository.GetAll())
                Vehicles.Add(v);
            ApplySearch();
        }

        private void ApplySearch()
        {
            FilteredVehicles.Clear();
            var source = string.IsNullOrWhiteSpace(SearchText)
                ? Vehicles
                : new ObservableCollection<Vehicle>(Vehicles.Where(v =>
                    v.LicensePlate.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    v.Manufacturer.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    v.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    v.YearOfManufacture.ToString().Contains(SearchText)));
            foreach (var v in source)
                FilteredVehicles.Add(v);
        }

        private void LoadSelectedVehicle(Vehicle v)
        {
            LicensePlate = v.LicensePlate;
            Manufacturer = v.Manufacturer;
            Model = v.Model;
            YearOfManufacture = v.YearOfManufacture;
        }

        private void ClearForm()
        {
            LicensePlate = string.Empty;
            Manufacturer = string.Empty;
            Model = string.Empty;
            YearOfManufacture = DateTime.Now.Year;
        }
    }
}