using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace AADI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ace Attorney Data.bin Inserter v1.5 by djmatheusito - Jacutemsabão 2018\nColoque o arm9.bin e data.bin da ROM PT-BR na mesma pasta do programa.\n\nModos de inserção(Digite barra + o número do modo (ex: -2) e aperte enter:\n" +
                "-1 Cenarios, textos, nomes de caso fundo preto, samurai de aço, mapas etc...\n-2 Botões maiores, subtitulos/nomes de prova tela de cima.\n" +
                "-3 Botões menores, nomes de provas/subtitulos tela de baixo, descrições.");
            string modo = Console.ReadLine();

            if (File.Exists("arm9.bin"))
            {
                if (File.Exists("data.bin"))
                {
                    if (modo == "-1")
                    {
                        string lista = File.ReadAllText("Lista_de_insercao\\Cenarios_e_outros.txt");
                        List<string> comandos = new List<string>(lista.Split('~'));
                        comandos.RemoveAt(comandos.Count - 1);

                        foreach (string comando in comandos)
                        {
                            string[] graficos = comando.Replace("\r", string.Empty).Replace("\n", string.Empty).Split(';');

                            string[] fatiarComando1 = graficos[0].Split(',');
                            int tamanhoPaleta = Convert.ToInt32(fatiarComando1[0], 16);
                            int numeroPartes = Convert.ToInt32(fatiarComando1[1], 16);
                            FatiarGraficos(tamanhoPaleta, numeroPartes, fatiarComando1[2]);

                            string[] fatiarComando2 = graficos[1].Split(',');
                            int temPaleta = Convert.ToInt32(fatiarComando2[1], 16);
                            CompressaoLZ77(fatiarComando2[0], temPaleta);

                            string[] fatiarComando3 = graficos[2].Split(',');
                            int indexTab = Convert.ToInt32(fatiarComando3[0], 16);
                            int atualizarArm9 = Convert.ToInt32(fatiarComando3[3], 16);
                            int indexArm9 = Convert.ToInt32(fatiarComando3[4], 16);
                            int temTamanho = Convert.ToInt32(fatiarComando3[6], 16);
                            InserirComPonteiros(indexTab, fatiarComando3[1], fatiarComando3[2], atualizarArm9, indexArm9, indexTab, temTamanho);


                        }

                    }

                    else if (modo == "-2")
                    {
                        string lista = File.ReadAllText("Lista_de_insercao\\Comprimidos.txt");
                        List<string> comandos = new List<string>(lista.Split('~'));
                        comandos.RemoveAt(comandos.Count - 1);

                        foreach (string comando in comandos)
                        {
                            string[] graficos = comando.Replace("\r", string.Empty).Replace("\n", string.Empty).Split(';');

                            string[] fatiarComando1 = graficos[0].Split(',');
                            int temPaleta = Convert.ToInt32(fatiarComando1[1], 16);
                            CompressaoLZ77(fatiarComando1[0], temPaleta);

                            string[] fatiarComando2 = graficos[1].Split(',');
                            int indexTab = Convert.ToInt32(fatiarComando2[0], 16);
                            int atualizarArm9 = Convert.ToInt32(fatiarComando2[3], 16);
                            int indexArm9 = Convert.ToInt32(fatiarComando2[4], 16);
                            int temTamanho = Convert.ToInt32(fatiarComando2[6], 16);
                            InserirComPonteiros(indexTab, fatiarComando2[1], fatiarComando2[2], atualizarArm9, indexArm9, indexTab, temTamanho);
                        }

                    }
                    else if (modo == "-3")
                    {
                        string lista = File.ReadAllText("Lista_de_insercao\\Descomprimidos.txt");
                        List<string> comandos = new List<string>(lista.Split('~'));
                        comandos.RemoveAt(comandos.Count - 1);

                        foreach (string comando in comandos)
                        {

                            string[] fatiarComando1 = comando.Replace("\r", string.Empty).Replace("\n", string.Empty).Split(',');
                            int ofsetGrafico = Convert.ToInt32(fatiarComando1[1], 16);
                            int tamanhoGrafico = Convert.ToInt32(fatiarComando1[2], 16);
                            InserirDescomprimidos(fatiarComando1[0], ofsetGrafico, tamanhoGrafico);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("arm9.bin ou data.bin não encontrados, aperte qualquer tecla para sair do programa!");
                Console.ReadKey();
            }

            Console.WriteLine("Gráficos inseridos com sucesso!");

        }

        static void InserirDescomprimidos(string nomeDapasta, int ofsetGrafico, int tamanhoGrafico)
        {
            Console.WriteLine(">Inserindo... {0}\n", nomeDapasta);
            string lista = File.ReadAllText(nomeDapasta + "\\" + "Lista\\Lista_de_arquivos.txt");
            string cortalista = lista.Replace("\r\n", "~");
            List<string> arquivos = new List<string>(cortalista.Split('~'));
            arquivos.RemoveAt(arquivos.Count - 1);
            byte[] bArquivo;
            foreach (string arquivo in arquivos)
            {

                bArquivo = File.ReadAllBytes(arquivo);


                using (BinaryWriter w = new BinaryWriter(File.Open("data.bin", FileMode.Open)))
                {
                    w.BaseStream.Seek(ofsetGrafico, SeekOrigin.Begin);
                    w.Write(bArquivo);
                    ofsetGrafico += tamanhoGrafico;
                    w.Close();
                }


            }
        }

        static void CompressaoLZ77(string nomeDapasta, int temPaleta)
        {
            Directory.CreateDirectory(nomeDapasta + "\\lz77");
            string[] arquivos = Directory.GetFiles(nomeDapasta);

            Console.WriteLine(">Comprimindo gráficos... {0}", nomeDapasta);



            int contador = 0;


                if (temPaleta == 1)
                {
                    contador += 1;
                    byte[] paleta = File.ReadAllBytes(nomeDapasta + "\\Arquivo0.bin");
                    File.Create(nomeDapasta + "\\lz77" + "\\Arquivo0.bin").Close();
                    File.WriteAllBytes(nomeDapasta + "\\lz77" + "\\Arquivo0.bin", paleta);

                }

                while (contador < arquivos.Length)
                {
                    string[] nomeArq = arquivos[contador].Replace("\\", ">").Split('>');
                   
                        
                        Process process = new Process();
                        
                        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c GBACrusherCL.exe -L " + arquivos[contador] + " -O " + nomeDapasta + "\\lz77\\" + nomeArq[2]);
                        process.StartInfo = processInfo;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                        process.WaitForExit();



                    contador++;
                }


            if (temPaleta == 1)
            {
                File.Delete(nomeDapasta + "\\lz77\\Grafico.bin");
            }


        }

        static void FatiarGraficos(int tamanhoDaPaleta, int numeroDepartes, string nomeDaPasta)
        {
            Console.WriteLine(">Dividindo gráficos... {0}", nomeDaPasta);
            long tamanhoArq = new FileInfo(nomeDaPasta + "\\Grafico.bin").Length;
            int tamanhoArquivo = ((int)tamanhoArq - tamanhoDaPaleta) / 6;

            using (BinaryReader b = new BinaryReader(File.Open(nomeDaPasta + "\\Grafico.bin", FileMode.Open)))
            {
                byte[] paleta = b.ReadBytes(tamanhoDaPaleta);
                File.Create(nomeDaPasta + "\\Arquivo0.bin").Close();
                File.WriteAllBytes(nomeDaPasta + "\\Arquivo0.bin", paleta);
                b.Close();
            }



            using (BinaryReader b = new BinaryReader(File.Open(nomeDaPasta + "\\Grafico.bin", FileMode.Open)))
            {
                b.BaseStream.Seek(tamanhoDaPaleta, SeekOrigin.Begin);
                for (int i = 0; i < numeroDepartes; i++)
                {
                    byte[] arq = b.ReadBytes(tamanhoArquivo);
                    int numArq = i + 1;
                    File.Create(nomeDaPasta + "\\Arquivo" + numArq + ".bin").Close();
                    File.WriteAllBytes(nomeDaPasta + "\\Arquivo" + numArq + ".bin", arq);

                }


            }


        }



        static void InserirComPonteiros(int inderecoTab, string nomeDapasta, string novaPasta, int atualizarArm9, int endArm9, int offsetAt, int temTama)
        {
            Console.WriteLine(">Inserindo... {0}", nomeDapasta);
            string lista = File.ReadAllText(nomeDapasta + "\\" + "Lista\\Lista_de_arquivos.txt");
            string cortalista = lista.Replace("\r\n", "~");
            List<string> arquivos = new List<string>(cortalista.Split('~'));
            arquivos.RemoveAt(arquivos.Count - 1);
            int numeroArqs = arquivos.Count;
            int seek = numeroArqs * 8 + inderecoTab + 4;
            int index = inderecoTab;
            int tamanhoDetudo = seek - index;

            using (BinaryWriter w = new BinaryWriter(File.Open("data.bin", FileMode.Open)))
            {
                w.BaseStream.Seek(inderecoTab, SeekOrigin.Begin);
                w.Write(numeroArqs);
                inderecoTab += 4;
                w.Close();

            }

            byte[] bArquivo;
            foreach (string arquivo in arquivos)
            {

                string[] corte = arquivo.Split('\\');
                string certo = corte[0] + "\\" + corte[1] + "\\" + novaPasta + "\\" + corte[2];

                Thread.Sleep(150);
                bArquivo = File.ReadAllBytes(certo);
                long tamanhoArquivo = bArquivo.Length;
                int tamanhoConvertido = (int)tamanhoArquivo;
                tamanhoDetudo += tamanhoConvertido;


                using (BinaryWriter w = new BinaryWriter(File.Open("data.bin", FileMode.Open)))
                {
                    w.BaseStream.Seek(inderecoTab, SeekOrigin.Begin);
                    int ponteiro = seek - index;
                    w.Write(ponteiro);
                    w.Write(tamanhoConvertido);
                    inderecoTab += 8;
                    w.BaseStream.Seek(seek, SeekOrigin.Begin);
                    w.Write(bArquivo);
                    seek += tamanhoConvertido;
                    w.Close();

                }


            }

            if (atualizarArm9 == 1)
            {
                AtualizarArm9(endArm9, offsetAt, tamanhoDetudo, temTama);
            }

        }

        static void AtualizarArm9(int indereco, int valor, int tamanho, int temTamanho)
        {
            Console.WriteLine(">Atualizando ponteiros no arm9.bin...\n");
            using (BinaryWriter w = new BinaryWriter(File.Open("arm9.bin", FileMode.Open)))
            {
                w.BaseStream.Seek(indereco, SeekOrigin.Begin);
                w.Write(valor);
                if (temTamanho == 1)
                {
                    w.Write(tamanho);
                }

                w.Close();
            }

        }
    }
}
