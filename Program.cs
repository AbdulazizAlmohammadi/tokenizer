using System;
using System.Collections.Generic;

namespace Tokenizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string val = "# #azzzb1ab.1 gg aa ## #  #dd #1a #";

            List<string> ls = tokenizer(val);
        }
        static List<string> tokenizer(string passage)
        {
            String hex = "0123456789abcdefABCDEF";
            List<string> ls = new List<string>();
            string token = "";
            int i = 0;
            int pick=0;

            while (i < passage.Length)
            {
                token = "#";
                pick = i + 1;
                //HEX
                if (passage[i] == '#' && pick < passage.Length && hex.Contains(passage[pick]))
                {
                    //to start check after #
                    ++i;
                    while (i < passage.Length && !char.IsWhiteSpace(passage[i])&& token.Length<7 && hex.Contains(passage[i]))
                    {
                        
                            token += passage[i];
                        i++;
                    }
                    while (token.Length < 7)
                    {
                        token += "0";
                    }


                    Console.WriteLine(token);
                }

                i++;
            }
            return ls;
        }
    }

  
}
