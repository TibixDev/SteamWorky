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
using System.IO.Compression;

namespace SteamWorky
{
    public partial class Form1 : Form
    {
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        OpenFileDialog ofd = new OpenFileDialog();
        string path32s;
        string path64s;
        string path32cream;
        string path64cream;
        string outputS;
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        bool copyDone32;
        bool copyDone64;
        bool crEditDone;
        string zipFile;
        Random rnd = new Random();
        private bool _mouseDown;
        private Point _lastLocation;

        public Form1()
        {
            InitializeComponent();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuCheckbox1_OnChange(object sender, EventArgs e)
        {
            if (bunifuCheckbox1.Checked)
            {
                path32.Visible = true;
                path32l.Visible = true;
                btn32.Visible = true;
            }

            else
            {
                path32.Visible = false;
                path32l.Visible = false;
                btn32.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            path32.Visible = false;
            path32l.Visible = false;
            btn32.Visible = false;
            path64.Visible = false;
            path64l.Visible = false;
            btn64.Visible = false;
            copyDone32 = false;
            copyDone64 = false;
            bunifuProgressBar1.Visible = false;
        }

        private void bunifuCheckbox2_OnChange(object sender, EventArgs e)
        {
            if (bunifuCheckbox2.Checked)
            {
                path64.Visible = true;
                path64l.Visible = true;
                btn64.Visible = true;
            }

            else
            {
                path64.Visible = false;
                path64l.Visible = false;
                btn64.Visible = false;
            }
        }

        private void btn32_Click(object sender, EventArgs e)
        {
            OpenFileDialog Search = new OpenFileDialog();
            Search.ShowDialog();
            path32s = Search.FileName;
            path32.Text = path32s;
        }

        private void btn64_Click(object sender, EventArgs e)
        {
            OpenFileDialog Search = new OpenFileDialog();
            Search.ShowDialog();
            path64s = Search.FileName;
            path64.Text = path64s;
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            fbd.RootFolder = System.Environment.SpecialFolder.Desktop;
            DialogResult result = this.fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                outputS = fbd.SelectedPath;
                outputP.Text = fbd.SelectedPath;
            }
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(outputS + @"\Result");
            if (bunifuCheckbox1.Checked == false && bunifuCheckbox2.Checked == false)
            {
                MessageBox.Show("Please select at least one API and it's location.");
            }
            if (bunifuCheckbox1.Checked && outputP.Text != "" && path32.Text != "")
            {
                string outputF = outputS + @"\Result\steam_api_o.dll";
                FileInfo f32 = new FileInfo(path32s);
                f32.CopyTo(outputF, true);

                path32cream = appPath + @"\steam_api.dll";
                FileInfo f32cream = new FileInfo(path32cream);
                string outputF32cream = outputS + @"\Result\steam_api.dll";
                f32cream.CopyTo(outputF32cream);
                copyDone32 = true;
            }

            if (bunifuCheckbox2.Checked && outputP.Text != "" && path64.Text != "")
            {
                string outputF64 = outputS + @"\Result\steam_api64_o.dll";
                string outputF64cream = outputS + @"\Result\steam_api64.dll";
                FileInfo f64 = new FileInfo(path64s);
                f64.CopyTo(outputF64, true);

                path64cream = appPath + @"\steam_api64.dll";
                FileInfo f64cream = new FileInfo(path64cream);
                f64cream.CopyTo(outputF64cream);
                copyDone64 = true;
            }

            if (copyDone32 || copyDone64 && appID.Text != "")
            {
                string creamPath = appPath + @"\cream_api.ini";
                string creamText = File.ReadAllText(creamPath);
                string appIDstr = appID.Text;
                creamText = creamText.Replace("531510", appIDstr);
                string newCream = outputS + @"\Result\cream_api.ini";
                File.WriteAllText(newCream, creamText);
                crEditDone = true;
            }

            if (crEditDone)
            {
                bunifuProgressBar1.Visible = true;
                timer1.Start();
                string appIDstr = appID.Text;
                zipFile = outputS + @"\" + appIDstr + ".SteamWorky.zip";
                string folderToZip = outputS + @"\Result";
                ZipFile.CreateFromDirectory(folderToZip, zipFile, CompressionLevel.Optimal, true);

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bunifuProgressBar1.Value = bunifuProgressBar1.Value + 1;
        }

        private void bunifuProgressBar1_progressChanged(object sender, EventArgs e)
        {
            if (bunifuProgressBar1.Value == bunifuProgressBar1.MaximumValue)
            {
                timer1.Stop();
                bunifuProgressBar1.Visible = false;
                bunifuProgressBar1.Value = 0;
                Directory.Delete(outputS + @"\Result", true);
                MessageBox.Show("All done!" + Environment.NewLine + "Location: " + zipFile);
                
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            _lastLocation = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                Location = new Point(
                    (Location.X - _lastLocation.X) + e.X, (Location.Y - _lastLocation.Y) + e.Y);

                Update();
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }
    }
}