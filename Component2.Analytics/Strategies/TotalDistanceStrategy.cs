using System.Collections.Generic;
using System.Linq;
using Component2.Analytics.Interfaces;
using Shared.Contracts.DTOs;

namespace Component2.Analytics.Strategies
{
    /// <summary>
    /// Computes the total distance traveled (km) across all telemetry records.
    /// </summary>
    public class TotalDistanceStrategy : IStatisticsStrategy
    {
        public string Name => "Total Distance Traveled (km)";

        public double Calculate(IEnumerable<TelemetryDto> telemetryRecords)
        {
            return telemetryRecords.Sum(t => t.DistanceTraveled);
        }
    }
}
