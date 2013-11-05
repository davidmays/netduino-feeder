using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;

namespace NetduinoFeeder
{
    class Blinker
    {
        private OutputPort led;

        public Blinker()
        {
            led = new OutputPort(Pins.ONBOARD_LED, false);
        }

        public void Blink(int times, int msecdelay)
        {
            for (var i = 0; i < times; i++)
            {
                led.Write(true);
                Thread.Sleep(msecdelay);
                led.Write(false);
                Thread.Sleep(msecdelay);
            }
        }
    }
}
