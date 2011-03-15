using System;
using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using MIProgram.Core;
using MIProgram.Core.DataParsers;
using MIProgram.Core.Logging;
using MIProgram.Core.Writers;
using MIProgram.Model;
using System.Linq;


namespace MetalImpactApp.Operations
{
    public class PublishReviewsWithNewModelProcessor : IOperationProcessor
    {
        private readonly StylesParser _stylesParser;
        private readonly IWriter _writer;
        private readonly string _outputDir;
        private readonly PublishAlbumCountriesProcessor _publishAlbumCountriesProc;
        private readonly AlbumTypesParser _albumTypesParser;
        private readonly SQLSerializer _sqlSerializer;

        public PublishReviewsWithNewModelProcessor(IWriter writer, string outputDir)
        {
            _writer = writer;
            _outputDir = outputDir;
            _stylesParser = new StylesParser();
            _albumTypesParser = new AlbumTypesParser();
            _publishAlbumCountriesProc = new PublishAlbumCountriesProcessor(writer, null);
            _sqlSerializer = new SQLSerializer(_writer, outputDir);
        }

        public void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            var newReviews = new List<Review>();

            var newReviewers = PublishReviewers(worker, e, managerDeprecated);

            var countries = _publishAlbumCountriesProc.ProcessMigration(worker, e, managerDeprecated, _outputDir);

            var artists = PublishArtists(worker, e, managerDeprecated, countries);

            /*foreach (var review in managerDeprecated.ParsedReviews)
            {
                try
                {
                    var newReview = Mapper.Map<MIProgram.WorkingModel.Review, Review>(review);

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
                    if (_countryCodesParser.TryParse(review.Album.Artist.OriginCountry, review.Id, ref countryDefinition))
                    {
                        ((Album)newReview.ReviewProduct).SharedArtist.Countries = countryDefinition.CountryLabels;
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

                managerDeprecated.Infos = "Publication des reviews avec le nouveau modèle... ";
                worker.ReportProgress(++count * 100 / (managerDeprecated.ParsedReviews.Count));
            }

            
            managerDeprecated.Infos = "Ecriture des artistes...";
            _writer.WriteCSV(newReviews.Select(x => ((Album)x.ReviewProduct).SharedArtist).Distinct(Artist.ComparerInstance).ToList(), "MIArtists", _outputDir);

            managerDeprecated.Infos = "Ecriture des albums...";
            _writer.WriteCSV(newReviews.Select(x => ((Album)x.ReviewProduct)).Distinct().ToList(), "MIAlbums", _outputDir);

            managerDeprecated.Infos = "Ecriture des reviews...";
            _writer.WriteCSV(newReviews.Distinct().ToList(), "MIReviews", _outputDir);*/
        }

        private IList<Reviewer> PublishReviewers(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            managerDeprecated.Infos = "Publication des reviewers avec le nouveau modèle... ";
            var count = 0;
            worker.ReportProgress(count);
            var newReviewers = new List<Reviewer>();

            // publish Reviewers
            foreach (var reviewer in managerDeprecated.ParsedReviewers)
            {
                try
                {
                    var newReviewer = Mapper.Map<MIProgram.WorkingModel.Reviewer, Reviewer>(reviewer);
                    newReviewers.Add(newReviewer);

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la génération des reviewers avec le nouveau modèle (reviewer '{0}') : {1}", reviewer.Name, ex.Message), ErrorLevel.Error);
                    continue;
                }

                managerDeprecated.Infos = "Publication des reviewers avec le nouveau modèle... ";
                worker.ReportProgress(++count * 100 / (managerDeprecated.ParsedReviewers.Count));
            }

            managerDeprecated.Infos = "Ecriture des reviewers...";
            _sqlSerializer.SerializeReviewers(newReviewers, "reviewers");

            return newReviewers;
        }

        private IList<Artist> PublishArtists(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated, IDictionary<int, CountryDefinition> artistCountries)
        {
            managerDeprecated.Infos = "Publication des artistes avec le nouveau modèle... ";
            var count = 0;
            worker.ReportProgress(count);
            var newArtists = new List<Artist>();

            // publish Reviewers
            foreach (var artist in managerDeprecated.ParsedArtists)
            {
                try
                {
                    var newArtist = Mapper.Map<MIProgram.WorkingModel.Artist, Artist>(artist);
                    newArtist.Countries = artistCountries.Where(x => x.Key == newArtist.Id).Select(x => x.Value.CountryLabels).First();
                    newArtists.Add(newArtist);

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la génération des artistes avec le nouveau modèle (artiste '{0}') : {1}", artist.Name, ex.Message), ErrorLevel.Error);
                    continue;
                }

                managerDeprecated.Infos = "Publication des artistes avec le nouveau modèle... ";
                worker.ReportProgress(++count * 100 / (managerDeprecated.ParsedReviews.Count));
            }

            //Build similar artists references based on names
            foreach (var artist in newArtists)
            {
                var currentArtist = artist;
                currentArtist.SimilarArtists = newArtists.Where(
                    x => currentArtist.ArtistSimilarArtistsNames.Contains(x.Name, new UpperInvariantComparer())
                        && x != currentArtist).ToList();
                currentArtist.ArtistSimilarArtistsNames = currentArtist.ArtistSimilarArtistsNames.Where(
                    y => !currentArtist.SimilarArtists.Select(x => x.Name).Contains(y, new UpperInvariantComparer())).ToList();
            }

            managerDeprecated.Infos = "Ecriture des artistes...";
            _sqlSerializer.SerializeArtists(newArtists, "artists");

            return newArtists;
        }
    }
}