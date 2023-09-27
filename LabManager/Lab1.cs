using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LabManager
{
    public partial class Lab1 : Form
    {
        Form1 f1;

        Color[] colors = { Color.Red, Color.Orange, Color.Gold, Color.Green, Color.LightBlue, Color.Blue, Color.Purple};

        public Lab1(Form1 f)
        {
            InitializeComponent();
            this.f1 = f;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            try
            {

                openFileDialog1.Filter = "Text files(*.txt)|*.txt| All files(*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                string FileName = openFileDialog1.FileName;
                string FileText = System.IO.File.ReadAllText(FileName);
                richTextBox1.Text = FileText;

            }
            catch (Exception ex)
            {
                // Фиксирование исключения на главной форме проекта
                MessageBox.Show(ex.Message);

                // Запись исключения в текстовый файл
                string exceptionText = $"{DateTime.Now}:{ex.InnerException},\n{ex.Message},\n{ex.Source},\n{ex.StackTrace},\n{ex.TargetSite}\n";
                File.AppendAllText("exceptions.txt", exceptionText);

            }
        }

        private void SetSelectionStyle(int address, int len, FontStyle style, Color select_color)
        {
            richTextBox1.Select(address, len);
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style | style);
            richTextBox1.SelectionColor = select_color;
        }

        private void HighlightMatches(object sender, EventArgs e)
        {
            try
            {

                string s = richTextBox1.Text;
                string signature = comboBox1.Text;
                textBox1.Text = "All appearances of " + signature + " in text:" + "\r\n";
                Regex regex = new Regex(signature, RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(s);

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        Color clr = colors[comboBox1.SelectedIndex];
                        textBox1.Text += "Postion: " + (match.Index + 1) + " - " + match.Value + "\r\n";
                        SetSelectionStyle(match.Index, match.Length, FontStyle.Bold, clr);
                    }

                }
                else
                {
                    textBox1.Text = "No matches";
                }
            }
            catch (Exception ex)
            {
                // Фиксирование исключения на главной форме проекта
                MessageBox.Show(ex.Message);
                // Запись исключения в текстовый файл
                string exceptionText = $"{DateTime.Now}:{ex.InnerException},\n{ex.Message},\n{ex.Source},\n{ex.StackTrace},\n{ex.TargetSite}\n";
                File.AppendAllText("exceptions.txt", exceptionText);
            }
        }
    }
}
