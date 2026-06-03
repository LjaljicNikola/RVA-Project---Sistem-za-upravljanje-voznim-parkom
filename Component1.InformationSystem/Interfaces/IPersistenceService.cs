using System.Collections.Generic;

namespace Component1.InformationSystem.Interfaces
{
    public interface IPersistenceService<T>
    {
        void Save(IEnumerable<T> items, string filePath);
        IEnumerable<T> Load(string filePath);
    }
}
