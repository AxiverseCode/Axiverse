using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Interface
{
    public class TreeItem
    {
        private Tree tree;
        public Tree Tree
        {
            get => tree;
            set
            {
                tree = value;
                foreach (var item in Children)
                {
                    item.tree = tree;
                }
            }
        }

        public TreeItemCollection Children { get; } = new TreeItemCollection();

        public bool Expanded { get; set; }

        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
            }
        }


        public TreeItem(string text)
        {
            Text = text;
        }
    }
}
