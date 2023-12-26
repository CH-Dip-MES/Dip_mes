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

namespace dip_mes
{
    public partial class AddOrder : Form
    {
        private Order ProductForm;
        private int orderCount = 0; // 주문 번호 카운터
        public AddOrder(Order productForm)
        {
            InitializeComponent();
            textBox3.Leave += textBox3_Leave;
            ProductForm = productForm;
        }

      

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string columnName = textBox2.Text.Trim();
            LoadDataToComboBoxForColumn(columnName);
        }
        private void LoadDataToComboBoxForColumn(string columnName)
        {
            // 콤보박스 초기화
            comboBox1.Items.Clear();

            // 입력된 컬럼 이름으로 데이터를 조회하고 콤보박스에 추가
            using (MySqlConnection connection = new MySqlConnection("Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;"))
            {
                try
                {
                    connection.Open();

                    // 입력된 컬럼 이름을 기반으로 데이터 조회
                    string query = $"SELECT DISTINCT Process FROM manufacture WHERE Product = @inputValue";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@inputValue", textBox2.Text.Trim());

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 콤보박스에 데이터 추가
                                comboBox1.Items.Add(reader["process"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        // 이벤트 정의
        public event EventHandler Button2Clicked;
        private void button2_Click(object sender, EventArgs e)
        {
            // 데이터베이스 연결 문자열
            string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;"; //테이블 변경과 패스워드 설정

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 텍스트박스 및 콤보박스에서 입력된 값 가져오기
                    string textBox1Value = textBox1.Text.Trim();
                    string textBox2Value = textBox2.Text.Trim();
                    string comboBox1Value = comboBox1.Text.Trim();
                    string textBox3Value = textBox3.Text.Trim();
                    string textBox4Value = textBox4.Text.Trim();

                    // 중복되지 않는 Lot 번호 생성
                    string lotNumber = GenerateLotNumber(connection, textBox2Value);

                    // 데이터베이스에 데이터 추가하는 SQL 쿼리
                    string query = "INSERT INTO manufacture (No, Product, Process, Planned, Estimated, Status, Lot) " +
                            "VALUES (@textBox1, @textBox2, @comboBox1, @textBox3, @textBox4, '작업대기', @lotNumber )"; // '작업대기'로 추가


                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 설정
                        cmd.Parameters.AddWithValue("@textBox1", textBox1Value);
                        cmd.Parameters.AddWithValue("@textBox2", textBox2Value);
                        cmd.Parameters.AddWithValue("@comboBox1", comboBox1Value);
                        cmd.Parameters.AddWithValue("@textBox3", textBox3Value);
                        cmd.Parameters.AddWithValue("@textBox4", textBox4Value);
                        cmd.Parameters.AddWithValue("@lotNumber", lotNumber);

                        OnButton2Clicked(EventArgs.Empty);

                        // 쿼리 실행
                        cmd.ExecuteNonQuery();

                        // 데이터그리드 최신화
                        ProductForm.LoadDataToDataGridView1();

                        this.Close();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
       
        // 이벤트 호출 메서드
        protected virtual void OnButton2Clicked(EventArgs e)
        {
            Button2Clicked?.Invoke(this, e);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            // 텍스트박스3에서 입력된 숫자 가져오기
            if (!int.TryParse(textBox3.Text, out int textBox3Value))
            {
                MessageBox.Show("숫자를 입력하세요.");
                return;
            }

            // MySQL 연결 문자열
            string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 콤보박스1에서 선택된 값 가져오기
                    string selectedProcess = comboBox1.Text.Trim();

                    // MySQL에서 데이터 조회하는 SQL 쿼리
                    string query = "SELECT Time FROM manufacture WHERE Process = @selectedProcess";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 설정
                        cmd.Parameters.AddWithValue("@selectedProcess", selectedProcess);

                        // 데이터를 담을 DataTable 생성
                        DataTable dataTable = new DataTable();

                        // MySQLDataAdapter를 사용하여 데이터 가져오기
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }

                        // 조회된 데이터가 없을 경우 메시지 표시 후 리턴
                        if (dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("데이터가 없습니다.");
                            return;
                        }

                        // 텍스트박스4에 표시할 값 계산
                        TimeSpan timeSpan = (TimeSpan)dataTable.Rows[0]["Time"];
                        double totalSeconds = timeSpan.TotalSeconds; // double 형식으로 받아옴

                        int resultInSeconds = (int)totalSeconds * textBox3Value;

                        // 초를 시간 간격으로 변환
                        TimeSpan resultTimeSpan = TimeSpan.FromSeconds(resultInSeconds);

                        // 텍스트박스4에 결과 표시
                        textBox4.Text = resultTimeSpan.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private string GenerateLotNumber(MySqlConnection connection, string product)
        {
            string today = DateTime.Now.ToString("yyMMdd");

            string query = "SELECT Lot FROM manufacture WHERE Lot LIKE @pattern ORDER BY Lot DESC LIMIT 1";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@pattern", $"{today}-{product}-%");

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string lastLotNumber = reader["Lot"].ToString();
                        string[] Material = lastLotNumber.Split('-');
                        if (Material.Length == 3 && int.TryParse(Material[2], out int lastNumber))
                        {
                            // 마지막으로 사용된 번호에 1을 더하여 반환
                            return $"{today}-{product}-{(lastNumber + 1).ToString("0000")}";
                        }
                    }
                }
            }

            // 해당 날짜와 제품으로 시작하는 Lot 번호가 없으면 0001로 반환
            return $"{today}-{product}-0001";
        }
    }
}
