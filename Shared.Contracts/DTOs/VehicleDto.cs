using System;
using System.Runtime.Serialization;

namespace Shared.Contracts.DTOs
{
    [DataContract]
    public class VehicleDto
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public string LicensePlate { get; set; } = string.Empty;
        [DataMember] public string Manufacturer { get; set; } = string.Empty;
        [DataMember] public string Model { get; set; } = string.Empty;
        [DataMember] public int YearOfManufacture { get; set; }
    }
}
