using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Extrator_de_dados
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Directory.Exists(@"Data"))
            {
                Directory.Delete(@"Data", true);
            }

            Directory.CreateDirectory(@"Data");


            // TelasDeTitulo(0x1A807B4, 0x11CA0);
            //  TelasDeTitulo(0x1A77D1C, 0x44AD);
            //  TelasDeTitulo(0x1A7C1CC, 0x3DE8);
            //  DumperGenerico(0x1AB4BD4, 0x1AB4BD8);
             DumperGenerico(0x1ADC8D4, 0x1ADC8D8, "Subtitulos_Comprimidos");
             Descompresao(@"Data\Subtitulos_Comprimidos\");
             GraficosCompSpaleta(@"Data\Subtitulos_Comprimidos\", 0x1AB4BB4);
             DumperGenerico(0x1B04B3C, 0x1B04B40, "Nomes_Prova_Perfil_Comprimidos");
             DumperGenerico(0x1B0E4A8, 0x1B0E4AC, "Nomes_Prova_Perfil_Comprimidos");
             Descompresao(@"Data\Nomes_Prova_Perfil_Comprimidos\");
             GraficosCompSpaleta(@"Data\Nomes_Prova_Perfil_Comprimidos\", 0x1B04B1C);
           
            DumperGenerico(0x1D5A1D4, 0x1D5A1D8, "Texto Prova 1");
            List<string> texp1 = new List<string>(Directory.GetFiles(@"Data\Texto Prova 1\"));
            texp1.RemoveAt(0);
            DescomprimirLZ77(texp1);
            JuntarGraficos(@"Data\Texto Prova 1\");

            DumperGenerico(0x1D5C1DC, 0x1D5C1E0, "Texto Prova 2");
            List<string> texp2 = new List<string>(Directory.GetFiles(@"Data\Texto Prova 2\"));
            texp2.RemoveAt(0);
            DescomprimirLZ77(texp2);
            JuntarGraficos(@"Data\Texto Prova 2\");

            DumperGenerico(0x1D601EC, 0x1D601F0, "Texto Prova 3");
            List<string> texp3 = new List<string>(Directory.GetFiles(@"Data\Texto Prova 3\"));
            texp3.RemoveAt(0);
            DescomprimirLZ77(texp3);
            JuntarGraficos(@"Data\Texto Prova 3\");

            DumperGenerico(0x1DAC62C, 0x1DAC630, "Mapa Estudio");
            List<string> mpstd = new List<string>(Directory.GetFiles(@"Data\Mapa Estudio\"));
            mpstd.RemoveAt(0);
            DescomprimirLZ77(mpstd);
            JuntarGraficos(@"Data\Mapa Estudio\");


            DumperGenerico(0x201BF10, 0x201BF14, "First Turnabout");
            List<string> firstTurnabout = new List<string>(Directory.GetFiles(@"Data\First Turnabout\"));
            firstTurnabout.RemoveAt(0);
            DescomprimirLZ77(firstTurnabout);
            JuntarGraficos(@"Data\First Turnabout\");

            DumperGenerico(0x201DF44, 0x201DF48, "Turnabout Sisters");
            List<string> turnaboutSisters = new List<string>(Directory.GetFiles(@"Data\Turnabout Sisters\"));
            turnaboutSisters.RemoveAt(0);
            DescomprimirLZ77(turnaboutSisters);
            JuntarGraficos(@"Data\Turnabout Sisters\");
            DumperGenerico(0x201FF80, 0x201FF84, "Turnabout Samurai");
            List<string> turnaboutSamurai = new List<string>(Directory.GetFiles(@"Data\Turnabout Samurai\"));
            turnaboutSamurai.RemoveAt(0);
            DescomprimirLZ77(turnaboutSamurai);
            JuntarGraficos(@"Data\Turnabout Samurai\");

            DumperGenerico(0x2021F68, 0x2021F6C, "Turnabout Goodbyes");
            List<string> turnaboutGoodbyes = new List<string>(Directory.GetFiles(@"Data\Turnabout Goodbyes\"));
            turnaboutGoodbyes.RemoveAt(0);
            DescomprimirLZ77(turnaboutGoodbyes);
            JuntarGraficos(@"Data\Turnabout Goodbyes\");


            DumperGenerico(0x2024114, 0x2024118, "Rise From The Ashes");
            List<string> riseFromTheAshes = new List<string>(Directory.GetFiles(@"Data\Rise From The Ashes\"));
            riseFromTheAshes.RemoveAt(0);
            DescomprimirLZ77(riseFromTheAshes);
            JuntarGraficos(@"Data\Rise From The Ashes\");

            DumperGenerico(0x2679FFC, 0x267A000, "Botoes_julgamento");
            List<string> btJulga = new List<string>(Directory.GetFiles(@"Data\Botoes_julgamento\"));
            DescomprimirLZ77(btJulga);

            GraficosDescomprimidos(0x26E0478, 432, 0x8B4, "Botoes_investigacao_local");
            GraficosDescomprimidos(0x29EE4CC, 354, 0x1034, "Subtitulos_Descomprimidos");
            GraficosDescomprimidos(0x2B54CB4, 286, 0x434, "Nomes_Prova_Perfil_Descomprimidos");
            GraficosDescomprimidos(0x2B9FECC, 352, 0x2034, "Discricao_Prova_Perfil");
            GraficosDescomprimidos(0x28C0D5C, 588, 0x834, "Provas");
                        



        }

        static void DumperGenerico(int poTabela, int poInicioPont, string nomePasta)
        {

            Console.WriteLine("\n\n========================{0:x}===============================", poInicioPont);

            using (BinaryReader b = new BinaryReader(File.Open("data.bin", FileMode.Open)))
            {
                b.BaseStream.Seek(poTabela, SeekOrigin.Begin);
                int totalArquivos = b.ReadInt32();

                for (int i = 0; i < totalArquivos; i++)
                {
                    b.BaseStream.Seek(poInicioPont, SeekOrigin.Begin);
                    int ponteiroParcial = b.ReadInt32();
                    int ponteiroFinal = ponteiroParcial + poTabela;
                    int tamanhoArquivo = b.ReadInt32();


                    Console.WriteLine("Ponteiro: {0:x} /// Tamanho do arquivo: {1:x}", ponteiroFinal, tamanhoArquivo);
                    b.BaseStream.Seek(ponteiroFinal, SeekOrigin.Begin);
                    byte[] nomeDoarc = b.ReadBytes(tamanhoArquivo);
                    string myString = nomePasta;
                    Directory.CreateDirectory("Data\\" + myString);
                    File.WriteAllBytes(@"Data\" + myString + @"\Arquivo" + i + ".bin", (nomeDoarc));

                    poInicioPont += 8;
                    ponteiroParcial = 0;
                    ponteiroFinal = 0;
                    tamanhoArquivo = 0;
                }


                b.Close();


            }


        }


        static void TelasDeTitulo(int ponteiroDatela, int tamanhoArquivo)
        {
            Console.WriteLine("\n\n========================Tela de Titulo Inglês===============================");

            using (BinaryReader b = new BinaryReader(File.Open("data.bin", FileMode.Open)))
            {
                b.BaseStream.Seek(ponteiroDatela, SeekOrigin.Begin);
                byte[] nomeDoarc = b.ReadBytes(tamanhoArquivo);
                string myString = ponteiroDatela.ToString();
                Console.WriteLine("Ponteiro: {0:x} /// Tamanho do arquivo: {1:x}", ponteiroDatela, tamanhoArquivo);
                File.WriteAllBytes(@"Data\" + "Tela_de_titulo(" + myString + ").comp", (nomeDoarc));

                b.Close();
            }




        }

        static void GraficosDescomprimidos(int posicaodeinicio, int numerodeArquivos, int tamanhoArquivo, string nomeDaPasta)
        {

            Console.WriteLine("\n\n========================{0}===============================", nomeDaPasta);
            using (BinaryReader b = new BinaryReader(File.Open("data.bin", FileMode.Open)))
            {

                for (int i = 0; i < numerodeArquivos; i++)
                {

                    b.BaseStream.Seek(posicaodeinicio, SeekOrigin.Begin);
                    byte[] nomeDoarc = b.ReadBytes(tamanhoArquivo);
                    Console.WriteLine(nomeDaPasta);
                    string myString = nomeDaPasta;
                    Directory.CreateDirectory("Data\\" + myString);
                    File.WriteAllBytes(@"Data\" + myString + @"\Arquivo" + i + ".bin", (nomeDoarc));
                    posicaodeinicio += tamanhoArquivo;
                }



                b.Close();
            }

        }

        static void Descompresao(string nomeDapasta)
        {
            string[] arquivos = Directory.GetFiles(nomeDapasta);

            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            using (StreamWriter sw = cmd.StandardInput)
            {
                foreach (var arquivo in arquivos)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("lzss.exe -d " + arquivo);
                        Console.Clear();
                        Thread.Sleep(250);
                    }
                }


            }
            Console.Clear();


        }

        static void JuntarGraficos(string nomeDapasta)
        {
            List<string> arquivos = new List<string> (Directory.GetFiles(nomeDapasta));
            File.Create(nomeDapasta+"Grafico.bin").Close();
            int tamanhoSeek = 0;
            foreach (string arquivo in arquivos)
            {
                long tamanhoArq = new FileInfo(arquivo).Length;
                int tamanhoConv = (int)tamanhoArq;
                byte[] dados;
                
                using (BinaryReader b = new BinaryReader(File.Open(arquivo, FileMode.Open)))
                {
                   dados = b.ReadBytes(tamanhoConv);
                   b.Close();
                   File.Delete(arquivo);
                }

                using (BinaryWriter w = new BinaryWriter(File.Open(nomeDapasta + "Grafico.bin", FileMode.Open)))
                {
                    w.BaseStream.Seek(tamanhoSeek, SeekOrigin.Begin);
                    w.Write(dados);
                    w.Close();
                    tamanhoSeek += tamanhoConv;
                }

            }

        }

        static void GraficosCompSpaleta(string nomeDapasta, int posicaoPaleta)
        {
            byte[] paleta;
            byte[] header = new byte[] { 03, 04, 03, 00, 14, 00, 00, 00, 00, 10, 00, 00, 14, 10, 00, 00, 20, 00, 00, 00 };
            string[] arquivos = Directory.GetFiles(nomeDapasta);

            using (BinaryReader b = new BinaryReader(File.Open("data.bin", FileMode.Open)))
            {
                b.BaseStream.Seek(posicaoPaleta, SeekOrigin.Begin);
                paleta = b.ReadBytes(0x20);
                b.Close();
            }

            foreach (string arquivo in arquivos)
            {
                long tamanhoArq = new FileInfo(arquivo).Length;
                int tamanhoArqConv = (int)tamanhoArq;
                byte[] backupArquivo = File.ReadAllBytes(arquivo);
                using (BinaryWriter w = new BinaryWriter(File.Open(arquivo, FileMode.Open)))
                {

                    w.Write(header);
                    w.Write(backupArquivo);
                    w.Write(paleta);
                    w.Close();
                }
            }

        }

        static void DescomprimirLZ77(List<string> arquivos)
        {
            
            foreach (string arquivo in arquivos)
            {
                DescompressorLZ77.Decompress(arquivo);
            }
        }

        
    }
}
