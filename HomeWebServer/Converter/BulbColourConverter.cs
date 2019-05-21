using System.Globalization;
using ColorMine;
using ColorMine.ColorSpaces;

namespace HomeWebServer.Converter
{
    class BulbColourConverter
    {
        public int rgbRed;
        public int rgbGreen;
        public int rgbBlue;
        public double hue;
        public double sat;
        public double val;

        public void Convert(string input)
        {
            //Convert hex to RGB first
            HexToRGB(input);
            //Then convert RGB to HSV
            RGBToHSV(rgbRed, rgbGreen, rgbBlue);
            //HSV is then stored in public hue/sat/val
        }

        public void HexToRGB(string input)
        {
            //Remove the #
            if (input.IndexOf('#') != -1)
                input = input.Replace("#", "");

            if (input.Length == 6)
            {
                //RRGGBB
                rgbRed = int.Parse(input.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                rgbGreen = int.Parse(input.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                rgbBlue = int.Parse(input.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            }
            else if (input.Length == 3)
            {
                //RGB
                rgbRed = int.Parse(input[0].ToString() + input[0].ToString(), NumberStyles.AllowHexSpecifier);
                rgbGreen = int.Parse(input[1].ToString() + input[1].ToString(), NumberStyles.AllowHexSpecifier);
                rgbBlue = int.Parse(input[2].ToString() + input[2].ToString(), NumberStyles.AllowHexSpecifier);
            }
        }

        public void RGBToHSV(int rgbRed, int rgbGreen, int rgbBlue)
        {
            var rgbCol = new Rgb { R = rgbRed, G = rgbGreen, B = rgbBlue };
            var hsvCol = rgbCol.To<Hsv>();
            hue = hsvCol.H;
            sat = hsvCol.S * 100;
            val = hsvCol.V * 100;
        }
    }
}
