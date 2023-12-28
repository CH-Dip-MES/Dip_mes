﻿using MySql.Data.MySqlClient;
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
        private DateTime? startTime; // 작업시 작 상태의 시간을 저장할 변수
        private const string SelectQuery = "SELECT No, Product AS '제품명', Process AS '공정명', Planned AS '계획수량', Duration AS '지시일자', Estimated AS '예상시간', Status AS '작업상태' FROM manufacture";
        private const string UpdateStatusQuery = "UPDATE manufacture SET Status = @newStatus WHERE No = @primaryKey";
        //private const string SaveStartTimeQuery = "UPDATE manufacture SET Timesave = CURRENT_TIMESTAMP WHERE No = @primaryKey";
        private const string SaveWorkTimeQuery = "UPDATE manufacture SET Worktime = TIMEDIFF(NOW(), Timesave) WHERE No = @primaryKey";

        public Order()
        {
            InitializeComponent();
            // Form_Load 이벤트 핸들러 등록
            this.Load += UserControl1_Load;
            LoadDataToDataGridView1();
            textBox1.KeyDown += textBox1_KeyDown;
            // add_order 이벤트 핸들러 등록
            AddOrderForm = new AddOrder(this);
            AddOrderForm.Button2Clicked += AddOrderForm_Button2Clicked;
        }
        private void UserControl1_Load(object sender, EventArgs e)
        {
            // DataGridView1_CellValueChanged 이벤트 핸들러 등록
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;

            //폼 로드 시에 텍스트 박스 속성 설정
            SetDefaultText();

            // 포커스 이벤트 핸들러 등록
            textBox1.GotFocus += TextBox1_GotFocus;
            textBox1.LostFocus += TextBox1_LostFocus;

            ShowAddOrderForm();
        }

        private void AddOrderForm_Button2Clicked(object sender, EventArgs e)
        {
            LoadDataToDataGridView1();
        }

        public void LoadDataToDataGridView1()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string getValue = textBox1.Text.Trim();
                    string selectQuery = "SELECT No, Product AS '제품명', Process AS '공정명', Planned AS '계획수량', Duration AS '지시일자', Estimated AS '예상시간', Status AS '작업상태' FROM manufacture";
                    if (!string.IsNullOrEmpty(getValue)) // 검색창에 입력한 문자 있을 시 활성화 없으면 위의 fItem 문구 그대로
                    {
                        selectQuery += $" WHERE Product = '{getValue}'";
                        Console.WriteLine("Null확인 후 쿼리추가");
                    }
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        // DataGridView에 데이터 바인딩
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dataTable;

                        // Duration 컬럼의 데이터를 포맷팅하여 표시
                        FormatDurationColumn();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FormatDurationColumn()
        {
            // Duration 컬럼이 존재하면서 DateTime 형식으로 변환 가능한 경우
            if (dataGridView1.Columns.Contains("지시일자") && dataGridView1.Columns["지시일자"] is DataGridViewTextBoxColumn durationColumn)
            {
                durationColumn.DefaultCellStyle.Format = "M월 d일 HH:mm";
            }
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dataGridView1.Columns["작업상태"].Index && e.RowIndex >= 0)
                {
                    string newStatusValue = dataGridView1.Rows[e.RowIndex].Cells["작업상태"].Value?.ToString();

                    if (string.IsNullOrEmpty(newStatusValue))
                    {
                        return;
                    }

                    string primaryKeyValue = dataGridView1.Rows[e.RowIndex].Cells["No"].Value.ToString();

                    if (newStatusValue.Equals("작업완료", StringComparison.OrdinalIgnoreCase))
                    {
                        // 작업 완료 상태에서 작업 시작 시간 저장 및 작업 시간 저장
                        HandleWorkCompleteStatus(primaryKeyValue, e.RowIndex);

                        // DB에 상태 업데이트
                        UpdateStatusInDatabase(primaryKeyValue, newStatusValue, UpdateStatusQuery);

                        // 그 후에 행 삭제
                        RemoveCompletedRow(e.RowIndex);
                    }
                    else if (newStatusValue.Equals("작업시작", StringComparison.OrdinalIgnoreCase))
                    {
                        // 작업 시작 상태에서 현재 시간 저장
                        SaveStartTimeInDatabase(primaryKeyValue);

                        // DB에 상태 업데이트
                        UpdateStatusInDatabase(primaryKeyValue, newStatusValue, UpdateStatusQuery);
                    }
                    else
                    {
                        // 다른 상태의 경우에는 상태만 업데이트
                        UpdateStatusInDatabase(primaryKeyValue, newStatusValue, UpdateStatusQuery);
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
                    string updateQuery = SaveWorkTimeQuery;

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
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in SaveWorkTimeInDatabase: " + ex.Message);
                }
            }
        }
        private void UpdateStatusInDatabase(string primaryKeyValue, string newStatusValue, string updateQuery)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@newStatus", newStatusValue);
                        cmd.Parameters.AddWithValue("@primaryKey", primaryKeyValue);

                        int affectedRows = cmd.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            Console.WriteLine($"Status updated for No {primaryKeyValue}");
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
        private void HandleWorkCompleteStatus(string primaryKeyValue, int rowIndex)
        {
            string statusValue = dataGridView1.Rows[rowIndex].Cells["작업상태"].Value?.ToString();

            if (statusValue.Equals("작업완료", StringComparison.OrdinalIgnoreCase))
            {
                // 작업 완료 상태에서 작업 시작 시간 저장 및 작업 시간 저장
                SaveStartTimeInDatabase(primaryKeyValue);
                SaveWorkTimeInDatabase(primaryKeyValue);
            }
        }
        private void RemoveCompletedRow(int rowIndex)
        {
            if (rowIndex >= 0)
            {
                // 행을 제거하기 전에 필요한 데이터를 추출
                string removedNoValue = dataGridView1.Rows[rowIndex].Cells["No"].Value.ToString();

                // 행 제거
                dataGridView1.Rows.RemoveAt(rowIndex);
            }
        }
        private void RemoveCompletedRows()
        {
            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["작업상태"].Value?.ToString() == "작업완료")
                {
                    rowsToRemove.Add(row);
                }
            }

            foreach (DataGridViewRow rowToRemove in rowsToRemove)
            {
                if (!rowToRemove.IsNewRow)
                {
                    int rowIndex = rowToRemove.Index;
                    RemoveCompletedRow(rowIndex);
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
        private void ShowAddOrderForm()
        {
            AddOrder myForm = new AddOrder(this);
            myForm.TopLevel = false; // 폼이 최상위 수준이 아닌 자식으로 설정
            myForm.FormBorderStyle = FormBorderStyle.None; // 테두리 제거
            panel1.Controls.Add(myForm); // 패널에 폼 추가
            myForm.Show(); // 폼을 표시
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("버튼1클릭");
            LoadDataToDataGridView1();
            
    }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadDataToDataGridView1();
            }
            Console.WriteLine("textbox1_키다운,엔터");
        }

    }
}