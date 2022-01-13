using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace dnDesktopAlerts
{
    public class hDesktopAlerts
    {
        private Color frmBackColor = Color.Yellow;

        private string content = "";

        private frmAlert alert;

        private static List<frmAlert> alerts = new List<frmAlert>();


        public  delegate void AlertClosedEventHandler(object sender, EventArgs e);

        public static event AlertClosedEventHandler AlertClosed;

        internal static bool isClosed = false;

        internal static List<int> ShowList = new List<int>();

        internal static List<frmAlert> Alerts
        {
            set
            {
                alerts = value;
            }
            get
            {
                return alerts;
            }
        }

        public Color FrmBackColor
        {
            set
            {
                this.frmBackColor = value;
            }
            get
            {
                return this.frmBackColor;
            }
        }

        public int ID
        {
            set;
            get;
        }

        public int RemovingID
        {
            get;
            set;
        }

        public string Content
        {
            set;
            get;
        }

        public frmAlert AlertForm
        {
            set
            {
                this.alert = value;
            }
            get
            {
                return this.alert;
            }
        }

        public void Show(System.Windows.Forms.Form form)
        {
            form.Invoke(new Action(() =>
               {
                   foreach (frmAlert frm in Alerts)
                   {
                       if (frm.ID == ID)
                       {
                           return;
                       }
                   }
                   this.AlertForm = new frmAlert(ID, frmBackColor, this.Content);
                   hDesktopAlerts.Alerts.Add(AlertForm);
                   this.AlertForm.Show(form);
               }));
        }

        public void Show(System.Windows.Forms.Form form,int id)
        {
            form.Invoke(new Action(() =>
            {
                foreach (frmAlert frm in Alerts)
                {
                    if (frm.ID == id)
                    {
                        return;
                    }
                }
                this.AlertForm = new frmAlert(id, frmBackColor, this.Content);
                hDesktopAlerts.Alerts.Add(AlertForm);
                this.AlertForm.Show(form);
            }));
        }

        public void Show(System.Windows.Forms.Form form, int id,string content)
        {
            form.Invoke(new Action(() =>
            {
                foreach (frmAlert frm in Alerts)
                {
                    if (frm.ID == id)
                    {
                        return;
                    }
                }
                this.AlertForm = new frmAlert(id, frmBackColor, content);
                hDesktopAlerts.Alerts.Add(AlertForm);
                this.AlertForm.Show(form);
            }));
        }

        public void Show(System.Windows.Forms.Form form, string content)
        {
            form.Invoke(new Action(() =>
            {
                foreach (frmAlert frm in Alerts)
                {
                    if (frm.ID == ID)
                    {
                        return;
                    }
                }
                this.AlertForm = new frmAlert(ID, frmBackColor, content);
                hDesktopAlerts.Alerts.Add(AlertForm);
                this.AlertForm.Show(form);
            }));
        }

        public void RemoveAlert()
        {
            foreach (frmAlert temp in Alerts)
            {
                if (temp.ID == RemovingID)
                {
                    temp.Close();
                    break;
                }
            }
        }
        public void RemoveAlert(int ID)
        {
            foreach (frmAlert temp in Alerts)
            {
                if (temp.ID == ID)
                {
                    temp.Close();
                    break;
                }
            }
        }

        public void Clear()
        {
            foreach(frmAlert temp in Alerts)
            {
                temp.Close();
            }
        }
        internal static void DoClosed(frmAlert frm,EventArgs e)
        {
            ShowList.Remove(frm.CountIndex);
           
            if (hDesktopAlerts.AlertClosed != null)
            {   
                hDesktopAlerts.AlertClosed(frm,e);
            } 
            Alerts.Remove(frm);
        }
    }
}
