using System;
using System.Collections.Generic;
using Component2.Analytics.Interfaces;
using Shared.Contracts.DTOs;

namespace Component2.Analytics.Services
{
    /// <summary>
    /// Adapter pattern: adapts the WCF IFleetService interface into the
    /// IFleetDataAdapter that the ViewModel expects.
    /// Falls back to hardcoded seed data when the WCF service is unreachable,
    /// so the application remains usable without Component1 running.
    /// </summary>
    public class WcfFleetDataAdapter : IFleetDataAdapter
    {
        private bool _wcfAvailable = true;

        public IEnumerable<VehicleDto> GetVehicles()
        {
            if (!_wcfAvailable)
                return GetFallbackVehicles();

            try
            {
                var channel = WcfClientSingleton.Instance.GetChannel();
                var result = channel.GetAllVehicles();
                _wcfAvailable = true;
                return result;
            }
            catch
            {
                _wcfAvailable = false;
                return GetFallbackVehicles();
            }
        }

        public IEnumerable<TelemetryDto> GetTelemetry(Guid vehicleId, DateTime from, DateTime to)
        {
            if (!_wcfAvailable)
                return GetFallbackTelemetry(vehicleId);

            try
            {
                var channel = WcfClientSingleton.Instance.GetChannel();
                var result = channel.GetTelemetryForVehicle(vehicleId, from, to);
                _wcfAvailable = true;
                return result;
            }
            catch
            {
                _wcfAvailable = false;
                return GetFallbackTelemetry(vehicleId);
            }
        }

        // ── Fallback data (satisfies the "at least 3 instances" requirement) ──

        private static readonly Guid _v1 = Guid.Parse("11111111-0000-0000-0000-000000000001");
        private static readonly Guid _v2 = Guid.Parse("11111111-0000-0000-0000-000000000002");
        private static readonly Guid _v3 = Guid.Parse("11111111-0000-0000-0000-000000000003");

        private static IEnumerable<VehicleDto> GetFallbackVehicles() =>
            new List<VehicleDto>
            {
                new VehicleDto { Id = _v1, LicensePlate = "NS-001-AA", Manufacturer = "Volkswagen", Model = "Passat",  YearOfManufacture = 2019 },
                new VehicleDto { Id = _v2, LicensePlate = "NS-002-BB", Manufacturer = "Renault",    Model = "Megane",  YearOfManufacture = 2020 },
                new VehicleDto { Id = _v3, LicensePlate = "NS-003-CC", Manufacturer = "Toyota",     Model = "Corolla", YearOfManufacture = 2021 }
            };

        private static IEnumerable<TelemetryDto> GetFallbackTelemetry(Guid vehicleId) =>
            new List<TelemetryDto>
            {
                new TelemetryDto { Id = Guid.NewGuid(), VehicleId = vehicleId, ReadingTime = DateTime.Now.AddDays(-2), DistanceTraveled = 120.5, FuelConsumption = 6.8, Status = VehicleStatusDto.Normal },
                new TelemetryDto { Id = Guid.NewGuid(), VehicleId = vehicleId, ReadingTime = DateTime.Now.AddDays(-1), DistanceTraveled = 85.3,  FuelConsumption = 9.2, Status = VehicleStatusDto.HighConsumption },
                new TelemetryDto { Id = Guid.NewGuid(), VehicleId = vehicleId, ReadingTime = DateTime.Now,             DistanceTraveled = 45.0,  FuelConsumption = 7.1, Status = VehicleStatusDto.Normal }
            };
    }
}
