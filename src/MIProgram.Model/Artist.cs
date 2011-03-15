using System;
using System.Collections.Generic;

namespace MIProgram.Model
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Country> Countries { get; set; }
        //public string ArtistUnparsedCountries { get; set; }
        //public string ArtistParsedCountries { get; set; }
        //public string ArtistActivity { get; set; }
        //public IList<Artist> ArtistLineUpMember { get; set; }
        public IList<Artist> SimilarArtists { get; set; }
        public string OfficialUrl { get; set; }
        //public IList<string> ArtistSimilarArtistsNames { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public Reviewer Reviewer { get; set; }
        //public string ArtistBiography { get; set; }

        public int SortWeight { get { return SimilarArtists.Count; } }

        public Artist(int id, string name, IList<Country> countries, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<Artist> similarArtists)
        {
            #region parameters validation

            if (id == 0)
            { throw new ArgumentException("Artist creation : 'Id' cannot be null"); }
            if (string.IsNullOrEmpty(name))
            { throw new ArgumentException("Artist creation : 'name' cannot be null nor empty"); }
            if (reviewer == null)
            { throw new ArgumentException("Artist creation : 'reviewer' cannot be null nor empty"); }
            if (lastUpdate == DateTime.MinValue)
            { throw new ArgumentException("Artist creation : 'lastUpdate' must be a valid date"); }

            #endregion

            Reviewer = reviewer;
            Id = id;
            SimilarArtists = similarArtists;
            Name = name.ToUpperInvariant();
            Countries = countries;
            OfficialUrl = officialUrl;
            LastUpdate = lastUpdate;
            CreationDate = lastUpdate;

        }

        public void UpdateInfos(IList<Country> countries, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<Artist> similarArtists)
        {
            #region parameters validation

            if (reviewer == null)
            { throw new ArgumentException("Artist update : 'reviewer' cannot be null nor empty"); }
            if (lastUpdate == DateTime.MinValue)
            { throw new ArgumentException("Artist update : 'lastUpdate' must be a valid date"); }

            #endregion

            //if older version than the current one, return without doing anything
            if (lastUpdate < LastUpdate)
            {
                return;
            }

            foreach (var country in countries)
            {
                if(!Countries.Contains(country))
                    Countries.Add(country);
            }

            LastUpdate = lastUpdate;
            Reviewer = reviewer;

            if (!string.IsNullOrEmpty(officialUrl))
            {
                OfficialUrl = officialUrl;
            }

            //Add new similar artists, avoiding duplicates
            foreach (var similarArtist in similarArtists)
            {
                if (!SimilarArtists.Contains(similarArtist))
                {
                    SimilarArtists.Add(similarArtist);
                }
            }
        }

        #region IEqualityMembers

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Artist)) return false;
            return Equals((Artist) obj);
        }


        public bool Equals(Artist obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.Name, Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        #endregion
    }
}