using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Entites;
using Axiverse.Interface2.Interface;

namespace Axiverse.Interface2.Engine
{
    public abstract class EngineProcess
    {
        public Scene Scene { get; set; }
        public Chrome Chrome { get; set; }

        public EngineProcess()
        {
            Scene = new Scene();
            Chrome = new Chrome();
        }

        public void Update(Clock clock)
        {
            OnUpdate(clock);
        }

        public void Render()
        {

        }

        protected internal virtual void OnInitialize()
        {

        }

        protected virtual void OnUpdate(Clock clock)
        {

        }

        protected virtual void OnRender()
        {

        }

        protected virtual void OnDispose()
        {

        }

        protected internal virtual void OnEnter(Engine engine)
        {

        }

        protected internal virtual void OnLeave(Engine engine)
        {

        }
    }
}
