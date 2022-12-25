using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    DATATYPEINT,DATATYPEFLOAT,DATATYPESTRING, IF, ELSE, ELSEIF, UNTIL, READ, RETURN, WRITE, ENDLINE, END, CONSTANT, 
    PROGRAM, REPEATSTATEMENT, ASSIGNMENTOPERATOR, IDENTIFIER, SEMICOLON, THEN, PLUSOPERATOR,MINUSOPERATOR,EQUALTO,
    MULTIPLICATIONOPERATOR,LESSTHAN,DIVISIONOPERATOR,GREATERTHAN,LEFTCURLYBRACKETS,RIGHTCURLYBRACKETS,LEFTPARENTHESES,
    RIGHTPARENTHESES,COMMA,NOTEQUAL,AND,OR,COMMENT,MAIN,ERROR,NUMBER,STRING,FUNCTIONCALL,FUNCTIONDECLARATION,NEWLINE
}
namespace JASON_Compiler
{
    

    public class Token
    {
       public string lex;
       public Token_Class token_type;
        public Token() { } //constractor mlnash da3wa beeh
        public Token(string lex, Token_Class typ)
        {
            this.lex = lex;
            this.token_type = typ;
        }
    }

    public class Scanner
    {
        public List<Token> tokens = new List<Token>();
        Dictionary<string, Token_Class> data = new Dictionary<string, Token_Class>{
             {"int",Token_Class.DATATYPEINT},{"float",Token_Class.DATATYPEFLOAT},
            {"string",Token_Class.DATATYPESTRING},{"if",Token_Class.IF},{"else",Token_Class.ELSE},
            {"elseif",Token_Class.ELSEIF},{"until",Token_Class.UNTIL},{"read",Token_Class.READ},
            {"return",Token_Class.RETURN},{"endl",Token_Class.ENDLINE},
            {"end",Token_Class.END},{"constant",Token_Class.CONSTANT},{"program",Token_Class.PROGRAM},
            {"repeat",Token_Class.REPEATSTATEMENT},{":=",Token_Class.ASSIGNMENTOPERATOR},
            {"<>",Token_Class.NOTEQUAL},{"&&",Token_Class.AND},{"||",Token_Class.OR},
            {"main",Token_Class.MAIN},{"write",Token_Class.WRITE},{"then",Token_Class.THEN}
        };
        Dictionary<Char, Token_Class> operators = new Dictionary<Char, Token_Class>{
            {';',Token_Class.SEMICOLON},{'+',Token_Class.PLUSOPERATOR},{'-',Token_Class.MINUSOPERATOR},
            {'*',Token_Class.MULTIPLICATIONOPERATOR},{'<',Token_Class.LESSTHAN},{'/',Token_Class.DIVISIONOPERATOR},
            {'>',Token_Class.GREATERTHAN},{'=',Token_Class.EQUALTO},{'{',Token_Class.LEFTCURLYBRACKETS},
            {'(',Token_Class.LEFTPARENTHESES},{')',Token_Class.RIGHTPARENTHESES},{'}',Token_Class.RIGHTCURLYBRACKETS},
            {',',Token_Class.COMMA},
        };
        public Token_Class typeassign(string s)  // return type for lexeme
        {
            if(data.ContainsKey(s)) return data[s];
            return Token_Class.IDENTIFIER;
        }

        public Scanner()
        {
           /* ReservedWords.Add("IF", Token_Class.If);
            ReservedWords.Add("BEGIN", Token_Class.Begin);
            ReservedWords.Add("CALL", Token_Class.Call);
            ReservedWords.Add("DECLARE", Token_Class.Declare);
            ReservedWords.Add("END", Token_Class.End);
            ReservedWords.Add("DO", Token_Class.Do);
            ReservedWords.Add("ELSE", Token_Class.Else);
            ReservedWords.Add("ENDIF", Token_Class.EndIf);
            ReservedWords.Add("ENDUNTIL", Token_Class.EndUntil);
            ReservedWords.Add("ENDWHILE", Token_Class.EndWhile);
            ReservedWords.Add("INTEGER", Token_Class.Integer);
            ReservedWords.Add("PARAMETERS", Token_Class.Parameters);
            ReservedWords.Add("PROCEDURE", Token_Class.Procedure);
            ReservedWords.Add("PROGRAM", Token_Class.Program);
            ReservedWords.Add("READ", Token_Class.Read);
            ReservedWords.Add("REAL", Token_Class.Real);
            ReservedWords.Add("SET", Token_Class.Set);
            ReservedWords.Add("THEN", Token_Class.Then);
            ReservedWords.Add("UNTIL", Token_Class.Until);
            ReservedWords.Add("WHILE", Token_Class.While);
            ReservedWords.Add("WRITE", Token_Class.Write);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("!", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
           */


        }
        public string lexeme = "";
        public Token last;
        public void addtotokens(string s, Token_Class t)
        {
            Token to = new Token(s, t);
            tokens.Add(to);
            last = new Token(s, t);
            lexeme = "";
        }
        public void StartScanning(string tinyCode)
        {
            for(int i=0; i< tinyCode.Length;i++)
            {
                if (Char.IsLetterOrDigit(tinyCode[i]))
                {
                    if (Char.IsLetter(tinyCode[i]))
                    {
                        int j = i;
                        while (j < tinyCode.Length && (Char.IsLetterOrDigit(tinyCode[j])))
                        {
                            lexeme += tinyCode[j];
                            j++;
                        }
                        addtotokens(lexeme, typeassign(lexeme));
                        i = j - 1;
                    }
                    else
                    {
                        int j = i;
                        while (j < tinyCode.Length && (Char.IsDigit(tinyCode[j]) || tinyCode[j] == '.'))
                        {
                            lexeme += tinyCode[j];
                            j++;
                        
                        }
                    bool er = false;
                    while (j < tinyCode.Length && (Char.IsLetter(tinyCode[j])))
                        {
                            er = true;
                            lexeme += tinyCode[j];
                            j++;
                        }
                        if (er)
                        {
                            addtotokens(lexeme, Token_Class.ERROR);
                            
                        }
                        else if (lexeme.Count(x => x == '.') > 1) addtotokens(lexeme, Token_Class.ERROR);
                        else if(lexeme[lexeme.Length-1]=='.') addtotokens(lexeme, Token_Class.ERROR);
                        else addtotokens(lexeme, Token_Class.NUMBER);
                        i = j - 1;
                    }
                }
                else if (tinyCode[i] == '"')
                {
               
                    int j = i;
                    do
                    {
                        lexeme += tinyCode[j];
                        j++;
                    } while (j < tinyCode.Length && tinyCode[j] != '"');
                    if (j == tinyCode.Length && tinyCode[j - 1] != '"') addtotokens(lexeme, Token_Class.ERROR);
                    else
                    {
                        if (lexeme.Length == 1)
                        {
                            addtotokens(lexeme, Token_Class.ERROR);
                            continue;
                        }
                        lexeme += tinyCode[j];
                        addtotokens(lexeme, Token_Class.STRING);
                    }
                    i = j;
                }

                else if (i + 1 < tinyCode.Length && (tinyCode[i] == '/' && tinyCode[i + 1] == '*'))
                {
                    int j = i;
                    lexeme += tinyCode[j];
                    lexeme += tinyCode[j + 1];
                    j += 2;
                    while (j+1< tinyCode.Length && (tinyCode[j] != '*' || tinyCode[j + 1] != '/'))
                    {
                        lexeme += tinyCode[j];
                        j++;
                    }
                    if(i+2== tinyCode.Length)
                    {
                        addtotokens(lexeme, Token_Class.ERROR);
                        i++;
                    }
                    else if (j+1>= tinyCode.Length)
                    {
                        lexeme += tinyCode[j];
                        addtotokens(lexeme, Token_Class.ERROR);
                        String x = lexeme;
                       // Errors.Error_List.Add(x);
                        i = j;
                    }
                    else
                    {
                        lexeme += tinyCode[j];
                        lexeme += tinyCode[j + 1];
                        addtotokens(lexeme, Token_Class.COMMENT); i = j + 1;
                    }
                }
                else if (i + 1 < tinyCode.Length && (tinyCode[i] == '/' && Char.IsLetter(tinyCode[i+1])))
                {
                    lexeme += tinyCode[i];
                    int j = i + 1;
                    while(j<tinyCode.Length)
                    {
                        lexeme +=tinyCode[j];
                        j++;
                    }
                    addtotokens(lexeme, Token_Class.ERROR);
                    i = j;
                }
                else if (tinyCode[i] == '(')
                {
                    lexeme += tinyCode[i];
                    tokens.Add(new Token(lexeme, Token_Class.LEFTPARENTHESES));
                    lexeme = "";

                    if (last != null && last.token_type == Token_Class.IDENTIFIER)
                    {
                        i++;
                        int j = i;
                        while (j < tinyCode.Length && (Char.IsLetterOrDigit(tinyCode[j]) || tinyCode[j] == '_'))
                        {
                            lexeme += tinyCode[j];
                            j++;
                        }
                        if (typeassign(lexeme) == Token_Class.IDENTIFIER) tokens[tokens.Count - 2].token_type = Token_Class.IDENTIFIER;
                        else tokens[tokens.Count - 2].token_type = Token_Class.IDENTIFIER;
                        lexeme = "";
                        i--;
                    }
                    else
                    {
                    }

                }
                else if (tinyCode[i] == '|' && i + 1 < tinyCode.Length && (tinyCode[i + 1] == '|')) { 
                    lexeme += "||"; addtotokens(lexeme, Token_Class.OR); i++; 
                }
                else if (tinyCode[i] == '&' && i + 1 < tinyCode.Length && (tinyCode[i + 1] == '&')) { 
                    lexeme += "&&"; addtotokens(lexeme, Token_Class.AND); i++; 
                }
                else if (tinyCode[i] == ':' && i + 1 < tinyCode.Length && (tinyCode[i + 1] == '=')) { 
                    lexeme += ":="; addtotokens(lexeme, Token_Class.ASSIGNMENTOPERATOR); i++; 
                }

                else if (operators.ContainsKey(tinyCode[i]))
                {
                    if (tinyCode[i] == '<' && i + 1 < tinyCode.Length && (tinyCode[i + 1] == '>')) {
                        lexeme += "<>";
                        addtotokens(lexeme, Token_Class.NOTEQUAL); i++;
                    }
                    else if (tinyCode[i] == '>' && i + 1 < tinyCode.Length && (tinyCode[i + 1] == '='))
                    {
                        lexeme += ">=";
                        addtotokens(lexeme, Token_Class.ERROR); i++;
                    }
                    else
                    {
                        lexeme += tinyCode[i];
                        addtotokens(lexeme, operators[tinyCode[i]]);
                    }
                }
                else if (tinyCode[i] == ' ' || tinyCode[i] == '\n')
                {
                    continue;
                }
                else
                {
                    if (tinyCode[i] == '.')
                    {
                        int j = i;
                        lexeme += tinyCode[j];
                        j++;
                        while (j<tinyCode.Length && (Char.IsLetterOrDigit(tinyCode[j])))
                        {
                            lexeme += tinyCode[j];
                            j++;
                        }
                        i = j;
                        addtotokens(lexeme, Token_Class.ERROR);
                    }
                    else
                    {
                        lexeme += tinyCode[i];
                        addtotokens(lexeme, Token_Class.ERROR);
                    }
                   
                }
            }
            JASON_Compiler.TokenStream = tokens;
        }
       /* void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            

            //Is it an identifier?
            

            //Is it a Constant?

            //Is it an operator?


        }

    

        bool isIdentifier(string lex)
        {
            bool isValid=true;

            
            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;
          
            return isValid;
        }
       */
    }
}
