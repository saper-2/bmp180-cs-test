using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace bmp180_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // calibration
            short ac1, ac2, ac3, b1, b2, mb, mc, md;
            ushort ac4, ac5, ac6;
            byte oss;
            // values
            ushort ut;
            uint up;

            ac1 = Convert.ToInt16(eeAC1.Value);
            ac2 = Convert.ToInt16(eeAC2.Value);
            ac3 = Convert.ToInt16(eeAC3.Value);
            ac4 = Convert.ToUInt16(eeAC4.Value);
            ac5 = Convert.ToUInt16(eeAC5.Value);
            ac6 = Convert.ToUInt16(eeAC6.Value);
            b1 = Convert.ToInt16(eeB1.Value);
            b2 = Convert.ToInt16(eeB2.Value);
            mb = Convert.ToInt16(eeMB.Value);
            mc = Convert.ToInt16(eeMC.Value);
            md = Convert.ToInt16(eeMD.Value);

            ut = Convert.ToUInt16(eUT.Value);
            up = Convert.ToUInt32(eUP.Value);
            oss = Convert.ToByte(eOSS.Value);


            int x1, x2, b5, T;
            // calc temperature
            {
                x1 = (ut - ac6) * ac5 >> 15;
                x2 = (mc << 11) / (x1 + md);
                b5 = x1 + x2;
                T = (b5 + 8) >> 4;
                rtX1.Text = x1.ToString();
                rtX2.Text = x2.ToString();
                rtB5.Text = b5.ToString();
                rtT.Text = T.ToString();
            }
            // calc pressure
            int b6, x3, b3, p;
            uint b4, b7;
            {
                // 1st round :)
                b6 = b5 - 4000;
                x1 = (b2 * ((b6 * b6) >> 12)) >> 11;
                x2 = (ac2 * b6) >> 11;
                x3 = x1 + x2;
                b3 = (((ac1 * 4 + x3) << oss) + 2) >> 2;
                // result of 1st round
                rpB6.Text = b6.ToString();
                rpX1_1.Text = x1.ToString();
                rpX2_1.Text = x2.ToString();
                rpX3_1.Text = x3.ToString();
                rpB3.Text = b3.ToString();
                // 2nd round
                x1 = (ac3 * b6) >> 13;
                x2 = (b1 * ((b6 * b6) >> 12)) >> 16;
                x3 = ((x1 + x2) + 2) >> 2;
                b4 = (ac4 * (UInt32)(x3 + 32768)) >> 15;
                b7 = (uint)((uint)up - b3) * (uint)(50000 >> oss);
                // results of 2nd round
                rpX1_2.Text = x1.ToString();
                rpX2_2.Text = x2.ToString();
                rpX3_2.Text = x3.ToString();
                rpB4.Text = b4.ToString();
                rpB7.Text = b7.ToString();
                // 3rd round
                if (b7 < 0x80000000)
                {
                    ifB7.Checked = true;
                    p = (int)((b7 * 2) / b4);
                }
                else
                {
                    ifB7.Checked = false;
                    p = (int)((b7 / b4) * 2);
                }
                // results of 3rd round
                rpP1.Text = p.ToString();
                // 4th round
                x1 = (p >> 8) * (p >> 8);
                // results of 4th round
                rpX1_3.Text = x1.ToString();
                // 5th round
                x1 = (x1 * 3038) >> 16;
                x2 = (-7357 * p) >> 16;
                p = p + ((x1 + x2 + 3791) >> 4);
                // results of 5th round
                rpX1_4.Text = x1.ToString();
                rpX2_3.Text = x2.ToString();
                rpP.Text = p.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            eeAC1.Value = 408;
            eeAC2.Value = -72;
            eeAC3.Value = -14383;
            eeAC4.Value = 32741;
            eeAC5.Value = 32757;
            eeAC6.Value = 23153;
            eeB1.Value = 6190;
            eeB2.Value = 4;
            eeMB.Value = -32768;
            eeMC.Value = -8711;
            eeMD.Value = 2868;
            eUP.Value = 23843;
            eUT.Value = 27898;
            eOSS.Value = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamReader r = new StreamReader("calib.txt");
            int lcnt = 1;
            string line;
            while ((line = r.ReadLine()) != null)
            {
                int val = 0;
                try
                {
                    val = int.Parse(line.Trim());
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Error in line: " + lcnt.ToString() + Environment.NewLine + "Error: " + ee.Message, "Number convert error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                switch (lcnt)
                {
                    case 1: eeAC1.Value = val; break;
                    case 2: eeAC2.Value = val; break;
                    case 3: eeAC3.Value = val; break;
                    case 4: eeAC4.Value = val; break;
                    case 5: eeAC5.Value = val; break;
                    case 6: eeAC6.Value = val; break;
                    case 7: eeB1.Value = val; break;
                    case 8: eeB2.Value = val; break;
                    case 9: eeMB.Value = val; break;
                    case 10: eeMC.Value = val; break;
                    case 11: eeMD.Value = val; break;
                }
                lcnt++;
            }
            r.Close();
            r.Dispose();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/saper-2");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://promikro.com.pl/");
        }
    }
}
