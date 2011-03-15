using FileHelpers;
using MIProgram.WorkingModel;

namespace MIProgram.Core.Creators
{
    [DelimitedRecord(";")] 
    public class CSVRecord
    {
        private int _00Id;
        private string _01ArtistName;
        private string _02AlbumTitle;
        private string _03ReleaseDate;
        private string _04MusicType;
        private string _05Label;
        private string _06Distributor;
        private int _07SongsCount;
        private string _08PlayTime;
        private string _09RecordType;
        private int _10Score;
        private string _11ArtistOriginCountry;
        private string _12ArtistOfficialUrl;
        private string _13ReviewerName;
        private int _14Hits;
        private string _15ReviewerMailAddress;
        private string _16CoverFileName;

        public const string CSVHeader = "Id;Artiste;Album;Sortie du Scud;Genre;Label;Distributeur;Nombre de chansons;Durée en minutes;Type;Note;Pays;Site du groupe;Chroniqueur;Hits;Mail du chroniqueur;Fichier d'Artwork";

        private CSVRecord()
        {
        }

        public CSVRecord(Review review)
        {
            _00Id = review.Id;
            _14Hits = review.Hits;
            _10Score = review.Score;
            _13ReviewerName = review.Reviewer.Name.Replace(';', ',');
            _15ReviewerMailAddress = review.Reviewer.MailAddress.Replace(';', ',');
            _01ArtistName = review.Album.Artist.Name.Replace(';', ',');
            _12ArtistOfficialUrl = review.Album.Artist.OfficialUrl.Replace(';', ',');
            _11ArtistOriginCountry = review.Album.Artist.OriginCountry.Replace(';', ',');
            _02AlbumTitle = review.Album.Title.Replace(';', ',');
            _16CoverFileName = review.Album.CoverFileName.Replace(';', ',');
            _06Distributor = review.Album.Distributor.Replace(';', ',');
            _05Label = review.Album.Label.Replace(';', ',');
            _04MusicType = review.Album.MusicType.Replace(';', ',');
            _08PlayTime = review.Album.Playtime.TotalMinutes.ToString();
            _09RecordType = review.Album.RecordType.Replace(';', ',');
            _03ReleaseDate = review.Album.ReleaseDate.Replace(';', ',');
            _07SongsCount = review.Album.SongsCount;

        }
    }
}
