using System;
using System.Collections.Generic;
using Shared.Contracts.DTOs;

namespace Component2.Analytics.Interfaces
{
    /// <summary>
    /// Strategy pattern: defines a family of statistical algorithms,
    /// each encapsulated so they can be interchanged at runtime.
    /// </summary>
    public interface IStatisticsStrategy
    {
        /// <summary>Human-readable name shown in the UI.</summary>
        string Name { get; }

        /// <summary>
        /// Computes a statistical result over the provided telemetry records.
        /// </summary>
        double Calculate(IEnumerable<TelemetryDto> telemetryRecords);
    }
}
