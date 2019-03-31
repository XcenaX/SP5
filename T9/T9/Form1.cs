using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T9
{
    
    public partial class Form1 : Form
    {
        private List<string> dictionary = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }
        private bool isTyped = false;
        private delegate void Change(string text);
        private delegate void ChangeSelect(int startIndex, int length);

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.SelectedText.Trim() == "") return;
            dictionary.Add(textBox1.SelectedText);
        }

        private void CheckWords()
        {
            foreach(var word in dictionary)
            {
                var splited = textBox1.Text.Split(' ',',','.',':','!','?','(',')','*','-','/','[',']','&','^','$','#');
                var inputText = splited[splited.Length - 1];
                if (inputText == "") return;                
                if (word.Substring(0, inputText.Length) == inputText)
                {
                    if (word.Length == inputText.Length) return;
                    string stringToEnd = word.Substring(inputText.Length, word.Length - inputText.Length);
                    int index = word.Length -inputText.Length ;

                    textBox1.Invoke(new Change((x) => { textBox1.Text += x; }), stringToEnd);
                    textBox1.Invoke(new ChangeSelect((x,y) => { textBox1.SelectionStart = x; textBox1.SelectionLength = y; }),textBox1.Text.Length-index,index);
                    break;
                }
            }
        }



        private void KeyPress(object sender, EventArgs e)
        {

        }

        private void KeyPress(object sender, KeyEventArgs e)
        {
            if (textBox1.SelectionLength > 0)
            {
                textBox1.Text.Remove(textBox1.SelectionStart, textBox1.SelectionLength);
            }
            Thread thread = new Thread(CheckWords)
            {
                IsBackground = true
            };
            thread.Start();            
        }

        private void IsSpace(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && textBox1.SelectionLength > 0)
            {

                textBox1.SelectionLength = 0;
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.SelectionLength = 0;

            }
        }
    }
}
