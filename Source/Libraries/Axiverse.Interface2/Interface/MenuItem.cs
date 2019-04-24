using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Interface
{
    public class MenuItem
    {
        public MenuItemCollection Children { get; }

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
            Children = new MenuItemCollection();
            Text = text;
        }

        protected internal void OnClick(EventArgs args)
        {
            Clicked?.Invoke(this, args);
        }

        public event EventHandler Clicked;
        public event EventHandler TextChanged;
    }
}
