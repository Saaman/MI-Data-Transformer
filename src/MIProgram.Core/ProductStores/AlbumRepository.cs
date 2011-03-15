using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Model;

namespace MIProgram.Core.ProductStores
{
    public class AlbumRepository : IProductRepository<Album>
    {
        private readonly IList<Artist> _artists = new List<Artist>();
        private readonly IList<Reviewer> _reviewers = new List<Reviewer>();
        private readonly IList<Album> _albums = new List<Album>();

        /*Id generators*/
        private readonly IDGenerator _artistIdGenerator = new IDGenerator();
        private readonly IDGenerator _reviewerIdGenerator = new IDGenerator();

        #region IProductRepository Implementation

        public Artist GetOrBuildArtist(string artistName, IList<Country> countries, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<Artist> similarArtists)
        {
            var selectedArtist = (from artist in Artists where artist.Name == artistName.ToUpperInvariant() select artist).SingleOrDefault();

            if (selectedArtist == null)
            {
                selectedArtist = new Artist(_artistIdGenerator.NewID(), artistName, countries, officialUrl, lastUpdate, reviewer, similarArtists);
                Artists.Add(selectedArtist);
                return selectedArtist;
            }

            selectedArtist.UpdateInfos(countries, officialUrl, lastUpdate, reviewer, similarArtists);
            
            return selectedArtist;
        }

        public Reviewer GetOrBuildReviewer(string name, string mailAddress, DateTime lastUpdate)
        {
            var selectedReviewer = (from reviewer in Reviewers where reviewer.Name == name select reviewer).SingleOrDefault();

            if (selectedReviewer == null)
            {
                selectedReviewer = new Reviewer(_reviewerIdGenerator.NewID(), name, mailAddress, lastUpdate);
                Reviewers.Add(selectedReviewer);
                return selectedReviewer;
            }

            if (selectedReviewer.LastUpdate < lastUpdate)
            {
                selectedReviewer.UpdateInfos(name, mailAddress, lastUpdate);
            }

            return selectedReviewer;
        }

        
        public IList<Artist> Artists
        {
            get { return _artists; }
        }

        public IList<Reviewer> Reviewers
        {
            get { return _reviewers; }
        }

        public IList<Album> Products
        {
            get { return _albums; }
        }

        #endregion
    }
}