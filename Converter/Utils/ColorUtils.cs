using System;
using System.Drawing;

namespace MbJsonToYaml.Utils
{
    public class ColorUtils
    {
        public static string ProcessColorString(object colorValue, StoppedDouble opacity)
        {
            string val = (string)colorValue;

            if (opacity != null && (opacity.Stops != null || (opacity.SingleVal > 0 && opacity.SingleVal < 1)))
            {
                if (opacity.Stops == null)
                {
                    Color color = ColorFromString(val);
                    color = Blend(color, opacity.SingleVal);

                    return $"[{NumToString(color.R)},{NumToString(color.G)},{NumToString(color.B)}]";
                }
            }           

            if (val.StartsWith("rgba"))
                return $"'{ColorTranslator.ToHtml(ColorFromRGBA(val))}'";
            if (val.StartsWith("hsla"))
                return $"'{ColorTranslator.ToHtml(ColorFromHSLA(val))}'";
            if (val.StartsWith("hsl"))
                return $"'{ColorTranslator.ToHtml(ColorFromHSL(val))}'";
            return $"'{val}'";            
        }

        private static string NumToString(int num)
        {
            return ((double) num / 255).ToString("0.###");
        }

        private static Color ColorFromRGBA(string val)
        {
            val = val.Substring(5, val.Length - 6);
            var split = val.Split(',');

            double multiplier = double.Parse(split[3]);
            int alpha = (int)(255 * multiplier);

            Color color = Color.FromArgb(255, int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));

            return Blend(color, (double)alpha / 255);
        }

        private static Color ColorFromRGB(string val)
        {
            val = val.Substring(4, val.Length - 5);
            var split = val.Split(',');            

            return Color.FromArgb(255, int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
        }

        private static Color ColorFromHSLA(string val)
        {
            val = val.Substring(5, val.Length - 6);
            var split = val.Split(',');

            float h = float.Parse(split[0]);
            float s = float.Parse(split[1].Remove(split[1].Length - 1).Trim()) / 100;
            float l = float.Parse(split[2].Remove(split[2].Length - 1).Trim()) / 100;

            int a = (int)(double.Parse(split[3]) * 255);

            Color color = ColorFromAhsb(a, h, s, l);

            return Blend(color, (double)a / 255);
        }

        private static Color ColorFromHSL(string val)
        {
            val = val.Substring(4, val.Length - 5);
            var split = val.Split(',');

            float h = float.Parse(split[0]);
            float s = float.Parse(split[1].Remove(split[1].Length - 1).Trim()) / 100;
            float l = float.Parse(split[2].Remove(split[2].Length - 1).Trim()) / 100;

            return ColorFromAhsb(255, h, s, l);
        }

        public static Color ColorFromAhsb(int a, float h, float s, float b)
        {

            if (0 > a || 255 < a)            
                throw new Exception("InvalidAlpha");            
            if (0f > h || 360f < h)           
                throw new Exception("InvalidHue");            
            if (0f > s || 1f < s)            
                throw new Exception("InvalidSaturation");            
            if (0f > b || 1f < b)            
                throw new Exception("InvalidBrightness");            

            if (0 == s)
            {
                return Color.FromArgb(a, Convert.ToInt32(b * 255),
                  Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)            
                h -= 360f;            
            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)            
                fMid = h * (fMax - fMin) + fMin;            
            else            
                fMid = fMin - h * (fMax - fMin);            

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(a, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(a, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(a, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(a, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(a, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(a, iMax, iMid, iMin);
            }
        }

        public static Color BackgroundColor { get; set; } = Color.Beige;

        /// <summary>Blends the specified colors together.</summary>
        /// <param name="color">Color to blend onto the background color.</param>
        /// <param name="backColor">Color to blend the other color onto.</param>
        /// <param name="amount">How much of <paramref name="color"/> to keep,
        /// “on top of” <paramref name="backColor"/>.</param>
        /// <returns>The blended colors.</returns>
        public static Color Blend(Color color, double amount)
        {
            byte r = (byte)((color.R * amount) + BackgroundColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + BackgroundColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + BackgroundColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }

        public static Color ColorFromString(string val)
        {
            val = val.Trim(new char[] {'\''});

            Color color;
            if (val.StartsWith("rgba"))
                color = ColorFromRGBA(val);
            else if (val.StartsWith("rgb"))
                color = ColorFromRGB(val);            
            else if (val.StartsWith("hsla"))
                color = ColorFromHSLA(val);
            else if (val.StartsWith("hsl"))
                color = ColorFromHSL(val);
            else
                color = ColorTranslator.FromHtml(val);

            return color;
        }
    }
}
