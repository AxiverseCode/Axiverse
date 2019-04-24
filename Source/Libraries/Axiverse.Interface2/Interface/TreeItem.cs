using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Interface
{
    public class TreeItem
    {
        public TreeItemCollection Children { get; } = new TreeItemCollection();

        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
            }
        }

        public bool Expanded { get; set; }

        public TreeItem(string text)
        {
            Text = text;
        }
    }
}
