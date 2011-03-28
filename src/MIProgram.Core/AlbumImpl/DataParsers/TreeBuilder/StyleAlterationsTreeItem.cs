using MIProgram.Core.AlbumImpl.LocalRepositories;

namespace MIProgram.Core.AlbumImpl.DataParsers.TreeBuilder
{
    public class StyleAlterationsTreeItem : StylesTreeItem
    {
        public StyleAlterationsTreeItem(int styleAlterationIdx)
        {
            _styleAlterationsIdxs.Add(styleAlterationIdx);
            _styleRebuildedString = StylesRepository.StyleAlterationsRepository.Values[styleAlterationIdx];
        }
    }
}