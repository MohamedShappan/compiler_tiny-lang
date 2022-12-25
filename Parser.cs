using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;


namespace JASON_Compiler
{
    //int x:=4;
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        public TreeNode root = new TreeNode("Program");
        public TreeNode statements = new TreeNode("Statements");
        public TreeNode functions = new TreeNode("Functions");
        public TreeNode cc1;
        Stack<char> parentheses = new Stack<char>();
        public List<TreeNode> children = new List<TreeNode>();
        public List<Token> list;
        public string myStart;
        public int temp = 0;
        public ListBox ll = new ListBox();

        public int ind = 0;
        public Parser()
        {

        } //constructor
        public void StartParsing(List<Token> tokenslist)
        {
            root.Nodes.Add(statements);
            root.Nodes.Add(functions);
            list = tokenslist;
            children.Clear();
            bool flag = false;
            for (int i = 0; i < tokenslist.Count; i++)
            {
                myStart = "";
                ind = i;
                if (tokenslist[i].token_type == Token_Class.READ) { flag = true; read(); }
                else if (tokenslist[i].token_type == Token_Class.REPEATSTATEMENT) { repeat(); flag = true; }
                else if (tokenslist[i].token_type == Token_Class.IF) { ifSatament(); flag = true; }
                else if (tokenslist[i].token_type == Token_Class.MAIN) { mainFunction(); flag = true; }
                else if (tokenslist[i].token_type == Token_Class.LEFTCURLYBRACKETS) { functionBody(); flag = true; }
                else if (tokenslist[i].token_type == Token_Class.WRITE) { flag = true; write(); }
                else if (tokenslist[i].token_type == Token_Class.RETURN) { flag = true; ritorno(); }
                else if (tokenslist[i].token_type == Token_Class.IDENTIFIER) { bool s = assignment(); if (!s) { children.Clear(); ind = temp; functionCall(); } else { PrintParseTree(root, children, "Assignment Statement"); ind--; flag = true; } }
                else if (tokenslist[i].token_type == Token_Class.NUMBER || tokenslist[i].token_type == Token_Class.LEFTPARENTHESES) { flag = true; expression(); }
                else if (i + 1 < tokenslist.Count && i + 2 < tokenslist.Count && ((tokenslist[i].token_type == Token_Class.DATATYPEFLOAT) || (tokenslist[i].token_type == Token_Class.DATATYPEINT) || (tokenslist[i].token_type == Token_Class.DATATYPESTRING)) && tokenslist[i + 1].token_type == Token_Class.IDENTIFIER && tokenslist[i + 2].token_type == Token_Class.LEFTPARENTHESES) { flag = true; functionDec(); ind--; }
                else if (((tokenslist[i].token_type == Token_Class.DATATYPEFLOAT) || (tokenslist[i].token_type == Token_Class.DATATYPEINT) || (tokenslist[i].token_type == Token_Class.DATATYPESTRING))) { flag = true; decStatment(); }
                if (flag) i = ind;
                children.Clear();
            }

        }

        public bool assignment()
        {
            temp = ind;
            if (myStart == "") myStart = "assignment";
            bool c1 = match(Token_Class.IDENTIFIER);
            bool c2 = match(Token_Class.ASSIGNMENTOPERATOR);
            if (!c2 || !c1) { return false; }
            bool c3 = expression();

            bool c4 = match(Token_Class.SEMICOLON);
            return c1 && c2 && c3 && c4;
        }
        public bool match(Token_Class x)
        {
            if (ind < list.Count && list[ind].token_type == x)
            {
                children.Add(new TreeNode((list[ind].lex.ToString() + " ^ " + list[ind].token_type.ToString())));
                ind++;
                return true;
            }
            return false;
        }

        public bool read()
        {

            if (myStart == "") myStart = "read";
            bool c1 = match(Token_Class.READ);
            bool c2 = match(Token_Class.IDENTIFIER);
            bool c3 = match(Token_Class.SEMICOLON);
            if (c1 && c2 && c3) { PrintParseTree(statements, children, "Read Statement"); ind--; return true; }
            return false;
        }

        public bool ritorno()
        {
            if (myStart == "") myStart = "return";
            bool c1 = match(Token_Class.RETURN);
            bool c2 = expression();
            bool c3 = match(Token_Class.SEMICOLON);
            if (c1 && c2 && c3) { if (myStart == "return") { PrintParseTree(statements, children, "Return Statement"); ind--; return true; } else return true; }
            return false;
        }

        public bool expression()
        {
            bool c1 = term();
            bool c2 = espressione();
            if (c1 || c2)
            {
                if (myStart == "assignment")
                { return true; }
                else if (myStart == "read") { PrintParseTree(statements, children, "Read Statement"); return true; }
                else if (myStart == "return") { return true; }
                else if (myStart == "repeat") return true;
                else if (myStart == "write") { return true; }
                else if (myStart == "if") { return true; }
                else
                { return true; }
            }
            return c1 || c2;
        }

        public bool espressione()
        {
            bool c1 = match(Token_Class.PLUSOPERATOR) || match(Token_Class.MINUSOPERATOR);
            bool c2 = term();
            bool c3 = false;
            if (c1 && c2)
            { c3 = espressione(); }
            return c1 && c2 && c3;
        }

        public bool write()
        {
            if (myStart == "") myStart = "write";
            bool c1 = match(Token_Class.WRITE);
            bool c2 = expression();
            bool c3 = match(Token_Class.ENDLINE);
            bool c4 = match(Token_Class.SEMICOLON);
            if ((c1 && c2 && c4) || (c1 && c3 && c4)) { if (myStart != "write") return true; else { PrintParseTree(statements, children, "Write Statement"); ind--; return true; } }
            return ((c1 && c2 && c3) || (c3));
        }

        public bool factor()
        {
            bool c1 = match(Token_Class.LEFTPARENTHESES), c2 = false, c3 = false;
            if (parentheses.Count >= 1 && match(Token_Class.RIGHTPARENTHESES)) { parentheses.Pop(); c3 = true; }
            else if (c1)
            {
                parentheses.Push('(');
                c2 = expression();
                c3 = match(Token_Class.RIGHTPARENTHESES);
            }
            bool c4 = match(Token_Class.NUMBER);
            bool c5 = match(Token_Class.STRING);
            bool c6 = match(Token_Class.IDENTIFIER);
            bool c7 = functionCall();
            return (c1 && c2 && c3) || c4 || c5 || c6 || c7;
        }

        public bool functionDec()
        {
            bool c1 = match(Token_Class.DATATYPEFLOAT) || match(Token_Class.DATATYPEINT) || match(Token_Class.DATATYPESTRING);
            bool c2 = match(Token_Class.IDENTIFIER);
            bool c3 = match(Token_Class.LEFTPARENTHESES);
            bool c4 = parameter();
            bool c6 = false;
            if (c4 == false) c6 = match(Token_Class.RIGHTPARENTHESES);
            while (c4)
            {
                bool c5 = match(Token_Class.COMMA);
                if (c5) c4 = parameter();
                else { c6 = match(Token_Class.RIGHTPARENTHESES); c4 = false; }
            }
            //if(!c4 && c5) error
            if (c1 && c2 && c3 && c6) { PrintParseTree(functions, children, "Function Declaration"); return true; }
            else return false;

        }

        public bool parameter()
        {
            bool c1 = match(Token_Class.DATATYPEFLOAT) || match(Token_Class.DATATYPEINT) || match(Token_Class.DATATYPESTRING);
            if (!c1) return false;
            bool c2 = match(Token_Class.IDENTIFIER);
            return c1 && c2;
        }
        public bool term()
        {
            bool c1 = factor();
            if (c1) return true;
            bool c2 = termine();
            return c1 || c2;
        }
        public bool termine()
        {
            bool c1 = match(Token_Class.MULTIPLICATIONOPERATOR) || match(Token_Class.DIVISIONOPERATOR) || match(Token_Class.PLUSOPERATOR) || match(Token_Class.MINUSOPERATOR);
            if (!c1) return false;
            bool c2 = factor();
            if (!c2) return false;
            bool c3 = false;
            if (c1 && c2) c3 = termine();
            return c1 && c2 && c3;
        }

        public bool functionCall()
        {
            bool c1 = match(Token_Class.IDENTIFIER);
            bool c2 = functionPart();
            if (c1 && c2) { PrintParseTree(functions, children, "Function Call"); return true; }
            else return false;


        }

        public bool functionPart()
        {
            bool c1 = match(Token_Class.LEFTPARENTHESES);
            bool c2 = match(Token_Class.IDENTIFIER);
            bool c4 = false;
            if (c2) while (c2)
                {
                    bool c3 = match(Token_Class.COMMA);
                    if (c3) c2 = match(Token_Class.IDENTIFIER);
                    else { c4 = match(Token_Class.RIGHTPARENTHESES); c2 = false; }
                }
            else c4 = match(Token_Class.RIGHTPARENTHESES);

            return (c1 && c4);

        }

        public bool decStatment()
        {
            if (myStart == "") myStart = "declarationstatement";
            bool c1 = match(Token_Class.DATATYPEFLOAT) || match(Token_Class.DATATYPEINT) || match(Token_Class.DATATYPESTRING);
            bool c2 = true;
            bool c3 = true;
            bool c4 = true;
            bool c9 = true;
            if (c2) while (c4)
                {
                    c2 = assignment();
                    c3 = match(Token_Class.COMMA);
                    if (c3) c4 = true;
                    else c4 = false;
                }
            bool c5 = match(Token_Class.SEMICOLON);
            if (list[ind - 1].token_type == Token_Class.SEMICOLON) c5 = true;
            if (c1 && !c3 && c5) { PrintParseTree(statements, children, "Declaration_Statement"); ind--; return true; }
            return c1 && !c3 && c5;

        }

        public bool ifSatament()
        {
            int temp = ind;
            if (myStart == "") myStart = "if";
            bool c1 = match(Token_Class.IF);
            bool c2 = conditionStatement();
            bool c3 = match(Token_Class.THEN);
            bool c4 = true;
            int checker = 0;
            while (c4)
            {
                int tmp = ind;
                c4 = write();
                if (!c4) { ind = tmp; tmp = ind; c4 = read(); }
                if (!c4) { ind = tmp; tmp = ind; c4 = ritorno(); }
                if (c4) checker++;
            }

            bool c5 = elseIf();
            bool c6 = elseStatement();
            bool c7 = match(Token_Class.END);
            if ((c1 && c2 && c3 && checker > 0) && (c5 || c6 || c7)) { PrintParseTree(statements, children, "If Statement"); ind--; return true; }
            return false;
        }

        public bool functionBody()
        {
            if (myStart == "") myStart = "functionBody";
            bool c1 = match(Token_Class.LEFTCURLYBRACKETS);
            int check = 0;
            bool c2 = true;
            int x = 0;
            while (c2)
            {
                int tmp = ind;
                c2 = write();
                if (!c2) { ind = tmp; tmp = ind; c2 = assignment(); }
                if (!c2) { ind = tmp; tmp = ind; c2 = read(); }
                //if (!c2) { ind = tmp; tmp = ind; c2 = ifSatament(); }
                if (!c2) { ind = tmp; tmp = ind; c2 = decStatment(); }
                if (!c2) { ind = tmp; tmp = ind; c2 = ritorno(); if (c2) x++; }
                if (c2) check++;
            }
            bool c4 = match(Token_Class.RIGHTCURLYBRACKETS);
            if (c1 && check > 0 && c4 && x < 2) { if (myStart == "functionBody") { PrintParseTree(functions, children, "Function Body"); ind--; return true; } else return true; }
            if (x >= 2) { ll.Items.Add("two return statements in same functionBody"); }
            return c1 && check > 0 && c4 && x < 2;
        }

        public bool functionStatement()
        {
            bool c1 = functionDec();
            bool c2 = functionBody();
            if (c1 && c2) { PrintParseTree(statements, children, "Function Statement"); ind--; return true; }
            return c1 && c2;
        }

        public bool mainFunction()
        {
            if (myStart == "") myStart = "mainFunction";
            bool c1 = match(Token_Class.MAIN);
            bool c2 = match(Token_Class.LEFTPARENTHESES);
            bool c3 = match(Token_Class.RIGHTPARENTHESES);
            bool c4 = functionBody();
            if (c1 && c2 && c3 && c4) { PrintParseTree(root, children, "Main Function"); ind--; return true; }
            return c1 && c2 && c3 && c4;
        }

        public bool elseIf()
        {
            bool c1 = match(Token_Class.ELSEIF);
            if (!c1) return false;
            bool c2 = conditionStatement();
            bool c3 = match(Token_Class.THEN);
            bool c4 = write() || read() || ritorno() || assignment();
            bool c5 = true;// elseIf();
            bool c6 = elseStatement();
            bool c7 = match(Token_Class.END);
            return ((c1 && c2 && c3 && c4) && (c5 || c6 || c7));
        }

        public bool elseStatement()
        {
            bool c1 = match(Token_Class.ELSE);
            if (!c1) return false;
            bool c2 = true;
            int check = 0;
            while (c2)
            {
                int tmp = ind;
                c2 = write();
                if (!c2) { ind = tmp; tmp = ind; c2 = assignment(); }
                if (!c2) { ind = tmp; tmp = ind; c2 = read(); }
                //if (!c2) { ind = tmp; tmp = ind; c2 = ifSatament(); }
                if (!c2) { ind = tmp; tmp = ind; c2 = decStatment(); }
                if (c2) check++;
            }
            //bool c3 = match(Token_Class.END);
            return c1 && check > 0;// && c3;
        }

        public bool repeat()
        {
            if (myStart == "") myStart = "repeat";
            bool c1 = match(Token_Class.REPEATSTATEMENT);
            int check = 1;
            bool c2 = true;
            while (c2)
            {
                int tmp = ind;
                c2 = write();
                if (!c2) { ind = tmp; tmp = ind; c2 = assignment(); }
                if (!c2) { ind = tmp; tmp = ind; c2 = read(); }
                if (!c2) { ind = tmp; tmp = ind; c2 = ifSatament(); }
                if (!c2) { ind = tmp; tmp = ind; c2 = decStatment(); }
                if (c2) check++;
            }
            bool c3 = match(Token_Class.UNTIL);
            bool c4 = conditionStatement();
            if (c1 && check > 0 && c3 && c4) { PrintParseTree(statements, children, "Repeat Statement"); ind--; return true; }
            return c1 && check > 0 && c3 && c4;
        }

        public bool conditionStatement()
        {
            bool c3 = false;
            bool c1 = condition();
            bool c2 = match(Token_Class.OR);
            if (c2) c3 = conditionTerm();
            else c3 = false;

            return ((c1 && c2 && c3) || c1);
        }
        public bool condition()
        {
            bool c1 = expression();
            bool c2 = match(Token_Class.GREATERTHAN) || match(Token_Class.LESSTHAN) || match(Token_Class.EQUALTO) || match(Token_Class.NOTEQUAL);
            bool c3 = expression();
            return c1 && c2 && c3;
        }

        public bool conditionTerm()
        {
            bool c1 = conditionStatement();
            bool c2 = conditionTermine();
            return c1 && c2;

        }

        public bool conditionTermine()
        {
            bool c1 = match(Token_Class.AND);
            bool c2 = condition();
            return c1 && c2;
        }
        public void PrintParseTree(TreeNode rooter, List<TreeNode> tn, string child)
        {
            //statements.Remove();
            //rooter = root;
            cc1 = new TreeNode(child);
            for (int i = 0; i < tn.Count; i++)
            {
                cc1.Nodes.Add(tn[i]);
            }
            rooter.Nodes.Add(cc1);
            //root.Nodes.Add(rooter);
            tn.Clear();
            children.Clear();
        }

    }
}
