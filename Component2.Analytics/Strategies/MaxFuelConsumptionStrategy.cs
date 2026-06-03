using System.Collections.Generic;
using System.Linq;
using Component2.Analytics.Interfaces;
using Shared.Contracts.DTOs;

namespace Component2.Analytics.Strategies
{
    /// <summary>
    /// Finds the maximum fuel consumption (L/100km) recorded in any single telemetry entry.
    /// </summary>
    public class MaxFuelConsumptionStrategy : IStatisticsStrategy
    {
        public string Name => "Max Fuel Consumption (L/100km)";

        public double Calculate(IEnumerable<TelemetryDto> telemetryRecords)
        {
            var list = telemetryRecords.ToList();
            return list.Count == 0 ? 0 : list.Max(t => t.FuelConsumption);
        }
    }
}
