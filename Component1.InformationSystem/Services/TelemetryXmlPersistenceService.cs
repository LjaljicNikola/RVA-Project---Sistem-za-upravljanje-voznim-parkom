using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;

namespace Component1.InformationSystem.Services
{
    public class TelemetryXmlPersistenceService : IPersistenceService<VehicleTelemetry>
    {
        public void Save(IEnumerable<VehicleTelemetry> items, string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<VehicleTelemetry>));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, new List<VehicleTelemetry>(items));
        }

        public IEnumerable<VehicleTelemetry> Load(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<VehicleTelemetry>();

            var serializer = new XmlSerializer(typeof(List<VehicleTelemetry>));
            using var reader = new StreamReader(filePath);
            return (List<VehicleTelemetry>)serializer.Deserialize(reader);
        }
    }
}
