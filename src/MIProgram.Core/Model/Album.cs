using System;
using System.Collections.Generic;
using System.Linq;

namespace MIProgram.Core.Model
{
    public class Album : Product
    {
        //Existing
        public string Title { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public int Score { get; private set; }
        public string Label { get; private set; }
        public string ParsedType { get; private set; }
        public string UnParsedType { get; private set; }
        public string CoverPath { get; private set; }

        public string DeezerAlbum { get; private set;}
        public string DeezerArtist { get; private set; }
        public string ReviewText { get; private set; }

        public IList<string> RawSimilarAlbums { get; set; }
        public IList<Album> SimilarAlbums { get; set; }

        //Taxo
        public IList<string> MainStyles { get; private set; }
        public string MainStyle2 { get; private set; }
        public IList<string> StyleAlterations { get; private set; }
        public string ParsedStyle { get; private set; }
        public string UnparsedStyle { get; private set; }

        //Relations
        public IList<Disc> Discs { get; private set; }

        public Album(string title, DateTime releaseDate, int score, string label, string coverPath, Artist artist, IList<string> albumSimilarAlbums)
        {
            Title = title;
            ReleaseDate = releaseDate;
            Score = score;
            Label = label;
            CoverPath = coverPath;
            Artist = artist;
            RawSimilarAlbums = albumSimilarAlbums.Where(x => x.ToUpperInvariant() != title.ToUpperInvariant()).ToList();
        }
    }
}