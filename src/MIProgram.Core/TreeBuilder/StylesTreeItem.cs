using System.Collections.Generic;
using MIProgram.Core.DataParsers;
using System.Linq;

namespace MIProgram.Core.TreeBuilder
{
    public class StylesTreeItem
    {
        protected string _styleOriginalString;
        protected string _styleRebuildedString;
        protected IList<int> _musicGenresIdxs = new List<int>();
        protected IList<int> _subStylesIdxs = new List<int>();
        protected IList<int> _styleAlterationsIdxs = new List<int>();

        public int Complexity
        {
            get { return _musicGenresIdxs.Count + _subStylesIdxs.Count * 2 + _styleAlterationsIdxs.Count * 3; }
        }

        public IList<StylesTreeItem> Parents { get; private set; }

        public int? ID { get; set; }

        public void SetParents(IList<StylesTreeItem> parents)
        {
            Parents = parents;
        }

        protected StylesTreeItem()
        {}

        public StylesTreeItem(StyleDefinition definition)
        {
            _styleRebuildedString = definition.RebuildFromParsedValuesRepository();
            _musicGenresIdxs = definition.MusicTypesIdxs;
            _subStylesIdxs = definition.MainStylesIdxs;
            _styleAlterationsIdxs = definition.StyleAlterationsIdxs;
        }

        public void UpdateFrom(StylesTreeItem treeItem)
        {
            _styleRebuildedString = treeItem._styleRebuildedString;
            if (string.IsNullOrEmpty(_styleOriginalString) || _styleOriginalString.Length < treeItem._styleOriginalString.Length)
            {
                _styleOriginalString = treeItem._styleOriginalString;
            }
        }

        public IList<StylesTreeItem> ExtractParentStyles()
        {
            var parentsToReturn = new List<StylesTreeItem>();

            foreach (var musicGenresIdx in _musicGenresIdxs)
            {
                var subStylePlusMusicGenreItems = new Dictionary<int, int>();
                //create parent level 1
                parentsToReturn.Add(new RealStylesTreeItem(musicGenresIdx));

                foreach (var subStylesIdx in _subStylesIdxs)
                {
                    if(StylesRepository.StylesAssociations.Contains(new KeyValuePair<int, int>(subStylesIdx, musicGenresIdx)))
                    {                        
                        subStylePlusMusicGenreItems.Add(subStylesIdx, musicGenresIdx);
                        //create parent level 2
                        parentsToReturn.Add(new RealStylesTreeItem(musicGenresIdx, subStylesIdx));
                    }
                }

                //If all subStyles are part of the same style => create parent level 3 
                if (_subStylesIdxs.Count > 1 && subStylePlusMusicGenreItems.Count == _subStylesIdxs.Count)
                {
                    parentsToReturn.Add(new RealStylesTreeItem(musicGenresIdx, _subStylesIdxs));
                }
            }

            foreach (var styleAlterationsIdx in _styleAlterationsIdxs)
            {
                parentsToReturn.Add(new StyleAlterationsTreeItem(styleAlterationsIdx));

                foreach (var parent in parentsToReturn)
                {
                    if (parent is RealStylesTreeItem)
                    {
                        parentsToReturn.Add(new RealStylesTreeItem(parent as RealStylesTreeItem, styleAlterationsIdx));
                    }
                }
            }

            return parentsToReturn.Where(x => !ContentEqualsTo(x)).ToList();
        }

        public bool ContentEqualsTo(StylesTreeItem item)
        {
            #region test lists count

            if(item._musicGenresIdxs.Count != _musicGenresIdxs.Count)
                return false;
            if(item._subStylesIdxs.Count != _subStylesIdxs.Count)
                return false;
            if (item._styleAlterationsIdxs.Count != _styleAlterationsIdxs.Count)
                return false;

            #endregion

            #region test musicGenres

            foreach (var musicGenresIdx in item._musicGenresIdxs)
            {
                if(!_musicGenresIdxs.Contains(musicGenresIdx))
                    return false;
            }

            #endregion

            #region test subStyles

            foreach (var subStylesIdx in item._subStylesIdxs)
            {
                if (!_subStylesIdxs.Contains(subStylesIdx))
                    return false;
            }

            #endregion

            #region test styleAlterations

            foreach (var styleAlterationsIdx in item._styleAlterationsIdxs)
            {
                if (!_styleAlterationsIdxs.Contains(styleAlterationsIdx))
                    return false;
            }

            #endregion

            return true;

        }

        public string ConvertToPrintableString()
        {
            string parentsString = string.Empty;
            parentsString = Parents.Aggregate(parentsString, (current, parent) => current + parent.ID + ",");
            parentsString.TrimEnd(',');

            if(!string.IsNullOrEmpty(_styleOriginalString))
            {
                return string.Format("style {0} : '{1}' est parsé en '{2}' et ses parents sont : '{3}' ", ID,
                                _styleOriginalString, _styleRebuildedString, parentsString);
            }

            if (this is RealStylesTreeItem)
            {
                return string.Format("style {0} : '{1}' crée de toutes pièces et ses parents sont : '{2}' ", ID, _styleRebuildedString, parentsString);
            }

            return string.Format("style {0} : altération '{1}' crée de toutes pièces et ses parents sont : '{2}' ", ID, _styleRebuildedString, parentsString);
        }
    }
}