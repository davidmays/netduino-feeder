using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Net;
using System.Net;

namespace NetduinoFeeder
{
    class StaticEthernetNetwork
    {
        Microsoft.SPOT.Net.NetworkInformation.NetworkInterface _ethernet = null;

        public StaticEthernetNetwork(string ipaddress, string netmask, string gateway)
        {
            _ethernet = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0];
            if (_ethernet == null)
                throw new NullReferenceException();

            _ethernet.EnableStaticIP(ipaddress, netmask, gateway);
        }
    }
}
