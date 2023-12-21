using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_mes
{
    public partial class main_screen : Form
    {
        buy.buy01 buySc1 = new buy.buy01();
        sale.sale01 saleSc1 = new sale.sale01();
        product.product02 productSc2 = new product.product02();
        standard stanSc = new standard();
        goods.standard02 stanSc2 = new goods.standard02();
        public main_screen()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form_load(object sender, PaintEventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(buySc1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(saleSc1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(productSc2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(stanSc);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(stanSc2);
        }
    }
}
