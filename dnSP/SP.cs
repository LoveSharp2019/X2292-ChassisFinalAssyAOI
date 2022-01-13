using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace dnSP
{

    public class SP
    {
        private static Splash SplashForm;

        public static void ShowSplash(string imageName)
        {
            Thread t = new Thread(() => showingSplash(imageName));
            t.Start();
            Thread t1 = new Thread(() => ShowIMG());
            t1.Start();
        }



        private static void showingSplash(string iName)
        {
            KillMe();
            SplashForm = new Splash();
            if (iName != null)
            {
                if (iName != "" && System.IO.File.Exists(iName))
                {
                    System.Windows.Forms.PictureBox pb = SplashForm.Controls["pictureBox1"] as System.Windows.Forms.PictureBox;
                    pb.Image = System.Drawing.Image.FromFile(iName);
                }
            }
            SplashForm.ShowDialog();
            SplashForm.Activate();
        }

        public static void KillMe()
        {
            if (SplashForm == null)
            {
                return;
            }

            if (SplashForm.InvokeRequired)
            {
                SplashForm.Invoke(new MethodInvoker(() => KillMe()));
                return;
            }

            if (SplashForm.Created)
            {
                SplashForm.Dispose();
            }
        }

        private static void ShowIMG()
        {
            int i = 1;
            while (true)
            {
                if (SplashForm != null && SplashForm.Created)
                {
                    Thread.Sleep(200);
                    System.Windows.Forms.PictureBox pb = SplashForm.Controls["pictureBox3"] as System.Windows.Forms.PictureBox;
                    switch (i)
                    {
                        case 1:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_1 as System.Drawing.Image;
                                i++;
                                break;
                            }
                        case 2:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_2 as System.Drawing.Image;
                                i++;
                                break;
                            }
                        case 3:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_3 as System.Drawing.Image;
                                i++;
                                break;
                            }
                        case 4:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_4 as System.Drawing.Image;
                                i++;
                                break;
                            }
                        case 5:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_5 as System.Drawing.Image;
                                i++;
                                break;
                            }
                        case 6:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_6 as System.Drawing.Image;
                                i++;
                                break;
                            }
                        case 7:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_7 as System.Drawing.Image;
                                i++;
                                break;
                            }
                        case 8:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_8 as System.Drawing.Image;
                                i++;
                                break;
                            }
                        case 9:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_9 as System.Drawing.Image;
                                i++;
                                break;
                            }
                        case 10:
                            {
                                if (SplashForm != null && SplashForm.Created && SplashForm.IsDisposed)
                                {
                                    return;
                                }
                                if (pb == null)
                                {
                                    return;
                                }
                                pb.Image = dnSP.Properties.Resources.start_10 as System.Drawing.Image;
                                i = 1;
                                break;
                            }

                    }
                }
            }




        }
    }
}

