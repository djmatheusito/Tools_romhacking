using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;


namespace Drill_Inserter
{
    class Program
    {

          

        static void Main(string[] args)
        {


            int posicao = 0;
            int diminuir = 0;
          //  Console.WriteLine("Digite o nome do arquivo a ser inserido.");
            string[] arquivos = Directory.GetFiles("Scripts");
            Console.WriteLine("Inserindo...");

            foreach (string arquivo in arquivos)
            {
                if (arquivo == "Scripts\\Texto_historia_parte_1.txt")
                {
                    posicao = 0x6E8260;
                    diminuir = 0;
                }
                else if (arquivo == "Scripts\\Texto_historia_parte_2.txt")
                {
                    posicao = 0x6F9000;
                    diminuir = 2;
                }
                else if (arquivo == "Scripts\\Texto_historia_parte_3.txt")
                {
                    posicao = 0x6FA8E0;
                    diminuir = 0;
                }
                else if (arquivo == "Scripts\\Nomes_de_locais.txt")
                {
                    posicao = 0x6FC9F0;
                    diminuir = 0;
                }

                string texto = File.ReadAllText(arquivo).Replace("\n", string.Empty).Replace("<FIM>", "<FIM>}");
                List<string> dialogos = new List<string>(texto.Split('}'));
                dialogos.RemoveAt(dialogos.Count - 1);


                foreach (string dialogo in dialogos)
                {
                    string[] ponteiros = dialogo.Replace("]]", "}").Split('}');
                    string[] ponteiro = ponteiros[0].Replace(" ", string.Empty).Split(':');
                    string numero = (ponteiro[1]);
                    var varStringConvert = numero;
                    int varInt;
                    int.TryParse(varStringConvert.ToString(), out varInt);
                    varInt = varInt - diminuir;
                    string[] dialogo2 = ponteiros[1].Replace(">", ">*").Replace("<", "~<").Split(new string[] { "*", "~" }, StringSplitOptions.RemoveEmptyEntries);

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
                "_", "_", "_", "_", "_", "_", "<DIALOGO", "<Codigo", "<BOTÃO", "_", "_", "_", "<COR", "_", "<LINHA", "<FIM",
                };

                    List<byte> binario = new List<byte>();
                    for (int d = 0; d < dialogo2.Length; d++)
                    {

                        if (dialogo2[d].Contains("<"))
                        {

                            string[] splitin = dialogo2[d].Replace(">", string.Empty).Split(',');
                            int pos = Array.IndexOf(tabela, splitin[0]);
                           // Console.Write(" {0:x}", pos);
                            byte dados0 = Convert.ToByte(pos);
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
                                int pos2 = Array.IndexOf(tabela, letra);
                             //   Console.Write(" {0:x}", pos2);
                                byte dados2 = Convert.ToByte(pos2);
                                binario.Add(dados2);
                            }
                        }

                    }

                    byte[] final = binario.ToArray();
                    binario.Clear();

                    int tamanho = final.Count() -12;
                   byte[] tamanhobytes = BitConverter.GetBytes(tamanho);

                    using (BinaryWriter w = new BinaryWriter(File.Open("Drill Dozer (USA).gba", FileMode.Open)))
                    {
                        w.BaseStream.Seek(posicao, SeekOrigin.Begin);
                        w.Write(final);
                        w.BaseStream.Seek(posicao+4, SeekOrigin.Begin);
                        w.Write(tamanhobytes[0]);
                        w.BaseStream.Seek(varInt, SeekOrigin.Begin);
                        w.Write(posicao + 0x8000000);
                        posicao += final.Length;
                        w.Close();
                    }





                }
            }

            
            /*
           switch (nome)
             {
                 case "a":
                     Console.Write("1");
                     break;
                 case "b":
                     Console.Write("2");
                     break;
                 case "c":
                     Console.Write(3);
                     break;
                 case "dialogo":
                     Console.Write(4);
                     break;
                 default:
                     break;
             }

             Console.ReadKey();
             */

        }

    }
}
