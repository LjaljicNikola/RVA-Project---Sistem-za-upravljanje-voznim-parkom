using System;
using System.Collections.Generic;
using System.Linq;
using Component1.InformationSystem.Interfaces;
using Component1.InformationSystem.Models;

namespace Component1.InformationSystem.Repositories
{
    public class VehicleRepository : IRepository<Vehicle>
    {
        private readonly List<Vehicle> _vehicles = new();

        public IEnumerable<Vehicle> GetAll() => _vehicles.ToList();

        public Vehicle? GetById(Guid id) => _vehicles.FirstOrDefault(v => v.Id == id);

        public void Add(Vehicle entity) => _vehicles.Add(entity);

        public void Update(Vehicle entity)
        {
            int index = _vehicles.FindIndex(v => v.Id == entity.Id);
            if (index >= 0)
                _vehicles[index] = entity;
        }

        public void Remove(Guid id) => _vehicles.RemoveAll(v => v.Id == id);

        public void SetAll(IEnumerable<Vehicle> vehicles)
        {
            _vehicles.Clear();
            _vehicles.AddRange(vehicles);
        }
    }
}
