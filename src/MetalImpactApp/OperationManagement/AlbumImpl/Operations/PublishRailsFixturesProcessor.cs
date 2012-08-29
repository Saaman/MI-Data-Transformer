using System;
using System.Linq;
using MIProgram.Core;
using MIProgram.Core.AlbumImpl.LocalRepositories;
using MIProgram.Core.DAL.Writers;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;


namespace MetalImpactApp.OperationManagement.AlbumImpl.Operations
{
    public class PublishRailsFixturesProcessor : IOperationProcessor<Album>
    {
        private readonly RailsSerializer _railsSerializer;

        public PublishRailsFixturesProcessor(IWriter writer)
        {
            _railsSerializer = new RailsSerializer(writer, Constants.RailsPath);
        }

        public void Process(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;

            if (albumRepository == null)
            {
                throw new InvalidCastException("ProductRepository cannot be cast to AlbumRepository");
            }

            //Publication des reviewers
            _railsSerializer.SerializeReviewers(albumRepository.Reviewers, "0-reviewers");
            
            //Publication des pays
            //_railsSerializer.SerializeCountries(CountriesRepository.CountriesLabelsAndCodesDictionnary, "countries");

            //Publication des artistes
            _railsSerializer.SerializeArtists(albumRepository.Artists.OrderBy(x => x.SortWeight).ToList(), "1-artists");

            
            //Publication des types d'album
            //_railsSerializer.SerializeAlbumTypes(AlbumTypesRepository.Repo.Values, "album_types");

            //Publication des styles d'album
            //_railsSerializer.SerializeAlbumStyles(albumRepository.StylesTree.OrderStylesItems, "album_styles");

            //Publication des labels
            _railsSerializer.SerializeLabelVendors(AlbumLabelsRepository.Repo, "2-music_labels");

            //Publication des albums
            _railsSerializer.SerializeAlbums(albumRepository.Products.OrderBy(x => x.SortWeight).ToList(), "3-albums");
        }

        public string ProcessDescription
        {
            get { return "Publication des reviews pour rails... "; }
        }
    }
}