using MIProgram.Core.Cleaners;
using MIProgram.Core.ProductStores;
using MIProgram.Model;

namespace MIProgram.Core
{
    public class ReviewsManager<T> where T: Product
    {
        public ProductRepository<T> ProductRepository { get; private set; }

        //private readonly IDGenerator _reviewerIdGenerator = new IDGenerator();
        //private readonly IDGenerator _artistIdGenerator = new IDGenerator();

        //public AlbumReviewBodyCleaner ReviewsBuilderDeprecated { get; private set; }
        private readonly ReviewTextCleaner _reviewTextCleaner = new ReviewTextCleaner();
        private readonly IReviewExploder<T> _reviewExploder;

        public ReviewsManager(ProductRepository<T> productRepository, IReviewExploder<T> reviewExploder)
        {
            ProductRepository = productRepository;
            _reviewExploder = reviewExploder;
        }

        /*public void AddReviewFrom(MIDBRecord record)
        {
            Reviews.Add(ReviewsBuilderDeprecated.BuildReviewFrom(record, this));
        }*/

        /*#region GetOrBuildMethod

        public Artist GetOrBuildArtist(string artistName, IList<Country> originCountry, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<Artist> similarArtists)
        {
            var selectedArtist = (from artist in ProductRepository.Artists where artist.Name.ToUpperInvariant() == artistName.ToUpperInvariant() select artist).FirstOrDefault();

            if (selectedArtist == null)
            {
                selectedArtist = new Artist(_artistIdGenerator.NewID(), artistName, originCountry, officialUrl, lastUpdate, reviewer, similarArtists);
                Artists.Add(selectedArtist);
                return selectedArtist;
            }

            if (selectedArtist.LastUpdate < lastUpdate)
            {
                selectedArtist.UpdateInfos(originCountry, officialUrl, lastUpdate, reviewer, similarArtists);
            }

            return selectedArtist;
        }

        public Reviewer GetOrBuildReviewer(string name, string mailAddress, DateTime lastUpdate)
        {
            var selectedReviewer = (from reviewer in Reviewers where reviewer.Name == name select reviewer).FirstOrDefault();

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
        
        #endregion*/

        public void FinalizeWork()
        {
            if (_reviewTextCleaner != null)
                _reviewTextCleaner.FinalizeWork();
        }
    }
}
