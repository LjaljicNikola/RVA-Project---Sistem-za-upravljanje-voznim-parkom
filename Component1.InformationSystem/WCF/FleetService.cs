using System;
using System.Collections.Generic;
using System.Linq;
using CoreWCF;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;
using Shared.Contracts.DTOs;
using Shared.Contracts.Interfaces;

namespace Component1.InformationSystem.WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FleetService : IFleetService
    {
        private readonly VehicleRepository _vehicleRepository;
        private readonly TelemetryRepository _telemetryRepository;

        public FleetService(VehicleRepository vehicleRepository, TelemetryRepository telemetryRepository)
        {
            _vehicleRepository = vehicleRepository;
            _telemetryRepository = telemetryRepository;
        }

        public List<VehicleDto> GetAllVehicles()
        {
            return _vehicleRepository.GetAll().Select(v => new VehicleDto
            {
                Id = v.Id,
                LicensePlate = v.LicensePlate,
                Manufacturer = v.Manufacturer,
                Model = v.Model,
                YearOfManufacture = v.YearOfManufacture
            }).ToList();
        }

        public List<TelemetryDto> GetAllTelemetry()
        {
            return _telemetryRepository.GetAll().Select(MapToDto).ToList();
        }

        public List<TelemetryDto> GetTelemetryForVehicle(Guid vehicleId, DateTime from, DateTime to)
        {
            return _telemetryRepository
                .GetByVehicleAndDateRange(vehicleId, from, to)
                .Select(MapToDto)
                .ToList();
        }

        private TelemetryDto MapToDto(VehicleTelemetry t) => new TelemetryDto
        {
            Id = t.Id,
            VehicleId = t.VehicleId,
            ReadingTime = t.ReadingTime,
            DistanceTraveled = t.DistanceTraveled,
            FuelConsumption = t.FuelConsumption,
            Status = (VehicleStatusDto)t.Status
        };
    }
}