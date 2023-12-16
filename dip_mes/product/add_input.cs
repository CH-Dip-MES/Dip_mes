using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_mes.product
{
    public partial class add_input : Form
    {
        public add_input()
        {
            InitializeComponent();
        }

        private void add_input_Load(object sender, EventArgs e)
        {

        }
        
        public void DisplayDataInTextBox1(string data)
        {
            textBox1.Text = data;
        }
        public void DisplayDataInTextBox2(string data)
        {
            textBox2.Text = data;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
