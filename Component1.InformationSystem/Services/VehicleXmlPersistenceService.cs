using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;

namespace Component1.InformationSystem.Services
{
    public class VehicleXmlPersistenceService : IPersistenceService<Vehicle>
    {
        public void Save(IEnumerable<Vehicle> items, string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Vehicle>));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, new List<Vehicle>(items));
        }

        public IEnumerable<Vehicle> Load(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<Vehicle>();

            var serializer = new XmlSerializer(typeof(List<Vehicle>));
            using var reader = new StreamReader(filePath);
            return (List<Vehicle>?)serializer.Deserialize(reader) ?? new List<Vehicle>();
        }
    }
}
