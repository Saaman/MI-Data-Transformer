﻿using System;
using MIProgram.Core;
using MIProgram.Core.AlbumImpl.LocalRepositories;
using MIProgram.Core.DAL.Writers;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;
using MIProgram.Core.Writers;


namespace MetalImpactApp.OperationManagement.AlbumImpl.Operations
{
    public class PublishReviewsWithNewModelProcessor : IOperationProcessor<Album>
    {
        private readonly SQLSerializer _sqlSerializer;

        public PublishReviewsWithNewModelProcessor(IWriter writer)
        {
            _sqlSerializer = new SQLSerializer(writer, Constants.MigrationOperationsOutputDirectoryPath);
        }

        public void Process(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;

            if (albumRepository == null)
            {
                throw new InvalidCastException("ProductRepository cannot be cast to AlbumRepository");
            }

            //Publication des reviewers
            _sqlSerializer.SerializeReviewers(albumRepository.Reviewers, "reviewers");

            //Publication des pays
            _sqlSerializer.SerializeCountries(CountriesRepository.CountriesLabelsAndCodesDictionnary, "countries");

            //Publication des artistes
            _sqlSerializer.SerializeArtists(albumRepository.Artists, "artists");

            //Publication des types d'album
            _sqlSerializer.SerializeAlbumTypes(AlbumTypesRepository.Repo.Values, "album_types");

            //Publication des styles d'album
            _sqlSerializer.SerializeAlbumStyles(albumRepository.StylesTree.OrderStylesItems, "album_styles");
        }

        public string ProcessDescription
        {
            get { return "Publication des reviews avec le nouveau modèle... "; }
        }
    }
}