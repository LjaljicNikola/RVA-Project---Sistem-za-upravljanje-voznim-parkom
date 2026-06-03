using System;
using System.Collections.Generic;
using System.Linq;
using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;

namespace Component1.InformationSystem.Repositories
{
    public class TelemetryRepository : IRepository<VehicleTelemetry>
    {
        private readonly List<VehicleTelemetry> _telemetries = new();

        public IEnumerable<VehicleTelemetry> GetAll() => _telemetries.ToList();

        public VehicleTelemetry GetById(Guid id) => _telemetries.FirstOrDefault(t => t.Id == id);

        public void Add(VehicleTelemetry entity) => _telemetries.Add(entity);

        public void Update(VehicleTelemetry entity)
        {
            int index = _telemetries.FindIndex(t => t.Id == entity.Id);
            if (index >= 0)
                _telemetries[index] = entity;
        }

        public void Remove(Guid id) => _telemetries.RemoveAll(t => t.Id == id);

        public void SetAll(IEnumerable<VehicleTelemetry> telemetries)
        {
            _telemetries.Clear();
            _telemetries.AddRange(telemetries);
        }

        public IEnumerable<VehicleTelemetry> GetByVehicleId(Guid vehicleId) =>
            _telemetries.Where(t => t.VehicleId == vehicleId).ToList();

        public IEnumerable<VehicleTelemetry> GetByVehicleAndDateRange(Guid vehicleId, DateTime from, DateTime to) =>
            _telemetries.Where(t => t.VehicleId == vehicleId && t.ReadingTime >= from && t.ReadingTime <= to).ToList();
    }
}
