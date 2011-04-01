﻿using System;
using System.Collections.Generic;
using MIProgram.Core.Extensions;
using System.Linq;

namespace MIProgram.Core.Model
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Country> Countries { get; set; }
        public IList<string> RawSimilarArtists { get; set; }
        public IList<Artist> SimilarArtists { get; set; }
        public string OfficialUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public Reviewer Reviewer { get; set; }
        
        public int SortWeight { get { return SimilarArtists.Count; } }

        public Artist(int id, string name, IList<Country> countries, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<string> similarArtists)
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
            RawSimilarArtists = similarArtists.Where(x => x.ToUpperInvariant() != name.ToUpperInvariant()).ToList();
            Name = name.ToUpperInvariant();
            Countries = countries;
            OfficialUrl = officialUrl;
            LastUpdate = lastUpdate;
            CreationDate = lastUpdate;

        }

        public void UpdateInfos(IList<Country> countries, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<string> similarArtists)
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
                if (!RawSimilarArtists.Contains(similarArtist) && similarArtist != Name)
                {
                    RawSimilarArtists.Add(similarArtist);
                }
            }
        }

        public string ToSQLInsert()
        {
            return string.Format("INSERT INTO  `midatabase`.`{0}` (`artist_id`, `name`, `creation_date`, `official_site`, `reviewer_id`, `countries`, `similar_artists_nodes`, `additionnal_similar_artists_names`, `sorting_weight`) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');"
                    , SQLTableName, Id, Name.ToCamelCase().GetSafeMySQL(), CreationDate.ToUnixTimeStamp(), OfficialUrl, Reviewer.Id
                    , Countries.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.CountryName.ToCamelCase()).Trim('|')
                    , SimilarArtists.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.Id).Trim('|')
                    , RawSimilarArtists.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.ToCamelCase()).Trim('|')
                    , SortWeight);
        }

        public const string SQLTableName = "mi_artist";

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