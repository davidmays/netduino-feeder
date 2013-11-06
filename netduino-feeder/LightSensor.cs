using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace NetduinoFeeder
{
    class LightSensor
    {
        private InputPort analogIn;

        public LightSensor(Cpu.Pin pin)
        {
            analogIn = new InputPort(pin, true, Port.ResistorMode.PullDown);
        }

        public void WatchLight() 
        {
            var value = analogIn.Read();
        }

    }
}
