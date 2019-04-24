using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Collections;

namespace Axiverse.Interface2.Interface
{
    public class TreeItemCollection : TrackedList<TreeItem>
    {
        private TreeItem item;
        private Tree tree;

        public TreeItemCollection(TreeItem item)
        {
            this.item = item;
        }

        public TreeItemCollection(Tree tree)
        {
            this.tree = tree;
        }

        protected override void OnItemAdded(TreeItem item)
        {
            item.Tree = tree ?? item.Tree;
            item.Tree?.HandleItemAdded(item, null);
        }

        protected override void OnItemRemoved(TreeItem item)
        {
            item.Tree = tree ?? item.Tree;
            item.Tree?.HandleItemAdded(item, null);
        }
    }
}
