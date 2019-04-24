using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Drawing
{
    public partial struct Color
    {        /// <summary>
             /// Zero color.
             /// </summary>
        public static readonly Color Zero = Color.NameColor("Zero", 0x00000000);

        /// <summary>
        /// Transparent color.
        /// </summary>
        public static readonly Color Transparent = Color.NameColor("Transparent", 0x00000000);

        /// <summary>
        /// AliceBlue color.
        /// </summary>
        public static readonly Color AliceBlue = Color.NameColor("AliceBlue", 0xFFF0F8FF);

        /// <summary>
        /// AntiqueWhite color.
        /// </summary>
        public static readonly Color AntiqueWhite = Color.NameColor("AntiqueWhite", 0xFFFAEBD7);

        /// <summary>
        /// Aqua color.
        /// </summary>
        public static readonly Color Aqua = Color.NameColor("Aqua", 0xFF00FFFF);

        /// <summary>
        /// Aquamarine color.
        /// </summary>
        public static readonly Color Aquamarine = Color.NameColor("Aquamarine", 0xFF7FFFD4);

        /// <summary>
        /// Azure color.
        /// </summary>
        public static readonly Color Azure = Color.NameColor("Azure", 0xFFF0FFFF);

        /// <summary>
        /// Beige color.
        /// </summary>
        public static readonly Color Beige = Color.NameColor("Beige", 0xFFF5F5DC);

        /// <summary>
        /// Bisque color.
        /// </summary>
        public static readonly Color Bisque = Color.NameColor("Bisque", 0xFFFFE4C4);

        /// <summary>
        /// Black color.
        /// </summary>
        public static readonly Color Black = Color.NameColor("Black", 0xFF000000);

        /// <summary>
        /// BlanchedAlmond color.
        /// </summary>
        public static readonly Color BlanchedAlmond = Color.NameColor("BlanchedAlmond", 0xFFFFEBCD);

        /// <summary>
        /// Blue color.
        /// </summary>
        public static readonly Color Blue = Color.NameColor("Blue", 0xFF0000FF);

        /// <summary>
        /// BlueViolet color.
        /// </summary>
        public static readonly Color BlueViolet = Color.NameColor("BlueViolet", 0xFF8A2BE2);

        /// <summary>
        /// Brown color.
        /// </summary>
        public static readonly Color Brown = Color.NameColor("Brown", 0xFFA52A2A);

        /// <summary>
        /// BurlyWood color.
        /// </summary>
        public static readonly Color BurlyWood = Color.NameColor("BurlyWood", 0xFFDEB887);

        /// <summary>
        /// CadetBlue color.
        /// </summary>
        public static readonly Color CadetBlue = Color.NameColor("CadetBlue", 0xFF5F9EA0);

        /// <summary>
        /// Chartreuse color.
        /// </summary>
        public static readonly Color Chartreuse = Color.NameColor("Chartreuse", 0xFF7FFF00);

        /// <summary>
        /// Chocolate color.
        /// </summary>
        public static readonly Color Chocolate = Color.NameColor("Chocolate", 0xFFD2691E);

        /// <summary>
        /// Coral color.
        /// </summary>
        public static readonly Color Coral = Color.NameColor("Coral", 0xFFFF7F50);

        /// <summary>
        /// CornflowerBlue color.
        /// </summary>
        public static readonly Color CornflowerBlue = Color.NameColor("CornflowerBlue", 0xFF6495ED);

        /// <summary>
        /// Cornsilk color.
        /// </summary>
        public static readonly Color Cornsilk = Color.NameColor("Cornsilk", 0xFFFFF8DC);

        /// <summary>
        /// Crimson color.
        /// </summary>
        public static readonly Color Crimson = Color.NameColor("Crimson", 0xFFDC143C);

        /// <summary>
        /// Cyan color.
        /// </summary>
        public static readonly Color Cyan = Color.NameColor("Cyan", 0xFF00FFFF);

        /// <summary>
        /// DarkBlue color.
        /// </summary>
        public static readonly Color DarkBlue = Color.NameColor("DarkBlue", 0xFF00008B);

        /// <summary>
        /// DarkCyan color.
        /// </summary>
        public static readonly Color DarkCyan = Color.NameColor("DarkCyan", 0xFF008B8B);

        /// <summary>
        /// DarkGoldenrod color.
        /// </summary>
        public static readonly Color DarkGoldenrod = Color.NameColor("DarkGoldenrod", 0xFFB8860B);

        /// <summary>
        /// DarkGray color.
        /// </summary>
        public static readonly Color DarkGray = Color.NameColor("DarkGray", 0xFFA9A9A9);

        /// <summary>
        /// DarkGreen color.
        /// </summary>
        public static readonly Color DarkGreen = Color.NameColor("DarkGreen", 0xFF006400);

        /// <summary>
        /// DarkKhaki color.
        /// </summary>
        public static readonly Color DarkKhaki = Color.NameColor("DarkKhaki", 0xFFBDB76B);

        /// <summary>
        /// DarkMagenta color.
        /// </summary>
        public static readonly Color DarkMagenta = Color.NameColor("DarkMagenta", 0xFF8B008B);

        /// <summary>
        /// DarkOliveGreen color.
        /// </summary>
        public static readonly Color DarkOliveGreen = Color.NameColor("DarkOliveGreen", 0xFF556B2F);

        /// <summary>
        /// DarkOrange color.
        /// </summary>
        public static readonly Color DarkOrange = Color.NameColor("DarkOrange", 0xFFFF8C00);

        /// <summary>
        /// DarkOrchid color.
        /// </summary>
        public static readonly Color DarkOrchid = Color.NameColor("DarkOrchid", 0xFF9932CC);

        /// <summary>
        /// DarkRed color.
        /// </summary>
        public static readonly Color DarkRed = Color.NameColor("DarkRed", 0xFF8B0000);

        /// <summary>
        /// DarkSalmon color.
        /// </summary>
        public static readonly Color DarkSalmon = Color.NameColor("DarkSalmon", 0xFFE9967A);

        /// <summary>
        /// DarkSeaGreen color.
        /// </summary>
        public static readonly Color DarkSeaGreen = Color.NameColor("DarkSeaGreen", 0xFF8FBC8B);

        /// <summary>
        /// DarkSlateBlue color.
        /// </summary>
        public static readonly Color DarkSlateBlue = Color.NameColor("DarkSlateBlue", 0xFF483D8B);

        /// <summary>
        /// DarkSlateGray color.
        /// </summary>
        public static readonly Color DarkSlateGray = Color.NameColor("DarkSlateGray", 0xFF2F4F4F);

        /// <summary>
        /// DarkTurquoise color.
        /// </summary>
        public static readonly Color DarkTurquoise = Color.NameColor("DarkTurquoise", 0xFF00CED1);

        /// <summary>
        /// DarkViolet color.
        /// </summary>
        public static readonly Color DarkViolet = Color.NameColor("DarkViolet", 0xFF9400D3);

        /// <summary>
        /// DeepPink color.
        /// </summary>
        public static readonly Color DeepPink = Color.NameColor("DeepPink", 0xFFFF1493);

        /// <summary>
        /// DeepSkyBlue color.
        /// </summary>
        public static readonly Color DeepSkyBlue = Color.NameColor("DeepSkyBlue", 0xFF00BFFF);

        /// <summary>
        /// DimGray color.
        /// </summary>
        public static readonly Color DimGray = Color.NameColor("DimGray", 0xFF696969);

        /// <summary>
        /// DodgerBlue color.
        /// </summary>
        public static readonly Color DodgerBlue = Color.NameColor("DodgerBlue", 0xFF1E90FF);

        /// <summary>
        /// Firebrick color.
        /// </summary>
        public static readonly Color Firebrick = Color.NameColor("Firebrick", 0xFFB22222);

        /// <summary>
        /// FloralWhite color.
        /// </summary>
        public static readonly Color FloralWhite = Color.NameColor("FloralWhite", 0xFFFFFAF0);

        /// <summary>
        /// ForestGreen color.
        /// </summary>
        public static readonly Color ForestGreen = Color.NameColor("ForestGreen", 0xFF228B22);

        /// <summary>
        /// Fuchsia color.
        /// </summary>
        public static readonly Color Fuchsia = Color.NameColor("Fuchsia", 0xFFFF00FF);

        /// <summary>
        /// Gainsboro color.
        /// </summary>
        public static readonly Color Gainsboro = Color.NameColor("Gainsboro", 0xFFDCDCDC);

        /// <summary>
        /// GhostWhite color.
        /// </summary>
        public static readonly Color GhostWhite = Color.NameColor("GhostWhite", 0xFFF8F8FF);

        /// <summary>
        /// Gold color.
        /// </summary>
        public static readonly Color Gold = Color.NameColor("Gold", 0xFFFFD700);

        /// <summary>
        /// Goldenrod color.
        /// </summary>
        public static readonly Color Goldenrod = Color.NameColor("Goldenrod", 0xFFDAA520);

        /// <summary>
        /// Gray color.
        /// </summary>
        public static readonly Color Gray = Color.NameColor("Gray", 0xFF808080);

        /// <summary>
        /// Green color.
        /// </summary>
        public static readonly Color Green = Color.NameColor("Green", 0xFF008000);

        /// <summary>
        /// GreenYellow color.
        /// </summary>
        public static readonly Color GreenYellow = Color.NameColor("GreenYellow", 0xFFADFF2F);

        /// <summary>
        /// Honeydew color.
        /// </summary>
        public static readonly Color Honeydew = Color.NameColor("Honeydew", 0xFFF0FFF0);

        /// <summary>
        /// HotPink color.
        /// </summary>
        public static readonly Color HotPink = Color.NameColor("HotPink", 0xFFFF69B4);

        /// <summary>
        /// IndianRed color.
        /// </summary>
        public static readonly Color IndianRed = Color.NameColor("IndianRed", 0xFFCD5C5C);

        /// <summary>
        /// Indigo color.
        /// </summary>
        public static readonly Color Indigo = Color.NameColor("Indigo", 0xFF4B0082);

        /// <summary>
        /// Ivory color.
        /// </summary>
        public static readonly Color Ivory = Color.NameColor("Ivory", 0xFFFFFFF0);

        /// <summary>
        /// Khaki color.
        /// </summary>
        public static readonly Color Khaki = Color.NameColor("Khaki", 0xFFF0E68C);

        /// <summary>
        /// Lavender color.
        /// </summary>
        public static readonly Color Lavender = Color.NameColor("Lavender", 0xFFE6E6FA);

        /// <summary>
        /// LavenderBlush color.
        /// </summary>
        public static readonly Color LavenderBlush = Color.NameColor("LavenderBlush", 0xFFFFF0F5);

        /// <summary>
        /// LawnGreen color.
        /// </summary>
        public static readonly Color LawnGreen = Color.NameColor("LawnGreen", 0xFF7CFC00);

        /// <summary>
        /// LemonChiffon color.
        /// </summary>
        public static readonly Color LemonChiffon = Color.NameColor("LemonChiffon", 0xFFFFFACD);

        /// <summary>
        /// LightBlue color.
        /// </summary>
        public static readonly Color LightBlue = Color.NameColor("LightBlue", 0xFFADD8E6);

        /// <summary>
        /// LightCoral color.
        /// </summary>
        public static readonly Color LightCoral = Color.NameColor("LightCoral", 0xFFF08080);

        /// <summary>
        /// LightCyan color.
        /// </summary>
        public static readonly Color LightCyan = Color.NameColor("LightCyan", 0xFFE0FFFF);

        /// <summary>
        /// LightGoldenrodYellow color.
        /// </summary>
        public static readonly Color LightGoldenrodYellow = Color.NameColor("LightGoldenrodYellow", 0xFFFAFAD2);

        /// <summary>
        /// LightGray color.
        /// </summary>
        public static readonly Color LightGray = Color.NameColor("LightGray", 0xFFD3D3D3);

        /// <summary>
        /// LightGreen color.
        /// </summary>
        public static readonly Color LightGreen = Color.NameColor("LightGreen", 0xFF90EE90);

        /// <summary>
        /// LightPink color.
        /// </summary>
        public static readonly Color LightPink = Color.NameColor("LightPink", 0xFFFFB6C1);

        /// <summary>
        /// LightSalmon color.
        /// </summary>
        public static readonly Color LightSalmon = Color.NameColor("LightSalmon", 0xFFFFA07A);

        /// <summary>
        /// LightSeaGreen color.
        /// </summary>
        public static readonly Color LightSeaGreen = Color.NameColor("LightSeaGreen", 0xFF20B2AA);

        /// <summary>
        /// LightSkyBlue color.
        /// </summary>
        public static readonly Color LightSkyBlue = Color.NameColor("LightSkyBlue", 0xFF87CEFA);

        /// <summary>
        /// LightSlateGray color.
        /// </summary>
        public static readonly Color LightSlateGray = Color.NameColor("LightSlateGray", 0xFF778899);

        /// <summary>
        /// LightSteelBlue color.
        /// </summary>
        public static readonly Color LightSteelBlue = Color.NameColor("LightSteelBlue", 0xFFB0C4DE);

        /// <summary>
        /// LightYellow color.
        /// </summary>
        public static readonly Color LightYellow = Color.NameColor("LightYellow", 0xFFFFFFE0);

        /// <summary>
        /// Lime color.
        /// </summary>
        public static readonly Color Lime = Color.NameColor("Lime", 0xFF00FF00);

        /// <summary>
        /// LimeGreen color.
        /// </summary>
        public static readonly Color LimeGreen = Color.NameColor("LimeGreen", 0xFF32CD32);

        /// <summary>
        /// Linen color.
        /// </summary>
        public static readonly Color Linen = Color.NameColor("Linen", 0xFFFAF0E6);

        /// <summary>
        /// Magenta color.
        /// </summary>
        public static readonly Color Magenta = Color.NameColor("Magenta", 0xFFFF00FF);

        /// <summary>
        /// Maroon color.
        /// </summary>
        public static readonly Color Maroon = Color.NameColor("Maroon", 0xFF800000);

        /// <summary>
        /// MediumAquamarine color.
        /// </summary>
        public static readonly Color MediumAquamarine = Color.NameColor("MediumAquamarine", 0xFF66CDAA);

        /// <summary>
        /// MediumBlue color.
        /// </summary>
        public static readonly Color MediumBlue = Color.NameColor("MediumBlue", 0xFF0000CD);

        /// <summary>
        /// MediumOrchid color.
        /// </summary>
        public static readonly Color MediumOrchid = Color.NameColor("MediumOrchid", 0xFFBA55D3);

        /// <summary>
        /// MediumPurple color.
        /// </summary>
        public static readonly Color MediumPurple = Color.NameColor("MediumPurple", 0xFF9370DB);

        /// <summary>
        /// MediumSeaGreen color.
        /// </summary>
        public static readonly Color MediumSeaGreen = Color.NameColor("MediumSeaGreen", 0xFF3CB371);

        /// <summary>
        /// MediumSlateBlue color.
        /// </summary>
        public static readonly Color MediumSlateBlue = Color.NameColor("MediumSlateBlue", 0xFF7B68EE);

        /// <summary>
        /// MediumSpringGreen color.
        /// </summary>
        public static readonly Color MediumSpringGreen = Color.NameColor("MediumSpringGreen", 0xFF00FA9A);

        /// <summary>
        /// MediumTurquoise color.
        /// </summary>
        public static readonly Color MediumTurquoise = Color.NameColor("MediumTurquoise", 0xFF48D1CC);

        /// <summary>
        /// MediumVioletRed color.
        /// </summary>
        public static readonly Color MediumVioletRed = Color.NameColor("MediumVioletRed", 0xFFC71585);

        /// <summary>
        /// MidnightBlue color.
        /// </summary>
        public static readonly Color MidnightBlue = Color.NameColor("MidnightBlue", 0xFF191970);

        /// <summary>
        /// MintCream color.
        /// </summary>
        public static readonly Color MintCream = Color.NameColor("MintCream", 0xFFF5FFFA);

        /// <summary>
        /// MistyRose color.
        /// </summary>
        public static readonly Color MistyRose = Color.NameColor("MistyRose", 0xFFFFE4E1);

        /// <summary>
        /// Moccasin color.
        /// </summary>
        public static readonly Color Moccasin = Color.NameColor("Moccasin", 0xFFFFE4B5);

        /// <summary>
        /// NavajoWhite color.
        /// </summary>
        public static readonly Color NavajoWhite = Color.NameColor("NavajoWhite", 0xFFFFDEAD);

        /// <summary>
        /// Navy color.
        /// </summary>
        public static readonly Color Navy = Color.NameColor("Navy", 0xFF000080);

        /// <summary>
        /// OldLace color.
        /// </summary>
        public static readonly Color OldLace = Color.NameColor("OldLace", 0xFFFDF5E6);

        /// <summary>
        /// Olive color.
        /// </summary>
        public static readonly Color Olive = Color.NameColor("Olive", 0xFF808000);

        /// <summary>
        /// OliveDrab color.
        /// </summary>
        public static readonly Color OliveDrab = Color.NameColor("OliveDrab", 0xFF6B8E23);

        /// <summary>
        /// Orange color.
        /// </summary>
        public static readonly Color Orange = Color.NameColor("Orange", 0xFFFFA500);

        /// <summary>
        /// OrangeRed color.
        /// </summary>
        public static readonly Color OrangeRed = Color.NameColor("OrangeRed", 0xFFFF4500);

        /// <summary>
        /// Orchid color.
        /// </summary>
        public static readonly Color Orchid = Color.NameColor("Orchid", 0xFFDA70D6);

        /// <summary>
        /// PaleGoldenrod color.
        /// </summary>
        public static readonly Color PaleGoldenrod = Color.NameColor("PaleGoldenrod", 0xFFEEE8AA);

        /// <summary>
        /// PaleGreen color.
        /// </summary>
        public static readonly Color PaleGreen = Color.NameColor("PaleGreen", 0xFF98FB98);

        /// <summary>
        /// PaleTurquoise color.
        /// </summary>
        public static readonly Color PaleTurquoise = Color.NameColor("PaleTurquoise", 0xFFAFEEEE);

        /// <summary>
        /// PaleVioletRed color.
        /// </summary>
        public static readonly Color PaleVioletRed = Color.NameColor("PaleVioletRed", 0xFFDB7093);

        /// <summary>
        /// PapayaWhip color.
        /// </summary>
        public static readonly Color PapayaWhip = Color.NameColor("PapayaWhip", 0xFFFFEFD5);

        /// <summary>
        /// PeachPuff color.
        /// </summary>
        public static readonly Color PeachPuff = Color.NameColor("PeachPuff", 0xFFFFDAB9);

        /// <summary>
        /// Peru color.
        /// </summary>
        public static readonly Color Peru = Color.NameColor("Peru", 0xFFCD853F);

        /// <summary>
        /// Pink color.
        /// </summary>
        public static readonly Color Pink = Color.NameColor("Pink", 0xFFFFC0CB);

        /// <summary>
        /// Plum color.
        /// </summary>
        public static readonly Color Plum = Color.NameColor("Plum", 0xFFDDA0DD);

        /// <summary>
        /// PowderBlue color.
        /// </summary>
        public static readonly Color PowderBlue = Color.NameColor("PowderBlue", 0xFFB0E0E6);

        /// <summary>
        /// Purple color.
        /// </summary>
        public static readonly Color Purple = Color.NameColor("Purple", 0xFF800080);

        /// <summary>
        /// Red color.
        /// </summary>
        public static readonly Color Red = Color.NameColor("Red", 0xFFFF0000);

        /// <summary>
        /// RosyBrown color.
        /// </summary>
        public static readonly Color RosyBrown = Color.NameColor("RosyBrown", 0xFFBC8F8F);

        /// <summary>
        /// RoyalBlue color.
        /// </summary>
        public static readonly Color RoyalBlue = Color.NameColor("RoyalBlue", 0xFF4169E1);

        /// <summary>
        /// SaddleBrown color.
        /// </summary>
        public static readonly Color SaddleBrown = Color.NameColor("SaddleBrown", 0xFF8B4513);

        /// <summary>
        /// Salmon color.
        /// </summary>
        public static readonly Color Salmon = Color.NameColor("Salmon", 0xFFFA8072);

        /// <summary>
        /// SandyBrown color.
        /// </summary>
        public static readonly Color SandyBrown = Color.NameColor("SandyBrown", 0xFFF4A460);

        /// <summary>
        /// SeaGreen color.
        /// </summary>
        public static readonly Color SeaGreen = Color.NameColor("SeaGreen", 0xFF2E8B57);

        /// <summary>
        /// SeaShell color.
        /// </summary>
        public static readonly Color SeaShell = Color.NameColor("SeaShell", 0xFFFFF5EE);

        /// <summary>
        /// Sienna color.
        /// </summary>
        public static readonly Color Sienna = Color.NameColor("Sienna", 0xFFA0522D);

        /// <summary>
        /// Silver color.
        /// </summary>
        public static readonly Color Silver = Color.NameColor("Silver", 0xFFC0C0C0);

        /// <summary>
        /// SkyBlue color.
        /// </summary>
        public static readonly Color SkyBlue = Color.NameColor("SkyBlue", 0xFF87CEEB);

        /// <summary>
        /// SlateBlue color.
        /// </summary>
        public static readonly Color SlateBlue = Color.NameColor("SlateBlue", 0xFF6A5ACD);

        /// <summary>
        /// SlateGray color.
        /// </summary>
        public static readonly Color SlateGray = Color.NameColor("SlateGray", 0xFF708090);

        /// <summary>
        /// Snow color.
        /// </summary>
        public static readonly Color Snow = Color.NameColor("Snow", 0xFFFFFAFA);

        /// <summary>
        /// SpringGreen color.
        /// </summary>
        public static readonly Color SpringGreen = Color.NameColor("SpringGreen", 0xFF00FF7F);

        /// <summary>
        /// SteelBlue color.
        /// </summary>
        public static readonly Color SteelBlue = Color.NameColor("SteelBlue", 0xFF4682B4);

        /// <summary>
        /// Tan color.
        /// </summary>
        public static readonly Color Tan = Color.NameColor("Tan", 0xFFD2B48C);

        /// <summary>
        /// Teal color.
        /// </summary>
        public static readonly Color Teal = Color.NameColor("Teal", 0xFF008080);

        /// <summary>
        /// Thistle color.
        /// </summary>
        public static readonly Color Thistle = Color.NameColor("Thistle", 0xFFD8BFD8);

        /// <summary>
        /// Tomato color.
        /// </summary>
        public static readonly Color Tomato = Color.NameColor("Tomato", 0xFFFF6347);

        /// <summary>
        /// Turquoise color.
        /// </summary>
        public static readonly Color Turquoise = Color.NameColor("Turquoise", 0xFF40E0D0);

        /// <summary>
        /// Violet color.
        /// </summary>
        public static readonly Color Violet = Color.NameColor("Violet", 0xFFEE82EE);

        /// <summary>
        /// Wheat color.
        /// </summary>
        public static readonly Color Wheat = Color.NameColor("Wheat", 0xFFF5DEB3);

        /// <summary>
        /// White color.
        /// </summary>
        public static readonly Color White = Color.NameColor("White", 0xFFFFFFFF);

        /// <summary>
        /// WhiteSmoke color.
        /// </summary>
        public static readonly Color WhiteSmoke = Color.NameColor("WhiteSmoke", 0xFFF5F5F5);

        /// <summary>
        /// Yellow color.
        /// </summary>
        public static readonly Color Yellow = Color.NameColor("Yellow", 0xFFFFFF00);

        /// <summary>
        /// YellowGreen color.
        /// </summary>
        public static readonly Color YellowGreen = Color.NameColor("YellowGreen", 0xFF9ACD32);
    }
}
