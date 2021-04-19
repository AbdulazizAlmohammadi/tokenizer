using System;
using System.Collections.Generic;

namespace Tokenizer
{
    class Program
    {
        public class Token
        {
            public string type;
            public string value;
            public int position;
            public int lineNumber;
        }
        public abstract class Tokenizeable
        {
            public abstract bool tokenizable(Tokenizer source);
            public abstract Token tokenize(Tokenizer source);
        }

        public class Tokenizer
        {
            public string input;
            public int currentPosition;
            public int lineNumber;
            public Tokenizeable[] handlers;

            public Tokenizer(string input)
            {
                this.input = input;
                this.currentPosition = -1;
                this.lineNumber = 1;
            }

            public char peek(int numOfPosition = 1) {
                if (this.hasMore(numOfPosition))
                {
                    return this.input[this.currentPosition + numOfPosition];
                }
                else
                {
                    return '\0';
                }
            }

            public char next()
            {
                char currentChar = this.input[++this.currentPosition];
                if(currentChar == '\n')
                {
                    this.lineNumber++;
                }
                return currentChar;
            }
            public bool hasMore(int num =1) { return (this.currentPosition + num) < this.input.Length; }

            public Token tokenize(Tokenizeable[] handelrs)
            {
                foreach (var t in handelrs)
                {
                    if (t.tokenizable(this))
                    {
                        return t.tokenize(this);
                    }
                }
                return null;
            }

        }
        public class NumberTokenizer : Tokenizeable
        {
            

            public override bool tokenizable(Tokenizer t)
            {
                return t.hasMore() && char.IsDigit(t.peek());
            }

            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "number";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while(t.hasMore()&& char.IsDigit(t.peek()))
                {
                    token.value += t.next();
                }
                return token;
            }

        }

        public class IdTokenizer : Tokenizeable
        {


            public override bool tokenizable(Tokenizer t)
            {
                return t.hasMore() && char.IsLetter(t.peek());
            }

            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "id";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (t.hasMore() && (char.IsLetterOrDigit(t.peek()) || t.peek()=='_' ))
                {
                    token.value += t.next();
                }
                return token;
            }

        }

        public class WhiteSpaceTokenizer : Tokenizeable
        {


            public override bool tokenizable(Tokenizer t)
            {
                return t.hasMore() && char.IsWhiteSpace(t.peek());
            }

            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "whiteSpace";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (t.hasMore() && (char.IsWhiteSpace(t.peek())))
                {
                    token.value += t.next();
                }
                return token;
            }

        }

        public class CommentTokenizer : Tokenizeable
        {


            public override bool tokenizable(Tokenizer t)
            {
                
                return t.hasMore() &&( t.peek()=='/' && t.peek(2) == '/');
            }

            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "comment";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (t.hasMore() && (t.peek()!= '\n')&&( t.peek() == '/') || char.IsLetterOrDigit(t.peek()) || char.IsWhiteSpace(t.peek()))
                {
                    token.value += t.next();
                }
                return token;
            }

        }


        static void Main(string[] args)
        {
            Tokenizer t = new Tokenizer("ggh 348668 k //fdfdsf");
            Tokenizeable[] handlers = new Tokenizeable[] { new IdTokenizer(), new NumberTokenizer() , new WhiteSpaceTokenizer() , new CommentTokenizer()};
            Token token = t.tokenize(handlers);
            while(token != null)
            {
                if (token.type == "number")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }else if (token.type == "id") Console.ForegroundColor = ConsoleColor.Blue;
                else if(token.type == "comment") Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.White;
                Console.Write(token.value);
                token = t.tokenize(handlers);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }










        /*static List<string> tokenizer(string passage)
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


         static List<string> tokenizer2(string value)
        {
            if (value == null || value.Trim().Length == 0) return null;
            List<string> result = new List<string> { };
            string world="";
            int i = 0; 
            
            while(i < value.Length)
            {
                // do action
                world = "";
                if (char.IsDigit(value[i]))
                {
                    while (i < value.Length && char.IsDigit(value[i]))
                    {
                        world += value[i++];
                        
                    }
                    result.Add(world);
                }
                else if(char.IsLetter(value[i])|| value[i]=='_')
                {
                    while (i < value.Length && (char.IsLetterOrDigit(value[i]) || value[i] == '_'))
                    {
                         world += value[i++];
                        
                    }
                    result.Add(world);
                }
                else i++;
            }

            //get output
            return result;
        }


        */
    }


}
