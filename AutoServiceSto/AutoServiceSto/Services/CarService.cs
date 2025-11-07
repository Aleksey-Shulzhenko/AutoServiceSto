using AutoServiceSto.Data;
using AutoServiceSto.Models;
using System.Collections.Generic;
using System.Linq;

namespace AutoServiceSto.Services
{
    public class CarService
    {
        private readonly AppDbContext _ctx;
        public CarService(AppDbContext ctx) => _ctx = ctx;

        public List<Car> GetAll() => _ctx.Cars.ToList();

        public Car? GetById(int id) => _ctx.Cars.Find(id);

        public void Add(string brand, string model, string licensePlate, int clientId, int year = 0, string vin = "", int mileage = 0)
        {
            var car = new Car
            {
                Brand = brand,
                Model = model,
                LicensePlate = licensePlate,
                ClientId = clientId,
                Year = year,
                VIN = vin,
                Mileage = mileage
            };
            _ctx.Cars.Add(car);
            _ctx.SaveChanges();
        }

        public void Update(int id, string brand, string model, string licensePlate, int clientId, int year = 0, string vin = "", int mileage = 0)
        {
            var car = _ctx.Cars.Find(id);
            if (car != null)
            {
                car.Brand = brand;
                car.Model = model;
                car.LicensePlate = licensePlate;
                car.ClientId = clientId;
                car.Year = year;
                car.VIN = vin;
                car.Mileage = mileage;
                _ctx.SaveChanges();
            }
        }

        public void UpdateCar(Car updatedCar)
        {
            var car = _ctx.Cars.Find(updatedCar.Id);
            if (car != null)
            {
                car.Brand = updatedCar.Brand;
                car.Model = updatedCar.Model;
                car.LicensePlate = updatedCar.LicensePlate;
                car.ClientId = updatedCar.ClientId;
                car.Year = updatedCar.Year;
                car.VIN = updatedCar.VIN;
                car.Mileage = updatedCar.Mileage;
                _ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var c = _ctx.Cars.Find(id);
            if (c != null)
            {
                _ctx.Cars.Remove(c);
                _ctx.SaveChanges();
            }
        }

        public List<Car> GetCarsByClientId(int clientId)
        {
            return _ctx.Cars.Where(c => c.ClientId == clientId).ToList();
        }

        public bool ClientExists(int clientId)
        {
            return _ctx.Clients.Any(c => c.Id == clientId);
        }
    }
}