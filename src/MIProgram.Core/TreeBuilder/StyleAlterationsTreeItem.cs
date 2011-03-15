using MIProgram.Core.DataParsers;

namespace MIProgram.Core.TreeBuilder
{
    public class StyleAlterationsTreeItem : StylesTreeItem
    {
        public StyleAlterationsTreeItem(int styleAlterationIdx)
        {
            _styleAlterationsIdxs.Add(styleAlterationIdx);
            _styleRebuildedString = StyleDefinition.StyleAlterationsValues[styleAlterationIdx];
        }
    }
}