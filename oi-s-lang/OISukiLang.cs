using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oi_s_lang
{
    class OISukiLang
    {
        const string POINTER_INCREMENT = "大石泉";
        const string POINTER_DECREMENT = "愛してる";
        const string VALUE_INCREMENT = "すき";
        const string VALUE_DECREMENT = "好き";
        const string VALUE_PUT = "すき！";
        const string VALUE_GET = "すき？";
        const string LOOP_OPEN = "いずみん";
        const string LOOP_CLOSE = "イズミン";

        const int DEFAULT_MEMORY_SIZE = 512;

        static readonly string[] ORDERS;

        private int cursor;//プログラムカウンタ

        private int[] memory;
        int pointer;

        Stack<int> braceStack = new Stack<int>();// [ のスタック

        static OISukiLang()
        {
            ORDERS = new String[]{
                       POINTER_INCREMENT,
                       POINTER_DECREMENT,
                       VALUE_INCREMENT,
                       VALUE_DECREMENT,
                       VALUE_GET,
                       VALUE_PUT,
                       LOOP_OPEN,
                       LOOP_CLOSE
                     }.OrderBy((s) => { return -s.Length; }).ToArray<String>();
        }
        
        public OISukiLang()
        {
            memory = new int[DEFAULT_MEMORY_SIZE];
        }

        public void Exec(string source)
        {
            while (source.Length > cursor)
            {
                bool hit = false;
                foreach (string order in ORDERS)
                {
                    if (source.Length - cursor < order.Length)continue;

                    if (source.IndexOf(order,cursor,order.Length) == cursor)
                    {
                        cursor += order.Length;
                        hit = true;
                        OrderExec(order);
                        break;
                    }
                }
                if (!hit) cursor++;
            }
        }

        private void OrderExec(string order)
        {
            switch (order)
            {
                case POINTER_INCREMENT:
                    pointer++;
                    break;
                case POINTER_DECREMENT:
                    pointer--;
                    break;
                case VALUE_INCREMENT:
                    memory[pointer]++;
                    break;
                case VALUE_DECREMENT:
                    memory[pointer]--;
                    break;
                case VALUE_GET:
                    memory[pointer] = Console.Read();
                    break;
                case VALUE_PUT:
                    Console.Write((char)memory[pointer]);
                    break;
                case LOOP_OPEN:
                    braceStack.Push(cursor);
                    break;
                case LOOP_CLOSE:
                    if (memory[pointer] != 0)
                        cursor = braceStack.Peek();
                    else
                        braceStack.Pop();
                    break;
            }   
        }

        public static string GeneratePrintStrCode(string str, bool format = true)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in str.ToCharArray())
            {
                var times = 1;//かける数
                var surplus = 0;//あまり

                while (times * times < c)
                {
                    times++;
                }
                times--;
                surplus = c - (times * times);

                sb.Append(POINTER_INCREMENT);
                sb.Append("\n");
                for (var i = 0; i < times; i++)
                {
                    sb.Append(VALUE_INCREMENT);
                }
                sb.Append("\n\n");
                sb.Append(LOOP_OPEN);
                sb.Append("\n    ");
                sb.Append(POINTER_DECREMENT);
                sb.Append("\n    ");
                for (var i = 0; i < times; i++)
                {
                    sb.Append(VALUE_INCREMENT);
                }
                sb.Append("\n    ");
                sb.Append(POINTER_INCREMENT);
                sb.Append("\n    ");
                sb.Append(VALUE_DECREMENT);
                sb.Append("\n");
                sb.Append(LOOP_CLOSE);
                sb.Append("\n\n");

                sb.Append(POINTER_DECREMENT);
                sb.Append("\n");

                for (var i = 0; i < surplus; i++)
                {
                    sb.Append(VALUE_INCREMENT);
                }

                sb.Append("\n");
                sb.Append(POINTER_INCREMENT);
                sb.Append("\n\n");
            }

            for (var i = 0; i < str.Length; i++)
            {
                sb.Append(POINTER_DECREMENT);
            }

            sb.Append("\n");
            sb.Append(VALUE_PUT);
            for (var i = 1; i < str.Length; i++)
            {
                sb.Append(POINTER_INCREMENT);
                sb.Append(VALUE_PUT);
            }

            return format ? sb.ToString() : sb.Replace(" ", "").Replace("\n", "").ToString();
        }

    }
}
