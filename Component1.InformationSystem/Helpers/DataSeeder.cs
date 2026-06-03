using System;
using Component1.InformationSystem.Models;
using Component1.InformationSystem.Repositories;

namespace Component1.InformationSystem.Helpers
{
    public static class DataSeeder
    {
        public static void Seed(VehicleRepository vehicleRepo, TelemetryRepository telemetryRepo)
        {
            var v1 = new Vehicle { LicensePlate = "BG-001-AA", Manufacturer = "Volkswagen", Model = "Passat", YearOfManufacture = 2018 };
            var v2 = new Vehicle { LicensePlate = "NS-222-BB", Manufacturer = "Toyota", Model = "Corolla", YearOfManufacture = 2020 };
            var v3 = new Vehicle { LicensePlate = "KG-333-CC", Manufacturer = "Ford", Model = "Transit", YearOfManufacture = 2019 };

            vehicleRepo.Add(v1);
            vehicleRepo.Add(v2);
            vehicleRepo.Add(v3);

            telemetryRepo.Add(new VehicleTelemetry { VehicleId = v1.Id, ReadingTime = DateTime.Now.AddHours(-3), DistanceTraveled = 120.5, FuelConsumption = 8.2, Status = VehicleStatus.Normal });
            telemetryRepo.Add(new VehicleTelemetry { VehicleId = v2.Id, ReadingTime = DateTime.Now.AddHours(-2), DistanceTraveled = 85.0, FuelConsumption = 12.5, Status = VehicleStatus.HighConsumption });
            telemetryRepo.Add(new VehicleTelemetry { VehicleId = v3.Id, ReadingTime = DateTime.Now.AddHours(-1), DistanceTraveled = 200.0, FuelConsumption = 9.0, Status = VehicleStatus.Malfunction });
        }
    }
}
