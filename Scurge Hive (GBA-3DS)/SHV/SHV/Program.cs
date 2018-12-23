using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SHV
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Scurge Five\n Escolha de qual versão você deseja dumpar o texto");
            Console.WriteLine("Pressione digite a versão e o modo e pressione enter para iniciar\n-e para extrair\n-i para inserir");
            string comando = Console.ReadLine();

            if (comando == "-e")
            {
                Console.WriteLine("Digite a versão:\n-1 NDS\n-2 GBA");
                string versao = Console.ReadLine();
                if (versao == "-1")
                {
                    Extrair(versao);
                } else if (versao == "-2")
                {
                    Extrair(versao);
                }
                else
                {
                    Console.WriteLine("Modo inválido!");

                    Environment.Exit(0);
                }

            }
            else if (comando == "-i")
            {
                Console.WriteLine("Digite a versão:\n-1 NDS\n-2 GBA");
                string versao = Console.ReadLine();
                if (versao == "-1")
                {
                    Inserir(versao);
                }
                else if (versao == "-2")
                {
                    Inserir(versao);
                }
                else
                {
                    Console.WriteLine("Modo inválido!");

                    Environment.Exit(0);
                }
                
            }
            else { Console.WriteLine("Modo inválido!");

                Environment.Exit(0);
            }
            
            //  Console.ReadKey();

        }

        static void Extrair(string console)
        {
            Console.WriteLine("Modo de extração\nExtraindo...");
            string valores = File.ReadAllText("scurge.tbl");
            string[] corta = valores.Replace("\r\n", "º").Split('º');
            List<int> valor = new List<int>();
            List<string> caracteres = new List<string>();



            for (int i = 0; i < corta.Length; i++)
            {

                string[] sp = corta[i].Split('=');

                int value = Convert.ToInt32(sp[0], 16);
                valor.Add(value);
                caracteres.Add(sp[1]);
            }

            int[] intList = valor.ToArray();
            string[] letras = caracteres.ToArray();
            int posicaoTabela = 0;
            int tamanhoTabela = 0;
            int ponteiro = 0;
            int proximoPonteiro = 0;
            string txt = null;
            string rom = null;
            if (console == "-1")
            {
                posicaoTabela = 0x2DB64;
                tamanhoTabela = 0x31430;
                txt = "sc.txt";
                rom = "char";
            }
            else if (console == "-2")
            {

                posicaoTabela = 0x1CADDC;
                tamanhoTabela = 0x1CD5A4;
                txt = "sc_gba.txt";
                rom = "2544 - Scurge Hive (E)(Rising Sun).gba";
            }
            
            
            int stringLengh = 0;
            

            using (StreamWriter sw = new StreamWriter(txt, true))
            {

                using (BinaryReader b = new BinaryReader(File.Open(rom, FileMode.Open)))
                {
                    while (posicaoTabela < tamanhoTabela)
                    {
                        b.BaseStream.Seek(posicaoTabela, SeekOrigin.Begin);
                        
                        ponteiro = b.ReadInt32();
                        proximoPonteiro = b.ReadInt32();
                        if (console == "-2")
                        {
                            ponteiro += tamanhoTabela;
                            proximoPonteiro += tamanhoTabela;
                        }
                        stringLengh = proximoPonteiro - ponteiro;

                        b.BaseStream.Seek(ponteiro, SeekOrigin.Begin);
                       // Console.Write("[Ponteiro:{0}]\n", posicaoTabela);
                        sw.Write("[Ponteiro:{0}]\n", posicaoTabela);
                        for (int i = 0; i < stringLengh;)
                        {
                            b.BaseStream.Seek(ponteiro, SeekOrigin.Begin);
                            ushort inde = b.ReadByte();
                            int pos = Array.IndexOf(intList, inde);


                            if (pos > letras.Length)
                            {
                        //        Console.Write(inde);
                                sw.Write("0x{0:x}:", inde);
                                i += 1;
                                ponteiro += 1;
                            }
                            else if
                                       (pos <= -1)
                            {

                             //   Console.Write(inde);
                                sw.Write("0x{0:x}:", inde);
                                i += 1;
                                ponteiro += 1;
                            }
                            else
                            {

                                Console.Write(letras[pos]);
                                sw.Write(letras[pos]);
                                i += 1;
                                ponteiro += 1;

                                switch (letras[pos])
                                {
                                    case "<END":

                                        inde = b.ReadByte();
                                        if (inde == 0xFE)
                                        {
                                            sw.Write(": " + inde + ">\n");
                                            i += 1;
                                            ponteiro += 1;
                                        }
                                        else
                                        {
                               //             Console.Write(">\n");
                                            sw.Write(">\n");
                                        }
                                        break;

                                    case "<Pausa":
                                        sw.Write(":");
                                 //       Console.Write(":");
                                        for (int j = 0; j < 2; j++)
                                        {
                                            inde = b.ReadByte();
                                            sw.Write(" " + inde);
                                            i += 1;
                                            ponteiro += 1;
                                        }

                                        sw.Write(">");
                                  //      Console.Write(">");
                                        break;


                                    case "<FIM_DLG_P>":
                                        sw.Write("\n\n");
                                    //    Console.Write("\n\n");
                                        break;


                                }
                            }


                        }
                        ponteiro = 0;
                        if (console =="-2")
                        {
                            posicaoTabela += 0x1C;
                        }
                        else
                        {
                            posicaoTabela += 0x28;
                        }
                        
                    }
                    b.Close();
                }


            }
        }

        static void Inserir(string console)
        {
            Console.WriteLine("Modo de inserção\nInserindo...");
            string valores = File.ReadAllText("scurge(I).tbl");
            string[] corta = valores.Replace("\r\n", "º").Split('º');
            List<int> valor = new List<int>();
            List<string> caracteres = new List<string>();



            for (int i = 0; i < corta.Length; i++)
            {

                string[] sp = corta[i].Split('=');

                int value = Convert.ToInt32(sp[0], 16);
                valor.Add(value);
                caracteres.Add(sp[1]);
            }
            string arquivo = null;
            string rom = null;

            int[] intList = valor.ToArray();
            string[] letras = caracteres.ToArray();
            int posicaoTabela = 0;
            int primeiroPonteiro = 0;
            int offsetReal = 0;
            if (console == "-1")
            {
                posicaoTabela = 0x2DB50;
                primeiroPonteiro = 0x31440;
                arquivo = "sc.txt";
                rom = "char";

            }
            else
            {
                primeiroPonteiro = 0xFC0000;
                arquivo = "sc_gba.txt";
                rom = "2544 - Scurge Hive (E)(Rising Sun).gba";
                offsetReal = 0xDF2A5C;
            }

            string texto = File.ReadAllText(arquivo).Replace("\n", string.Empty).Replace("<FIM_DLG_P>", "<FIM_DLG_P>}");
            List<string> dialogos = new List<string>(texto.Split('}'));
            dialogos.RemoveAt(dialogos.Count - 1);


            foreach (string dialogo in dialogos)
            {
                string[] ponteiros = dialogo.Replace("]", "}").Split('}');
                string[] ponteiro = ponteiros[0].Replace(" ", string.Empty).Split(':');
                string numero = (ponteiro[1]);
                var varStringConvert = numero;
                int varInt;
                int.TryParse(varStringConvert.ToString(), out varInt);
                string[] dialogo2 = ponteiros[1].Replace(">", ">*").Replace("<", "~<").Split(new string[] { "*", "~" }, StringSplitOptions.RemoveEmptyEntries);

                List<byte> binario = new List<byte>();
                for (int d = 0; d < dialogo2.Length; d++)
                {

                    if (dialogo2[d].Contains("<"))
                    {

                        string[] splitin = dialogo2[d].Replace(">", string.Empty).Replace(":", string.Empty).Split(' ');
                        int pos = Array.IndexOf(letras, splitin[0]);
                        int pos2 = intList[pos];
                        // Console.Write(" {0:x}", pos);
                        byte dados0 = Convert.ToByte(pos2);
                        binario.Add(dados0);

                        for (int j = 1; j < splitin.Length; j++)
                        {
                            string nu = (splitin[j]);
                            var tringConvert = splitin[j];
                            int var;
                            int.TryParse(tringConvert.ToString(), out var);
                            //    Console.Write(" {0}", var);
                            byte dados1 = Convert.ToByte(var);
                            binario.Add(dados1);
                        }
                    }
                    else
                    {
                        string palavras = dialogo2[d];

                        char[] charArr = palavras.ToCharArray();

                        for (int g = 0; g < charArr.Length; g++)
                        {
                            String letra = charArr[g].ToString();
                            int pos = Array.IndexOf(letras, letra);
                            int pos2 = intList[pos];

                            //   Console.Write(" {0:x}", pos2);
                            byte dados2 = Convert.ToByte(pos2);
                            binario.Add(dados2);
                        }
                    }

                }

                byte[] final = binario.ToArray();
                binario.Clear();

                int tamanho = final.Count();

                using (BinaryWriter w = new BinaryWriter(File.Open(rom, FileMode.Open)))
                {
                    w.BaseStream.Seek(primeiroPonteiro, SeekOrigin.Begin);
                    w.Write(final);
                    w.BaseStream.Seek(varInt + 4, SeekOrigin.Begin);
                    if (console == "-2")
                    {
                        w.Write(offsetReal);
                        offsetReal += final.Length;
                        primeiroPonteiro += final.Length;
                    }
                    else
                    {
                        w.Write(primeiroPonteiro);
                        primeiroPonteiro += final.Length;
                    }
                    
                    
                    w.Close();
                }
            }

            if (console == "-1")
            {
                byte[] newStr;
                using (BinaryReader b2 = new BinaryReader(File.Open("char", FileMode.Open)))
                {
                    b2.BaseStream.Seek(posicaoTabela, SeekOrigin.Begin);
                    newStr = b2.ReadBytes(0x38E0);
                    b2.Close();
                }

                File.WriteAllBytes("str", newStr);
            }
            
        }
    }
}
