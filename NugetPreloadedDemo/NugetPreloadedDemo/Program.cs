using System;
using System.Collections.Generic;
using Simple.Data;

namespace NugetPreloadedDemo
{
    internal class Program
    {
        private static void Main()
        {
            //Using the MvcMusicStore DB (http://mvcmusicstore.codeplex.com/)
            const string sqlConnString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MvcMusicStore;Integrated Security=True";
            dynamic db = Database.OpenConnection(sqlConnString);

            var pricesDescending = db.Albums.Select(db.Albums.Price).OrderByPriceDescending().ToList();

            var allVersions = db.AMBest.Select(db.AMBest.Version).OrderByVersionDescending().ToList();
            

            //##################################
            //Initial DEMO
            var albumsByArtistId = db.albums.FindAllByArtistId(1);
            foreach (var al in albumsByArtistId)
                Console.WriteLine(al.Title);

            //##################################
            //Requires all the non-null parameters to be filled
            db.albums.Insert(GenreId: 1, ArtistId: 1, Title: "AAA", Price: 9);

            //##################################
            //Using a fully formed object
            db.albums.Insert(new Album {GenreId = 1, ArtistId = 1, Title = "AAA", Price = 9});

            //##################################
            //Using a full object
            var albumToUpdate = db.albums.Get(675);
            albumToUpdate.Title = "CCC";
            db.albums.Update(albumToUpdate);

            //##################################
            //Delete a full object
            var albumToDelete = db.Albums.Get(675);
            db.Albums.Delete(albumToDelete);

            //##################################
            //Simply retrieve all albums and check the data
            var allAlbums = db.Albums.All(); //Show "Children Could not be evaluated" in debug mode
            var allAlbumsList = db.Albums.All().ToList();

            //##################################
            //Multiple retrieve
            var albumsViaFindAll = db.Albums.FindAll(db.Albums.ArtistId == 50).ToList();

            //##################################
            //Simpler form 
            var albumsViaFindAllBy = db.Albums.FindAllByArtistId(50).ToList();

            //##################################
            //Implicit casting to an album
            Album albumViaGet = db.albums.Get(392); //Works as is

            //##################################
            //Casting to a list requires another function
            var albumsNotEnumerated = db.albums.FindAllByGenreId(1).ToList(); //Show the necessity to enumerate in the debugger
            //IList<Album> albumsRaisingAnException = db.albums.FindAllByGenreId(1).ToList(); // Exception, requires explicit cast
            IList<Album> albumsListFinallyCasted = db.albums.FindAllByGenreId(1).ToList<Album>(); //Deploy in debugger

            //##################################
            //Experience the natural lazy-loading
            var artistWithAlbumsLazyLoaded = db.artists.Get(50);
            foreach (var al in artistWithAlbumsLazyLoaded.Albums) //Where does Albums come from?
                Console.WriteLine(al.Title); //Show the second SQL select in the output

            //##################################
            //Cast the newly discovered natural way and see if it works
            Artist artistWithoutAlbumsLazyLoaded = db.Artists.Get(50);
            foreach (var al in artistWithoutAlbumsLazyLoaded.Albums) //No elements lazy-fetched
                Console.WriteLine(al.Title);

            //##################################
            //Eager fetch the Albums to enable the cast to work
            Artist artistWithAlbumsEagerLoaded = db.Artists.WithAlbums().Get(22); //Eager fetch + casting
            foreach (var al in artistWithAlbumsEagerLoaded.Albums)
                Console.WriteLine(al.Title);

            //##################################
            //Natural chaining in a select to perform an eager loading
            var albumsWithNaturalJoin = db.Albums
                           .FindAllByGenreId(1)
                           .Select(db.Albums.Title, db.Albums.Genre.Name) //Chaining tables eager loads
                           .ToList();

            //##################################
            //POCO modification and casting            
            IList<Album> albumsListCasted = db.Albums
                                    .FindAllByGenreId(1)
                                    .Select(db.Albums.Title, db.Albums.Genre.Name) //Name is now casted
                                    .ToList<Album>();

            //##################################
            //What if the collection has a different name?
            IList<Album> albumsWithGenreAliased = db.Albums
                                    .FindAllByGenreId(1)
                                    .Select(db.Albums.Title, db.Albums.Genre.Name.As("GenreName")) //Introduce AS
                                    .ToList<Album>();

            //##################################
            //What if I want to join „a batch“ of data?
            var albumsWithArtists = db.Albums.All().WithArtists().ToList(); //Artists are joined as a dynamic sub-element

            //##################################
            //Display all the albums of a band, the band name and genre of the album (join on 3 tables)
            var albumsTitleWithArtistNameAndGenreName = db.Albums.FindAllByArtistId(50)
                           .Select(
                               db.Albums.Artist.Name.As("ArtistName"),
                               db.Albums.Title,
                               db.Albums.Genre.Name.As("GenreName")
                )
                           .LeftJoin(db.Artist).On(db.Artists.ArtistId == db.Albums.ArtistId) //Join1
                           .LeftJoin(db.Genre).On(db.Genre.GenreId == db.Albums.GenreId) //Join2
                           .ToList();
        }
    }

    internal class Album
    {
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string GenreName { get; set; }
    }

    internal class Artist
    {
        public Int32 ArtistId { get; set; }
        public String Name { get; set; }
        public IList<Album> Albums { get; set; }
    }
}