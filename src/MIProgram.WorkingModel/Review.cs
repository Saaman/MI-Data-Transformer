using System;

namespace MIProgram.WorkingModel
{
    public class Review
    {
        public int Id { get; private set; }
        public Album Album { get; private set; }
        public Reviewer Reviewer { get; private set; }
        public int Score { get; private set; }
        public int Hits { get; private set; }
        public string Text { get; private set; }
        public DateTime LastUpdateDate { get; set; }
        public string DeezerAlbum {get; private set;}
        public string DeezerArtist {get; private set;}

        public Review(int id, Album album, Reviewer reviewer, int score, int hits, string text, DateTime lastUpdateDate, string deezerAlbum, string deezerArtist)
        {
            Id = id;
            Album = album;
            Reviewer = reviewer;
            Score = score;
            Hits = hits;
            Text = text;
            LastUpdateDate = lastUpdateDate;
            DeezerAlbum = deezerAlbum;
            DeezerArtist = deezerArtist;
        }
    }
}
