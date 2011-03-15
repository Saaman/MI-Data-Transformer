using System;
using System.Collections.Generic;

namespace MIProgram.WorkingModel
{
    public class Album : Product
    {
        public int Id { get; private set; }
        public IList<string> SimilarAlbums { get; private set; }
        public Artist Artist { get; private set; }
        public string Title { get; private set; }
        public string ReleaseDate { get; private set; }
        public string Label { get; private set; }
        public string Distributor { get; private set; }
        public string MusicType { get; private set; }
        public string RecordType { get; private set; }
        public TimeSpan Playtime { get; private set; }
        public string CoverFileName { get; private set; }
        public int SongsCount { get; private set; }

        public Album(Artist artist, string title, string releaseDate, string label, string distributor, string musicType, string recordType, TimeSpan playTime, string coverFileName, int songsCount, int id, IList<string> similarAlbums)
        {
            Artist = artist;
            Title = title;
            ReleaseDate = releaseDate;
            Label = label;
            Distributor = distributor;
            MusicType = musicType;
            RecordType = recordType;
            Playtime = playTime;
            CoverFileName = coverFileName;
            SongsCount = songsCount;
            Id = id;
            SimilarAlbums = similarAlbums;
        }
    }
}
