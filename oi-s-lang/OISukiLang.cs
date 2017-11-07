/* Copyright (c) 2017 as-is-prog */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private string source;
        private string[] tokens;
        private int cursor;//プログラムカウンタ

        private int[] memory;
        private int pointer;

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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memorySize">確保する配列のサイズ</param>
        public OISukiLang(int memorySize = DEFAULT_MEMORY_SIZE)
        {
            memory = new int[memorySize];
        }

        /// <summary>
        /// 大石泉すき言語を実行します。
        /// </summary>
        /// <param name="source">大石泉言語のコード文字列</param>
        public void Exec(string source)
        {
            PreAnalyze(source);

            cursor = 0;

            while(true)
            {
                //Console.Write(tokens[cursor]);
                OrderExec(tokens[cursor]);
                cursor++;
                if (cursor >= tokens.Length) break;
            }
        }
        
        private void PreAnalyze(string source)
        {
            var cursor = 0;

            source = new Regex(@"[\s]*\{[^\}]+\}").Replace(source, "");

            var tokenList = new List<string>();

            while (source.Length > cursor)
            {
                bool hit = false;
                foreach (string order in ORDERS)
                {
                    if (source.Length - cursor < order.Length) continue;

                    if (source.IndexOf(order, cursor, order.Length) == cursor)
                    {
                        cursor += order.Length;
                        hit = true;
                        tokenList.Add(order);
                        break;
                    }
                }
                if (!hit) cursor++;
            }

            tokens = tokenList.ToArray();
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
                    if (memory[pointer] == 256)
                    {
//                        memory[pointer] = 0;
                    }
                    break;
                case VALUE_DECREMENT:
                    memory[pointer]--;
                    if (memory[pointer] == -1)
                    {
//                        memory[pointer] = 255;
                    }
                    break;
                case VALUE_GET:
                    memory[pointer] = Console.Read();
                    break;
                case VALUE_PUT:
                    Console.Write((char)memory[pointer]);
                    break;
                case LOOP_OPEN:
                    if (memory[pointer] == 0)
                    {
                        int idx = cursor;
                        int depth = 0;

                        while (idx < tokens.Length)
                        {
                            if (tokens[idx] == LOOP_OPEN)
                            {
                                depth++;
                            }
                            else if (tokens[idx] == LOOP_CLOSE)
                            {
                                depth--;
                                if (depth == 0) break;
                            }
                            idx++;
                        }
                        if (idx >= tokens.Length) throw new Exception("いずみんイズミンの(かっこの)対応とれてないよ");
                        cursor = idx;
                    }
                    else
                    {
                        braceStack.Push(cursor);
                    }
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
