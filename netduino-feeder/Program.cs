using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

using Servo_API;

namespace NetduinoFeeder
{
    public class Program
    {

        private static Blinker blinker;

        public static void Main()
        {
            blinker = new Blinker();

            WakeupBlink();

            try
            {
                var network = new StaticEthernetNetwork("192.168.1.199","255.255.255.0","192.168.1.1");

                var buzzer = new Buzzer(Pins.GPIO_PIN_D6);
                buzzer.Buzz(150,1000);

                var servo = new RadioShackMicroServo(Pins.GPIO_PIN_D9);
                servo.Center();
                Thread.Sleep(1000);

                servo.Left();
                Thread.Sleep(1000);
                servo.Right();
                Thread.Sleep(1000);                
                servo.Center();
                Thread.Sleep(1000);

                var server = new WebServer(80, servo, buzzer);
                server.Wait();

                ShutdownBlink();
            }
            catch 
            {
                ErrorBlink();
            }
        }


        private static void ShutdownBlink()
        {
            blinker.Blink(20, 100); 
        }

        private static void ErrorBlink()
        {
            blinker.Blink(5, 100);
        }
        
        private static void WakeupBlink()
        {
            blinker.Blink(3, 250);
        }

    }
}
