using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Model;

namespace MIProgram.Core.ProductStores
{
    public abstract class ProductRepository<T> where T : Product
    {
        private readonly List<Func<Product, bool>> _filtersDefinitions;
        private readonly IList<Artist> _artists = new List<Artist>();
        private readonly IList<Reviewer> _reviewers = new List<Reviewer>();
        private IList<T> _filteredProducts;

        /*Id generators*/
        private readonly IDGenerator _artistIdGenerator = new IDGenerator();
        private readonly IDGenerator _reviewerIdGenerator = new IDGenerator();

        protected ProductRepository(List<Func<Product, bool>> filtersDefinitions)
        {
            _filtersDefinitions = filtersDefinitions;
        }

        #region Common stuff

        protected Artist GetOrBuildArtist(string artistName, IList<Country> countries, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<Artist> similarArtists)
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

        protected Reviewer GetOrBuildReviewer(string name, string mailAddress, DateTime lastUpdate)
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

        #endregion
        
        #region Getters

        public IList<Artist> Artists
        {
            get { return _artists; }
        }

        public IList<Reviewer> Reviewers
        {
            get { return _reviewers; }
        }

        public IList<T> FilteredProducts
        {
            get
            {
                if(_filteredProducts != null)
                {
                    return _filteredProducts;
                }

                if(_filtersDefinitions == null || _filtersDefinitions.Count == 0)
                {
                    return Products;
                }

                var results = Products.AsEnumerable();

                foreach (var filterDefinition in _filtersDefinitions)
                {
                    var filter = filterDefinition;
                    results = results.Where(x => filter((Product)x));
                }

                _filteredProducts = results.ToList();
                return _filteredProducts;
            }
        }

        #endregion

        public abstract void Add(IExplodedReview<T> explodedReview);

        public abstract IList<T> Products { get; }
    }
}