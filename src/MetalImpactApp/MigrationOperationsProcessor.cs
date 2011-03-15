using System;
using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using MIProgram.Core;
using MIProgram.Core.DataParsers;
using MIProgram.Core.Logging;
using MIProgram.Core.Writers;
using System.Linq;
using MIProgram.Model;
using Review = MIProgram.WorkingModel.Review;

namespace MetalImpactApp
{
    public class MigrationOperationsProcessor
    {
        private readonly IWriter _writer;
        private readonly string _migrationDir;
        private readonly StylesParser _stylesParser;
        private readonly AlbumTypesParser _albumTypesParser;
        private readonly CountryCodesParser _countryCodesParser;

        static MigrationOperationsProcessor()
        {
            MappingConfigurations.Init();
        }

        public MigrationOperationsProcessor(IWriter writer, string migrationDir)
        {
            _writer = writer;
            _migrationDir = migrationDir;
            _stylesParser = new StylesParser();
            _albumTypesParser = new AlbumTypesParser();
            _countryCodesParser = new CountryCodesParser();
        }

        public void ExtractCountries(BackgroundWorker worker, DoWorkEventArgs e, IList<Review> reviews, OperationsManager manager)
        {
            manager.Infos = "extraction des pays d'origine... ";
            var count = 0;
            worker.ReportProgress(count);
            var countryCodesDefinitions = new Dictionary<string, CountryDefinition>();

            foreach (var review in reviews)
            {
                try
                {
                    CountryDefinition countryDefinition = null;
                    if (_countryCodesParser.TryParse(review.Album.Artist.OriginCountry, review.Id, ref countryDefinition))
                    {
                        if (!countryCodesDefinitions.Keys.Contains(review.Album.Artist.OriginCountry))
                        {
                            countryCodesDefinitions.Add(review.Album.Artist.OriginCountry, countryDefinition);
                        }
                    }

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction des pays de l'artiste (review {0}) : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                manager.Infos = "extraction des pays d'artistes... ";
                worker.ReportProgress(++count * 100 / (reviews.Count));
            }

            manager.Infos = "publication du dictionnaire des pays... ";
            _writer.WriteTextCollection(
                CountryDefinition.CountriesLabelsAndCodesDictionnary.Select(
                    x => string.Format("{0}({1}) : {2} occurences", x.Key, x.Value
                        , countryCodesDefinitions.Values.Where(y => y.CountryLabels.Contains(x.Key)).Count()))
                .ToList(), "CountryCodesDictionnary", _migrationDir);

            manager.Infos = "publication des pays parsés... ";
            _writer.WriteTextCollection(countryCodesDefinitions.Select(x => string.Format("'{0}' est parsé en '{1}'", x.Key, x.Value.RebuildFromParsedValuesRepository())).ToList(), "countries", _migrationDir);
        }

        public void ExtractAlbumTypes(BackgroundWorker worker, DoWorkEventArgs e, IList<Review> reviews, OperationsManager manager)
        {
            manager.Infos = "extraction des types d'albums... ";
            var count = 0;
            worker.ReportProgress(count);
            var albumTypesDefinitions = new Dictionary<string, AlbumTypeDefinition>();

            foreach (var review in reviews)
            {
                try
                {
                    AlbumTypeDefinition albumTypeDefinition = null;
                    if (_albumTypesParser.TryParse(review.Album.RecordType, review.Id, ref albumTypeDefinition))
                    {
                        if (!albumTypesDefinitions.Keys.Contains(review.Album.RecordType))
                        {
                            albumTypesDefinitions.Add(review.Album.RecordType, albumTypeDefinition);
                        }
                    }

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction des types d'album (review {0}) : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                manager.Infos = "extraction des types d'albums... ";
                worker.ReportProgress(++count * 100 / (reviews.Count));
            }

            /*processor.Infos = "publication des types d'albums... ";
            _writer.WriteTextCollection(
                AlbumTypeDefinition.AlbumTypesValues.Select(
                    x => string.Format("{0} : {1} occurences", x
                        , albumTypesDefinitions.Values.Where(y => y.AlbumType == x).Count()))
                .ToList(), "albumTypesDictionnary", _migrationDir);*/

            manager.Infos = "publication des types d'album parsés... ";
            _writer.WriteTextCollection(albumTypesDefinitions.Select(x => string.Format("'{0}' est parsé en '{1}'", x.Key, x.Value.RebuildFromParsedValuesRepository())).ToList(), "albumTypes", _migrationDir);
        }

        public void ExtractMusicStyles(BackgroundWorker worker, DoWorkEventArgs e, IList<Review> reviews, OperationsManager manager)
        {
            manager.Infos = "extraction des styles... ";
            var count = 0;
            worker.ReportProgress(count);
            var stylesDefinitions = new Dictionary<string, StyleDefinition>();

            foreach (var review in reviews)
            {
                try
                {
                    StyleDefinition styleDefinition = null;
                    if (_stylesParser.TryParse(review.Album.MusicType, review.Id, ref styleDefinition))
                    {
                        if (!stylesDefinitions.Keys.Contains(review.Album.MusicType))
                        {
                            stylesDefinitions.Add(review.Album.MusicType, styleDefinition);
                        }
                    }

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction des styles (review {0}) : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                manager.Infos = "extraction des styles... ";
                worker.ReportProgress(++count * 100 / (reviews.Count));
            }

            manager.Infos = "publication des types de musique... ";
            _writer.WriteTextCollection(
                StyleDefinition.MusicTypesLabels.Select(
                    x => string.Format("{0} : {1} occurences", x.Value
                        , stylesDefinitions.Values.Where(y => y.MusicTypes.Contains(x.Key)).Count()))
                .ToList(), "musicTypes", _migrationDir);

            manager.Infos = "publication des styles principaux... ";
            _writer.WriteTextCollection(
                StyleDefinition.MainStylesValues.Select(
                    x => string.Format("{0} : {1} occurences", x
                        , stylesDefinitions.Values.Where(y => y.MainStyles.Contains(x)).Count()))
                .ToList(), "mainStyles", _migrationDir);

            manager.Infos = "publication des associations type de musique / styles principaux... ";
            _writer.WriteTextCollection(StyleDefinition.StylesAssociations.Select(
                    x => string.Format("{0} associé à {1}", x.Key
                        , StyleDefinition.MusicTypesLabels[x.Value])).ToList(), 
                        "musicStylesAndTypesAssociations", _migrationDir);

            manager.Infos = "publication des altérations de style... ";
            _writer.WriteTextCollection(StyleDefinition.StyleAlterationsValues.Select(
                    x => string.Format("{0} : {1} occurences", x
                        , stylesDefinitions.Values.Where(y => y.StyleAlterations.Contains(x)).Count())
                ).ToList(), "styleAlterations", _migrationDir);


            manager.Infos = "publication des styles parsés... ";
            _writer.WriteTextCollection(stylesDefinitions.Select(x => string.Format("'{0}' est parsé en '{1}'", x.Key, x.Value.RebuildFromParsedValuesRepository())).ToList(), "styles", _migrationDir);
        }

        public void PublishReviewsWithNewModel(BackgroundWorker worker, DoWorkEventArgs e, ReviewsManager reviewsManager, OperationsManager manager)
        {
            manager.Infos = "Publication des reviews avec le nouveau modèle... ";
            var count = 0;
            worker.ReportProgress(count);
            var newReviews = new List<MIProgram.Model.Review>();

            foreach (var review in reviewsManager.Reviews)
            {
                try
                {
                    var newReview = Mapper.Map<Review, MIProgram.Model.Review>(review);
                    
                    StyleDefinition styleDefinition = null;
                    if (_stylesParser.TryParse(review.Album.MusicType, review.Id, ref styleDefinition))
                    {
                        ((Album)newReview.ReviewProduct).AlbumMainStyles = styleDefinition.MainStyles;
                        ((Album)newReview.ReviewProduct).AlbumStyleAlterations = styleDefinition.StyleAlterations;
                        ((Album)newReview.ReviewProduct).AlbumParsedStyle = styleDefinition.RebuildFromParsedValuesRepository();
                    }

                    AlbumTypeDefinition albumTypeDefinition = null;
                    if (_albumTypesParser.TryParse(review.Album.RecordType, review.Id, ref albumTypeDefinition))
                    {
                        ((Album)newReview.ReviewProduct).AlbumParsedType = albumTypeDefinition.RebuildFromParsedValuesRepository();
                    }

                    CountryDefinition countryDefinition = null;
                    if(_countryCodesParser.TryParse(review.Album.Artist.OriginCountry, review.Id, ref countryDefinition))
                    {
                        ((Album) newReview.ReviewProduct).SharedArtist.ArtistCountries = countryDefinition.CountryLabels;
                        ((Album)newReview.ReviewProduct).SharedArtist.ArtistParsedCountries = countryDefinition.RebuildFromParsedValuesRepository();
                    }

                    newReviews.Add(newReview);

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la génération des reviews avec le nouveau modèle (review {0}) : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                manager.Infos = "Publication des reviews avec le nouveau modèle... ";
                worker.ReportProgress(++count * 100 / (reviewsManager.Reviews.Count));
            }

            manager.Infos = "Ecriture des reviewers...";
            //_writer.WriteCSV(newReviews.Select(x => x.ReviewProduct).SharedArtist).Distinct(Artist.ComparerInstance).ToList(), "MIArtists", _migrationDir);

            manager.Infos = "Ecriture des artistes...";
            _writer.WriteCSV(newReviews.Select(x => ((Album)x.ReviewProduct).SharedArtist).Distinct(Artist.ComparerInstance).ToList(), "MIArtists", _migrationDir);

            manager.Infos = "Ecriture des albums...";
            _writer.WriteCSV(newReviews.Select(x => ((Album)x.ReviewProduct)).Distinct().ToList(), "MIAlbums", _migrationDir);

            manager.Infos = "Ecriture des reviews...";
            _writer.WriteCSV(newReviews.Distinct().ToList(), "MIReviews", _migrationDir);
        }
    }
}