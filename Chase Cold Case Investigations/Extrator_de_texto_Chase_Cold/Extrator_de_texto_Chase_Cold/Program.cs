using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Extrator_de_texto_Chase_Cold
{
    class Program
    {
        static void Main(string[] args)
        {
            string nomeArqTexto = "message_english";
            Console.WriteLine("(EN)");
            Console.WriteLine("Text dumper - Chase: Cold Case Investigations - Distant Memories 3DS");
            Console.WriteLine("Please put the message_english.mes file in the same program folder.");
            Console.WriteLine("Warning! If there is already a file named message_english.txt in the same program folder it will be erased!");
            Console.WriteLine("Press the ENTER key to start the extraction:\n");
            Console.WriteLine("(BR)");
            Console.WriteLine("Extrator de textos - Chase: Cold Case Investigations - Distant Memories 3DS");
            Console.WriteLine("Por favor coloque o arquivo message_english.mes na mesma pasta do programa.");
            Console.WriteLine("Aviso! Se já existir um arquivo chamado message_english.txt na mesma pasta do programa ele será apagado!");
            Console.WriteLine("Pressione a tecla ENTER para iniciar a extração:");
            Console.ReadKey();
            

            if (File.Exists(nomeArqTexto+".txt"))
            {
                File.Delete(nomeArqTexto + ".txt");
            }

            if (File.Exists(nomeArqTexto + ".mes"))
            {

                
                Console.WriteLine("\nExtracting...");
                Console.WriteLine("Extraindo...");
            }
            else
            {
                Console.WriteLine("(EN)");
                Console.WriteLine("\n\nFile not found!");
                Console.Write("Press any key to exit the Program.\n");
                Console.WriteLine("(BR)");
                Console.WriteLine("\n\nArquivo não encontrando!");
                Console.Write("Pressione qualquer tecla para sair do Programa.");
                Console.ReadKey();
                Environment.Exit(0);
            }



            using (BinaryReader b = new BinaryReader(File.Open(nomeArqTexto+ ".mes", FileMode.Open)))
            {
                int posicaoTabela = 0x40;
                int numeroPonteiros = 1512;

                for (int i = 0; i < numeroPonteiros; i++)
                {
                    
                    b.BaseStream.Seek(posicaoTabela, SeekOrigin.Begin);
                    int ponteiroAsc = b.ReadInt32();
                    int tamanhoAsc = b.ReadInt32();

                    int ponteiroUni = b.ReadInt32();
                    int tamanhoUni = b.ReadInt32();
                    int tamanhoUnireal = tamanhoUni * 2;

                    b.BaseStream.Seek(ponteiroAsc, SeekOrigin.Begin);
                    Byte[] textoAsc = b.ReadBytes(tamanhoAsc);
                    string textAscConv = Encoding.ASCII.GetString(textoAsc, 0, textoAsc.Length);

                    b.BaseStream.Seek(ponteiroUni, SeekOrigin.Begin);
                    Byte[] textoUni = b.ReadBytes(tamanhoUnireal);                                      
                    string textUniConv = Encoding.Unicode.GetString(textoUni);
                    string resta = Regex.Replace(textUniConv, @"\p{C}+", "\r\n");

                    


                  //  Console.Write(textAscConv + "\n" + resta + "\n\n");

                    using (FileStream fs = new FileStream("message_english" + ".txt", FileMode.Append, FileAccess.Write))

                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine("{"+ textAscConv + "}\r\n" + resta+ "\r\n_______________________________________");
                    }

                    

                    if (ponteiroUni >= 216776)
                    {

                        b.BaseStream.Seek(0x0, SeekOrigin.Begin);
                        byte[] ponteiros = b.ReadBytes(0xC540);
                        File.WriteAllBytes(@"TBL0(Do_not_Delete)" + ".bin", (ponteiros));
                        

                        b.BaseStream.Seek(0x34EE4, SeekOrigin.Begin);
                        byte[] ponteirosDos = b.ReadBytes(0xDD2C);                        
                        File.WriteAllBytes(@"TBL1(Do_not_Delete)"+".bin", (ponteirosDos));
                        b.Close();

                        
                        Console.WriteLine("\nDump completed!");
                       // Console.WriteLine("Do not delete any *(asterisk) or edit the names before *(asterisk).");
                        Console.WriteLine("Press any key to exit the Program.");

                        
                        Console.WriteLine("\nExtração Completa!");
                      //  Console.WriteLine("Não apague nenhum *(asterisco) ou edite os nomes antes do *(asterisco).");
                        Console.WriteLine("Pressione qualquer tecla para sair do Programa.");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }

                    posicaoTabela += 0x20;
                    ponteiroAsc = 0;
                    tamanhoAsc = 0;
                    ponteiroUni = 0;
                    tamanhoUni = 0;
                    tamanhoUnireal = 0;
                    textoAsc = null;
                    textAscConv = null;
                    textoUni = null;
                    textUniConv = null;

                    
                    
                }

                


            }

        }

        



    }
}
