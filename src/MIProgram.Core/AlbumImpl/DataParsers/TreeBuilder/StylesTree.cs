using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MIProgram.Core.Helpers;

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
            
            ComputeParents(newStyle);

            CreateOrUpdateStylesTreeItem(newStyle);
        }

        protected StylesTreeItem ComputeParents(StylesTreeItem newStyle)
        {
            var computedParents = newStyle.ExtractParentStyles();
            computedParents = computedParents.OrderBy(x => x.Complexity).ToList();
            var integratedParents = computedParents.Select(GetOrCreateParent).ToList();

            newStyle.SetParents(integratedParents);
            return newStyle;
        }

        private StylesTreeItem CreateOrUpdateStylesTreeItem(StylesTreeItem stylesTreeItem)
        {
            StylesTreeItem result = stylesTreeItem;
            if (TryGetStylesTreeItem(ref result))
            {
                result.UpdateFrom(stylesTreeItem);
                return result;
            }

            CreateStylesTreeItem(stylesTreeItem);
            return stylesTreeItem;
        }

        private StylesTreeItem GetOrCreateParent(StylesTreeItem stylesTreeItem)
        {
            StylesTreeItem result = stylesTreeItem;
            if (!TryGetStylesTreeItem(ref result))
            {
                ComputeParents(stylesTreeItem);
                return CreateOrUpdateStylesTreeItem(stylesTreeItem);
            }

            return result;
        }

        private void CreateStylesTreeItem(StylesTreeItem stylesTreeItem)
        {
            stylesTreeItem.ID = _idGenerator.NewID();

            Debug.Assert(!(_stylesTreeItems.Select(x => x.ID).Contains(stylesTreeItem.ID)));
            Debug.Assert(GetStylesTreeItem(stylesTreeItem) == null);

            _stylesTreeItems.Add(stylesTreeItem);

            return;
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
            if (result == null)
            {
                return false;
            }

            stylesTreeItem = result;
            return true;
        }
    }
}