using System;
using static TPLinkSmartDevices.Devices.TPLinkSmartBulb;

namespace HomeWebServer.BulbControl
{
    class SmartBulb
    {
        public void Bulb_On(string bulbIP)
        {
            var smartBulb = new TPLinkSmartDevices.Devices.TPLinkSmartBulb(bulbIP);
            smartBulb.PoweredOn = true;
        }
        public void Bulb_Off(string bulbIP)
        {
            var smartBulb = new TPLinkSmartDevices.Devices.TPLinkSmartBulb(bulbIP);
            smartBulb.PoweredOn = false;
        }
        public void Bulb_Colour(string bulbIP, double hue, double sat, double val)
        {
           var smartBulb = new TPLinkSmartDevices.Devices.TPLinkSmartBulb(bulbIP);
            int roundh = (int)Math.Round(hue, 0);
            int rounds = (int)Math.Round(sat, 0);
            int roundv = (int)Math.Round(val, 0);
            var newHSV = new BulbHSV()
            {
                Hue = roundh,
                Saturation = rounds,
                Value = roundv
            };
            smartBulb.HSV = newHSV;
        }
    }
}
