using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace X2292_ChassisFinalAssyAOI
{
    [Serializable]
    public class Counter
    {
        public delegate void CounterRefreshedEventHandler(object sender, CounterEventArgs e);

        public event CounterRefreshedEventHandler CounterRefreshed;

        private long pass, fail0, fail1, fail2, fail3, total;

        private long err0, err1, err2, err3;

        private string counterName;


        public Counter(string name)
        {
            this.counterName = name;
        }

        public long Pass
        {
            get
            {
                return this.pass;
            }
        }
        public long Fail_0
        {
            get
            {
                return this.fail0;
            }
        }
        public long Fail_1
        {
            get
            {
                return this.fail1;
            }
        }
        public long Fail_2
        {
            get
            {
                return this.fail2;
            }
        }
        public long Fail_3
        {
            get
            {
                return this.fail3;
            }
        }
        public long Total
        {
            get
            {
                return total;
            }
        }

        public long Err0
        {
            get
            {
                return err0;
            }
        }

        public long Err1
        {
            get
            {
                return err1;
            }
        }

        public long Err2
        {
            get
            {
                return err2;
            }
        }

        public long Err3
        {
            get
            {
                return err3;
            }
        }

        public double NGPercent_0
        {
            get
            {
                if (this.Total != 0)
                {
                    return this.fail0 * 1.00 / this.Total;
                }
                else
                {
                    return 0.00;
                }
            }
        }
        public double NGPercent_1
        {
            get
            {
                if (this.Total != 0)
                {
                    return this.fail1 * 1.00 / this.Total;
                }
                else
                {
                    return 0.00;
                }
            }
        }
        public double NGPercent_2
        {
            get
            {
                if (this.Total != 0)
                {
                    return this.fail2 * 1.00 / this.Total;
                }
                else
                {
                    return 0.00;
                }
            }
        }
        public double NGPercent_3
        {
            get
            {
                if (this.Total != 0)
                {
                    return this.fail3 * 1.00 / this.Total;
                }
                else
                {
                    return 0.00;
                }
            }
        }
        public double OKPercent
        {
            get
            {
                if (this.Total != 0)
                {
                    return this.pass * 1.00 / this.Total;
                }
                else
                {
                    return 0.00;
                }
            }
        }
        public double ERRPercent_0
        {
            get
            {
                if (this.Total != 0)
                {
                    return this.err0 * 1.00 / this.Total;
                }
                else
                {
                    return 0.00;
                }
            }
        }

        public double ERRPercent_1
        {
            get
            {
                if (this.Total != 0)
                {
                    return this.err1 * 1.00 / this.Total;
                }
                else
                {
                    return 0.00;
                }
            }
        }

        public double ERRPercent_2
        {
            get
            {
                if (this.Total != 0)
                {
                    return this.err2 * 1.00 / this.Total;
                }
                else
                {
                    return 0.00;
                }
            }
        }

        public string CounterName
        {
            set
            {
                this.counterName = value;
            }
            get
            {
                if (string.IsNullOrEmpty(this.counterName))
                {
                    return "Count";
                }
                else
                {
                    return this.counterName;
                }
            }
        }

        public void TotalIncrease()
        {
            this.total++;
            DoDataRefreshedEvent(false);
        }

        public void OKIncrease()
        {
            pass++;
            DoDataRefreshedEvent(false);
        }

        public void NG0Increase()
        {
            fail0++;
            DoDataRefreshedEvent(false);
        }
        public void NG1Increase()
        {
            fail1++;
            DoDataRefreshedEvent(false);
        }
        public void NG2Increase()
        {
            fail2++;
            DoDataRefreshedEvent(false);
        }
        public void NG3Increase()
        {
            fail3++;
            DoDataRefreshedEvent(false);
        }
        public void ERR0Incresase()
        {
            err0++;
            DoDataRefreshedEvent(false);
        }

        public void ERR1Incresase()
        {
            err1++;
            DoDataRefreshedEvent(false);
        }

        public void ERR2Incresase()
        {
            err2++;
            DoDataRefreshedEvent(false);
        }

        public void ERR3Incresase()
        {
            err3++;
            DoDataRefreshedEvent(false);
        }

        public void Reset()
        {
            this.pass = 0;
            this.fail0 = 0;
            this.fail1 = 0;
            this.fail2 = 0;
            this.fail3 = 0;
            this.total = 0;
            this.err0 = 0;
            this.err1 = 0;
            this.err2 = 0;
            this.err3 = 0;
            DoDataRefreshedEvent(false);
        }

        public void IniCounter()
        {
            if (System.IO.File.Exists(string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)))
            {
                this.total = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Total", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
                this.pass = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Pass", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
                this.fail0 = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Fail0", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
                this.fail1 = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Fail1", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
                this.fail2 = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Fail2", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
                this.fail3 = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Fail3", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
                this.err0 = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Err0", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
                this.err1 = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Err1", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
                this.err2 = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Err2", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
                this.err3 = Convert.ToInt64(IniOperator.GetConfig(this.CounterName, "Err3", string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath)));
            }
            DoDataRefreshedEvent(true);
        }
        private bool SaveCounter()
        {
            bool result = false;
            try
            {
                IniOperator.SaveConfig(this.CounterName, "Total", total.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                IniOperator.SaveConfig(this.CounterName, "Pass", pass.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                IniOperator.SaveConfig(this.CounterName, "Fail0", fail0.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                IniOperator.SaveConfig(this.CounterName, "Fail1", fail1.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                IniOperator.SaveConfig(this.CounterName, "Fail2", fail2.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                IniOperator.SaveConfig(this.CounterName, "Fail3", fail3.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                IniOperator.SaveConfig(this.CounterName, "Err0", err0.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                IniOperator.SaveConfig(this.CounterName, "Err1", err1.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                IniOperator.SaveConfig(this.CounterName, "Err2", err2.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                IniOperator.SaveConfig(this.CounterName, "Err3", err3.ToString(), string.Format("{0}\\Counter.ini", System.Windows.Forms.Application.StartupPath));
                result = true;
            }
            catch
            { }

            return result;
        }
        private void DoDataRefreshedEvent(bool isIni)
        {
            if (!isIni)
            {
                SaveCounter();
            }
            if (this.CounterRefreshed != null)
            {
                CounterRefreshed(this, new CounterEventArgs
                {
                    Fail0 = this.fail0,
                    Pass = this.pass,
                    NGPercent0 = this.NGPercent_0,
                    OKPercent = this.OKPercent,
                    Total = this.Total,
                    Fail1 = this.fail1,
                    Fail2 = this.fail2,
                    Fail3 = this.fail3,
                    NGPercent1 = this.NGPercent_1,
                    NGPercent2 = this.NGPercent_2,
                    NGPercent3 = this.NGPercent_3,
                    Err0 = this.err0,
                    Err1 = this.err1,
                    Err2 = this.err2,
                    Err3 = this.err3,
                    ERRPercent0 = this.ERRPercent_0,
                    ERRPercent1 = this.ERRPercent_1,
                    ERRPercent2 = this.ERRPercent_2
                });
            }

        }

        public class CounterEventArgs : EventArgs
        {
            public long Pass
            {
                internal set;
                get;
            }
            public long Fail0
            {
                internal set;
                get;
            }
            public long Fail1
            {
                internal set;
                get;
            }
            public long Fail2
            {
                internal set;
                get;
            }
            public long Fail3
            {
                internal set;
                get;
            }
            public long Total
            {
                internal set;
                get;
            }
            public double OKPercent
            {
                internal set;
                get;
            }
            public double NGPercent0
            {
                internal set;
                get;
            }
            public double NGPercent1
            {
                internal set;
                get;
            }
            public double NGPercent2
            {
                internal set;
                get;
            }
            public double NGPercent3
            {
                internal set;
                get;
            }

            public long Err0
            {
                internal set;
                get;
            }

            public long Err1
            {
                internal set;
                get;
            }

            public long Err2
            {
                internal set;
                get;
            }
            public long Err3
            {
                internal set;
                get;
            }
            public double ERRPercent0
            {
                internal set;
                get;
            }
            public double ERRPercent1
            {
                internal set;
                get;
            }
            public double ERRPercent2
            {
                internal set;
                get;
            }
        }

    }


}
