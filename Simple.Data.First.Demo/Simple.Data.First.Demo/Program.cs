using System;
using Simple.Data.MongoDB;

namespace Simple.Data.First.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string conStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=MvcMusicStore;Integrated Security=True";
            var db = Database.OpenConnection(conStr);

            //const string conStr = @"mongodb://localhost:27017/MvcMusicStore";
            //dynamic db = Database.Opener.OpenMongo(conStr);

            var albums = db.album.FindAllByArtistId(1);

            foreach (var album in albums)
                Console.WriteLine(album.Title);
        }
    }
}