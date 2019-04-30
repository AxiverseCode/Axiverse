using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Engine
{
    public class PhysicalMaterial
    {
        public PhysicalMaterial(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public Vector4 Ambient { get; set; }
        public Vector4 Diffuse { get; set; }
        public Vector4 Specular { get; set; }
        public float Shininess { get; set; }

        public static readonly PhysicalMaterial Brass =
            new PhysicalMaterial("Brass")
            {
                Ambient = new Vector4(0.329412f, 0.223529f, 0.027451f, 1f),
                Diffuse = new Vector4(0.780392f, 0.568627f, 0.113725f, 1f),
                Specular = new Vector4(0.992157f, 0.941176f, 0.807843f, 1f),
                Shininess = 27.8974f,
            };

        public static readonly PhysicalMaterial Bronze =
            new PhysicalMaterial("Bronze")
            {
                Ambient = new Vector4(0.2125f, 0.1275f, 0.054f, 1f),
                Diffuse = new Vector4(0.714f, 0.4284f, 0.18144f, 1f),
                Specular = new Vector4(0.393548f, 0.271906f, 0.166721f, 1f),
                Shininess = 25.6f,
            };

        public static readonly PhysicalMaterial PolishedBronze =
            new PhysicalMaterial("Polished Bronze")
            {
                Ambient = new Vector4(0.25f, 0.148f, 0.06475f, 1f),
                Diffuse = new Vector4(0.4f, 0.2368f, 0.1036f, 1f),
                Specular = new Vector4(0.774597f, 0.458561f, 0.200621f, 1f),
                Shininess = 76.8f,
            };

        public static readonly PhysicalMaterial Chrome =
            new PhysicalMaterial("Chrome")
            {
                Ambient = new Vector4(0.25f, 0.25f, 0.25f, 1f),
                Diffuse = new Vector4(0.4f, 0.4f, 0.4f, 1f),
                Specular = new Vector4(0.774597f, 0.774597f, 0.774597f, 1f),
                Shininess = 76.8f,
            };

        public static readonly PhysicalMaterial Copper =
            new PhysicalMaterial("Copper")
            {
                Ambient = new Vector4(0.19125f, 0.0735f, 0.0225f, 1f),
                Diffuse = new Vector4(0.7038f, 0.27048f, 0.0828f, 1f),
                Specular = new Vector4(0.256777f, 0.137622f, 0.086014f, 1f),
                Shininess = 12.8f,
            };

        public static readonly PhysicalMaterial PolishedCopper =
            new PhysicalMaterial("Polished Copper")
            {
                Ambient = new Vector4(0.2295f, 0.08825f, 0.0275f, 1f),
                Diffuse = new Vector4(0.5508f, 0.2118f, 0.066f, 1f),
                Specular = new Vector4(0.580594f, 0.223257f, 0.0695701f, 1f),
                Shininess = 51.2f,
            };

        public static readonly PhysicalMaterial Gold =
            new PhysicalMaterial("Gold")
            {
                Ambient = new Vector4(0.24725f, 0.1995f, 0.0745f, 1f),
                Diffuse = new Vector4(0.75164f, 0.60648f, 0.22648f, 1f),
                Specular = new Vector4(0.628281f, 0.555802f, 0.366065f, 1f),
                Shininess = 51.2f,
            };

        public static readonly PhysicalMaterial PolishedGold =
            new PhysicalMaterial("Polished Gold")
            {
                Ambient = new Vector4(0.24725f, 0.2245f, 0.0645f, 1f),
                Diffuse = new Vector4(0.34615f, 0.3143f, 0.0903f, 1f),
                Specular = new Vector4(0.797357f, 0.723991f, 0.208006f, 1f),
                Shininess = 83.2f,
            };

        public static readonly PhysicalMaterial Pewter =
            new PhysicalMaterial("Pewter")
            {
                Ambient = new Vector4(0.105882f, 0.058824f, 0.113725f, 1f),
                Diffuse = new Vector4(0.427451f, 0.470588f, 0.541176f, 1f),
                Specular = new Vector4(0.333333f, 0.333333f, 0.521569f, 1f),
                Shininess = 9.84615f,
            };

        public static readonly PhysicalMaterial Silver =
            new PhysicalMaterial("Silver")
            {
                Ambient = new Vector4(0.19225f, 0.19225f, 0.19225f, 1f),
                Diffuse = new Vector4(0.50754f, 0.50754f, 0.50754f, 1f),
                Specular = new Vector4(0.508273f, 0.508273f, 0.508273f, 1f),
                Shininess = 51.2f,
            };

        public static readonly PhysicalMaterial PolishedSilver =
            new PhysicalMaterial("Polished Silver")
            {
                Ambient = new Vector4(0.23125f, 0.23125f, 0.23125f, 1f),
                Diffuse = new Vector4(0.2775f, 0.2775f, 0.2775f, 1f),
                Specular = new Vector4(0.773911f, 0.773911f, 0.773911f, 1f),
                Shininess = 89.6f,
            };

        public static readonly PhysicalMaterial Emerald =
            new PhysicalMaterial("Emerald")
            {
                Ambient = new Vector4(0.0215f, 0.1745f, 0.0215f, 0.55f),
                Diffuse = new Vector4(0.07568f, 0.61424f, 0.07568f, 0.55f),
                Specular = new Vector4(0.633f, 0.727811f, 0.633f, 0.55f),
                Shininess = 76.8f,
            };

        public static readonly PhysicalMaterial Jade =
            new PhysicalMaterial("Jade")
            {
                Ambient = new Vector4(0.135f, 0.2225f, 0.1575f, 0.95f),
                Diffuse = new Vector4(0.54f, 0.89f, 0.63f, 0.95f),
                Specular = new Vector4(0.316228f, 0.316228f, 0.316228f, 0.95f),
                Shininess = 12.8f,
            };

        public static readonly PhysicalMaterial Obsidian =
            new PhysicalMaterial("Obsidian")
            {
                Ambient = new Vector4(0.05375f, 0.05f, 0.06625f, 0.82f),
                Diffuse = new Vector4(0.18275f, 0.17f, 0.22525f, 0.82f),
                Specular = new Vector4(0.332741f, 0.328634f, 0.346435f, 0.82f),
                Shininess = 38.4f,
            };

        public static readonly PhysicalMaterial Pearl =
            new PhysicalMaterial("Pearl")
            {
                Ambient = new Vector4(0.25f, 0.20725f, 0.20725f, 0.922f),
                Diffuse = new Vector4(1f, 0.829f, 0.829f, 0.922f),
                Specular = new Vector4(0.296648f, 0.296648f, 0.296648f, 0.922f),
                Shininess = 11.264f,
            };

        public static readonly PhysicalMaterial Ruby =
            new PhysicalMaterial("Ruby")
            {
                Ambient = new Vector4(0.1745f, 0.01175f, 0.01175f, 0.55f),
                Diffuse = new Vector4(0.61424f, 0.04136f, 0.04136f, 0.55f),
                Specular = new Vector4(0.727811f, 0.626959f, 0.626959f, 0.55f),
                Shininess = 76.8f,
            };

        public static readonly PhysicalMaterial Turquoise =
            new PhysicalMaterial("Turquoise")
            {
                Ambient = new Vector4(0.1f, 0.18725f, 0.1745f, 0.8f),
                Diffuse = new Vector4(0.396f, 0.74151f, 0.69102f, 0.8f),
                Specular = new Vector4(0.297254f, 0.30829f, 0.306678f, 0.8f),
                Shininess = 12.8f,
        };

        public static readonly PhysicalMaterial BlackPlastic =
            new PhysicalMaterial("Black Plastic")
            {
                Ambient = new Vector4(0f, 0f, 0f, 1f),
                Diffuse = new Vector4(0.01f, 0.01f, 0.01f, 1f),
                Specular = new Vector4(0.5f, 0.5f, 0.5f, 1f),
                Shininess = 32f,
            };

        public static readonly PhysicalMaterial BlackRubber =
            new PhysicalMaterial("Black Rubber")
            {
                Ambient = new Vector4(0.02f, 0.02f, 0.02f, 1f),
                Diffuse = new Vector4(0.01f, 0.01f, 0.01f, 1f),
                Specular = new Vector4(0.4f, 0.4f, 0.4f, 1f),
                Shininess = 10f,
            };
    }
}
