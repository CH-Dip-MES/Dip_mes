﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dip_mes
{
    public partial class product02 : UserControl
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        private DataGridViewComboBoxColumn statusComboColumn;
        private add_order addOrderForm;
        private DateTime? startTime; // 작업시작 상태의 시간을 저장할 변수

        public product02()
        {
            InitializeComponent();
            LoadDataToDataGridView1();
            textBox1.KeyDown += textBox1_KeyDown;
            // add_order 이벤트 핸들러 등록
            addOrderForm = new add_order(this);
            addOrderForm.Button2Clicked += AddOrderForm_Button2Clicked;
            // Form_Load 이벤트 핸들러 등록
            this.Load += UserControl1_Load;
        }
        
        // add_order 이벤트 핸들러
        private void AddOrderForm_Button2Clicked(object sender, EventArgs e)
        {
            // add_order 폼에서 버튼2가 클릭되었을 때 실행되는 로직
            // 여기에서 그리드를 최신화하는 메서드 호출
            LoadDataToDataGridView1();

        }
        // 최초 한 번만 수행하기 위한 플래그
        private static bool isComboBoxAdded = false;

        public void LoadDataToDataGridView1()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT No, Product AS '제품명', Process AS '공정명', Planned AS '계획수량', Duration AS '지시일자', Estimated AS '예상시간', Status  FROM manufacture";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // DataGridView에 데이터 바인딩
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dataTable;

                        // Duration 컬럼의 데이터를 포맷팅하여 표시
                        FormatDurationColumn();

                        dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
                        // 데이터 바인딩 후 작업 완료 상태인 행을 제거
                        RemoveCompletedRows();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void UserControl1_Load(object sender, EventArgs e)
        {
            // 최초 로드 시에 ComboBox 열을 추가
            if (!isComboBoxAdded)
            {
                // "Status" 열의 데이터를 새로운 ComboBox 열로 복사
                DataGridViewComboBoxColumn newComboColumn = new DataGridViewComboBoxColumn();
                newComboColumn.HeaderText = "작업상태";
                newComboColumn.Name = "Status";
                newComboColumn.DefaultCellStyle.NullValue = "작업대기";

                // DataGridView에 "Status" 열 추가
                dataGridView1.Columns.Add(newComboColumn);

                isComboBoxAdded = true;
            }

            // 폼 로드 시에 텍스트 박스 속성 설정
            SetDefaultText();

            // 포커스 이벤트 핸들러 등록
            textBox1.GotFocus += TextBox1_GotFocus;
            textBox1.LostFocus += TextBox1_LostFocus;

        }

        private void FormatDurationColumn()
        {
            // Duration 컬럼이 존재하면서 DateTime 형식으로 변환 가능한 경우
            if (dataGridView1.Columns.Contains("지시일자") && dataGridView1.Columns["지시일자"] is DataGridViewTextBoxColumn durationColumn)
            {
                durationColumn.DefaultCellStyle.Format = "M월 d일 HH:mm";
            }
        }
        private void RemoveCompletedRows()
        {
            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Status"].Value?.ToString() == "작업완료")
                {
                    rowsToRemove.Add(row);
                }
            }

            foreach (DataGridViewRow rowToRemove in rowsToRemove)
            {
                if (!rowToRemove.IsNewRow)
                {
                    dataGridView1.Rows.Remove(rowToRemove);
                }
            }
        }
        
        //private DataTable GetStatusOptions()
        //{
        //    DataTable statusTable = new DataTable();
        //    statusTable.Columns.Add("StatusOption");

        //    // 기본값 "작업대기" 추가
        //    statusTable.Rows.Add("작업대기");
        //    // 추가 옵션들
        //    statusTable.Rows.Add("작업시작");
        //    statusTable.Rows.Add("작업완료");

        //    // 기본값 "작업대기"를 설정하려면 데이터가 비어있을 때 추가
        //    if (statusTable.Rows.Count == 0)
        //    {
        //        statusTable.Rows.Add("작업대기");
        //    }

        //    return statusTable;
        //}


        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dataGridView1.Columns["Status"].Index && e.RowIndex >= 0)
                {
                    // 변경된 값 가져오기
                    string newStatusValue = dataGridView1.Rows[e.RowIndex].Cells["Status"].Value?.ToString();

                    if (string.IsNullOrEmpty(newStatusValue))
                    {
                        // 처리할 내용이 없으면 리턴
                        return;
                    }

                    // 해당 행의 기본 키(예: No) 가져오기
                    string primaryKeyValue = dataGridView1.Rows[e.RowIndex].Cells["No"].Value.ToString();

                    // 데이터베이스 업데이트
                    UpdateStatusInDatabase(primaryKeyValue, newStatusValue);

                    string statusValue = dataGridView1.Rows[e.RowIndex].Cells["Status"].Value?.ToString();

                    if (newStatusValue.Equals("작업완료", StringComparison.OrdinalIgnoreCase))
                    {
                        // 작업시작 상태인 경우 현재 시간을 Timesave 컬럼에 저장
                        if (statusValue.Equals("작업시작", StringComparison.OrdinalIgnoreCase))
                        {
                            SaveStartTimeInDatabase(primaryKeyValue);
                        }
                        // 작업완료 상태인 경우 작업시작 상태에서부터의 경과 시간을 계산하여 Worktime에 저장
                        else if (statusValue.Equals("작업완료", StringComparison.OrdinalIgnoreCase))
                        {
                            SaveWorkTimeInDatabase(primaryKeyValue);
                        }

                        // 행을 제거하기 전에 필요한 데이터를 추출
                        string removedNoValue = dataGridView1.Rows[e.RowIndex].Cells["No"].Value.ToString();

                        // 행 제거
                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in DataGridView1_CellValueChanged: {ex.Message}");
            }
        }
        private void SaveStartTimeInDatabase(string primaryKeyValue)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 데이터베이스 업데이트 쿼리 작성
                    string updateQuery = "UPDATE manufacture SET Timesave = CURRENT_TIMESTAMP WHERE No = @primaryKey";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@primaryKey", primaryKeyValue);

                        // 쿼리 실행
                        int affectedRows = cmd.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            // 현재 시간을 startTime 변수에 저장
                            startTime = DateTime.Now;
                            // 로그를 남깁니다.
                            Console.WriteLine($"Start time saved for No {primaryKeyValue}");
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
        private void SaveWorkTimeInDatabase(string primaryKeyValue)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 데이터베이스 업데이트 쿼리 작성
                    string updateQuery = "UPDATE manufacture SET Worktime = TIMEDIFF(NOW(), Timesave) WHERE No = @primaryKey";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@primaryKey", primaryKeyValue);

                        // 쿼리 실행
                        int affectedRows = cmd.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            // 로그를 남깁니다.
                            Console.WriteLine($"Work time saved for No {primaryKeyValue}");
                        }
                        else
                        {
                            MessageBox.Show("No rows affected. Update may not have occurred.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in SaveWorkTimeInDatabase: " + ex.Message);
                }
            }
        }

        private void UpdateStatusInDatabase(string primaryKeyValue, string newStatusValue)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 데이터베이스 업데이트 쿼리 작성
                    string updateQuery = "UPDATE manufacture SET Status = @newStatus WHERE No = @primaryKey";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@newStatus", newStatusValue);
                        cmd.Parameters.AddWithValue("@primaryKey", primaryKeyValue);

                        // 쿼리 실행
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            /*add_order Form = new add_order();
            Form.Show();*/
            add_order myForm = new add_order(this);
            myForm.TopLevel = false; // 폼이 최상위 수준이 아닌 자식으로 설정
            myForm.FormBorderStyle = FormBorderStyle.None; // 테두리 제거
            panel1.Controls.Add(myForm); // 패널에 폼 추가
            myForm.Show(); // 폼을 표시
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // 텍스트 박스에서 입력된 값 가져오기
                        string inputValue = textBox1.Text.Trim();

                        // MySQL에서 데이터 조회하는 SQL 쿼리
                        string query = "SELECT No, Product AS '제품명', Process AS '공정명', Planned AS '계획수량', Duration AS '지시일자', Estimated AS '예상시간', Status FROM manufacture WHERE Product LIKE @inputValue AND Status != '작업완료'";

                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            // 매개변수 설정
                            cmd.Parameters.AddWithValue("@inputValue", "%" + inputValue + "%");

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
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        
    }
}