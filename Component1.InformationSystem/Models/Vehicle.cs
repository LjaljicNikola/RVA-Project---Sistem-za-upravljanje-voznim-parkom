using System;

namespace Component1.InformationSystem.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int YearOfManufacture { get; set; }

        public Vehicle()
        {
            Id = Guid.NewGuid();
        }

        public Vehicle Clone()
        {
            return new Vehicle
            {
                Id = Id,
                LicensePlate = LicensePlate,
                Manufacturer = Manufacturer,
                Model = Model,
                YearOfManufacture = YearOfManufacture
            };
        }
    }
}