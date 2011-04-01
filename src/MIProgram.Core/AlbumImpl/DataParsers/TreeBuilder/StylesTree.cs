using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MIProgram.Core.AlbumImpl.DataParsers.TreeBuilder
{
    public class StylesTree
    {
        private readonly IDGenerator _idGenerator = new IDGenerator();
        private readonly IList<StylesTreeItem> _stylesTreeItems = new List<StylesTreeItem>();

        public const string SQLTableName = "mi_album_style";

        public IList<StylesTreeItem> OrderStylesItems
        {
            get { return _stylesTreeItems.OrderBy(x => x.Complexity).ToList(); }
        }

        public static StylesTree BuildFrom(IDictionary<string, StyleDefinition> styleDefinitions)
        {
            var stylesTree = new StylesTree();

            foreach (var definition in styleDefinitions)
            {
                stylesTree.AddNewStyle(definition.Value, definition.Key);
            }

            return stylesTree;
        }

        public void AddNewStyle(StyleDefinition definition, string styleOriginalString)
        {
            var newStyle = new RealStylesTreeItem(definition, styleOriginalString);

            var computedParents = newStyle.ExtractParentStyles();
            computedParents = computedParents.OrderBy(x => x.Complexity).ToList();
            var integratedParents = new List<StylesTreeItem>();

            foreach (var parent in computedParents)
            {
                integratedParents.Add(GetOrCreateStyle(parent));
            }

            newStyle.SetParents(integratedParents);
            CreateOrUpdateStylesTreeItem(newStyle);
        }

        private void CreateOrUpdateStylesTreeItem(StylesTreeItem stylesTreeItem)
        {
            StylesTreeItem result = stylesTreeItem;
            if (TryGetStylesTreeItem(ref result))
            {
                result.UpdateFrom(stylesTreeItem);
                return;
            }

            CreateStylesTreeItem(stylesTreeItem);
        }

        private StylesTreeItem GetOrCreateStyle(StylesTreeItem stylesTreeItem)
        {
            StylesTreeItem result = stylesTreeItem;
            if (!TryGetStylesTreeItem(ref result))
            {
                result = CreateStylesTreeItem(stylesTreeItem);
            }

            return result;
        }

        private StylesTreeItem CreateStylesTreeItem(StylesTreeItem stylesTreeItem)
        {
            stylesTreeItem.ID = _idGenerator.NewID();

            Debug.Assert(!(_stylesTreeItems.Select(x => x.ID).Contains(stylesTreeItem.ID)));
            Debug.Assert(GetStylesTreeItem(stylesTreeItem) == null);

            _stylesTreeItems.Add(stylesTreeItem);

            return stylesTreeItem;
        }

        private StylesTreeItem GetStylesTreeItem(StylesTreeItem treeItem)
        {
            return _stylesTreeItems.Where(x => x.ContentEqualsTo(treeItem)).SingleOrDefault();
        }

        public StylesTreeItem GetStylesTreeItem(int id)
        {
            return _stylesTreeItems.Where(x => x.ID == id).FirstOrDefault();
        }

        private bool TryGetStylesTreeItem(ref StylesTreeItem stylesTreeItem)
        {
            var result = GetStylesTreeItem(stylesTreeItem);
            if(GetStylesTreeItem(stylesTreeItem) == null)
            {
                return false;
            }

            stylesTreeItem = result;
            return true;
        }
    }
}