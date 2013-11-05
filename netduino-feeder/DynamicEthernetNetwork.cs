using System;
using Microsoft.SPOT;

namespace NetduinoFeeder
{
    class DynamicEthernetNetwork
    {
        Microsoft.SPOT.Net.NetworkInformation.NetworkInterface _ethernet = null;

        public DynamicEthernetNetwork(string ipaddress, string netmask, string gateway)
        {
            _ethernet = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0];
            if (_ethernet == null)
                throw new NullReferenceException();

            _ethernet.EnableDhcp();
        }
    }
}
