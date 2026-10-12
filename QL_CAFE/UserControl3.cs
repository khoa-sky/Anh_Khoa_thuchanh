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

namespace QL_CAFE
{
    public partial class UserControl3 : UserControl
    {
        public event Action OnClickBan;
        public event Action OnDoubleClickBan;
        public UserControl3()
        {
            InitializeComponent();

            but_tick.Visible = false;

            
            linepanel.Click += Card_View;

           
            linepanel.DoubleClick += Card_Order;
        }
        
        void Card_View(object sender, EventArgs e)
        {
            OnClickBan?.Invoke();
        }

        async void Card_Order(object sender, EventArgs e)
        {
            but_tick.Visible = true; 

            await Task.Delay(150);

            OnDoubleClickBan?.Invoke();
        }
       
       
        public string TenBan
        {
            get { return lab_ten.Text; }   
            set { lab_ten.Text = value; }
        }
        
        
        private async void UserControl3_Click(object sender, EventArgs e)
        {
           
        }

        private void linepanel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void UserControl3_Load(object sender, EventArgs e)
        {

        }

        private void linepanel_Click(object sender, EventArgs e)
        {
            
        }
        public void SetData(string tenBan, string trangThai, string hinhPath)
        {

            lab_ten.Text = tenBan;

            string path = Path.Combine(Application.StartupPath, hinhPath);

            if (File.Exists(path))
            {
                this.BackgroundImage = Image.FromFile(path);
                this.BackgroundImageLayout = ImageLayout.Zoom;
            }

        }
        public void BoChon()
        {
            but_tick.Visible = false;
            this.BackColor = Color.Transparent;
        }
        public void Highlight()
        {
            this.BackColor = Color.FromArgb(0, 200, 83);
        }
    }
}
