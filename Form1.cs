using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public partial class Form1 : Form
    {
        public string[] rsrvdwrds = {"read","write","repeat","until","if","elseif","else","then","return","endl","while","program","main"};
        public string[] dtatyps = { "int", "float", "string" }; 
        
        public void CheckKeyword(string word, Color color, int startIndex)
        {
            if (this.richTextBox1.Text.Contains(word))
            {
                int index = -1;
                int selectStart = this.richTextBox1.SelectionStart;
                while ((index = this.richTextBox1.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.richTextBox1.Select((index + startIndex), word.Length);
                    this.richTextBox1.SelectionColor = color;
                    this.richTextBox1.Select(selectStart, 0);
                   
                    this.richTextBox1.SelectionColor = Color.Black;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        void PrintTokens()
        {
            for (int i = 0; i < JASON_Compiler.Jason_Scanner.tokens.Count; i++)
            {
               dataGridView1.Rows.Add(JASON_Compiler.Jason_Scanner.tokens.ElementAt(i).lex, JASON_Compiler.Jason_Scanner.tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            /*for(int i=0; i<Errors.Error_List.Count; i++)
            {
                textBox2.Text += Errors.Error_List[i];
                textBox2.Text += "\r\n";
                listBox1.Items.Add(Errors.Error_List[i]);
            }*/
            for (int i = 0; i < JASON_Compiler.Jason_Scanner.tokens.Count; i++)
            {
                if (JASON_Compiler.Jason_Scanner.tokens.ElementAt(i).token_type == Token_Class.ERROR)
                {
                    String tst = JASON_Compiler.Jason_Scanner.tokens.ElementAt(i).lex;
                    textBox2.Text += tst;
                    textBox2.Text += "\r\n";
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < rsrvdwrds.Length; i++) this.CheckKeyword(rsrvdwrds[i], Color.Blue, 0);
            for (int i = 0; i < dtatyps.Length; i++) this.CheckKeyword(dtatyps[i], Color.Green, 0);
        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            richTextBox1.Text = "";
            dataGridView1.Rows.Clear();
            JASON_Compiler.TokenStream.Clear();
            dataGridView1.Rows.Clear();
            treeView1.Nodes.Clear();

        }

        private void button_WOC2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            
            //string Code=textBox1.Text.ToLower();
            string Code = richTextBox1.Text;
            JASON_Compiler.Start_Compiling(Code);
            PrintTokens();
            //   PrintLexemes();

            PrintErrors();
            Parser ps = new Parser();
            ps.StartParsing(JASON_Compiler.TokenStream);
            treeView1.Nodes.Add(ps.root);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        /*  void PrintLexemes()
{
for (int i = 0; i < JASON_Compiler.Lexemes.Count; i++)
{
textBox2.Text += JASON_Compiler.Lexemes.ElementAt(i);
textBox2.Text += Environment.NewLine;
}
}*/
    }
}   
