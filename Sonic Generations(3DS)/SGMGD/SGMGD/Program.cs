using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SGMGD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sonic Generations MG Dumper");
            Console.WriteLine("Digite o nome do Arquivo MG sem a extensão");
            string nomeDoArquivo = Console.ReadLine();
            string nomeDoArqCorreto = nomeDoArquivo.ToUpper();
            Extracao(nomeDoArqCorreto);
        }

        static void Extracao(string arquivo)
        {
            
            List<string> dialgos = new List<string>();
            int posicaoNumeDialogos = 8; 
            int posicaoTabPonteiros = 16;
            int bytesIgnorar = 4;
            int BytesIgnorar2 = 32;
            string dialogo = "";

            using (var b = new BinaryReader(File.OpenRead(arquivo+".MG")))
            {
                b.BaseStream.Seek(posicaoNumeDialogos, SeekOrigin.Begin);
                int numeroDialogos = b.ReadInt32();
              

                for (int i = 0; i < numeroDialogos; i++)
                {
                    b.BaseStream.Seek(posicaoTabPonteiros + bytesIgnorar, SeekOrigin.Begin);
                    int ponteiroDlgUni = b.ReadInt32();
                    int ponteiroNomeAsc = b.ReadInt32();

                    b.BaseStream.Seek(ponteiroDlgUni,SeekOrigin.Begin);

                    int numeroPonteiros = b.ReadInt32();
                    int posTabBloco = b.ReadInt32();

                    b.BaseStream.Seek(posTabBloco, SeekOrigin.Begin);


                    for (int j2 = 0; j2 < numeroPonteiros; j2++)
                    {
                        if (j2 == numeroPonteiros)
                        {

                        }

                        int ponteiroBloco = b.ReadInt32();
                        int ponteiroBloco2 = b.ReadInt32();
                        int tamanhoDlgUni = ponteiroBloco2 - ponteiroBloco;
                        b.BaseStream.Seek(ponteiroBloco, SeekOrigin.Begin);
                        byte[] textofinal = b.ReadBytes(tamanhoDlgUni);
                        dialogo = Encoding.Unicode.GetString(textofinal, 0, textofinal.Length).Trim('\0');

                        dialgos.Add(dialogo);

                        
                    }

                  

                    b.BaseStream.Seek(ponteiroNomeAsc,SeekOrigin.Begin);

                    byte[] nometexb = b.ReadBytes(0x10);
                    string nometexto = Encoding.ASCII.GetString(nometexb, 0, nometexb.Length).Trim('\0');

                    dialgos.Add(dialogo + "|" + nometexto + "~");

                    posicaoTabPonteiros += 16;
                }

                b.Close();
            }
            
            File.WriteAllLines(arquivo+".txt", dialgos); 
        }
    }
}
