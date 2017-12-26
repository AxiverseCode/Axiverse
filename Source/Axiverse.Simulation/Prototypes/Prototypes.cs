using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Axiverse.Simulation.Prototypes
{
    public class Prototypes
    {
        public string BasePath { get; private set; }

        public Dictionary<string, EntityPrototype> Entities { get; } = new Dictionary<string, EntityPrototype>();

        public Dictionary<string, EquiptmentPrototype> Equiptment { get; } = new Dictionary<string, EquiptmentPrototype>();

        public Dictionary<string, PresetPrototype> Presets { get; } = new Dictionary<string, PresetPrototype>();

        public Prototypes(string basePath)
        {
            BasePath = basePath;

            LoadEntities();
            LoadEquiptment();
            LoadPresets();
        }

        private void LoadEntities()
        {
            string path = Path.Combine(BasePath, "Entities");

            foreach (var protoPath in Directory.GetFiles(path))
            {
                var text = File.ReadAllText(protoPath);
                var proto = JsonConvert.DeserializeObject<EntityPrototype>(text);
                Entities.Add(proto.Name, proto);
                Console.WriteLine($"EntityPrototype: {proto.Name}");
            }
        }

        private void LoadPresets()
        {
            string path = Path.Combine(BasePath, "Presets");

            foreach (var protoPath in Directory.GetFiles(path))
            {
                var text = File.ReadAllText(protoPath);
                var proto = JsonConvert.DeserializeObject<PresetPrototype>(text);
                proto.Prototypes = this;
                Presets.Add(proto.Name, proto);
                Console.WriteLine($"PresetPrototype: {proto.Name}");
            }
        }

        private void LoadEquiptment()
        {
            string path = Path.Combine(BasePath, "Equiptment");
            Dictionary<string, Type> prototypes = GetPrototypes(typeof(EquiptmentPrototype));

            foreach (var filePath in Directory.GetFiles(path))
            {
                var segments = Path.GetFileNameWithoutExtension(filePath).Split('-');
                var prototypeName = $"{segments[0]}Prototype";
                if (!prototypes.TryGetValue(prototypeName, out var type))
                {
                    Console.WriteLine($"Cannot load EquiptmentPrototype: {prototypeName}");
                    continue;
                }

                var text = File.ReadAllText(filePath);
                var proto = JsonConvert.DeserializeObject(text, type) as EquiptmentPrototype;
                Equiptment.Add(proto.Name, proto);
                Console.WriteLine($"EquiptmentPrototype: {proto.Name} - {type.Name}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private Dictionary<string, Type> GetPrototypes(Type baseType)
        {
            Dictionary<string, Type> prototypes = new Dictionary<string, Type>();

            var subtypes =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsSubclassOf(baseType)
                select type;

            foreach (var type in subtypes)
            {
                prototypes.Add(type.Name, type);
            }

            return prototypes;
        }
    }
}
