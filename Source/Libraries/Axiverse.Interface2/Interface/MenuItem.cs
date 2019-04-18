using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Interface
{
    public class MenuItem
    {
        public MenuItemCollection Children { get; } = new MenuItemCollection();

        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                TextChanged?.Invoke(this, null);
            }
        }

        public MenuItem(string text)
        {
            Text = text;
        }

        public event EventHandler Clicked;
        public event EventHandler TextChanged;
    }
}
