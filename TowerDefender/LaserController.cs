using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TowerDefender
{
    public class LaserController
    {
        private SerialPort _port;
        private float _x = 100;
        private float _y = 100;
        private DateTime _lastFired = DateTime.MinValue;
        private bool _isFiring;
        private float gain = 0.5f;

        public void Connect(SerialPort port)
        {
            _port = port;
            port.Open();
        }

        public void Update(float deltax, float deltay, bool shouldFire)
        {
            if (deltax > 0)
                _x += gain;
            if (deltax < 0)
                _x -= gain;

            if (deltay < 0)
                _y += gain;
            if (deltay > 0)
                _y -= gain;

            _x = Math.Min(Math.Max(_x, 0), 180);
            _y = Math.Min(Math.Max(_y, 0), 180);

            if (shouldFire && !_isFiring && (DateTime.Now - _lastFired).TotalMilliseconds > 2000)
            {
                _isFiring = true;
                _lastFired = DateTime.Now;
            }

            if (_isFiring && (DateTime.Now - _lastFired).TotalMilliseconds > 500)
            {
                _isFiring = false;
            }

            var text = String.Format("{0:000},{1:000},{2}\r\n", _x, _y, _isFiring ? 1 : 0);
            //Thread.Sleep(100);
            _port.Write(text);
            var resp = _port.ReadExisting();
        }
    }
}
