using System;

namespace Component1.InformationSystem.Models
{
    public enum VehicleStatus
    {
        Normal,
        HighConsumption,
        Malfunction,
        Inactive
    }

    public class VehicleTelemetry
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime ReadingTime { get; set; }
        public double DistanceTraveled { get; set; }
        public double FuelConsumption { get; set; }
        public VehicleStatus Status { get; set; }

        public VehicleTelemetry()
        {
            Id = Guid.NewGuid();
            Status = VehicleStatus.Normal;
        }

        public VehicleTelemetry Clone()
        {
            return new VehicleTelemetry
            {
                Id = Id,
                VehicleId = VehicleId,
                ReadingTime = ReadingTime,
                DistanceTraveled = DistanceTraveled,
                FuelConsumption = FuelConsumption,
                Status = Status
            };
        }
    }
}