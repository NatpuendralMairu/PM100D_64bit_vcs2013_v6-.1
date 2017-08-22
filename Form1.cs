using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DotNetSample_64
{
    public partial class Form1 : Form
    {
        private Thorlabs.PM100D.PM100D pm100d;
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        double powerValue;
        double energyvalue;

        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = comboBox1.FindStringExact("Watt");
            comboBox_wavalength.SelectedIndex = comboBox_wavalength.FindStringExact("780");

            try
            {
                pm100d = new Thorlabs.PM100D.PM100D("USB0::0x1313::0x8075::P5000256::INSTR", true, true);  //  For valid Ressource_Name see NI-Visa documentation.

            }

            catch (BadImageFormatException bie)
            {
                labelPower.Text = bie.Message;
            }
            catch (NullReferenceException nre)
            {
                labelPower.Text = nre.Message;
            }
            catch (ExternalException ex)
            {
                labelPower.Text = ex.Message;
            }
            finally
            {
                if (pm100d != null)
                    pm100d.Dispose();
            }
        }



        public void Timer1_Tick(object sender, EventArgs e)
        {

            label2.Text = DateTime.Now.ToString();

            List<string> strdata = new List<string>();


            switch (comboBox1.SelectedIndex)
            {
                case 0:

                    pm100d.measEnergy(out energyvalue);
                    labelEnergy.Text = energyvalue.ToString();

                    break;

                case 1:
                    pm100d.measPower(out powerValue);
                    labelPower.Text = powerValue.ToString();
                    break;
            }


            strdata.Add(label2.Text);
            strdata.Add(labelPower.Text);
            textBox1.Text += strdata[0] + "," + strdata[1] + "\r\n";


            if (checkBox1.Checked)
            {

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\prajwal.nimmagadda\Desktop\Thorlabs\ThorlabsData\PowerArchive.xlsx", true))
                {
                    file.WriteLine(strdata[0] + "," + strdata[1] + "\n");
                }
            }
            else
            {
            }
        }

        private void comboBox_wavalength_SelectedIndexChanged(object sender, EventArgs e)
        {
            pm100d = new Thorlabs.PM100D.PM100D("USB0::0x1313::0x8075::P5000256::INSTR", false, false);  //  For valid Ressource_Name see NI-Visa documentation.

            double wavelength0 = 780;
            double wavelength1 = 980;
            double wavelength2 = 1064;

            switch (comboBox_wavalength.SelectedIndex)

            {
                case 0:
                    pm100d.setWavelength(wavelength0);
                    break;

                case 1:
                    pm100d.setWavelength(wavelength1);
                    break;

                case 2:
                    pm100d.setWavelength(wavelength2);
                    break;
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                pm100d = new Thorlabs.PM100D.PM100D("USB0::0x1313::0x8075::P5000256::INSTR", false, false);  //  For valid Ressource_Name see NI-Visa documentation.
                if (button1.Text == "STOP LOGGING")
                {
                    button1.Text = "START LOGGING";
                    timer1.Enabled = false;
                }
                else
                {
                    button1.Text = "STOP LOGGING";
                    timer1.Enabled = true;
                }
            }
            catch(ExternalException exz)
            {
                textBox2.Text = exz.Message;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Clear Buffer
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            short bb;

            pm100d.readRegister(20, out bb);

            label_status.Text = bb.ToString();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            System.IO.File.WriteAllText(@"C:\Users\prajwal.nimmagadda\Desktop\Thorlabs\ThorlabsData\PowerLog.txt", text);

        }

    }
}


