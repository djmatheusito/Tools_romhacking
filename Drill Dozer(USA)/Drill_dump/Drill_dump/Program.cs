using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Drill_dump
{
    class Program
    {
        static void Main(string[] args)
        {

            int[] tabelasPtr = new int[] { 0x224BD4, 0x3A8620, 0x368DD8, 0x4A9D3C};
  
           Dump(tabelasPtr[0], 248, "Nomes_de_locais.txt", 4,0,0,1);
           Dump(tabelasPtr[1], 239, "Texto_historia_parte_1.txt", 16,0,0,1);
           Dump(tabelasPtr[2], 44, "Texto_historia_parte_2.txt", 0, 1,0,0);
            Dump(tabelasPtr[3], 78, "Texto_historia_parte_3.txt", 0, 0, 1,0);

        }
      
        static void Dump(int tabelasPtr, int numeroPonteiros, string nomeArquivo, int adicionar, int ponteirosMarcados, int ponteirosScan, int ponteiroOG)
        { 
            string[] tabela = new string[]
            {  " ", "À", "Á", "Â", "Ã", "Ä", "Å", "Æ", "Ç", "È", "É", "Ê", "Ë", "Ì", "Í", "Î",
                //0x10
                "Ï", "Ð", "Ñ", "Ò", "Ó", "Ô", "Õ", "Ö", "Ø", "Ù", "Ú", "Û", "Ü", "Ý", "Þ", "ß",
                //0x20
                "×", "à","á","â", "ã", "ä", "å", "æ", "ç", "è", "é", "ê", "ë", "ì", "í", "î",
                //0x30
                "ï","ð", "ñ", "ò", "ó", "ô", "õ", "ö", "÷", "ù", "ú", "û", "ü", "ý", "þ", "ÿ",
                //0x40
                "Œ", "œ", "œ", "_", "¡", "¿", "!", "?", "_", "=", "_", "_", "_", "_", ".", ",",
                //0x50
                ":", ";", "_", "%", "\"", "_", "'", "_", "-", "_", "_", "_", "_", "_", "_", "_",
                //0x60
                "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_",
                //0x70
                "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "<Algo", "_", "_", "_",
                //0x80
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "_", "_", "_", "_", "_", "_",
                //0x90
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P",
                //0xA0
                "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f",
                //0xB0
                "g", "h", "i", "j", "k", "l", "m","n", "o", "p", "q", "r", "s", "t", "u", "v",
                //0xC0
                "w", "x", "y", "z", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_",
                //0xD0
                "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_",
                //0xE0
                "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_", "_",
                //0xF0
                "_", "_", "_", "_", "_", "_", "<DIALOGO", "<Codigo", "<BOTÃO", "_", "_", "_", "<COR", "_", "<LINHA>\n", "<FIM>\n\n",
            };

            /* List<int> ponteirosS = new List<int>();
             int inicio = 0x4A9C3C;
             int final =  0x4AAA98;
             int ponteiro = 0;
             using (BinaryReader scanner = new BinaryReader(File.Open("Drill Dozer (USA).gba", FileMode.Open)))
             {

                 while (inicio < final)
                 {
                     scanner.BaseStream.Seek(inicio, SeekOrigin.Begin);
                     ponteiro = scanner.ReadInt32();

                     if (ponteiro > 0x8000000)
                     {
                         ponteirosS.Add(inicio);
                     }

                     inicio += 4;
                 }

                 scanner.Close();
             }
             */

            int[] tabelaScaneada = new int[] {  4890800,4890812,4890824,4890836,4890992,4891004,
                                                4891028,4891040,4891148,4891160,4891184,4891196,4891208,4891268,
                                                4891280,4891292,4891316,4891328,4891340,4891364,4891376,4891388,4891400,4891412,
                                                4891472,4891496,4891508,4891568,4891580,4891592,
                                                4891616,4891628,4891640,4891664,4891676,4891700,4891712,4891724,
                                                4891784,4891808,4891820,4891832,4891844,4891904,4891916,4891928,
                                                4891940,4891964,4891976,4891988,4892048,4892060,4892084,4892096,
                                                4892108,4892120,4892132,4892192,4892216,4892312,
                                                4892372,4892384,4892408,4892420,4892432,4892456,4892468,4892480,4892492,4892516,
                                                4892540,4892600,4892660,4892708,4892768,4892792,4892900,
                                                4893260};

            int[] tabelaMapeda = new int[] {0x368DDA,0x368E9A,0x368EA6,0x368EFA,0x368F06,0x368F12,0x368F1E,
                                            0x368F2A,0x368F36,0x368FC6,0x368FD2,0x368FDE,0x368FEA,0x368FF6,
                                            0x36900E,0x36901A,0x369026,0x36906E,0x369092,0x3690FE,0x36912E,0x36918E,
                                            0x36919A,0x3691A6,0x3691B2,0x3691D6,0x3691FA,0x36924E,0x369272,0x3692BA,
                                            0x3692DE,0x369302,0x36933E,0x36937A,0x3693C2,0x3693CE,0x369452,
                                            0x36945E,0x369482,0x36948E,0x3694CA,0x3694D6,0x3694EE,0x3694FA};

            int[] argumentos = new int[]{11,4,5,1,3};

            List<string> dialogos = new List<string>();
            
            using (StreamWriter writer = new StreamWriter(nomeArquivo, true))
            {

                using (BinaryReader b = new BinaryReader(File.Open("Drill Dozer (USA).gba", FileMode.Open)))
                {
                    // int posicaoTabela = ta;
                    

                    for (int i = 0; i < numeroPonteiros; i++)
                    {
                        b.BaseStream.Seek(tabelasPtr, SeekOrigin.Begin);
                        if (ponteirosMarcados == 1)
                        {
                            b.BaseStream.Seek(tabelaMapeda[i] - 2, SeekOrigin.Begin);

                        }

                        if (ponteirosScan == 1)
                        {
                            b.BaseStream.Seek(tabelaScaneada[i], SeekOrigin.Begin);
                        }
                       
                        uint ponteiro1 = b.ReadUInt32() - 0x8000000;
                        //  int ponteiro2 = b.ReadInt32() - 0x8000000;
                        //   int tamanhoDlg = ponteiro2 - ponteiro1;
                        if (ponteiroOG == 1)
                        {
                            Console.WriteLine("\n[[Ponteiro: {0:x}]]\n", tabelasPtr);
                            writer.Write("\n[[Ponteiro: {0}]]\n", tabelasPtr);
                            b.BaseStream.Seek(ponteiro1, SeekOrigin.Begin);
                        }

                        if (ponteirosMarcados == 1)
                        {

                            Console.WriteLine("\n[[Ponteiro: {0:x}]]\n", tabelaMapeda[i]);
                            writer.Write("\n[[Ponteiro: {0}]]\n", tabelaMapeda[i]);
                            b.BaseStream.Seek(ponteiro1, SeekOrigin.Begin);
                        }

                        if (ponteirosScan == 1)
                        {
                            Console.WriteLine("\n[[Ponteiro: {0:x}]]\n", tabelaScaneada[i]);
                            writer.Write("\n[[Ponteiro: {0}]]\n", tabelaScaneada[i]);
                            b.BaseStream.Seek(ponteiro1, SeekOrigin.Begin);
                        }
                        
                        for (int j = 0; j < 500000; j++)
                        {

                            ushort valor = b.ReadByte();
                            Console.Write(tabela[valor]);
                            writer.Write(tabela[valor]);
                            if (valor == 255)
                            {
                                break;
                            }
                            j += 1;

                            if (valor == 0xF6)
                            {
                                for (int g = 0; g < argumentos[0]; g++)
                                {
                                    uint valorarg = b.ReadByte();
                                    Console.Write("," + valorarg);
                                    writer.Write("," + valorarg);


                                }
                                writer.Write(">\n");
                                Console.Write(">\n");



                            }
                            else if (valor == 0xF8)
                            {
                                for (int g = 0; g < argumentos[1]; g++)
                                {
                                    uint valorarg = b.ReadByte();
                                    Console.Write("," + valorarg);
                                    writer.Write("," + valorarg);


                                }
                                writer.Write(">\n");
                                Console.Write(">\n");
                            }
                            else if (valor == 0xFC)
                            {
                                for (int g = 0; g < argumentos[2]; g++)
                                {
                                    uint valorarg = b.ReadByte();
                                    Console.Write("," + valorarg);
                                    writer.Write("," + valorarg);


                                }
                                writer.Write(">\n");
                                Console.Write(">\n");
                            }

                            else if (valor == 0xFC)
                            {
                                for (int g = 0; g < argumentos[3]; g++)
                                {
                                    uint valorarg = b.ReadByte();
                                    Console.Write("," + valorarg);
                                    writer.Write("," + valorarg);


                                }
                                writer.Write(">\n");
                                Console.Write(">\n");
                            }
                            else if (valor == 0x7C)
                            {
                                for (int g = 0; g < argumentos[4]; g++)
                                {
                                    ushort valorarg = b.ReadByte();
                                    Console.Write("," + valorarg);
                                    writer.Write(tabela[valor]);


                                }
                                writer.Write(">\n");
                                Console.Write(">\n");
                            }

                        }

                        tabelasPtr += adicionar;
                        ponteiro1 = 0;
                    }

                    b.Close();
                }

                writer.Close();
              
            }
        }
    }
}
