using System;
using Microsoft.SPOT;
using System.Threading;

namespace NetduinoFeeder
{
    class FeederTrainingBeep
    {
        private Buzzer _buzzer;

        public FeederTrainingBeep(Buzzer buzzer)
        {
            _buzzer = buzzer;
        }

        public void Beep() 
        {
            _buzzer.Buzz(750, 985);
            Thread.Sleep(150);
            _buzzer.Buzz(750, 1428);
            Thread.Sleep(150);
            _buzzer.Buzz(750, 1776);
        }
    }
}
