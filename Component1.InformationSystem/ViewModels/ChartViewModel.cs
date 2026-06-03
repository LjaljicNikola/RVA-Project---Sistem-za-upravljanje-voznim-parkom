using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Component1.InformationSystem.Helpers;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;

namespace Component1.InformationSystem.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        private readonly TelemetryRepository _repository;

        public ObservableCollection<ISeries> Series { get; } = new();

        public Axis[] XAxes { get; set; } = System.Array.Empty<Axis>();

        public ChartViewModel(TelemetryRepository repository)
        {
            _repository = repository;
            XAxes = new[]
            {
                new Axis { Labels = System.Enum.GetNames(typeof(VehicleStatus)) }
            };
            Refresh();
        }

        public void Refresh()
        {
            var all = _repository.GetAll().ToList();
            var counts = System.Enum.GetValues(typeof(VehicleStatus))
                .Cast<VehicleStatus>()
                .Select(s => (double)all.Count(t => t.Status == s))
                .ToArray();

            Series.Clear();
            Series.Add(new ColumnSeries<double>
            {
                Name = "Telemetry by Status",
                Values = counts
            });
        }
    }
}
