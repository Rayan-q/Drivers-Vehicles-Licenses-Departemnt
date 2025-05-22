using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDManagement.Manage_LDLApplications.Tests.User_Controls
{
    public partial class crtTestTimer : UserControl
    {
        public crtTestTimer()
        {
            InitializeComponent();
        }
        public event EventHandler TimeUp;

        protected virtual void RaiseTimeUp()
        {
            TimeUp?.Invoke(this, EventArgs.Empty);
        }

        public void Start()
        {
            timer1.Start();
            _TotalSeconds = _TestTime * 60;
        }

        public void Stop()
        {
            timer1.Stop();
        }

        static int _TestTime { get; set; }

        public int SetTestTimeMinutes
        {
            set { _TestTime = value;}
            get { return _TestTime; }
        }
        int _TotalSeconds;
        int _Seconds;
        int _Minutes;

        private void timer1_Tick(object sender, EventArgs e)
        {
            _Seconds++;
            if (_Seconds == 60)
            {
                _Seconds = 0;
                _Minutes++;
            }
            lbSeconds.Text = _Seconds <= 9 ? 0 + _Seconds.ToString() : _Seconds.ToString();

            if(_Minutes == 60)
            {
                _Minutes = 0;
            }
            lbMinutes.Text = _Minutes <= 9 ? 0 + _Minutes.ToString() : _Minutes.ToString();

            _TotalSeconds--;

            if(_isTimeUp())
            {
                RaiseTimeUp();
            }
        }

        private bool _isTimeUp()
        { 
            return (_TotalSeconds == 0);
        }
    }
}
