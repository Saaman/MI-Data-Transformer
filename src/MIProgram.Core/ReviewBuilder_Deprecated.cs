using MIProgram.Core.Cleaners;

namespace MIProgram.Core
{
    public partial class ReviewBuilder_Deprecated
    {
        private readonly ReviewTextCleaner _reviewTextCleaner = new ReviewTextCleaner();
        //private readonly InMemoryReplacementsManager _temporaryReplacementsManager;
        private readonly ICanShowReviewCleaningForm _mainForm;
        private readonly IDGenerator _albumIdGenerator = new IDGenerator();

        public ReviewBuilder_Deprecated(RemovalManager main, InMemoryReplacementsManager temporaryReplacementsManager, ICanShowReviewCleaningForm mainForm)
        {
            //_temporaryReplacementsManager = temporaryReplacementsManager;
            _mainForm = mainForm;
        }

        /*public Review BuildReviewFrom(MIDBRecord record, ReviewsManager reviewsManager)
        {
            var recordTitle = ParseTitle(record.Title.Replace((char)8211, '-'), record.Id);
            var recordOriginCountry = ExtractInfo(_countryRegex, record.Text, record.Id, "Country");

            var reviewer = reviewsManager.GetOrBuildReviewer(record.ReviewerName, record.ReviewerMail, record.CreationDate);

            var text = ExtractText(record);

            var similarArtists = SearchForArtistsReferenced(text, (recordTitle.Split(Pipe))[0]);

            var artist = reviewsManager.GetOrBuildArtist((recordTitle.Split(Pipe))[0], recordOriginCountry,
                (record.OfficialUrl.Contains("mailto")) ? string.Empty : record.OfficialUrl, record.CreationDate, reviewer
                , similarArtists);

            var releaseDate = ExtractInfo(_releaseDateRegex, record.Text, record.Id, "Release date", 2);
            var labelInfos = ParseLabel(ExtractInfo(_labelRegex, record.Text, record.Id, "Label"));

            var label = (string.IsNullOrEmpty(labelInfos)) ? string.Empty : (labelInfos.Split(Pipe))[0];
            var distributor = (string.IsNullOrEmpty(labelInfos)) ? string.Empty : (labelInfos.Split(Pipe))[1];

            var musicType = ExtractInfo(_musicTypeRegex, record.Text, record.Id, "Music type");
            var recordType = ExtractInfo(_recordTypeRegex, record.Text, record.Id, "Record type");
            var playTimeInfos = ParsePlayTime(ExtractInfo(_playTimeRegex, record.Text, record.Id, "Playtime"), record.Id);

            var songsCount = (string.IsNullOrEmpty(playTimeInfos)) ? 0 : Convert.ToInt32((playTimeInfos.Split(Pipe))[0]);
            TimeSpan playTime = TimeSpan.FromMinutes(0);

            if (!string.IsNullOrEmpty(playTimeInfos))
            {
                if (playTimeInfos.Split(Pipe).Length == 2)
                {
                    playTime = TimeSpan.FromMinutes(Convert.ToInt32((playTimeInfos.Split(Pipe))[1]));
                }
                else if (playTimeInfos.Split(Pipe).Length == 3)
                {
                    var duration = TimeSpan.FromHours(Convert.ToInt32((playTimeInfos.Split(Pipe))[1]));
                    duration = duration.Add(TimeSpan.FromMinutes(Convert.ToInt32((playTimeInfos.Split(Pipe))[2])));
                    playTime = duration;
                }
                else if (playTimeInfos.Split(Pipe).Length == 4)
                {
                    var duration = TimeSpan.FromMinutes(Convert.ToInt32((playTimeInfos.Split(Pipe))[1]));
                    duration = duration.Add(TimeSpan.FromMinutes(Convert.ToInt32((playTimeInfos.Split(Pipe))[3])));
                    playTime = duration;
                    songsCount += Convert.ToInt32((playTimeInfos.Split(Pipe))[2]);
                }
            }

            var similarAlbums = SearchForAlbumsReferenced(text, recordTitle.Split(Pipe)[1]);

            var album = new Album(artist, recordTitle.Split(Pipe)[1], releaseDate, label, distributor, musicType, recordType, playTime, record.CoverFileName, songsCount, _albumIdGenerator.NewID(), similarAlbums);

            return new Review(record.Id, album, reviewer, record.Score, record.Hits, text, record.LastUpdateDate ?? record.CreationDate, record.DeezerAlbum, record.DeezerArtist);

        }*/

        public void FinalizeWork()
        {
            if (_reviewTextCleaner != null)
                _reviewTextCleaner.FinalizeWork();
        }
    }
}
