﻿using System;
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
        private readonly YAMLSerializer _yamlSerializer;

        public PublishRailsFixturesProcessor(IWriter writer)
        {
            string fileName = string.Format("{0}-metal_impact", DateTime.Now.ToString("yyyyMMdd"));
            _yamlSerializer = new YAMLSerializer(writer, Constants.RailsPath, fileName);
        }

        public void Process(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;

            if (albumRepository == null)
            {
                throw new InvalidCastException("ProductRepository cannot be cast to AlbumRepository");
            }

            _yamlSerializer.CleanPreviousFile();
            //Publication des reviewers
            _yamlSerializer.SerializeReviewers(albumRepository.Reviewers);
            
            //Publication des pays
            //_railsSerializer.SerializeCountries(CountriesRepository.CountriesLabelsAndCodesDictionnary, "countries");

            //Publication des artistes
            _yamlSerializer.SerializeArtists(albumRepository.Artists.OrderBy(x => x.SortWeight).ToList());

            
            //Publication des types d'album
            //_railsSerializer.SerializeAlbumTypes(AlbumTypesRepository.Repo.Values, "album_types");

            //Publication des styles d'album
            //_railsSerializer.SerializeAlbumStyles(albumRepository.StylesTree.OrderStylesItems, "album_styles");

            //Publication des labels
            _yamlSerializer.SerializeLabelVendors(AlbumLabelsRepository.Repo);

            //Publication des albums
            _yamlSerializer.SerializeAlbums(albumRepository.Products.OrderBy(x => x.SortWeight).ToList());

            //Publication des chroniques
            _yamlSerializer.SerializeReviews(albumRepository.Products.Select(x => x.Review).ToList());

            _yamlSerializer.CloseFile();
        }

        public string ProcessDescription
        {
            get { return "Publication des reviews pour rails... "; }
        }
    }
}