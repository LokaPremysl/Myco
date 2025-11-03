//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Mush.WinForms
//{
//    public partial class SpawnDialog : Form
//    {
//        public SpawnDialog()
//        {
//            InitializeComponent();
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mush.WinForms
{
    public partial class SpawnDialog : Form
    {
        public string Material => txtMaterial.Text.Trim();
        public DateTime SelectedDate => dtpDate.Value.Date;

        TextBox txtMaterial;
        DateTimePicker dtpDate;

        public SpawnDialog()
        {
            InitializeComponent();

            dtpDate = new DateTimePicker();
            //this.dtpDate = new System.Windows.Forms.TextBox();
            this.dtpDate.Location = new System.Drawing.Point(12, 80);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 23);
            this.Controls.Add(this.dtpDate);

            dtpDate.Value = DateTime.Today;

            txtMaterial = new TextBox();
            //this.txtMaterial = new System.Windows.Forms.TextBox();
            this.txtMaterial.Location = new System.Drawing.Point(12, 25);
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Size = new System.Drawing.Size(200, 23);
            this.Controls.Add(this.txtMaterial);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
        }

        //Controls.Add(dataGridSpawn);
        private readonly Button btnOk = new()
        {
            Text = "OK",
            Left = 116,
            Top = 90,
            Width = 75,
            DialogResult = DialogResult.OK
        };




        private readonly Button btnCancel = new()
        {
            Text = "Zrušit",
            Left = 197,
            Top = 90,
            Width = 75,
            DialogResult = DialogResult.Cancel
        };
    }
}
