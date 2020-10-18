using System;

namespace mc
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(line))
                return;

                var lexer = new Lexer(line);
                while(true)
                {
                    var token=lexer.NextToken();
                    if(token.Kind==SyntaxKind.EndofFileToken)
                    break;
                    Console.Write($"{token.Kind}:'{token.Text}'");
                    if(token.Value!=null)
                    {
                        Console.Write($"{token.Value}");
                    }

                    Console.WriteLine();
                }

                if(line == "1 + 2 * 3")
                Console.WriteLine("7");
                else
                Console.WriteLine("Invalid Expression");
            }
        }
    }

    enum SyntaxKind
    {
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParanthesisToken,
        BadToken,
        EndofFileToken
    }
    class SyntaxToken
    {
        public SyntaxToken(SyntaxKind kind,int pos,string text,object value)
        {
            Kind=kind;
            Pos=pos;
            Text=text;
        }
        public SyntaxKind Kind{get;}
        public int Pos{get;}
        public string Text{get;}
        public object Value{get;}
    }
    class Lexer
    {
        private readonly string _text;
        private int _pos;
        public Lexer(String text)
        {
            _text=text;
        }

        private char Current
        {
            get
            {
                if(_pos>=_text.Length)
                return '\0';

                return _text[_pos];
            }
        }

        private void Next()
        {
            _pos++;
        }
        public SyntaxToken NextToken()
        {
            if(_pos >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndofFileToken,_pos,"\0",null);
            }
            if(char.IsDigit(Current))
            {
                var start=_pos;
                while(char.IsDigit(Current))
                Next();

                var length=_pos-start;
                var text =_text.Substring(start,length);
                int.TryParse(text,out var value);
                return new SyntaxToken(SyntaxKind.NumberToken,start,text,value);
            }
            
            if(char.IsWhiteSpace(Current))
            {
                var start=_pos;
                while(char.IsDigit(Current))
                Next();

                var length=_pos-start;
                var text =_text.Substring(start,length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken,start,text,null);
            }

            if(Current == '+')
                 return new SyntaxToken(SyntaxKind.PlusToken,_pos++,"+",null);
            else if(Current == '-')
                 return new SyntaxToken(SyntaxKind.MinusToken,_pos++,"-",null);
             else if(Current == '*')
                 return new SyntaxToken(SyntaxKind.StarToken,_pos++,"*",null);
             else if(Current == '/')
                 return new SyntaxToken(SyntaxKind.SlashToken,_pos++,"/",null);
             else if(Current == '(')
                 return new SyntaxToken(SyntaxKind.OpenParenthesisToken,_pos++,"(",null);
             else if(Current == ')')
                 return new SyntaxToken(SyntaxKind.CloseParanthesisToken,_pos++,")",null);
            return new SyntaxToken(SyntaxKind.BadToken,_pos++,_text.Substring(_pos-1,1),null);
        }
    }
}
