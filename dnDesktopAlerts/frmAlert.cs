using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace dnDesktopAlerts
{
    public partial class frmAlert : Form
    {
        public frmAlert(int id, Color backcolor,string content)
        {
            InitializeComponent();

            this.ID = id;
            this.btnCLose.BackColor = backcolor;
            this.BackColor = backcolor;
            this.label1.BackColor = backcolor;
            this.pictureBox1.BackColor = backcolor;
            GetIndex();
            this.label1.Text = content;
        }
        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        private const int AW_SLIDE = 0x00040000;
        private const int AW_VER_NEGATIVE = 0x00000002;

        private int sWidth = Screen.PrimaryScreen.Bounds.Width;
        private int sHeight = Screen.PrimaryScreen.Bounds.Height;

        public int ID
        {
            set;
            get;
        }
        public int CountIndex
        {
            set;
            get;
        }
        private void frmAlert_FormClosed(object sender, FormClosedEventArgs e)
        {
            hDesktopAlerts.DoClosed(this, e);
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetIndex()
        {
            
            int maxCount = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / this.Size.Height;
            for (int i = 0; i < maxCount; i++)
            {
                int floors = hDesktopAlerts.ShowList.Count / maxCount;
                int left = hDesktopAlerts.ShowList.Count % maxCount;
                int k=0;
                foreach(int j in hDesktopAlerts.ShowList)
                {
                    if(j==i)
                        k++;
                }
                if (i < left)
                {
                    if (k < floors + 1)
                    {
                        if (i >= maxCount)
                        {
                            this.CountIndex = i - maxCount;
                        }
                        else
                        {
                            this.CountIndex = i;
                        }

                        return;
                    }
                }
                else
                {
                    if (k < floors)
                    {
                        if (i >= maxCount)
                        {
                            this.CountIndex = i - maxCount;
                        }
                        else
                        {
                            this.CountIndex = i;
                        }

                        return;
                    }
                }
            }
            if (hDesktopAlerts.ShowList.Count >= maxCount)
            {
                this.CountIndex = hDesktopAlerts.ShowList.Count - maxCount;
            }
            else
            {
                this.CountIndex = hDesktopAlerts.ShowList.Count;
            }

          
        }

        private void frmAlert_Load(object sender, EventArgs e)
        {

            this.Location = new Point(sWidth - this.Size.Width, this.CountIndex * (this.Size.Height + 5));

            hDesktopAlerts.ShowList.Add(this.CountIndex);

            AnimateWindow(this.Handle, 500, AW_SLIDE + AW_VER_NEGATIVE);
        }
    }
}
