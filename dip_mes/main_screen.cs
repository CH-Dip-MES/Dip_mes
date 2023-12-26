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
        buy01 buySc1 = new buy01();
        buy02 buySc2 = new buy02();
        sale01 saleSc1 = new sale01();
        sale02 saleSc2 = new sale02();
        product02 productSc2 = new product02();
        manage signSc1 = new manage();
        product01 productSc1 = new product01();
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
            panel.Controls.Add(saleSc2);
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

        private void button6_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(signSc1);
        }
        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(buySc2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(productSc1);

        }
    }
}
