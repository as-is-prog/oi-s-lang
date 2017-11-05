using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oi_s_lang
{
    class Program
    {

        static void Main(string[] args)
        {
            string source;
            if (args.Length >= 1) {
                source = File.ReadAllText(args[0]);

                OISukiLang oiSukiLang = new OISukiLang();

                oiSukiLang.Exec(source);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("任意の文字列を出力するコードを生成します。");
                Console.Write("出力したい文字列を入力してください\n>");
                string str = Console.ReadLine();
                
                Console.Write("生成コードを読みやすく成形しますか？ y/n default:y \n>");
                source = OISukiLang.GeneratePrintStrCode(str, Console.ReadLine()[0] != 'n');

                if (File.Exists("source.ois"))
                {
                    Console.Write("source.oisは既に存在します。上書きしてもよいですか？ y/n default:n \n>");
                    if (Console.ReadLine()[0] != 'y')
                    {
                        Console.WriteLine("生成処理をキャンセルしました。");
                    }
                    else
                    {
                        File.WriteAllText("source.ois", source);
                        Console.WriteLine("source.oisを生成しました。");
                    }
                }
                else
                {
                    File.WriteAllText("source.ois", source);
                    Console.WriteLine("source.oisを生成しました。");
                }

                
            }

            Console.Write("\nプログラムを終了するには何かキーを押してください…");
            Console.ReadKey();
        }
    }
}
