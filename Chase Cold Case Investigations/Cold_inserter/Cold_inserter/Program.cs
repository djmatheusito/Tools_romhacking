using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cold_inserter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("(EN)");
            Console.WriteLine(".Mes Creator - Chase: Cold Case Investigations - Distant Memories 3DS");
            Console.WriteLine("Please put the message_english.txt, TBL0(Do_not_Delete) and TBL1(Do_not_Delete) files in the same program folder.");
            Console.WriteLine("Warning! If there is already a file named Newmessage_english.mes in the same program folder it will be replaced!");
            Console.WriteLine("Press the ENTER key to start:\n");
            Console.WriteLine("(BR)");
            Console.WriteLine("Criador de .Mes - Chase: Cold Case Investigations - Distant Memories 3DS");
            Console.Write("Por favor coloque o arquivo message_english.txt, TBL0(Do_not_Delete) e TBL1(Do_not_Delete) na mesma pasta do programa.");
            Console.WriteLine("Aviso! Se já existir um arquivo chamado Newmessage_english.mes na mesma pasta do programa ele será substituido!");
            Console.WriteLine("Pressione a tecla ENTER para iniciar:");
            Console.ReadKey();

            Console.WriteLine("\nCreating...");
            Console.WriteLine("Criando...");

            Byte[] New = File.ReadAllBytes("TBL0(Do_not_Delete).bin");
            File.Create("Newmessage_english.mes").Close();
            File.WriteAllBytes("Newmessage_english.mes", New);


            int primeiroPonteiro = 0xC540;
            int posicaoTabela = 0x40;

            var lines = File.ReadAllText("message_english.txt");
            string dialogos = lines.Replace("\r\n_______________________________________\r\n", "^");
            List <string> dialogoSplit = new List<string> (dialogos.Split('^'));
            dialogoSplit.RemoveAt(dialogoSplit.Count - 1);
            foreach (var line in dialogoSplit)
            {
                string formatarLinha = line.Replace("{", string.Empty).Replace("}\r\n", "}");
                string[] splitString = formatarLinha.Split('}');

                byte[] Asc = Encoding.ASCII.GetBytes(splitString[0].Replace("\r\n", string.Empty));
                byte[] uniCode = Encoding.Unicode.GetBytes(splitString[1].Replace("\r\n", "\u001B").Replace("\n", "\u001B").Replace("ô", "\u00DF").Replace("ó", "\u00A2")
                    .Replace("é", "\u00BB").Replace("ú", "\u00A4").Replace("ç", "\u00AB").Replace("í", "\u00A1").Replace("ê", "\u00AC").Replace("ã", "\u00B5").Replace("õ", "\u00BF").Replace("ü", "\u00AA").Replace("ñ", "\u00A5"));
                byte[] separador = {0x00, 0x00, 0x00, 0x00};
                byte[] separadorB = { 0x00, 0x00, 0x00};
                byte[] separadorC = { 0x00, 0x00};
                int tamanhoAsc = splitString[0].Length;
                int tamanhoUni = splitString[1].Length*2;
                int tamanhoSepA = separador.Length;
                int tamanhoSepB = separadorB.Length;
                int tamanhoSepC = separadorC.Length;
                int ponteiroAsc = primeiroPonteiro;
                int ponteiroUni = primeiroPonteiro;

                
                using (BinaryWriter writer = new BinaryWriter(File.Open("Newmessage_english.mes", FileMode.Open)))
                    {
                    writer.BaseStream.Seek(primeiroPonteiro, SeekOrigin.Begin);
                    writer.Write(Asc);
                    if (tamanhoAsc % 2 == 0)
                    {
                        writer.Write(separadorC);
                        writer.Write(uniCode);
                        writer.Write(separador);

                        ponteiroUni += tamanhoSepC + tamanhoAsc;

                        writer.BaseStream.Seek(posicaoTabela, SeekOrigin.Begin);
                        writer.Write(ponteiroAsc);
                        writer.Write(tamanhoAsc);

                        writer.Write(ponteiroUni);
                        int uniReal = tamanhoUni / 2;
                        writer.Write(uniReal);


                        primeiroPonteiro += tamanhoAsc + tamanhoUni;
                        primeiroPonteiro += tamanhoSepC;
                        primeiroPonteiro += tamanhoSepA;

                        posicaoTabela += 0x20;
                    }  else
                    {
                        writer.Write(separadorB);
                        writer.Write(uniCode);
                        writer.Write(separador);


                        ponteiroUni += tamanhoSepB + tamanhoAsc;

                        writer.BaseStream.Seek(posicaoTabela, SeekOrigin.Begin);
                        writer.Write(ponteiroAsc);                        
                        writer.Write(tamanhoAsc);

                        writer.Write(ponteiroUni);
                        int uniReal = tamanhoUni/2;
                        writer.Write(uniReal);

                        primeiroPonteiro += tamanhoAsc + tamanhoUni;
                        primeiroPonteiro += tamanhoSepB;
                        primeiroPonteiro += tamanhoSepA;

                        posicaoTabela += 0x20;
                    }



                    writer.Close();
                   

                    
                }


            }
           
            Finalizador();
            CaculadorFixa();
            Console.WriteLine("\nFile created! \nPress any key to exit the Program.");
            Console.Write("\nArquivo criado!\nPressione qualquer tecla para sair do programa.");
            Console.ReadKey();


        }

        static void Finalizador() {

            FileInfo fileInfo = new FileInfo("Newmessage_english.mes");
            Byte[] Restante = {0x00, 0x00};

            long tamanho = fileInfo.Length;
            int tamanhoSeek = Convert.ToInt32(tamanho);

            using (BinaryWriter writer = new BinaryWriter(File.Open("Newmessage_english.mes", FileMode.Open)))
            {
                writer.BaseStream.Seek(tamanhoSeek, SeekOrigin.Begin);

                if (tamanhoSeek % 4 != 0)
                {
                    writer.Write(Restante);
                    writer.Close();
                }

            }
               
                
        }
        static void CaculadorFixa()
        {
            FileInfo fileInfo = new FileInfo("Newmessage_english.mes");
            

            long tamanho = fileInfo.Length;
            int tamanhoSeek = Convert.ToInt32(tamanho);
            int tamanhoOriginal = 216808;

            PonteirosPonteiros();

            int[] segTabPont = new int[] {0xBEFC, 0xBF04, 0XBF34,
                                          0XBF44, 0XBF6C, 0XBFA4,        
                                          0XBFBC, 0XBFDC, 0XC0FC,
                                          0XC174, 0XC194, 0XC1A4,
                                          0XC1CC, 0XC224, 0XC2EC,
                                          0XC37C, 0XC3B4, 0XC3E4,
                                          0XC3F4, 0XC42C, 0XC464,
                                          0XC48C, 0XC49C, 0XC504}; 

            foreach (int item in segTabPont)
            {
                

                using (BinaryReader r = new BinaryReader(File.Open("Newmessage_english.mes", FileMode.Open))) {

                    r.BaseStream.Seek(item, SeekOrigin.Begin);
                    int ponteiro = r.ReadInt32();
                    r.Close();

                    if (tamanhoSeek <= tamanhoOriginal)
                    {
                        int resultado = tamanhoOriginal - tamanhoSeek;
                        int atualizacaoDeponteiro = ponteiro - resultado + 4;

                        using (BinaryWriter writer = new BinaryWriter(File.Open("Newmessage_english.mes", FileMode.Open)))
                        {
                            writer.BaseStream.Seek(item, SeekOrigin.Begin);
                            writer.Write(atualizacaoDeponteiro);
                            writer.Close();

                        }

                    }else {


                        int resultado = tamanhoSeek - tamanhoOriginal;
                        int atualizacaoDeponteiro = ponteiro + resultado + 4;

                        using (BinaryWriter writer = new BinaryWriter(File.Open("Newmessage_english.mes", FileMode.Open)))
                        {
                            writer.BaseStream.Seek(item, SeekOrigin.Begin);
                            writer.Write(atualizacaoDeponteiro);
                            writer.Close();

                        }



                    }

                    
                }
            }
            

        }

        static void PonteirosPonteiros()
        {
            FileInfo fileInfo = new FileInfo("Newmessage_english.mes");
            long tamanho = fileInfo.Length;
            int tamanhoSeek = Convert.ToInt32(tamanho);

            Byte[] ponteirosFinal = File.ReadAllBytes("TBL1(Do_not_Delete).bin");

            using (BinaryWriter writer = new BinaryWriter(File.Open("Newmessage_english.mes", FileMode.Open)))
            {
                writer.BaseStream.Seek(tamanhoSeek, SeekOrigin.Begin);
                writer.Write(ponteirosFinal);
                writer.Close();

            }




        }



    }
}
