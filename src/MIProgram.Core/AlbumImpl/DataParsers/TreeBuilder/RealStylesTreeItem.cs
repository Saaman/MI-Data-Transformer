using System.Collections.Generic;

namespace MIProgram.Core.AlbumImpl.DataParsers.TreeBuilder
{
    public class RealStylesTreeItem : StylesTreeItem
    {
        public RealStylesTreeItem(StyleDefinition definition, string styleOriginalString) : base(definition)
        {
            _styleOriginalString = styleOriginalString;
        }

        internal RealStylesTreeItem(int musicGenresIdx) : base(new StyleDefinition(new List<int>{musicGenresIdx}))
        {}

        internal RealStylesTreeItem(int musicGenresIdx, int subStyleIdx)
            : this(musicGenresIdx, new List<int> { subStyleIdx })
        {}

        internal RealStylesTreeItem(int musicGenresIdx, IList<int> subStylesIdxs) : base(new StyleDefinition(new List<int>{musicGenresIdx}, subStylesIdxs))
        {}

        internal RealStylesTreeItem(RealStylesTreeItem parent, int styleAlterationIdx) : base((parent.GetDefinition()).AddStyleAlterationIdx(styleAlterationIdx))
        {}

        private StyleDefinition GetDefinition()
        {
            var def =  new StyleDefinition(_musicGenresIdxs, _subStylesIdxs);
            foreach(var styleAlterationIdx in _styleAlterationsIdxs)
            {
                def.AddStyleAlterationIdx(styleAlterationIdx);
            }

            return def;
        }
    }
}