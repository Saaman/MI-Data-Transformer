using System;

namespace MIProgram.Core.Model
{
    public class Book : Product
    {
        public string Title { get; set; }
        public Language Language { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int PagesCount { get; set; }
        public string Dimension { get; set; }
        public string ISBN { get; set; }
        public Uri Link { get; set; }

        public string PublishingHouse { get; set; }


        //Relations
        public Artist SharedArtist { get; set; }

        //Taxonomie
        public BookGenre Genre { get; set; }
    }
}