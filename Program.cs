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

            public bool isNumber(Tokenizer t)
            {
                return t.hasMore() && (char.IsDigit(t.peek()) || t.peek() == '.');
            }
            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "number";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while(isNumber(t))
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

            public bool isId(Tokenizer t)
            {
                return t.hasMore() && (char.IsLetterOrDigit(t.peek()) || t.peek() == '_');
            }
            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "id";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (isId(t))
                {
                    token.value += t.next();
                }
                return token;
            }

        }

        public class ClassTokenizer : Tokenizeable
        {


            public override bool tokenizable(Tokenizer t)
            {
                return t.hasMore() && char.IsUpper(t.peek());
            }

            public bool isClass(Tokenizer t)
            {
                return t.hasMore() && (char.IsLetterOrDigit(t.peek()) || t.peek() == '_');
            }

            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "class";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (isClass(t))
                {
                    token.value += t.next();
                }
                return token;
            }

        }
        public class ElseTokenizer : Tokenizeable
        {


            public override bool tokenizable(Tokenizer t)
            {
                return t.hasMore();
            }
            public bool isElse(Tokenizer t)
            {
                return t.hasMore() && !Char.IsWhiteSpace(t.peek()) && (t.peek() != '/' && t.peek(2) != '/');
            }
            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "else";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (isElse(t))
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

            public bool isComment(Tokenizer t)
            {
                return t.hasMore() && (t.peek() != '\n') && (t.peek() == '/') || char.IsLetterOrDigit(t.peek()) || t.peek()==' ';
            }

            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "comment";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (isComment(t))
                {
                    token.value += t.next();
                }
                return token;
            }

        }

        public class CommentStarTokenizer : Tokenizeable
        {


            public override bool tokenizable(Tokenizer t)
            {

                return t.hasMore() && (t.peek() == '/' && t.peek(2) == '*');
            }

            public bool isCommentStar(Tokenizer t)
            {
                return t.hasMore() ;
            }

            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "commentStar";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (isCommentStar(t))
                {
                    if((t.peek() == '*' && t.peek(2) == '/'))
                    {
                        token.value += t.next();
                        token.value += t.next();
                        break;

                    }
                    else token.value += t.next();

                }
                return token;
            }

        }

        public class StringTokenizer : Tokenizeable
        {


            public override bool tokenizable(Tokenizer t)
            {

                return t.hasMore() && t.peek() == '\"' ;
            }

            public bool isString(Tokenizer t)
            {
                return t.hasMore();
            }

            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.type = "string";
                token.value = "";
                token.position = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (isString(t))
                {
                    if ( t.peek(2) == '\"')
                    {
                        token.value += t.next();
                        token.value += t.next();
                        break;

                    }
                    else token.value += t.next();

                }
                return token;
            }

        }
        static void Main(string[] args)
        {
            //string[] keyWords = {"int"}


            string keyWords = "int double float string";
            Tokenizer t = new Tokenizer(
                "class Ezz {\n" +
                "  int X = 5 ; // variable \n" +
                "  int Y = 20 ; \n" +
                "  double Val = 2.0 ; \n" +
                "/* \n" +
                "something here \n" +
                "may helpfull \n " +
                "*/ \n" +
                "string Hello = \" hello \" ;" +
                "}");





            Tokenizeable[] handlers = new Tokenizeable[] {new ClassTokenizer(), new IdTokenizer(), new StringTokenizer(),
                new NumberTokenizer() , new WhiteSpaceTokenizer() ,new CommentStarTokenizer() ,new CommentTokenizer() , new ElseTokenizer()};
            Token token = t.tokenize(handlers);
            while(token != null)
            {
                if (token.type == "number")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (token.type == "id") {
                    if (keyWords.Contains(token.value))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else
                     Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (token.type == "comment"|| token.type == "commentStar") Console.ForegroundColor = ConsoleColor.Green;
                else if (token.type == "class") Console.ForegroundColor = ConsoleColor.Blue;
                else if (token.type == "string") Console.ForegroundColor = ConsoleColor.Yellow;
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
