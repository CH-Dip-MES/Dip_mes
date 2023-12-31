using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Tsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dip_mes
{
    public partial class Order : UserControl
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        //private DataGridViewComboBoxColumn statusComboColumn;
        private AddOrder AddOrderForm;
        private DateTime? startTime; // 작업시작 상태의 시간을 저장할 변수

        public Order()
        {
            InitializeComponent();
            //AddOrderForm = new AddOrder(this);  //작업지시등록 폼클래스의 객체 인스턴스화, 해당객체 포함
            // Form_Load 이벤트 핸들러 등록
            this.Load += UserControl1_Load;
            LoadDataToDataGridView1();
            textBox1.KeyDown += textBox1_KeyDown;
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            // DataGridView1_CellValueChanged 이벤트 핸들러 등록
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;

            //폼 로드 시에 텍스트 박스 속성 설정
            SetDefaultText();

            // 텍스트박스 포커스 이벤트
            textBox1.GotFocus += TextBox1_GotFocus;
            textBox1.LostFocus += TextBox1_LostFocus;

            // 작업지시등록 객체를 화면에 표시
            AddOrder addOrder = new AddOrder(this);
            AddOrderForm = addOrder;
            AddOrderForm.TopLevel = false;                                  // 작업등록폼을 하위 수준으로
            AddOrderForm.FormBorderStyle = FormBorderStyle.None;            // 폼테두리 제거
            panel1.Controls.Add(AddOrderForm);                              // 패널영역에 폼 추가
            AddOrderForm.Show(); 
            AddOrderForm.Button2Clicked += AddOrderForm_Button2Clicked;     // 등록폼의 이벤트
        }

        // 작업지시등록 폼클래스의 등록버튼을 누를면 발생하는 이벤트
        private void AddOrderForm_Button2Clicked(object sender, EventArgs e)
        {
            LoadDataToDataGridView1();
        }




        // ------------------- 텍스트 박스 포커스 기능 --------------------- 
        private void SetDefaultText()
        {
            // 텍스트 박스에 기본값 설정
            textBox1.Text = "제품명을 입력해주세요";
            textBox1.ForeColor = Color.Gray;
        }

        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            // 포커스가 들어오면 텍스트 지우고 글씨 색 변경
            if (textBox1.Text == "제품명을 입력해주세요")
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
        // ----------------------------------------------------------------- 




        // --------------------------------------- 데이터그리드 관련 메서드 ---------------------------------------------
        // DB의 데이터를 불러와 데이터그리드에 표시하는 기능
        public void LoadDataToDataGridView1()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string getProduct = textBox1.Text.Trim();
                string selectQuery = @"SELECT OrderNo AS '작업번호', product_name AS '제품명', process_name AS '공정명', PlannedQty AS '생산수량', 
                                     Duration AS '작업지시일자', StartTime AS '작업시작시간', EstTime AS '완료예상시간(분)', WorkStatus AS '작업상태' FROM manufacture";

                // 검색창에 입력한 문자가 있을 때 쿼리문에 조건 추가, 없으면 전체 조회
                if (!string.IsNullOrEmpty(getProduct) && textBox1.Text != "제품명을 입력해주세요")
                {
                    selectQuery += $" WHERE product_name = '{getProduct}'";
                }
                try
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // DataGridView에 데이터 바인딩
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dataTable;

                        // Duration 컬럼의 데이터를 포맷팅하여 표시
                        if (dataGridView1.Columns.Contains("작업지시일자") && dataGridView1.Columns["작업지시일자"] is DataGridViewTextBoxColumn durationColumn)
                        {
                            durationColumn.DefaultCellStyle.Format = "M월 d일 HH:mm";
                        }

                        if (dataGridView1.Columns.Contains("작업시작시간") && dataGridView1.Columns["작업시작시간"] is DataGridViewTextBoxColumn startTimeColumn)
                        {
                            startTimeColumn.DefaultCellStyle.Format = "M월 d일 HH:mm";
                        }

                        // 데이터 바인딩 후 작업 완료 상태인 행을 제거
                        RemoveCompletedRows();

                        // "작업상태" 열에 대한 콤보박스 초기화
                        if (dataGridView1.Columns.Contains("작업상태"))
                            dataGridView1.Columns.Remove("작업상태");

                        // "작업상태" 열이 이미 추가되어 있는지 확인
                        // "작업상태" 열의 데이터를 새로운 ComboBox 열로 복사
                        DataGridViewComboBoxColumn newComboColumn = new DataGridViewComboBoxColumn();
                        newComboColumn.DataPropertyName = "작업상태";
                        newComboColumn.HeaderText = "작업상태";
                        newComboColumn.Name = "작업상태";

                        newComboColumn.Items.Add("작업시작");
                        newComboColumn.Items.Add("작업대기");
                        newComboColumn.Items.Add("작업완료");

                        // DataGridView에 "작업상태" 열 추가
                        dataGridView1.Columns.Insert(6, newComboColumn);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // 작업상태가 완료로 변경되면 해당 행을 데이터그리드에서 지우는 기능
        private void RemoveCompletedRows()
        {
            List<DataGridViewRow> rowBuffer = new List<DataGridViewRow>();

            foreach (DataGridViewRow completedRow in dataGridView1.Rows)
            {
                if (completedRow.Cells["작업상태"].Value?.ToString() == "작업완료")
                {
                    rowBuffer.Add(completedRow);
                }
            }
            foreach (DataGridViewRow deleteRow in rowBuffer)
            {
                if (!deleteRow.IsNewRow)
                {   // 입력중인 데이터행은 제외, 데이터 무결성 유지
                    dataGridView1.Rows.RemoveAt(deleteRow.Index);
                }
            }
        }

        // 데이터그리드의 셀 값이 변경될 때 발생하는 이벤트 메서드
        public void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string startTime = "UPDATE manufacture SET StartTime = CURRENT_TIMESTAMP WHERE OrderNo = @OrderNo";
                string finishTime = "UPDATE manufacture SET FinishTime = CURRENT_TIMESTAMP WHERE OrderNo = @OrderNo";
                // 헤더행이 아닌 작업상태 컬럼을 선택했을 때
                if (e.ColumnIndex == dataGridView1.Columns["작업상태"].Index && e.RowIndex >= 0)
                {
                    // 선택된 행의 작업 번호와 작업상태 저장
                    string orderNo = dataGridView1.Rows[e.RowIndex].Cells["작업번호"].Value.ToString();
                    string workStatus = dataGridView1.Rows[e.RowIndex].Cells["작업상태"].Value?.ToString();

                    if (string.IsNullOrEmpty(workStatus))   //작업상태 값이 없다면 메서드 종료
                    {
                        return;
                    }

                    if (workStatus.Equals("작업완료", StringComparison.OrdinalIgnoreCase))
                    {   
                        // 작업상태가 "작업완료" 일 때
                        SaveTime(orderNo, finishTime);
                        // 데이터그리드에서 행 삭제
                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                        MessageBox.Show($" 작업지시번호 : {orderNo}이 완료 처리되어 목록에서 사라집니다.");
                        WorkTime(orderNo);
                    }
                    else if (workStatus.Equals("작업시작", StringComparison.OrdinalIgnoreCase))
                    {   
                        // 작업상태가 "작업시작" 일 때
                        SaveTime(orderNo, startTime);
                        MessageBox.Show($"작업지시번호 : {orderNo}의 상태가 '작업시작'으로 변경되었습니다.");
                    }
                    // DB에 상태 업데이트
                    UpdateStatus(orderNo, workStatus);
                    LoadDataToDataGridView1();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in DataGridView1_CellValueChanged: {ex.Message}");
            }
        }
        // ----------------------------------------------------------------- 

        // 시작시간과 완료시간을 DB에서 불러와 소요시간을 계산 후 DB에 저장
        private void WorkTime(string orderNo)
        {
            string selectQuery = "SELECT StartTime, FinishTime FROM manufacture WHERE OrderNo = @OrderNo";
            string updateQuery = "UPDATE manufacture SET takeTime = @takeTime WHERE OrderNo = @OrderNo";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand Cmd = new MySqlCommand(selectQuery, connection))
                    {
                        Cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                        using (MySqlDataReader reader = Cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime startTime = reader.GetDateTime("StartTime");
                                DateTime finishTime = reader.GetDateTime("FinishTime");

                                reader.Close();

                                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
                                {
                                    TimeSpan takeTime = finishTime - startTime;
                                    double dTakeTime = takeTime.TotalHours;
                                    double roundedTakeTime = Math.Round(dTakeTime,2);

                                    updateCmd.Parameters.AddWithValue("@takeTime", roundedTakeTime);
                                    updateCmd.Parameters.AddWithValue("@OrderNo", orderNo);

                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"데이터베이스 오류: {ex.Message}");
                }
            }
        }


        // 작업상태 변경 시간을 DB에 저장하는 메서드
        private void SaveTime(string orderNo, string updateQuery)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 데이터베이스 업데이트 쿼리 작성

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@OrderNo", orderNo);

                        // 쿼리 실행
                        int affectedRows = cmd.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            // 현재 시간을 startTime 변수에 저장
                            startTime = DateTime.Now;
                            // 로그를 남깁니다.
                            Console.WriteLine($"Start time saved for OrderNo {orderNo}");
                        }
                        else
                        {
                            MessageBox.Show("No rows affected. Update may not have occurred.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in SaveStartTimeInDatabase: " + ex.Message);
                }
            }
        }
        
        // DB에 작업상태를 저장하는 메서드
        private void UpdateStatus(string orderNo, string workStatus)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string updateQuery = "UPDATE manufacture SET WorkStatus = @WorkStatus WHERE OrderNo = @OrderNo";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkStatus", workStatus);
                        cmd.Parameters.AddWithValue("@OrderNo", orderNo);

                        int affectedRows = cmd.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            Console.WriteLine($"WorkStatus updated for No {orderNo}");
                        }
                        else
                        {
                            MessageBox.Show("No rows affected. Update may not have occurred.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // 조회버튼 클릭 이벤트
        private void button1_Click(object sender, EventArgs e)
        {
            if (Login.getAuth != 1 || Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            LoadDataToDataGridView1(); 
        }

        // 키입력 이벤트, 'Enter'키로 데이터그리드 조회
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadDataToDataGridView1();
            }
        }

    }
}