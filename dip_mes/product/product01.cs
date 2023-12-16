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

namespace dip_mes.product
{
    public partial class product01 : UserControl
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        private add_input addInputForm;
        public product01()
        {
            InitializeComponent();
            LoadDataToDataGridView();
            textBox1.KeyDown += textBox1_KeyDown;
            // DataGridView의 더블클릭 이벤트 핸들러 등록
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

        private void product01_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 더블클릭된 행의 데이터를 얻기
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            // 필요한 데이터를 추출하는 예시 (실제로는 데이터 소스로부터 가져와야 함)
            string orderId = selectedRow.Cells["No"].Value.ToString();

            // add_input 폼에 데이터 전달
            add_input addInputForm = GetOrCreateAddInputForm(orderId);
            addInputForm.Tag = orderId;

            // 폼 설정
            addInputForm.TopLevel = false;
            addInputForm.FormBorderStyle = FormBorderStyle.None;

            // 패널1에 폼 추가
            panel1.Controls.Clear(); // 패널을 초기화하고 기존의 컨트롤들을 제거
            panel1.Controls.Add(addInputForm);

            // 폼을 표시
            addInputForm.Show();

            // DataGridView에서 선택된 행의 데이터를 add_input 폼의 TextBox에 표시
            addInputForm.DisplayDataInTextBox1(selectedRow.Cells["No"].Value.ToString());
            // 두 번째 TextBox에 다른 데이터 표시 예시
            addInputForm.DisplayDataInTextBox2(selectedRow.Cells["Product"].Value.ToString());
        }

        private add_input GetOrCreateAddInputForm(string orderId)
        {
            var existingForm = panel1.Controls.OfType<add_input>().FirstOrDefault();

            if (existingForm != null)
            {
                existingForm.Tag = orderId;
                return existingForm;
            }
            else
            {
                // 패널에 이미 생성된 add_input 폼이 없으면 새로운 인스턴스 생성
                add_input addInputForm = new add_input();
                addInputForm.Tag = orderId; // 데이터 전달
                return addInputForm;
            }
        }
        private void LoadDataToDataGridView()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // MySQL에서 데이터 조회하는 SQL 쿼리 (Status가 '작업대기'인 데이터만 조회)
                    string query = "SELECT No, Product, Material, Planned, Input, Residue, Duration FROM product WHERE Status = '작업대기'";

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
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 엔터 키가 눌렸을 때의 동작 수행
                SearchData();
            }
        }
        private void SearchData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 텍스트 박스에서 입력된 값 가져오기
                    string inputValue = textBox1.Text.Trim();

                    // MySQL에서 데이터 조회하는 SQL 쿼리
                    string query = "SELECT No, Product, Material, Planned, Input, Residue, Duration FROM product WHERE No LIKE @inputValue";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 설정
                        cmd.Parameters.AddWithValue("@inputValue", inputValue);

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
        private void button1_Click(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
        }
        private void FormatDurationColumn()
        {
            // Duration 컬럼이 존재하면서 DateTime 형식으로 변환 가능한 경우
            if (dataGridView1.Columns.Contains("Duration") && dataGridView1.Columns["Duration"] is DataGridViewTextBoxColumn durationColumn)
            {
                durationColumn.DefaultCellStyle.Format = "M월 d일 H:mm";
            }
        }
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
