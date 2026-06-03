using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Shared.Contracts.DTOs;

namespace Component2.Analytics.Services
{
    /// <summary>
    /// Writes telemetry records to a CSV file on disk.
    /// Each row corresponds to one TelemetryDto plus the computed result column.
    /// </summary>
    public class CsvExportService
    {
        /// <summary>
        /// Exports the telemetry list and the statistical result to a CSV file.
        /// </summary>
        /// <param name="filePath">Absolute path of the target .csv file.</param>
        /// <param name="telemetry">Records to export.</param>
        /// <param name="strategyName">Name of the applied strategy (written as a header comment).</param>
        /// <param name="result">Computed statistical result value.</param>
        public void Export(string filePath, IEnumerable<TelemetryDto> telemetry, string strategyName, double result)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"# Strategy: {strategyName}");
            sb.AppendLine($"# Result: {result:F4}");
            sb.AppendLine("Id,VehicleId,ReadingTime,DistanceTraveled,FuelConsumption,Status");

            foreach (var t in telemetry)
            {
                sb.AppendLine(
                    $"{t.Id},{t.VehicleId},{t.ReadingTime:yyyy-MM-dd HH:mm:ss}," +
                    $"{t.DistanceTraveled:F2},{t.FuelConsumption:F2},{t.Status}");
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
    }
}
