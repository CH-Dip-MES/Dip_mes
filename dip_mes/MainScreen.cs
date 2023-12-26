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
        private Boolean showpanelbase = false;
        private Boolean showpanelsale = false;
        private Boolean showpanelbuy = false;
        private Boolean showpanelproduction = false;
        private Boolean showpaneluser = false;

        buy01 buySc1 = new buy01();
        buy02 buySc2 = new buy02();
        sale01 saleSc1 = new sale01();
        sale02 saleSc2 = new sale02();
        product02 productSc2 = new product02();
        manage signSc1 = new manage();
        product01 productSc1 = new product01();

        public MainScreen()
        {
            InitializeComponent();

            toglepanels();
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
   
        private void button7_Click_1(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(saleSc3);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(productSc1);
        }

        

        private void button9_Click(object sender, EventArgs e)
        {
            showpanelsale = !showpanelsale;
            toglepanels();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            showpanelbase = !showpanelbase;
            toglepanels();
        }

        private void toglepanels()
        {
            if (showpanelbase)
            {
                panelbase.Height = 184;
            }
            else
            {
                panelbase.Height = 0;
            }

            if (showpanelsale)
            {
                panelsale.Height = 138;
            }
            else
            {
                panelsale.Height = 0;
            }

            if (showpanelbuy)
            {
                panelbuy.Height = 92;
            }
            else
            {
                panelbuy.Height = 0;
            }
            
            if (showpanelproduction)
            {
                panelproduction.Height = 138;
            }
            else
            {
                panelproduction.Height = 0;
            }
            
            if (showpaneluser)
            {
                paneluser.Height = 92;
            }
            else
            {
                paneluser.Height = 0;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            showpanelbuy = !showpanelbuy;
            toglepanels();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            showpanelproduction = !showpanelproduction;
            toglepanels();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            showpaneluser = !showpaneluser;
            toglepanels();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(saleSc2);
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(buySc2);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
        
        }
    }
}
