using System;
using System.Collections.Generic;
using Shared.Contracts.DTOs;

namespace Component2.Analytics.Interfaces
{
    /// <summary>
    /// Adapter pattern: abstracts the source of fleet data so the ViewModel
    /// does not depend on WCF directly. Allows easy swapping with a fallback
    /// (e.g. hardcoded data) when the WCF service is unavailable.
    /// </summary>
    public interface IFleetDataAdapter
    {
        IEnumerable<VehicleDto> GetVehicles();
        IEnumerable<TelemetryDto> GetTelemetry(Guid vehicleId, DateTime from, DateTime to);
    }
}
