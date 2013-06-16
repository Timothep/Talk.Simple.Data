using Simple.Data;

namespace NugetPreloadedDemo
{
    class Program
    {
        static void Main()
        {
            const string sqlConnString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MvcMusicStore;Integrated Security=True";
            const string mongoConnString = @"mongodb://localhost:27017/MvcMusicStore";
        }
    }
}
