using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Searcher
{
    public partial class Form1 : Form
    {
        string[] icotypes = new string[] { "jpg", "png", "ico", "bmp", "mp4", };
        string path = "", filter = "", NameTitle = "Searcher";
        string[] everything = new string[0];
        int selfile = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            path = textBox1.Text;
            filter = textBox2.Text == "" ? "*.*" : textBox2.Text;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Not An Existing Path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.SelectAll();
                textBox1.Focus();
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            Array.Clear(everything, 0, everything.Length);
            everything = dir.EnumerateFiles(filter, SearchOption.AllDirectories)
                            .Select(x => x.FullName)
                            .ToArray();
            listView1.Clear();
            imageList1.Images.Clear();
            int step = 1000 / everything.Length;
            progressBar1.Value = 0;
            for (int i = 0, ii = 0; i < everything.Length; i++)
            {
                try
                {
                    imageList1.Images.Add(Image.FromFile(everything[i]));
                    listView1.Items.Add(new ListViewItem(everything[i].Split('\\').Last(), ii));
                    ii++;
                }
                catch
                {
                    listView1.Items.Add(new ListViewItem(everything[i].Split('\\').Last()));
                }
                if(progressBar1.Value + step <= 1000)
                    progressBar1.Value += step;
            }
            Text = NameTitle + " - " + everything.Length.ToString() + " items";
            progressBar1.Value = 0;
        }

        private void flagBtn_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(everything[selfile]);
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            selfile = e.ItemIndex;
            FileInfo file = new FileInfo(everything[selfile]);
            listBox1.Items.Clear();
            listBox1.Items.AddRange(new object[] { "Name:\t\t" + file.Name,
                "Location:\t\t" + file.Directory,
                "Size:\t\t" + file.Length + " bytes",
                "Created:\t\t" + file.CreationTime,
                "Modified:\t\t" + file.LastWriteTime,
                "Accessed:\t" + file.LastAccessTime });
        }
    }
}
