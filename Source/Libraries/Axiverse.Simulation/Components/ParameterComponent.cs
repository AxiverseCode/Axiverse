using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class ParameterComponent : Component
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get => parameters[key].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class => parameters[key] as T;

        public override Component Clone()
        {
            throw new NotImplementedException();
        }

        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();
    }
}
