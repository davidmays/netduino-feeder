using System;
using Microsoft.SPOT;
using System.Threading;

namespace NetduinoFeeder
{
    class Feeder
    {
        private FeederTrainingBeep _beep;
        private Blinker _blinker;
        private RadioShackMicroServo _servo;

        public Feeder(FeederTrainingBeep beep, Blinker blinker, RadioShackMicroServo servo)
        {
            _beep = beep;
            _blinker = blinker;
            _servo = servo;
        }

        public void OpenFeeder()
        {
            _blinker.Blink(4, 50);
            _beep.Beep();
            _servo.Right();
            Thread.Sleep(1000);
            _servo.MoveTo(RadioShackMicroServo.PositionHalfLeft);
        }
    }
}
