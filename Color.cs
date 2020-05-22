using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class Color
    {
        public static Color WHITE = new Color(255, 255, 255);
        public static Color GREEN = new Color(59, 229, 121);

        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Alpha;

        public Color(byte Red, byte Green, byte Blue, byte Alpha = 255)
        {
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Alpha = Alpha;
        }

        public Color Clone()
        {
            return new Color(Red, Green, Blue, Alpha);
        }

        public override string ToString()
        {
            return $"(Color: {Red}, {Green}, {Blue}, {Alpha})";
        }
    }

    public class HeaderColors
    {
        public static Color WHITE = Color.WHITE;
        public static Color GREEN = new Color(59, 229, 121);
    }
}
