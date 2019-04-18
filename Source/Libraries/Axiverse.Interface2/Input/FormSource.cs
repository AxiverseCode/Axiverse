using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Axiverse.Interface2.Input
{
    public class FormSource : IInputSource
    {
        public Form Form { get; }

        public FormSource(Form form)
        {
            Form = form;
            form.MouseDown += (s, e) =>
            {

            };
            form.MouseMove += (s, e) =>
            {

            };
            form.MouseUp += (s, e) =>
            {

            };
            form.MouseWheel += (s, e) =>
            {

            };
            form.KeyPress += (s, e) =>
            {

            };
            form.KeyUp += (s, e) =>
            {

            };
        }

        public void Update()
        {

        }
    }
}
