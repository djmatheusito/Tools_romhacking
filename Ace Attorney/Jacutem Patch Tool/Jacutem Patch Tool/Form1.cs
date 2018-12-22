using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Media;
using System.Drawing;


namespace Jacutem_Patch_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           if (Directory.Exists("AAT"))
            {
                Directory.Delete("AAT", true);
            }
            
            

            CenterToScreen();
            Icon = Properties.Resources.jacu;
            BackgroundImage = Properties.Resources.fundo;
            pictureBox1.Image = new Bitmap(Properties.Resources.BoxArt3DS);
            pictureBox2.Image = new Bitmap(Properties.Resources.jaculogo);
            button1.BackColor = ColorTranslator.FromHtml("#FFED01");
            button2.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#059847");
            button2.BackColor = ColorTranslator.FromHtml("#059847");
         //   Directory.CreateDirectory("ROM_Original");
        //Directory.CreateDirectory("Nova_Rom");

            
            



            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //BackColor = System.Drawing.Color.Transparent;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] nomeArquivo = Directory.GetFiles("ROM_Original", "*.cia*");

            

            string nomeFinal = nomeArquivo[0];
            File.Move(nomeFinal, "Rom_Original\\AAT.cia");
            nomeFinal = "Rom_Original\\AAT.cia";

            VerificarEncriptacao(nomeFinal, 0);

            
            

        }


        public void Insercao(string jogo , int cxi, int encript)
        {
            int titleID = 0;

            textBox1.Clear();
         /*   Directory.CreateDirectory("Tools");
            byte[] ctrtool = Properties.Resources.ctrtool_2;
            File.Create("Tools\\ctrtool.exe").Close();
            File.WriteAllBytes("Tools\\ctrtool.exe", ctrtool);

            byte[] _3dstool = Properties.Resources._3dstool;
            File.Create("Tools\\3dstool.exe").Close();
            File.WriteAllBytes("Tools\\3dstool.exe", _3dstool);

            byte[] makerom = Properties.Resources.makerom;
            File.Create("Tools\\makerom.exe").Close();
            File.WriteAllBytes("Tools\\makerom.exe", makerom);

            byte[] xdelta = Properties.Resources.xdelta;
            File.Create("Tools\\xdelta.exe").Close();
            File.WriteAllBytes("Tools\\xdelta.exe", xdelta);*/

           AppendText("Extraindo...");
              String[] jogos = Directory.GetFiles("ROM_Original");
           

            Directory.CreateDirectory("AAT");
               //File.Move(jogo, jogo.Replace(" ", "_"));

               Process process = new Process();

               ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c Tools\\ctrtool.exe --contents=AAT\\contents " + jogos[0]);
               process.StartInfo = processInfo;
               process.StartInfo.UseShellExecute = false;
               process.StartInfo.CreateNoWindow = true;
           
            process.Start();
            
            process.WaitForExit();
            



            Process process31 = new Process();

                ProcessStartInfo process31Info = new ProcessStartInfo("cmd.exe", "/c Tools\\ctrtool.exe --exheader=AAT\\exheader.bin AAT\\contents.0000.00000000");
                process31.StartInfo = process31Info;
                process31.StartInfo.UseShellExecute = false;
                process31.StartInfo.CreateNoWindow = true;
                process31.Start();
                process31.WaitForExit();

            

            /* Process process666 = new Process();

             ProcessStartInfo process666Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool -xvtf cxi AAT\\contents.0000.00000000 --header ncchheader.bin --exh exh.bin --logo logo.bcma.lz --plain plain.bin --exefs exefs.bin --romfs romfs.bin --key 00000000000000000000000000000000");
             process666.StartInfo = process666Info;
             process666.StartInfo.UseShellExecute = false;
             process666.StartInfo.CreateNoWindow = false;
             process666.Start();
             process666.WaitForExit();
             */
            using (BinaryReader b = new BinaryReader(File.Open("AAT\\exheader.bin", FileMode.Open)))
                {
                    b.BaseStream.Seek(0x200, SeekOrigin.Begin);
                    titleID = b.ReadInt32();
                    b.Close();
                }

                if (titleID != 0x138F00)
                {
                    
                    Form2 fm2 = new Form2("Este .cia não é americano(USA)!\nÉ necessário que ele tenha o title ID: 0004000000138F00", 0);
                    fm2.ShowDialog();
                    textBox1.Text = string.Empty;
                    Directory.Delete("AAT", true);
                  //  Directory.Delete("Tools", true);
                    return;
                }
                Process process2 = new Process();

                ProcessStartInfo process2Info = new ProcessStartInfo("cmd.exe", "/c Tools\\ctrtool.exe --romfsdir=AAT\\romfs AAT\\contents.0000.00000000");
                process2.StartInfo = process2Info;
                process2.StartInfo.UseShellExecute = false;
                process2.StartInfo.CreateNoWindow = true;
                process2.Start();
                process2.WaitForExit();

            

            Process process3 = new Process();

                ProcessStartInfo process3Info = new ProcessStartInfo("cmd.exe", "/c Tools\\ctrtool.exe --exefsdir=AAT\\exefs --exefs=AAT\\exefs.bin --logo=AAT\\logo.bcma.lz --plainrgn=AAT\\plain.bin AAT\\contents.0000.00000000");
                process3.StartInfo = process3Info;
                process3.StartInfo.UseShellExecute = false;
                process3.StartInfo.CreateNoWindow = true;
                process3.Start();
                process3.WaitForExit();

           



            Process process4 = new Process();

                ProcessStartInfo process4Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe -xvtf exefs AAT\\exefs.bin --exefs-dir AAT\\exefs3DS --header AAT\\exefsheader.bin");
                process4.StartInfo = process4Info;
                process4.StartInfo.UseShellExecute = false;
                process4.StartInfo.CreateNoWindow = true;
                process4.Start();
                process4.WaitForExit();

                byte[] copia = File.ReadAllBytes("AAT\\exefs\\code.bin");
                File.Create("AAT\\exefs3DS\\code.bin").Close();
                File.WriteAllBytes("AAT\\exefs3DS\\code.bin", copia);
           

            Process process5 = new Process();

                ProcessStartInfo process5Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe -xvtf cxi AAT\\contents.0000.00000000 --header AAT\\ncchheader0.bin");
                process5.StartInfo = process5Info;
                process5.StartInfo.UseShellExecute = false;
                process5.StartInfo.CreateNoWindow = true;
                process5.Start();
                process5.WaitForExit();

           
            Process process6 = new Process();

                ProcessStartInfo process6Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe -xvtf cfa AAT\\contents.0001.00000001 --header AAT\\ncchheader1.bin");
                process6.StartInfo = process6Info;
                process6.StartInfo.UseShellExecute = false;
                process6.StartInfo.CreateNoWindow = true;
                process6.Start();
                process6.WaitForExit();

            
            Process process7 = new Process();

                ProcessStartInfo process7Info = new ProcessStartInfo("cmd.exe", "/c Tools\\ctrtool.exe --romfsdir=AAT\\romfs1 AAT\\contents.0001.00000001");
                process7.StartInfo = process7Info;
                process7.StartInfo.UseShellExecute = false;
                process7.StartInfo.CreateNoWindow = true;
                process7.Start();
                process7.WaitForExit();

            
            XdeltaP(cxi, encript);
            



        }

        public void XdeltaP(int cxi , int encript)
        {
            /*  Directory.CreateDirectory("Patches");

              byte[] code = Properties.Resources.code_bin;
              File.Create("Patches\\code.bin.xdelta").Close();
              File.WriteAllBytes("Patches\\code.bin.xdelta", code);

              byte[] GS123B = Properties.Resources.GS123_bcsar;
              File.Create("Patches\\GS123.bcsar.xdelta").Close();
              File.WriteAllBytes("Patches\\GS123.bcsar.xdelta", GS123B);

              byte[] GS123X = Properties.Resources.GS123_xml;
              File.Create("Patches\\GS123.xml.xdelta").Close();
              File.WriteAllBytes("Patches\\GS123.xml.xdelta", GS123X);

              byte[] packdat = Properties.Resources.pack_dat;
              File.Create("Patches\\pack.dat.xdelta").Close();
              File.WriteAllBytes("Patches\\pack.dat.xdelta", packdat);

              byte[] packinc = Properties.Resources.pack_inc;
              File.Create("Patches\\pack.inc.xdelta").Close();
              File.WriteAllBytes("Patches\\pack.inc.xdelta", packinc);

      */

            AppendText("\r\nAplicando Patches...");

            Process process456 = new Process();
            ProcessStartInfo process456Info = new ProcessStartInfo("cmd.exe", "/c Tools\\xdelta.exe -d -s AAT\\romfs1\\Manual.bcma Patches\\Manual.bcma.xdelta  AAT\\romfs1\\Manual.bcma.new");
            process456.StartInfo = process456Info;
            process456.StartInfo.UseShellExecute = false;
            process456.StartInfo.CreateNoWindow = true;
            process456.Start();
            process456.WaitForExit();

            File.Delete("AAT\\romfs1\\Manual.bcma");
            File.Move("AAT\\romfs1\\Manual.bcma.new", "AAT\\romfs1\\Manual.bcma");

            Process process123 = new Process();
            ProcessStartInfo process123Info = new ProcessStartInfo("cmd.exe", "/c Tools\\xdelta.exe -d -s AAT\\exefs3DS\\banner.bnr Patches\\banner.bnr.xdelta AAT\\exefs3DS\\banner.bnr.new");
            process123.StartInfo = process123Info;
            process123.StartInfo.UseShellExecute = false;
            process123.StartInfo.CreateNoWindow = true;
            process123.Start();
            process123.WaitForExit();

            File.Delete("AAT\\exefs3DS\\banner.bnr");
            File.Move("AAT\\exefs3DS\\banner.bnr.new", "AAT\\exefs3DS\\banner.bnr");

            Process process6 = new Process();
            ProcessStartInfo process6Info = new ProcessStartInfo("cmd.exe", "/c Tools\\xdelta.exe -d -s AAT\\logo.bcma.lz Patches\\logo.bcma.lz.xdelta  AAT\\logo.bcma.lz.new");
            process6.StartInfo = process6Info;
            process6.StartInfo.UseShellExecute = false;
            process6.StartInfo.CreateNoWindow = true;
            process6.Start();
            process6.WaitForExit();

            File.Delete("AAT\\logo.bcma.lz");
            File.Move("AAT\\logo.bcma.lz.new", "AAT\\logo.bcma.lz");

            

            Process process = new Process();
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c Tools\\xdelta.exe -d -s AAT\\exefs3DS\\code.bin Patches\\code.bin.xdelta AAT\\exefs3DS\\coden.bin");
            process.StartInfo = processInfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();

            File.Delete("AAT\\exefs3DS\\code.bin");
            File.Move("AAT\\exefs3DS\\coden.bin", "AAT\\exefs3DS\\code.bin");
           

            Process process1 = new Process();
            ProcessStartInfo process1Info = new ProcessStartInfo("cmd.exe", "/c Tools\\xdelta.exe -d -s AAT\\romfs\\pack.dat Patches\\pack.dat.xdelta  AAT\\romfs\\pack.dat.new");
            process1.StartInfo = process1Info;
            process1.StartInfo.UseShellExecute = false;
            process1.StartInfo.CreateNoWindow = true;
            process1.Start();
            process1.WaitForExit();

            File.Delete("AAT\\romfs\\pack.dat");
            File.Move("AAT\\romfs\\pack.dat.new", "AAT\\romfs\\pack.dat");
           

            Process process2 = new Process();
            ProcessStartInfo process2Info = new ProcessStartInfo("cmd.exe", "/c Tools\\xdelta.exe -d -s AAT\\romfs\\pack.inc Patches\\pack.inc.xdelta  AAT\\romfs\\pack.inc.new");
            process2.StartInfo = process2Info;
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.Start();
            process2.WaitForExit();

            File.Delete("AAT\\romfs\\pack.inc");
            File.Move("AAT\\romfs\\pack.inc.new", "AAT\\romfs\\pack.inc");
           


            Process process3 = new Process();
            ProcessStartInfo process3Info = new ProcessStartInfo("cmd.exe", "/c Tools\\xdelta.exe -d -s AAT\\romfs\\sound\\GS123.bcsar Patches\\GS123.bcsar.xdelta  AAT\\romfs\\sound\\GS123.bcsar.new");
            process3.StartInfo = process3Info;
            process3.StartInfo.UseShellExecute = false;
            process3.StartInfo.CreateNoWindow = true;
            process3.Start();
            process3.WaitForExit();

            File.Delete("AAT\\romfs\\sound\\GS123.bcsar");
            File.Move("AAT\\romfs\\sound\\GS123.bcsar.new", "AAT\\romfs\\sound\\GS123.bcsar");
          

            Process process4 = new Process();
            ProcessStartInfo process4Info = new ProcessStartInfo("cmd.exe", "/c Tools\\xdelta.exe -d -s AAT\\romfs\\sound\\GS123.xml Patches\\GS123.xml.xdelta  AAT\\romfs\\sound\\GS123.xml.new");
            process4.StartInfo = process4Info;
            process4.StartInfo.UseShellExecute = false;
            process4.StartInfo.CreateNoWindow = true;
            process4.Start();
            process4.WaitForExit();

            File.Delete("AAT\\romfs\\sound\\GS123.xml");
            File.Move("AAT\\romfs\\sound\\GS123.xml.new", "AAT\\romfs\\sound\\GS123.xml");
           
             
            /*
             //Directory.Delete("Patches", true);
             */


            Rebuild(cxi,encript);

        }

        public void Rebuild(int cxi, int encript)
        {
            
            AppendText("\r\nCriando Rom Traduzida...");
            Process process7 = new Process();

            ProcessStartInfo process7Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe  -czvtf exefs AAT\\exefs.bin.new --header AAT\\exefsheader.bin --exefs-dir AAT\\exefs3DS");
            process7.StartInfo = process7Info;
            process7.StartInfo.UseShellExecute = false;
            process7.StartInfo.CreateNoWindow = true;
            process7.Start();
            process7.WaitForExit();
           
            File.Delete("AAT\\exefs.bin");
            File.Move("AAT\\exefs.bin.new", "AAT\\exefs.bin");


            Process process8 = new Process();

            ProcessStartInfo process8Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe  -cvtf romfs AAT\\romfs.bin --romfs-dir AAT\\romfs/");
            process8.StartInfo = process8Info;
            process8.StartInfo.UseShellExecute = false;
            process8.StartInfo.CreateNoWindow = true;
            process8.Start();
            process8.WaitForExit();
            


            if (encript == 1)
            {
                if (cxi == 0)
                {
                    Process process9 = new Process();

                    ProcessStartInfo process9Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe  -cvtf cxi AAT\\contents.0000.00000000.new --header AAT\\ncchheader0.bin --exh AAT\\exheader.bin --exh-auto-key --logo AAT\\logo.bcma.lz --plain AAT\\plain.bin --exefs AAT\\exefs.bin --exefs-auto-key --exefs-top-auto-key --romfs AAT\\romfs.bin --romfs-auto-key");
                    process9.StartInfo = process9Info;
                    process9.StartInfo.UseShellExecute = false;
                    process9.StartInfo.CreateNoWindow = true;
                    process9.Start();
                    process9.WaitForExit();
                    
                }
                else
                {
                    Process process9 = new Process();

                    ProcessStartInfo process9Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe  -cvtf cxi AAT\\contents.0000.00000000.new --header AAT\\ncchheader0.bin --exh AAT\\exheader.bin --logo AAT\\logo.bcma.lz --plain AAT\\plain.bin --exefs AAT\\exefs.bin --romfs AAT\\romfs.bin");
                    process9.StartInfo = process9Info;
                    process9.StartInfo.UseShellExecute = false;
                    process9.StartInfo.CreateNoWindow = true;
                    process9.Start();
                    process9.WaitForExit();
                    
                }
                
            }
            else {

                Process process9 = new Process();

                ProcessStartInfo process9Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe  -cvtf cxi AAT\\contents.0000.00000000.new --header AAT\\ncchheader0.bin --exh AAT\\exheader.bin --logo AAT\\logo.bcma.lz --plain AAT\\plain.bin --exefs AAT\\exefs.bin --romfs AAT\\romfs.bin");
                process9.StartInfo = process9Info;
                process9.StartInfo.UseShellExecute = false;
                process9.StartInfo.CreateNoWindow = true;
                process9.Start();
                process9.WaitForExit();
                
            }

            File.Delete("AAT\\contents.0000.00000000");
            File.Move("AAT\\contents.0000.00000000.new", "AAT\\contents.0000.00000000");


            Process process10 = new Process();

            ProcessStartInfo process10Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe  -cvtf romfs AAT\\romfs1.bin --romfs-dir AAT\\romfs1/");
            process10.StartInfo = process10Info;
            process10.StartInfo.UseShellExecute = false;
            process10.StartInfo.CreateNoWindow = true;
            process10.Start();
            process10.WaitForExit();
           

            Process process11 = new Process();

            ProcessStartInfo process11Info = new ProcessStartInfo("cmd.exe", "/c Tools\\3dstool.exe  -cvtf cfa AAT\\contents.0001.00000001.new --header AAT\\ncchheader1.bin --romfs AAT\\romfs1.bin --romfs-auto-key");
            process11.StartInfo = process11Info;
            process11.StartInfo.UseShellExecute = false;
            process11.StartInfo.CreateNoWindow = true;
            process11.Start();
            process11.WaitForExit();
           
            File.Delete("AAT\\contents.0001.00000001");
            File.Move("AAT\\contents.0001.00000001.new", "AAT\\contents.0001.00000001");

            

            if (cxi == 0)
            {
                Process process12 = new Process();
                ProcessStartInfo process12Info = new ProcessStartInfo("cmd.exe", "/c Tools\\makerom.exe -target p -ignoresign -f cia -o Nova_Rom\\Advogados_de_Primeira.cia -content AAT\\contents.0000.00000000:0:0 -content AAT\\contents.0001.00000001:1:1");
                process12.StartInfo = process12Info;
                process12.StartInfo.UseShellExecute = false;
                process12.StartInfo.CreateNoWindow = true;
                process12.Start();
                process12.WaitForExit();
               
                // Directory.Delete("AAT", true);
                //Directory.Delete("Tools", true);
                AppendText("\r\n.Cia criado com sucesso!");
                Directory.Delete("AAT", true);


            }
            else if (cxi == 1)
            {
                File.Move("AAT\\contents.0000.00000000", "Nova_ROM\\Advogados_de_Primeira.cxi");
                Directory.Delete("AAT", true);
                Directory.Delete("Tools", true);
                AppendText("\r\n.Cxi criado com sucesso!");
            }

            

            

        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public void AppendText(string texto)
        {

            if (InvokeRequired) {
                Invoke(new Action<string>(AppendText), new object[] { texto });
                return;
            }

            textBox1.Text += texto;

        }

       public void VerificarEncriptacao (string jogo, int cxi){

            long encryptCheck = 0;
            
            using (BinaryReader b = new BinaryReader(File.Open(jogo, FileMode.Open)))
            {
                b.BaseStream.Seek(0x3A40, SeekOrigin.Begin);
                encryptCheck = b.ReadInt32();
                b.Close();
            }

            if (encryptCheck != 0x4843434E)
            {

                // MessageBox.Show("Este .cia está encriptado!\nÉ necessário um .cia decriptado.");
                Insercao(jogo, cxi, 1);
            }
            else
                Insercao(jogo, cxi, 0);



        }
        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

       

      

        private void button2_Click(object sender, EventArgs e)
        {
            string[] nomeArquivo = Directory.GetFiles("ROM_Original", "*.cia*");



            string nomeFinal = nomeArquivo[0];
            File.Move(nomeFinal, "Rom_Original\\AAT.cia");
            nomeFinal = "Rom_Original\\AAT.cia";

            VerificarEncriptacao(nomeFinal, 1);

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        


        private void informaçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2("Jacutem Patch Tool V1.1 2018 - by djmatheusito\r\nAcesse o site https://jacutemsabao.bitbucket.io/ para mais informações.", 1);
            fm2.ShowDialog();
            // MessageBox.Show("Jacutem Patch Tool V1.0\nAcesse o site https://jacutemsabao.bitbucket.io/ para\nmais informações.");
            // return;
        }

        private void comoUtlizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2("1- Copie o .cia americano para a pasta \"Rom_original\".\r\n2- Inicie o programa.\r\n3- A ROM PT-BR será criada na pasta \"Nova_Rom.", 1);
            fm2.ShowDialog();
        }

        
    }
}
