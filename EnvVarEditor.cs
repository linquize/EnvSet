using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;

namespace EnvSet
{
    public partial class EnvVarEditor : UserControl
    {
        DataGridView dgvEditor;

        [DefaultValue(EnvironmentVariableTarget.User)]
        public EnvironmentVariableTarget Target { get; set; }

        public EnvVarEditor()
        {
            Target = EnvironmentVariableTarget.User;
            dgvEditor = new DataGridView
            {
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Dock = DockStyle.Fill,
                RowHeadersVisible = false
            };
            dgvEditor.DefaultCellStyle = new DataGridViewCellStyle(dgvEditor.DefaultCellStyle) { WrapMode = DataGridViewTriState.True };
            dgvEditor.RowTemplate.Height = 18;
            dgvEditor.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", Width = 100 });
            dgvEditor.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Value", Width = 100 });
            this.Controls.Add(dgvEditor);
        }

        public void LoadEnvVars()
        {
            RegistryKey loKey;
            string lsKey;
            if (Target == EnvironmentVariableTarget.User)
            {
                loKey = Registry.CurrentUser;
                lsKey = "Environment";
            }
            else if (Target == EnvironmentVariableTarget.Machine)
            {
                loKey = Registry.LocalMachine;
                lsKey = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
            }
            else
                throw new InvalidOperationException("Target must be User or Machine");

            dgvEditor.Rows.Clear();
            RegistryKey loSubKey = loKey.OpenSubKey(lsKey);
            string[] lsaKeys = loSubKey.GetValueNames();
            object[] laValues = Array.ConvertAll(lsaKeys, a => loSubKey.GetValue(a, null, RegistryValueOptions.DoNotExpandEnvironmentNames));
            for (int i = 0; i < lsaKeys.Length; i++)
                dgvEditor.Rows.Add(lsaKeys[i], laValues[i]);
        }

        public void SaveEnvVars()
        {
            RegistryKey loKey;
            string lsKey;
            if (Target == EnvironmentVariableTarget.User)
            {
                loKey = Registry.CurrentUser;
                lsKey = "Environment";
            }
            else if (Target == EnvironmentVariableTarget.Machine)
            {
                loKey = Registry.LocalMachine;
                lsKey = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
            }
            else
                throw new InvalidOperationException("Target must be User or Machine");

            RegistryKey loSubKey = loKey.OpenSubKey(lsKey, true);
            for (int i = 0; i < dgvEditor.Rows.Count - 1; i++)
            {
                string lsName = (string)dgvEditor[0, i].Value;
                string lsValue = (string)dgvEditor[1, i].Value;
                RegistryValueKind leKind = HasEnvVar(lsValue) ? RegistryValueKind.ExpandString : RegistryValueKind.String;
                loSubKey.SetValue(lsName, lsValue, leKind);
            }
            NativeMethods.SendMessageTimeout(new IntPtr(0xffff), NativeMethods.WM_SETTINGCHANGE, IntPtr.Zero, "Environment", 0, 1000, IntPtr.Zero);
        }

        static bool HasEnvVar(string asString)
        {
            int liIndex = asString.IndexOf('%');
            if (liIndex < 0 || liIndex >= asString.Length - 1) return false;
            return asString.IndexOf('%', liIndex + 1) >= 0;
        }
    }
}
