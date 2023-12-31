using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace dip_mes
{
    public partial class InputMaterial : UserControl
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        private AddInput addInputForm; // 클래스 레벨에서 선언
        public InputMaterial()
        {
            InitializeComponent();
            LoadDataToDataGridView();
            textBox1.KeyDown += textBox1_KeyDown;
        }

        private void InputMaterial_Load(object sender, EventArgs e)
        {
            //폼 로드 시에 텍스트 박스 속성 설정
            SetDefaultText();

            // 포커스 이벤트 핸들러 등록
            textBox1.GotFocus += TextBox1_GotFocus;
            textBox1.LostFocus += TextBox1_LostFocus;

            // AddInput 폼 생성 및 설정
            AddInput addInput = new AddInput();
            addInputForm = addInput;
            addInputForm.TopLevel = false;                                  // 작업등록폼을 하위 수준으로
            addInputForm.FormBorderStyle = FormBorderStyle.None;            // 폼테두리 제거
            panel1.Controls.Add(addInputForm);                              // 패널영역에 폼 추가
            addInputForm.Show();
        }

        // -----------------------------------------------------------------
        // 조회버튼 클릭 이벤트 
        public void button1_Click(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
        }

        // 텍스트 박스 값 입력 이벤트, 'Enter' 입력 시 데이터그리드 조회
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadDataToDataGridView();
            }
        }
        // -----------------------------------------------------------------
        // 데이터그리드 관련
        // DB 데이터를 불러와 데이터그리드에 표시하는 메서드
        public void LoadDataToDataGridView()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string getValue = textBox1.Text.Trim();
                    // MySQL에서 데이터 조회하는 SQL 쿼리 (Status가 '작업대기'인 데이터만 조회)
                    string query = @"SELECT OrderNo AS '작업번호', product_name AS '제품명', process_name AS '공정명', PlannedQty AS '제품생산계획수량', inputMaterial AS '투입자재명', 
                                    estQty AS '예상소모자재', InputQty AS '자재투입수량', WorkStatus AS '작업상태', Duration AS '등록일자' 
                                    FROM manufacture WHERE WorkStatus = '작업대기' OR WorkStatus = '작업시작'";
                    
                    if (!string.IsNullOrEmpty(getValue) && getValue != "작업번호를 입력해주세요")
                    {
                        query += $" AND OrderNo = '{getValue}'";
                        Console.WriteLine("NOT NULL 쿼리추가");
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 데이터를 담을 DataTable 생성
                        DataTable dataTable = new DataTable();

                        // MySQLDataAdapter를 사용하여 데이터 가져오기
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }

                        // DataGridView에 데이터 바인딩
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dataTable;

                        
                        // Duration 컬럼의 데이터를 포맷팅하여 표시
                        FormatDurationColumn();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Duration 컬럼의 데이터를 포맷팅하여 표시
        private void FormatDurationColumn()
        {
            // Duration 컬럼이 존재하면서 DateTime 형식으로 변환 가능한 경우
            if (dataGridView1.Columns.Contains("작업지시일자") && dataGridView1.Columns["작업지시일자"] is DataGridViewTextBoxColumn durationColumn)
            {
                durationColumn.DefaultCellStyle.Format = "M월 d일 H:mm";
            }
        }

        // 데이터그리드의 셀 클릭 이벤트
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {   // 클릭된 행의 데이터를 얻기
            if (e.RowIndex >= 0)
            {
                //Console.WriteLine("셀클릭");
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string orderNo = selectedRow.Cells["작업번호"].Value.ToString();

                // Addinput 클래스에서 RefreshData 이벤트가 발생하면 inputMaterial 클래스의 LoadDataToDataGrideView 메서드 실행
                addInputForm.RefreshData += (s, args) => LoadDataToDataGridView();
                addInputForm.Tag = orderNo;


                // DataGridView에서 선택된 행의 데이터를 AddInput 폼의 TextBox에 표시
                addInputForm.DisplayDataInTextBox1(selectedRow.Cells["작업번호"].Value.ToString());
                addInputForm.DisplayDataInTextBox2(selectedRow.Cells["제품명"].Value.ToString());

                // 자재명 콤보박스 데이터 로드
                addInputForm.LoadMaterialData(selectedRow.Cells["제품명"].Value.ToString());
            }
        }

        //------------------------------ 텍스트 박스 설정 -----------------------------
        private void SetDefaultText()
        {
            // 텍스트 박스에 기본값 설정
            textBox1.Text = "작업번호를 입력해주세요";
            textBox1.ForeColor = Color.Gray;
        }

        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            // 포커스가 들어오면 텍스트 지우고 글씨 색 변경
            if (textBox1.Text == "작업번호를 입력해주세요")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            // 포커스를 잃으면 텍스트가 비어있으면 다시 기본값 설정
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                SetDefaultText();
            }
        }
        // ----------------------------------------------------------------------------
    }
}
