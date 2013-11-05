using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Net;
using System.Net;

namespace NetduinoFeeder
{
    class EthernetNetwork
    {
        Microsoft.SPOT.Net.NetworkInformation.NetworkInterface _ethernet = null;

        public EthernetNetwork()
        {
            _ethernet = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0];
            Init();
        }


        private void Init() 
        {
            if (_ethernet == null)
                throw new NullReferenceException();

            
            _ethernet.EnableStaticIP("192.168.1.199", "255.255.255.0", "192.168.1.1");
        }

    }
}
