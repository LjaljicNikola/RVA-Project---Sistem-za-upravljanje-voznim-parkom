using System;
using System.Collections.Generic;
using CoreWCF;
using Shared.Contracts.DTOs;

namespace Shared.Contracts.Interfaces
{
    [ServiceContract]
    public interface IFleetService
    {
        [OperationContract] List<VehicleDto> GetAllVehicles();
        [OperationContract] List<TelemetryDto> GetAllTelemetry();
        [OperationContract] List<TelemetryDto> GetTelemetryForVehicle(Guid vehicleId, DateTime from, DateTime to);
    }
}