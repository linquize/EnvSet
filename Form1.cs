using System;
using System.Reflection;
using System.Windows.Forms;

namespace EnvSet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text += " " + Assembly.GetEntryAssembly().GetName().Version;
        }

        void LoadEnvironmentVariables()
        {
            dgvUser.LoadEnvVars();
            dgvSystem.LoadEnvVars();
        }

        void Form1_Load(object sender, EventArgs e)
        {
            LoadEnvironmentVariables();
        }

        void tsbSaveUser_Click(object sender, EventArgs e)
        {
            dgvUser.SaveEnvVars();
            dgvSystem.SaveEnvVars();
            MessageBox.Show("Saved!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void tsbSaveAll_Click(object sender, EventArgs e)
        {
            dgvUser.SaveEnvVars();
            dgvSystem.SaveEnvVars();
            MessageBox.Show("Saved!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void tsbReload_Click(object sender, EventArgs e)
        {
            LoadEnvironmentVariables();
            MessageBox.Show("Reloaded!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
