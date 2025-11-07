using AutoServiceSto.Data;
using AutoServiceSto.Models;
using System.Collections.Generic;
using System.Linq;

namespace AutoServiceSto.Services
{
    public class ClientService
    {
        private readonly AppDbContext _ctx;
        public ClientService(AppDbContext ctx) => _ctx = ctx;

        public List<Client> GetAll() => _ctx.Clients.ToList();

        public Client? GetById(int id) => _ctx.Clients.Find(id);

        public void Add(string fullName, string phone, string email, string address = "")
        {
            var client = new Client { FullName = fullName, Phone = phone, Email = email, Address = address };
            _ctx.Clients.Add(client);
            _ctx.SaveChanges();
        }

        public void Update(int id, string fullName, string phone, string email, string address = "")
        {
            var client = _ctx.Clients.Find(id);
            if (client != null)
            {
                client.FullName = fullName;
                client.Phone = phone;
                client.Email = email;
                client.Address = address;
                _ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var c = _ctx.Clients.Find(id);
            if (c != null)
            {
                _ctx.Clients.Remove(c);
                _ctx.SaveChanges();
            }
        }
    }
}