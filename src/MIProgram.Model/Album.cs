using System;
using System.Collections.Generic;

namespace MIProgram.Model
{
    public class Album : Product
    {
        //Existing
        public string AlbumTitle { get; set; }
        public DateTime AlbumReleaseDate { get; set; }
        public int SharedScore { get; set; }
        public string AlbumLabel { get; set; }
        public string AlbumParsedType { get; set; }
        public string AlbumUnParsedType { get; set; }
        public Image AlbumCover { get; set; }

        //New
        public DateTime AlbumOriginalReleaseDate { get; set; }
        public int AlbumNumberOfDiscs { get; set; }
        public string AlbumCommercialCode { get; set; }
        public int SharedVisitorsScore { get; set; }

        //Taxo
        public IList<string> AlbumMainStyles { get; set; }
        public string AlbumMainStyle2 { get; set; }
        public IList<string> AlbumStyleAlterations { get; set; }
        public string AlbumParsedStyle { get; set; }
        public string AlbumUnparsedStyle { get; set; }

        //Relations
        public Artist SharedArtist { get; set; }
        public IList<Disc> AlbumDiscs { get; set; }
    }
}
