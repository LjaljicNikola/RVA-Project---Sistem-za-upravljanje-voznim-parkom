using System.Collections.Generic;
using System.Linq;
using Component2.Analytics.Interfaces;
using Shared.Contracts.DTOs;

namespace Component2.Analytics.Strategies
{
    /// <summary>
    /// Counts how many telemetry records have Status == HighConsumption.
    /// </summary>
    public class HighConsumptionCountStrategy : IStatisticsStrategy
    {
        public string Name => "High Consumption Event Count";

        public double Calculate(IEnumerable<TelemetryDto> telemetryRecords)
        {
            return telemetryRecords.Count(t => t.Status == VehicleStatusDto.HighConsumption);
        }
    }
}
