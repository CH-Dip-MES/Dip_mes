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
    public partial class MainScreen : Form
    {
        MaterialOrder buySc1 = new MaterialOrder();
        OrderList buySc2 = new OrderList();
        Logistics saleSc1 = new Logistics();
        Sale saleSc2 = new Sale();
        Order productSc2 = new Order();
        manage signSc1 = new manage();
        InputMaterial productSc1 = new InputMaterial();
        
        public MainScreen()
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
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(stanSc2);
        }
    }
}
