using System;
using FileHelpers;

namespace MIProgram.Core.DAL.Models
{
    [DelimitedRecord("|")]
    [IgnoreEmptyLines]
    public class MIDBRecord : IMIDBRecord
    {
        private MIDBRecord() {}

        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.NotAllow)]
        private int _id;
        public int Id
        {
            get { return _id; }
            private set { _id = value; }
        }
        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.NotAllow)]
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd HH:mm:ss")]
        private DateTime _creationDate;
        public DateTime CreationDate
        {
            get { return _creationDate; }
            private set { _creationDate = value; }
        }
        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.NotAllow)]
        private string _title;
        public string Title
        {
            get { return _title; }
            private set { _title = value; }
        }
        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.AllowForBoth)]
        private string _text;
        public string Text
        {
            get { return _text.Replace(Environment.NewLine, ""); }
            private set { _text = value; }
        }
        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.NotAllow)]
        private string _reviewerName;
        public string ReviewerName
        {
            get { return _reviewerName; }
            private set { _reviewerName = value; }
        }
        [FieldQuoted('$', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        private string _reviewerMail;
        public string ReviewerMail
        {
            get { return _reviewerMail; }
            private set { _reviewerMail = value; }
        }
        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.NotAllow)]
        private int _score;
        public int Score
        {
            get { return _score; }
            private set { _score = value; }
        }
        [FieldQuoted('$', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        private string _coverFileName;
        public string CoverFileName
        {
            get { return _coverFileName; }
            private set { _coverFileName = value; }
        }
        [FieldQuoted('$', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        private string _officialUrl;
        public string OfficialUrl
        {
            get { return _officialUrl; }
            private set { _officialUrl = value; }
        }
        [FieldQuoted('$', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        private string _officialUrlTitle;
        public string OfficialUrlTitle
        {
            get { return _officialUrlTitle; }
            private set { _officialUrlTitle = value; }
        }
        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.NotAllow)]
        private int _hits;
        public int Hits
        {
            get { return _hits; }
            private set { _hits = value; }
        }
        [FieldQuoted('$', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        private string _language;
        public string Language
        {
            get { return _language; }
            private set { _language = value; }
        }

        [FieldQuoted('$', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        private string _upc;
        public string UPC
        {
            get { return _upc; }
            private set { _upc = value; }
        }

        [FieldQuoted('$', QuoteMode.OptionalForRead, MultilineMode.NotAllow)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd HH:mm:ss")]
        private DateTime? _lastUpdateDate;
        public DateTime? LastUpdateDate
        {
            get { return _lastUpdateDate; }
            private set { _lastUpdateDate = value; }
        }

        [FieldQuoted('$', QuoteMode.OptionalForRead, MultilineMode.NotAllow)]
        [FieldOptional]
        private string _deezerAlbum;
        public string DeezerAlbum
        {
            get { return _deezerAlbum; }
            private set { _deezerAlbum = value; }
        }

        [FieldQuoted('$', QuoteMode.OptionalForRead, MultilineMode.NotAllow)]
        [FieldOptional]
        private string _deezerArtist;
        public string DeezerArtist
        {
            get { return _deezerArtist; }
            private set { _deezerArtist = value; }
        }

        public MIDBRecord(string id, string creationDate, string title, string text, string reviewerName, string reviewerMail,
                          string score, string coverFileName, string officialUri, string officialUriTitle, string hits, string lastUpdateDate,
                          string deezerAlbum, string deezerArtist)
        {
            Id = Convert.ToInt32(id);
            CreationDate = Convert.ToDateTime(creationDate);
            Title = title;
            Text = text;
            ReviewerName = reviewerName;
            ReviewerMail = reviewerMail;
            Score = Convert.ToInt32(score);
            CoverFileName = coverFileName;
            OfficialUrl = officialUri;
            OfficialUrlTitle = officialUriTitle;
            Hits = Convert.ToInt32(hits);
            LastUpdateDate = Convert.ToDateTime(lastUpdateDate);
            DeezerAlbum = deezerAlbum;
            DeezerArtist = deezerArtist;
        }
    }
}