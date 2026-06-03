using System;
using System.Runtime.Serialization;

namespace Shared.Contracts.DTOs
{
    [DataContract]
    public enum VehicleStatusDto
    {
        [EnumMember] Normal,
        [EnumMember] HighConsumption,
        [EnumMember] Malfunction,
        [EnumMember] Inactive
    }

    [DataContract]
    public class TelemetryDto
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid VehicleId { get; set; }
        [DataMember] public DateTime ReadingTime { get; set; }
        [DataMember] public double DistanceTraveled { get; set; }
        [DataMember] public double FuelConsumption { get; set; }
        [DataMember] public VehicleStatusDto Status { get; set; }
    }
}
