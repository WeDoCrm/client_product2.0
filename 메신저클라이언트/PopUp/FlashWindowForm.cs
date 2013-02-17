using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Client.PopUp
{
    public partial class FlashWindowForm : Form
    {
        [DllImport("user32")]
        private static extern int FlashWindow(System.IntPtr hdl, int bInvert);
        public Timer _TimerFlashWindow = new Timer();

        public FlashWindowForm()
        {
            InitializeComponent();
            _TimerFlashWindow.Interval = 1000;
            _TimerFlashWindow.Tick += new EventHandler(_TimerFlash_Tick);
            TopMost = true;
        }

        private void _TimerFlash_Tick(object sender, EventArgs e)
        {
            _FlashWindow();
        }

        public void DoFlashWindow()
        {
            WindowState = FormWindowState.Minimized;
            Show();
            _TimerFlashWindow.Start();
        }

        private int _FlashWindow()
        {
            return FlashWindow(this.Handle, 1);
        }

        public void StopFlash()
        {
            _TimerFlashWindow.Stop();
        }

        private void FlashWindowForm_Activated(object sender, EventArgs e)
        {
            StopFlash();
        }

        private void FlashWindowForm_Deactivate(object sender, EventArgs e)
        {
            TopMost = false;
        }

        private void FlashWindowForm_FormClosing(object sender, FormClosingEventArgs e) {
            _TimerFlashWindow.Stop();
        }


    }
}
