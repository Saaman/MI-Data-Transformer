using System;
using MIProgram.DataAccess;
using MIProgram.Model;

namespace MIProgram.Core
{
    public class AlbumReviewExploder : IReviewExploder<Album>
    {
        readonly AlbumReviewParsingMethods _parsingMethods = new AlbumReviewParsingMethods();

        public IExplodedReview<Album> ExplodeReviewFrom(MIDBRecord record)
        {
            var recordTitle = _parsingMethods.ExtractTitle(record.Title.Replace((char)8211, '-'), record.Id);
            var recordOriginCountry = _parsingMethods.ExtractCountry(record.Text, record.Id);

            //var reviewer = reviewsManager.GetOrBuildReviewer(record.ReviewerName, record.ReviewerMail, record.CreationDate);

            var reviewBody = _parsingMethods.ExtractReviewBody(record.Text, record.Id);
            //var text = ExtractText(record); => TODO : add a step after the explode to apply & add replacements for the review

            var similarArtists = _parsingMethods.ExtractArtistsReferences(reviewBody);

            /*var artist = reviewsManager.GetOrBuildArtist((recordTitle.Split(Pipe))[0], recordOriginCountry,
                (record.OfficialUrl.Contains("mailto")) ? string.Empty : record.OfficialUrl, record.CreationDate, reviewer
                , similarArtists);
            */
            var releaseDate = _parsingMethods.ExtractReleaseDate(record.Text, record.Id);
            var labelInfos = _parsingMethods.ExtractLabelAndDistributor(record.Text, record.Id);

            var officialUrl = (record.OfficialUrl.Contains("mailto")) ? string.Empty : record.OfficialUrl;

            var label = (labelInfos.Count == 0) ? string.Empty : labelInfos[0];
            var distributor = (labelInfos.Count < 2) ? string.Empty : labelInfos[1];

            var musicType = _parsingMethods.ExtractMusicType(record.Text, record.Id);
            var recordType = _parsingMethods.ExtractAlbumType(record.Text, record.Id);
            var playTimeInfos = _parsingMethods.ExtractPlayTime(record.Text, record.Id);

            var songsCount = (playTimeInfos == null) ? 0 : Convert.ToInt32(playTimeInfos[0]);
            TimeSpan playTime = TimeSpan.FromMinutes(0);

            if (playTimeInfos != null)
            {
                if (playTimeInfos.Count == 2)
                {
                    playTime = TimeSpan.FromMinutes(Convert.ToInt32(playTimeInfos[1]));
                }
                else if (playTimeInfos.Count == 3)
                {
                    var duration = TimeSpan.FromHours(Convert.ToInt32(playTimeInfos[1]));
                    duration = duration.Add(TimeSpan.FromMinutes(Convert.ToInt32(playTimeInfos[2])));
                    playTime = duration;
                }
                else if (playTimeInfos.Count == 4)
                {
                    var duration = TimeSpan.FromMinutes(Convert.ToInt32(playTimeInfos[1]));
                    duration = duration.Add(TimeSpan.FromMinutes(Convert.ToInt32(playTimeInfos[3])));
                    playTime = duration;
                    songsCount += Convert.ToInt32(playTimeInfos[2]);
                }
            }

            var similarAlbums = _parsingMethods.ExtractAlbumsReferences(reviewBody);

            /*var album = new Album(artist, recordTitle.Split(Pipe)[1], releaseDate, label, distributor, musicType, recordType, playTime, record.CoverFileName, songsCount, _albumIdGenerator.NewID(), similarAlbums);

            return new Review(record.Id, album, reviewer, record.Score, record.Hits, reviewBody, record.LastUpdateDate ?? record.CreationDate, record.DeezerAlbum, record.DeezerArtist);*/

            return new AlbumExplodedReview(record.ReviewerName, record.ReviewerMail, record.CreationDate, recordTitle[0],
                                           recordOriginCountry, officialUrl, similarArtists, recordTitle[1], releaseDate,
                                           label, distributor, musicType, recordType, playTime, record.CoverFileName,
                                           songsCount, similarAlbums, record.Id, record.Score, record.Hits, reviewBody,
                                           record.LastUpdateDate ?? record.CreationDate, record.DeezerAlbum,
                                           record.DeezerArtist);

        }
    }
}