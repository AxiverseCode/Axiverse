using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Windows;

namespace Axiverse.Interface2.Engine
{
    /// <summary>
    /// Engine which holds references.
    /// </summary>
    public class Engine : IDisposable
    {
        public Device Device { get; }
        public Compositor Compositor { get; }
        public RenderForm Form { get; }

        private EngineProcess process;
        public EngineProcess Process
        {
            get => process;
            set
            {
                process?.OnLeave(this);
                process = value;
                value?.OnEnter(this);
            }
        }

        public Clock Clock { get; } = new Clock();

        public Engine()
        {
            Form = new RenderForm() { ClientSize = new System.Drawing.Size(800, 800) };
            Device = new Device(Form);
            Compositor = new Compositor();
        }

        public void Run()
        {
            Clock.Start();
            RenderLoop.Run(Form, () =>
            {
                Clock.Mark();
                Device.Start();
                Device.Clear(SharpDX.Color.Black);

                if (Process != null)
                {
                    Process.Update(Clock);
                    //Compositor.Draw(Process.Scene, Clock.FrameTime);

                    if (Process.Chrome != null)
                    {
                        Device.Canvas.Begin();
                        Process.Chrome.Update(Clock.FrameTime);
                        Process.Chrome.Draw(Device.Canvas);
                        Device.Canvas.End();
                    }
                }
                
                Device.Present();
            });
        }

        protected void OnProcessChanging()
        {

        }

        protected void OnProcessChanged()
        {

        }

        public void Dispose()
        {
            Device.Dispose();
            Form.Dispose();
        }
    }
}
