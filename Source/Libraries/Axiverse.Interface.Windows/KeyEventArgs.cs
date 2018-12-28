using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public class KeyEventArgs : EventArgs
    {
        public Keys Key => ModifiedKey & Keys.KeyCode;
        public Keys ModifiedKey { get; }
        public Keys Modifiers => ModifiedKey & Keys.Modifiers;
        public char? KeyValue { get; }

        public KeyEventArgs(Keys modifiedKey)
        {
            ModifiedKey = modifiedKey;

            if (Key >= Keys.A && Key <= Keys.Z)
            {
                KeyValue = (char)('a' + ((int)Key - (int)Keys.A));
            }
            else if (Key >= Keys.D0 && Key <= Keys.D9)
            {
                KeyValue = (char)('0' + ((int)Key - (int)Keys.D0));

            }
            else if (Key >= Keys.NumPad0 && Key <= Keys.NumPad9)
            {
                KeyValue = (char)('0' + ((int)Key - (int)Keys.NumPad0));
            }
            else
            {
                KeyValue = null;
            }
        }

        public KeyEventArgs(char keyValue)
        {
            ModifiedKey = Keys.None;
            KeyValue = keyValue;
        }
    }
}
