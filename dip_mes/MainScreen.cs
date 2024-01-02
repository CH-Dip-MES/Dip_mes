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
        //라벨 읽기
        static public string label2Text;

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


        private TabPage tabPageProduct; // 탭 페이지 변수 선언
        private TabPage tabPageMaterial; // 탭 페이지 변수 선언
        private TabPage tabPageProcess; // 탭 페이지 변수 선언
        private TabPage tabPageClient; // 탭 페이지 변수 선언
        private TabPage tabPageSale; // 탭 페이지 변수 선언
        private TabPage tabPageLogistics; // 탭 페이지 변수 선언
        private TabPage tabPageSaleResult; // 탭 페이지 변수 선언
        private TabPage tabPageMaterialOrder; // 탭 페이지 변수 선언
        private TabPage tabPageOrderList; // 탭 페이지 변수 선언
        private TabPage tabPageInputMaterial; // 탭 페이지 변수 선언
        private TabPage tabPageOrder; // 탭 페이지 변수 선언ㅁ
        private TabPage tabPageMfgResult; // 탭 페이지 변수 선언
        private TabPage tabPageManage; // 탭 페이지 변수 선언

        public string UserID { get; set; }
        public string UserName { get; set; }

        

        public MainScreen()
        {
            InitializeComponent();
            InitializeTabs();
            toglepanels();
            tabControl1.TabPages.Clear();
            // 여기에 tabControl1의 백그라운드 컬러를 설정하는 코드 추가
            // tableLayoutPanel2의 백그라운드 컬러 설정
            tableLayoutPanel2.BackColor = ColorTranslator.FromHtml("#263959");
            panelbase.BackColor = ColorTranslator.FromHtml("#ECEFF4");
        }
        private void InitializeTabs()
        {

            // 각 탭 페이지 초기화
            tabPageProduct = InitializeTabPage("제품관리", ProductSc);
            tabPageMaterial = InitializeTabPage("자재관리", MaterialSc);
            tabPageProcess = InitializeTabPage("공정관리", ProcessSc);
            tabPageClient = InitializeTabPage("거래처", ClientSc);
            tabPageSale = InitializeTabPage("판매관리", SaleSc);
            tabPageLogistics = InitializeTabPage("입출고관리", LogisticsSc);
            tabPageSaleResult = InitializeTabPage("매출현황", SaleResultSc);
            tabPageMaterialOrder = InitializeTabPage("자재발주등록", MaterialOrderSc);
            tabPageOrderList = InitializeTabPage("발주현황", OrderListSc);
            tabPageInputMaterial = InitializeTabPage("투입등록", InputMaterialSc);
            tabPageOrder = InitializeTabPage("작업지시", OrderSc);
            tabPageMfgResult = InitializeTabPage("실적조회", MfgResultSc);
            tabPageManage = InitializeTabPage("회원정보관리", ManageSc);

            // TabControl에 탭 페이지 추가
            tabControl1.TabPages.Add(tabPageProduct);
            tabControl1.TabPages.Add(tabPageMaterial);
            tabControl1.TabPages.Add(tabPageProcess);
            tabControl1.TabPages.Add(tabPageClient);
            tabControl1.TabPages.Add(tabPageSale);
            tabControl1.TabPages.Add(tabPageLogistics);
            tabControl1.TabPages.Add(tabPageSaleResult);
            tabControl1.TabPages.Add(tabPageMaterialOrder);
            tabControl1.TabPages.Add(tabPageOrderList);
            tabControl1.TabPages.Add(tabPageInputMaterial);
            tabControl1.TabPages.Add(tabPageOrder);
            tabControl1.TabPages.Add(tabPageMfgResult);
            tabControl1.TabPages.Add(tabPageManage);

        }

        private TabPage InitializeTabPage(string tabPageText, Control contentControl)
        {
            TabPage tabPage = new TabPage(tabPageText);
            tabPage.Controls.Add(contentControl);
            return tabPage;
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
            // 사용자 정보로 레이블 설정
            label2.Text = UserID;
            label3.Text = UserName;
            label2Text = label2.Text;
        }

        // 탭이 이미 열려있는지 확인하는 함수
        private bool IsTabAlreadyOpen(string tabText)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Text == tabText)
                {
                    return true; // 이미 열려있음
                }
            }
            return false; // 열려있지 않음
        }

        // 이미 열려있는 탭을 보여주는 함수
        private void ShowExistingTab(string tabText)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Text == tabText)
                {
                    tabControl1.SelectedTab = tabPage;
                    ShowOnlyActiveTab(tabPage);
                    return;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("자재발주등록"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("자재발주등록");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("자재발주등록", MaterialOrderSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(MaterialOrderSc);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("투입등록"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("투입등록");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("투입등록", InputMaterialSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(InputMaterialSc);
            }
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
            //슬라이드메뉴 설정
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
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("판매관리"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("판매관리");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("판매관리", SaleSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(SaleSc);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("발주현황"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("발주현황");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("발주현황", OrderListSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(OrderListSc);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("제품관리"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("제품관리");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("제품관리", ProductSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(ProductSc);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("자재관리"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("자재관리");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("자재관리", MaterialSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(MaterialSc);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("공정관리"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("공정관리");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("공정관리", ProcessSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(ProcessSc);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("거래처관리"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("거래처관리");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("거래처관리", ClientSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(ClientSc);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("입출고관리"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("입출고관리");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("입출고관리", LogisticsSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(LogisticsSc);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("매출현황"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("매출현황");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("매출현황", SaleResultSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(SaleResultSc);
            }
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("작업지시"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("작업지시");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("작업지시", OrderSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(OrderSc);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("실적조회"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("실적조회");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("실적조회", MfgResultSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(MfgResultSc);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (Login.getAuth != 3)
            {
                MessageBox.Show("관리자 권한이 필요합니다.");
                return;
            }
            // 자재발주등록 탭이 이미 생성되었는지 확인
            if (IsTabAlreadyOpen("회원정보관리"))
            {
                // 이미 열려있다면 해당 탭으로 이동
                ShowExistingTab("회원정보관리");
            }
            else
            {
                // 탭을 생성하고 추가
                TabPage tabPage = InitializeTabPage("회원정보관리", ManageSc);
                tabControl1.TabPages.Add(tabPage);

                // 탭을 선택하고 선택한 탭만 표시합니다.
                tabControl1.SelectedTab = tabPage;
                ShowOnlyActiveTab(tabPage);

                panel.Controls.Clear();
                panel.Controls.Add(ManageSc);
            }
        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void ShowOnlyActiveTab(TabPage activeTab)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage != activeTab)
                {
                    tabPage.Visible = false;
                }
            }
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 선택된 탭 가져오기
            TabPage selectedTab = tabControl1.SelectedTab;

            if (selectedTab != null)
            {
                // 모든 패널 숨기기
                foreach (Control control in panel.Controls)
                {
                    control.Visible = false;
                }

                // 선택된 탭에 따라 해당하는 패널 표시
                switch (selectedTab.Text)
                {
                    case "제품관리":
                        panel.Controls.Clear();
                        panel.Controls.Add(ProductSc);
                        ProductSc.Visible = true;
                        break;

                    case "자재관리":
                        panel.Controls.Clear();
                        panel.Controls.Add(MaterialSc);
                        MaterialSc.Visible = true;
                        break;

                    case "공정관리":
                        panel.Controls.Clear();
                        panel.Controls.Add(ProcessSc);
                        ProcessSc.Visible = true;
                        break;

                    case "거래처관리":
                        panel.Controls.Clear();
                        panel.Controls.Add(ClientSc);
                        ClientSc.Visible = true;
                        break;

                    case "판매관리":
                        panel.Controls.Clear();
                        panel.Controls.Add(SaleSc);
                        SaleSc.Visible = true;
                        break;

                    case "입출고관리":
                        panel.Controls.Clear();
                        panel.Controls.Add(LogisticsSc);
                        LogisticsSc.Visible = true;
                        break;

                    case "매출현황":
                        panel.Controls.Clear();
                        panel.Controls.Add(SaleResultSc);
                        SaleResultSc.Visible = true;
                        break;

                    case "자재발주등록":
                        panel.Controls.Clear();
                        panel.Controls.Add(MaterialOrderSc);
                        MaterialOrderSc.Visible = true;
                        break;

                    case "발주현황":
                        panel.Controls.Clear();
                        panel.Controls.Add(OrderListSc);
                        OrderListSc.Visible = true;
                        break;

                    case "투입등록":
                        panel.Controls.Clear();
                        panel.Controls.Add(InputMaterialSc);
                        InputMaterialSc.Visible = true;
                        break;

                    case "작업지시":
                        panel.Controls.Clear();
                        panel.Controls.Add(OrderSc);
                        OrderSc.Visible = true;
                        break;

                    case "실적조회":
                        panel.Controls.Clear();
                        panel.Controls.Add(MfgResultSc);
                        MfgResultSc.Visible = true;
                        break;

                    case "회원정보관리":
                        panel.Controls.Clear();
                        panel.Controls.Add(ManageSc);
                        ManageSc.Visible = true;
                        break;

                    default:
                        break;
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // 현재 선택된 탭의 인덱스 가져오기
            int currentIndex = tabControl1.SelectedIndex;

            if (currentIndex >= 0)
            {
                // 탭 닫기
                tabControl1.TabPages.RemoveAt(currentIndex);

                // 탭이 아직 남아있다면 맨 왼쪽 탭 선택
                if (tabControl1.TabPages.Count > 0)
                {
                    tabControl1.SelectedIndex = 0;
                }
                else
                {
                    // 탭이 하나도 없다면 빈 화면으로 설정
                    panel.Controls.Clear();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 현재 폼을 숨김
            this.Hide();
            Login.getAuth = 0;
            Console.WriteLine("권한조회 : " + Login.getAuth);

            // 새로운 로그인 폼 인스턴스 생성
            Login loginForm = new Login();

            // 로그인 폼을 보여줌
            loginForm.ShowDialog();

            this.Close();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}