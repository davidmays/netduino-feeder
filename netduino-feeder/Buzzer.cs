using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using System.Threading;

namespace NetduinoFeeder
{
    class Buzzer
    {
        private PWM pwm;
        private Timer timer;

        public Buzzer(Cpu.Pin pin)
        {
            pwm = new PWM(pin);
        }

        public void Buzz(int milliseconds, int frequency)
        {
            
            var period = (uint)(1000000 / frequency);
            pwm.SetPulse(period, period / 2); 

            timer = new Timer(TimerTick, null, milliseconds, 0);
        }

        private void TimerTick(object state) 
        {
            pwm.SetDutyCycle(0);
            timer.Dispose();
            timer = null;
        }
    }
}
