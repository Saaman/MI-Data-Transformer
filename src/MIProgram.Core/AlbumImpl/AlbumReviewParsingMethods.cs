using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MIProgram.Core.Logging;

namespace MIProgram.Core.AlbumImpl
{
    public class AlbumReviewParsingMethods
    {
        //Regex
        private readonly Regex _titleRegex = new Regex(@"^\s*(.+)\s+\(\S+\)\s*-\s+(.+)\s+\(\d+\)\s*$");
        private readonly Regex _titleRegex2 = new Regex(@"^\s*(.+)\s+-\s+(.+)\s+\(\d+\)\s*$");
        private readonly Regex _countryRegex = new Regex(@"Pays\s+:\s+([^<]*)\s*<");
        private readonly Regex _releaseDateRegex = new Regex(@"Sortie du (Scud|DVD)\s+:\s+([^<]*)\s*<");
        private readonly Regex _labelRegex = new Regex(@"Label\s+:\s+([^<]*)\s*<");
        private readonly Regex _musicTypeRegex = new Regex(@"Genre\s+:\s*([^<]*)\s*<");
        private readonly Regex _recordTypeRegex = new Regex(@"Type\s+:\s*([^<]*)\s*<");
        private readonly Regex _playTimeRegex = new Regex(@"Playtime\s+:\s+([^<]*)\s*<");
        private readonly Regex _albumReferencesRegex = new Regex(@"<i>\s*([^<]*)\s*</i>");
        private readonly Regex _textRegex = new Regex("^.+<p\\s*align=\\s*\"\\s*justify\\s*\"\\s*>(.+)<\\s*/p\\s*>.*$");

        private readonly Regex _labelInfosRegex = new Regex(@"^\s*(.*)\s*/\s*(.*)\s*$");
        private readonly Regex _playTimeInfosRegex = new Regex(@"^\s*(\d*)\s*Titres\s+-\s*(\d*)\s+Mins\s*$", RegexOptions.IgnoreCase);
        private readonly Regex _playTimeInfosRegex2 = new Regex(@"^\s*(\d*)\s+Titres\s+-\s+(\d+)\s+Heure\s+.*(\d+)\s+Mins\s*$");
        private readonly Regex _playTimeInfosRegex3 = new Regex(@"^\s*(\d*)\s+Titres\s*$");
        private readonly Regex _playTimeInfosRegex4 = new Regex(@"^\s*Cd\s*1\s*:\s*(\d+)\s*Titres\s*[\-]*\s*(\d*)\s*M[i]*n[s]*\s*\d*\s*/*-*\+*\s*Cd\s*[12]\s*:\s*(\d+)\s*Titre[s]*\s*[\-]*\s*(\d*)\s*M[i]*n[s]*\s*$", RegexOptions.IgnoreCase);
        private readonly Regex _playTimeInfosRegex5 = new Regex(@"^\s*(\d+)\s+Titres\s*[\-]*\s+(\d+)\s+Mins\s*[\-\+]+\s*(\d+)\s+Titre[s]*\s*[\-]*\s+(\d+)\s+Mins\s*$", RegexOptions.IgnoreCase);

        private readonly Regex _artistReferencesRegex = new Regex("[a-zA-Z0-9]+[,]{0,1}");
        private readonly Regex _HasLetterRegex = new Regex("[a-zA-Z]+");

        //private const char Pipe = '|';


        #region Parse methods

        private readonly IList<char> ponctuations = new List<char> { ',', '.', '(', ')', ':', (char)8230 };

        public IList<string> ExtractArtistsReferences(string text)
        {
            var words = text.Split(' ');
            var newSentence = true;
            var similarArtists = new List<string>();
            string currentSimilarArtist = string.Empty;
            foreach (var w in words)
            {
                string word = string.Empty;
                bool rem = false;

                foreach (char c in w)
                {
                    if (c == '<')
                        rem = true;
                    if (!rem)
                        word += c;
                    if (c == '>')
                        rem = false;
                }

                if (word.Length > 1 && word.ToUpperInvariant() == word
                    && _artistReferencesRegex.IsMatch(word) && _HasLetterRegex.IsMatch(word))
                {
                    if (ponctuations.Contains(word[0]))
                    {
                        if (string.IsNullOrEmpty(currentSimilarArtist))
                        {
                            currentSimilarArtist = word;
                            newSentence = false;
                        }
                        else
                        {
                            SafeAddSimilarArtist(similarArtists, currentSimilarArtist);
                            currentSimilarArtist = word;
                            newSentence = false;

                            if (ponctuations.Contains(word[word.Length - 1]))
                            {
                                SafeAddSimilarArtist(similarArtists, currentSimilarArtist);
                                currentSimilarArtist = string.Empty;
                                newSentence = true;
                            }
                        }
                    }
                    else if (ponctuations.Contains(word[word.Length - 1]))
                    {
                        SafeAddSimilarArtist(similarArtists, (string.IsNullOrEmpty(currentSimilarArtist)) ? word : currentSimilarArtist + " " + word);

                        currentSimilarArtist = string.Empty;
                        newSentence = true;
                    }
                    else if (newSentence)
                    {
                        currentSimilarArtist = word;
                        newSentence = false;
                    }
                    else
                    {
                        currentSimilarArtist += " " + word;
                    }
                }
                else
                {
                    newSentence = true;
                    if (!string.IsNullOrEmpty(currentSimilarArtist))
                    {
                        SafeAddSimilarArtist(similarArtists, currentSimilarArtist);
                        currentSimilarArtist = string.Empty;
                    }
                }
            }

            return similarArtists;
        }

        private void SafeAddSimilarArtist(ICollection<string> similarArtists, string artistNameInput)
        {
            var artistNames = artistNameInput.Split(ponctuations.ToArray());

            foreach (var artName in artistNames)
            {
                var artistName = artName.Trim(ponctuations.ToArray());
                artistName = artistName.Trim(' ');

                if (artistName.Length > 3 && !similarArtists.Contains(artistName))
                {
                    similarArtists.Add(artistName);
                }
            }
        }

        public IList<string> ExtractAlbumsReferences(string text)
        {
            var similarAlbums = new List<string>();
            var matches = _albumReferencesRegex.Matches(text);

            foreach (Match match in matches)
            {
                similarAlbums.Add(match.Groups[0].Value);
            }

            return similarAlbums;
        }

        public IList<string> ExtractTitle(string title, int id)
        {
            var match = _titleRegex.Match(title);
            if (match.Success)
            {
                return GroupsToStringList(match.Groups);
            }

            match = _titleRegex2.Match(title);
            if (match.Success)
            {
                return GroupsToStringList(match.Groups);
            }

            Logging.Logging.Instance.LogError(string.Format("Title '{0}' of review {1} could not be parsed correctly", title, id), ErrorLevel.Error);
            throw new FormatException("ExtractTitle");
        }

        private static IList<string> GroupsToStringList(GroupCollection groups)
        {
            return GroupsToStringList(groups, string.Empty);
        }

        public IList<string> ExtractLabelAndDistributor(string text, int id)
        {
            var label = ExtractInfo(_labelRegex, text, id, "Label");

            var match = _labelInfosRegex.Match(label);
            if (match.Success)
            {
                return GroupsToStringList(match.Groups, string.Empty);
            }

            return new List<string> { label };
        }

        public IList<string> ExtractPlayTime(string text, int id)
        {
            var playTime = ExtractInfo(_playTimeRegex, text, id, "Playtime");

            var match = _playTimeInfosRegex.Match(playTime);
            if (match.Success)
            {
                return GroupsToStringList(match.Groups, "0");
            }

            match = _playTimeInfosRegex2.Match(playTime);
            if (match.Success)
            {
                return GroupsToStringList(match.Groups, "0");
            }

            match = _playTimeInfosRegex3.Match(playTime);
            if (match.Success)
            {
                return GroupsToStringList(match.Groups, "0");
            }
            match = _playTimeInfosRegex4.Match(playTime);
            if (match.Success)
            {
                return GroupsToStringList(match.Groups, "0");
            }
            match = _playTimeInfosRegex5.Match(playTime);
            if (match.Success)
            {
                return GroupsToStringList(match.Groups, "0");
            }

            Logging.Logging.Instance.LogError(string.Format("PlayTime '{0}' of review {1} could not be parsed correctly", playTime, id), ErrorLevel.Warning);
            return null;
        }

        private static string ExtractInfo(Regex regex, string text, int id, string infoName)
        {
            return ExtractInfo(regex, text, id, infoName, 1);
        }
        private static string ExtractInfo(Regex regex, string text, int id, string infoName, int positionToExtract)
        {
            var match = regex.Match(text);
            if (match.Success)
            {
                return (match.Groups[positionToExtract].ToString()).Replace("\\r\\n", "").Replace((char)8211, '-');
            }

            Logging.Logging.Instance.LogError(string.Format("{0} of review {1} could not be parsed correctly", infoName, id), ErrorLevel.Warning);
            return string.Empty;
        }

        private static IList<string> GroupsToStringList(GroupCollection collection, string replacementString)
        {
            var res = new List<string>();
            for (var i = 1; i < collection.Count; i++)
            {
                res.Add(string.IsNullOrEmpty(collection[i].ToString()) ? replacementString : collection[i].ToString());
            }
            return res;
        }

        #endregion

        public string ExtractCountry(string text, int id)
        {
            return ExtractInfo(_countryRegex, text, id, "Country");
        }

        public string ExtractReviewBody(string text, int id)
        {
            return ExtractInfo(_textRegex, text, id, "Review body");
        }

        public string ExtractReleaseDate(string text, int id)
        {
            return ExtractInfo(_releaseDateRegex, text, id, "Release date", 2);
        }

        public string ExtractMusicType(string text, int id)
        {
            return ExtractInfo(_musicTypeRegex, text, id, "Music type");
        }

        public string ExtractAlbumType(string text, int id)
        {
            return ExtractInfo(_recordTypeRegex, text, id, "Record type");
        }
    }
}