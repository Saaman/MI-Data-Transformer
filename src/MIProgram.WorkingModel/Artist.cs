using System;
using System.Collections.Generic;
using System.Linq;

namespace MIProgram.WorkingModel
{
    public class Artist
    {
        public Reviewer Reviewer { get; private set; }
        public int Id { get; private set; }
        //Similar artist names are always upper invariant
        public IList<string> SimilarArtists { get; private set; }
        //Name is always upper invariant
        public string Name { get; private set; }
        public string OriginCountry { get; private set; }
        public string OfficialUrl { get; private set; }
        public DateTime LastUpdate { get; private set; }
        public DateTime CreationDate { get; private set; }

        public Artist(int id, string name, string originCountry, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<string> similarArtists)
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
            SimilarArtists = similarArtists.Select(x => x.ToUpperInvariant()).ToList();
            Name = name.ToUpperInvariant();
            OriginCountry = originCountry;
            OfficialUrl = officialUrl;
            LastUpdate = lastUpdate;
            CreationDate = lastUpdate;

        }

        public void UpdateInfos(string originCountry, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<string> similarArtists)
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

            if (!string.IsNullOrEmpty(originCountry))
            {
                OriginCountry = originCountry;
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
                string sa = similarArtist;
                if (SimilarArtists.Where(x => x == sa.ToUpperInvariant()).Count() == 0)
                {
                    SimilarArtists.Add(similarArtist.ToUpperInvariant());
                }
            }
        }
    }
}
