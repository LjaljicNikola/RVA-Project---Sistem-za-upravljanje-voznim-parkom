using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Component2.Analytics.Helpers;
using Component2.Analytics.Interfaces;
using Component2.Analytics.Services;
using Component2.Analytics.Strategies;
using Microsoft.Win32;
using Shared.Contracts.DTOs;

namespace Component2.Analytics.ViewModels
{
    public class AnalyticsViewModel : ViewModelBase
    {
        // ── Dependencies ──────────────────────────────────────────────────────
        private readonly IFleetDataAdapter _adapter;
        private readonly CsvExportService _csvExporter;

        // ── Backing fields ────────────────────────────────────────────────────
        private VehicleDto? _selectedVehicle;
        private IStatisticsStrategy? _selectedStrategy;
        private DateTime _dateFrom = DateTime.Today.AddDays(-7);
        private DateTime _dateTo = DateTime.Today;
        private double _result;
        private string _resultLabel = string.Empty;
        private string _statusMessage = string.Empty;
        private bool _isBusy;

        // ── Collections ───────────────────────────────────────────────────────
        public ObservableCollection<VehicleDto> Vehicles { get; } = new();
        public ObservableCollection<TelemetryDto> TelemetryRecords { get; } = new();

        /// <summary>Available statistics strategies (populated in ctor).</summary>
        public List<IStatisticsStrategy> Strategies { get; }

        // ── Properties ────────────────────────────────────────────────────────
        public VehicleDto? SelectedVehicle
        {
            get => _selectedVehicle;
            set { _selectedVehicle = value; OnPropertyChanged(); }
        }

        public IStatisticsStrategy? SelectedStrategy
        {
            get => _selectedStrategy;
            set { _selectedStrategy = value; OnPropertyChanged(); }
        }

        public DateTime DateFrom
        {
            get => _dateFrom;
            set { _dateFrom = value; OnPropertyChanged(); }
        }

        public DateTime DateTo
        {
            get => _dateTo;
            set { _dateTo = value; OnPropertyChanged(); }
        }

        public double Result
        {
            get => _result;
            private set { _result = value; OnPropertyChanged(); }
        }

        public string ResultLabel
        {
            get => _resultLabel;
            private set { _resultLabel = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set { _statusMessage = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged(); }
        }

        // ── Commands ──────────────────────────────────────────────────────────
        public ICommand LoadVehiclesCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand ExportCsvCommand { get; }

        // ── Constructor ───────────────────────────────────────────────────────
        public AnalyticsViewModel(IFleetDataAdapter adapter, CsvExportService csvExporter)
        {
            _adapter = adapter;
            _csvExporter = csvExporter;

            Strategies = new List<IStatisticsStrategy>
            {
                new TotalDistanceStrategy(),
                new MaxFuelConsumptionStrategy(),
                new HighConsumptionCountStrategy()
            };
            _selectedStrategy = Strategies[0];

            LoadVehiclesCommand = new RelayCommand(_ => LoadVehicles());
            CalculateCommand    = new RelayCommand(_ => Calculate(), _ => SelectedVehicle != null && SelectedStrategy != null);
            ExportCsvCommand    = new RelayCommand(_ => ExportCsv(), _ => TelemetryRecords.Count > 0);

            LoadVehicles();
        }

        // ── Command implementations ───────────────────────────────────────────

        private void LoadVehicles()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Loading vehicles…";
                var vehicles = _adapter.GetVehicles().ToList();
                Vehicles.Clear();
                foreach (var v in vehicles)
                    Vehicles.Add(v);

                SelectedVehicle = Vehicles.FirstOrDefault();
                StatusMessage = vehicles.Count > 0
                    ? $"{vehicles.Count} vehicle(s) loaded."
                    : "No vehicles found. Using fallback data.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading vehicles: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Calculate()
        {
            if (SelectedVehicle == null || SelectedStrategy == null)
                return;

            try
            {
                IsBusy = true;
                StatusMessage = "Fetching telemetry…";

                var telemetry = _adapter
                    .GetTelemetry(SelectedVehicle.Id, DateFrom, DateTo)
                    .ToList();

                TelemetryRecords.Clear();
                foreach (var t in telemetry)
                    TelemetryRecords.Add(t);

                Result = SelectedStrategy.Calculate(telemetry);
                ResultLabel = $"{SelectedStrategy.Name}: {Result:F4}";
                StatusMessage = $"Calculation complete. {telemetry.Count} record(s) processed.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error during calculation: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ExportCsv()
        {
            if (SelectedStrategy == null)
                return;

            var dialog = new SaveFileDialog
            {
                Title = "Export to CSV",
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = $"analytics_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                _csvExporter.Export(dialog.FileName, TelemetryRecords, SelectedStrategy.Name, Result);
                StatusMessage = $"Exported to {dialog.FileName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Export Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
