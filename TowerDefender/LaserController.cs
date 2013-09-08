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
        private float gain = 0.25f;

        public void Connect(SerialPort port)
        {
            _port = port;
            port.Open();
        }

        public void Update(float deltax, float deltay)
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

            var text = String.Format("{0:000},{1:000},{2}\r\n", _x, _y, 1);
            //Thread.Sleep(100);
            _port.Write(text);
            var resp = _port.ReadExisting();

        }
    }
}
