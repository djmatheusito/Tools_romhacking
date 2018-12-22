using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;

namespace Jacutem_Patch_Tool
{
    public partial class Form2 : Form
    {
        public Form2(string texto, int beep)
        {
            InitializeComponent();
            CenterToScreen();
            Icon = Properties.Resources.aviso;
            if (beep == 1)
            {
                SystemSounds.Beep.Play();
                pictureBox1.Image = new Bitmap(Properties.Resources.jaculogo2);
            }
            else if (beep == 0)
            {
                using (var soundPlayer = new SoundPlayer(Properties.Resources.Um_Momento))
                {
                    soundPlayer.Play();
                }
                pictureBox1.Image = new Bitmap(Properties.Resources.ummomento);
            }
            
            AppendText2(texto);
            


        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void AppendText2(string texto)
        {

            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendText2), new object[] { texto });
                return;
            }

            textBox2.Text += texto;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
