using System;
using Simple.Data.MongoDB;

namespace Simple.Data.First.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string conStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=world;Integrated Security=True";
            var db = Database.OpenConnection(conStr);

            //const string conStr = @"mongodb://localhost:27017/world";
            //dynamic db = Database.Opener.OpenMongo(conStr);

            var cities = db.city.FindAllByCountryCode("USA").Where(db.city.Population > 1000000);

            foreach (var city in cities)
                Console.WriteLine(city.Name);

            Console.ReadLine();
        }
    }
}