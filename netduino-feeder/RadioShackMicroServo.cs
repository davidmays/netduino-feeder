using System;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace NetduinoFeeder
{
    class RadioShackMicroServo
    {
        private readonly PWM servo;
        private static uint FiftyHertz = 20000;
        private static uint PositionFullLeft = 500;
        private static uint PositionFullRight = 2000;
        private static uint PositionCenter = 1250;

        private bool sweeping;

        public RadioShackMicroServo(Cpu.Pin pin) 
        {
            ValidatePin(pin);
            servo = new PWM(pin);
        }

        private void ValidatePin(Cpu.Pin pin)
        {
            switch (pin)
            {
                case Pins.GPIO_PIN_D5:
                case Pins.GPIO_PIN_D6:
                case Pins.GPIO_PIN_D9:
                case Pins.GPIO_PIN_D10:
                    return;
                default:
                    throw new InvalidOperationException("Pin is not PWM capable");
            }
        }

        public void Sweep()
        {
            sweeping = true;
            SweepLoop();
        }

        public void Stop() 
        {
            sweeping = false;
            Center();
        }

        public void Left()
        {
            servo.SetPulse(FiftyHertz, PositionFullLeft);
        }

        public void Center()
        {
            servo.SetPulse(FiftyHertz, PositionCenter);
        }

        public void Release() 
        {
            servo.SetDutyCycle(0);
        }
        
        public void Right() 
        {
            servo.SetPulse(FiftyHertz, PositionFullRight);
        }

        public void SweepLoop() 
        {
            int delay = 10;
            
            while (sweeping)
            {
                for (uint pos = PositionFullLeft; pos < PositionFullRight; pos += 10)
                {
                    SetAndWait(pos, delay);
                }

                for (uint pos = PositionFullRight; pos > PositionFullLeft; pos -= 10)
                {
                    SetAndWait(pos, delay);
                }
            }
        }

        private void SetAndWait(uint position, int delay) 
        {
            servo.SetPulse(FiftyHertz, position);
            Thread.Sleep(delay);
        }
    }
}
