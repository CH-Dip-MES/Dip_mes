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
        private Boolean showpanelStandard = false;
        private Boolean showpanelSale = false;
        private Boolean showpanelBuy = false;
        private Boolean showpanelManufacture = false;
        private Boolean showpanelUser = false;

        //기준정보
        Product ProductSc = new Product();
        Material MaterialSc = new Material();
        Process ProcessSc = new Process();
        Client ClientSc = new Client();
        //영업관리
        Sale SaleSc = new Sale();
        Logistics LogisticsSc = new Logistics();
        SaleResult SaleResultSc = new SaleResult();
        //구매관리
        MaterialOrder MaterialOrderSc = new MaterialOrder();
        OrderList OrderListSc = new OrderList();
        //생산관리
        InputMaterial InputMaterialSc = new InputMaterial();
        Order OrderSc = new Order();
        MfgResult MfgResultSc = new MfgResult();
        //회원관리
        Manage ManageSc = new Manage();

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

        

        

        private void button7_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(MaterialOrderSc);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(InputMaterialSc);
        }

        

        private void buttonsale_Click(object sender, EventArgs e)
        {
            showpanelSale = !showpanelSale;
            toglepanels();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            showpanelStandard = !showpanelStandard;
            toglepanels();
        }

        private void toglepanels()
        {
            if (showpanelStandard)
            {
                panelbase.Height = 184;
            }
            else
            {
                panelbase.Height = 0;
            }

            if (showpanelSale)
            {
                panelsale.Height = 138;
            }
            else
            {
                panelsale.Height = 0;
            }

            if (showpanelBuy)
            {
                panelbuy.Height = 92;
            }
            else
            {
                panelbuy.Height = 0;
            }
            
            if (showpanelManufacture)
            {
                panelproduction.Height = 138;
            }
            else
            {
                panelproduction.Height = 0;
            }
            
            if (showpanelUser)
            {
                paneluser.Height = 46;
            }
            else
            {
                paneluser.Height = 0;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            showpanelBuy = !showpanelBuy;
            toglepanels();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            showpanelManufacture = !showpanelManufacture;
            toglepanels();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            showpanelUser = !showpanelUser;
            toglepanels();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(SaleSc);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(OrderListSc);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(ProductSc);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(MaterialSc);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(ProcessSc);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(ClientSc);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(LogisticsSc);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(SaleResultSc);
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(OrderSc);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(MfgResultSc);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add(ManageSc);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 현재 폼을 숨김
            this.Hide();

            // 새로운 로그인 폼 인스턴스 생성
            Login loginForm = new Login();

            // 로그인 폼을 보여줌
            loginForm.ShowDialog();

            this.Close();
        }

    }
}
