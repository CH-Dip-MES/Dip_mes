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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dip_mes
{
    public partial class AddOrder : Form
    {
        private Order OrderForm;
        public event EventHandler Button2Clicked;   // 이벤트 정의

        public AddOrder(Order orderForm)
        {
            InitializeComponent();
            textBox3.Leave += textBox3_Leave;
            OrderForm = orderForm;
            LoadProductNames();
            this.comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            this.comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            textBox3.KeyDown += textBox3_KeyDown;
        }


        // 콤보박스 제품명 리스트업 메서드
        private void LoadProductNames()
        {
            string jConn = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
            using (MySqlConnection conn = new MySqlConnection(jConn))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT product_name FROM product";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        comboBox2.Items.Clear(); // 기존 항목을 지웁니다.

                        while (reader.Read())
                        {
                            string companyName = reader["product_name"].ToString();
                            comboBox2.Items.Add(companyName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"데이터베이스 오류: {ex.Message}");
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowEstTime();
            }
        }


        // 작업지시 등록 이벤트
        private void button2_Click(object sender, EventArgs e)
        {
            if (Login.getAuth != 1 && Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            // 데이터베이스 연결 문자열
            string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;"; //테이블 변경과 패스워드 설정

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 텍스트박스 및 콤보박스에서 입력된 값 가져오기
                    string textBox1Value = textBox1.Text.Trim();
                    string comboBox2Value = comboBox2.Text.Trim();
                    string comboBox1Value = comboBox1.Text.Trim();
                    string textBox3Value = textBox3.Text.Trim();
                    string textBox4Value = textBox4.Text.Trim();

                    // 중복되지 않는 Lot 번호 생성
                    string lotNumber = GenerateLotNumber(connection, comboBox2Value);

                    // 데이터베이스에 데이터 추가하는 SQL 쿼리
                    string query = "INSERT INTO manufacture (OrderNo, product_name, process_name, PlannedQty, EstTime, WorkStatus, Lot, Duration) " +
                            "VALUES (@textBox1, @comboBox2, @comboBox1, @textBox3, @textBox4, '작업대기', @lotNumber, CURRENT_TIMESTAMP )"; // '작업대기'로 추가

                    if (textBox1Value == "")
                    {
                        MessageBox.Show("작업지시번호를 입력해주세요");
                    }
                    else if (comboBox2Value == "")
                    {
                        MessageBox.Show("제품을 선택해주세요");

                    }
                    else if (comboBox2Value == "")
                    {
                        MessageBox.Show("공정을 선택해주세요");
                    }
                    else if (textBox3Value == "")
                    {
                        MessageBox.Show("목표수량을 입력해주세요");
                    }
                    else if (textBox4Value == "")
                    {
                        MessageBox.Show("예상완료시간이 필요합니다");
                    }
                    else
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            // 매개변수 설정
                            cmd.Parameters.AddWithValue("@textBox1", textBox1Value);
                            cmd.Parameters.AddWithValue("@comboBox2", comboBox2Value);
                            cmd.Parameters.AddWithValue("@comboBox1", comboBox1Value);
                            cmd.Parameters.AddWithValue("@textBox3", textBox3Value);
                            cmd.Parameters.AddWithValue("@textBox4", textBox4Value);
                            cmd.Parameters.AddWithValue("@lotNumber", lotNumber);

                            OnButton2Clicked(EventArgs.Empty);

                            // 쿼리 실행
                            cmd.ExecuteNonQuery();

                            // 데이터그리드 최신화
                            OrderForm.LoadDataToDataGridView1();

                            // 등록 성공 시 텍스트박스, 콤보박스 초기화
                            textBox1.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                            comboBox1.SelectedIndex = -1;
                            comboBox2.SelectedIndex = -1;
                            LoadProductNames();

                            MessageBox.Show("등록 완료");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        // 등록버튼 클릭 이벤트 호출 메서드
        protected virtual void OnButton2Clicked(EventArgs e)
        {
            Button2Clicked?.Invoke(this, e);
        }


        // 목표수량 입력후 포커스를 잃어버릴때 이벤트
        private void textBox3_Leave(object sender, EventArgs e) 
        {
            ShowEstTime();
        }

        private void ShowEstTime()
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
                    string productName = comboBox2.Text.Trim();

                    // MySQL에서 데이터 조회하는 SQL 쿼리
                    string query = @"SELECT product.product_name,product.product_code,product_process.process_name,product_process.process_time
                                   FROM product JOIN product_process ON product_process.product_code = product.product_code 
                                   WHERE product.product_name = @productName";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 설정
                        cmd.Parameters.AddWithValue("@selectedProcess", selectedProcess);
                        cmd.Parameters.AddWithValue("@productName", productName);

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
                            MessageBox.Show("예상시간을 계산할 수 없습니다.");
                            return;
                        }

                        Console.WriteLine("예상시간불러오기성공");
                        // DB에서 불러온 데이터 dataTable 객체의 값을 int형으로 변환하여 저장
                        int processTime = Convert.ToInt32(dataTable.Rows[comboBox1.SelectedIndex]["process_time"]);

                        // 목표수량과 DB정보를 곱해 예상시간을 계산
                        textBox4.Text = (textBox3Value * processTime).ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }



        // Lot넘버 생성 메서드
        private string GenerateLotNumber(MySqlConnection connection, string product)
        {
            string today = DateTime.Now.ToString("yyMMdd");

            string query = "SELECT Lot FROM manufacture WHERE Lot LIKE @selectLot ORDER BY Lot DESC LIMIT 1";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@selectLot", $"{today}-{product}-%");

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


        // 제품 콤보박스에서 항목선택시 이벤트
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Clear();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataToComboBoxForColumn(comboBox2.Text);
        }


        // 제품명 선택 시 공정명 콤보박스 리스트업 메서드
        private void LoadDataToComboBoxForColumn(string productName)
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
                    string query = @"SELECT product.product_name,product.product_code,product_process.process_name,product_process.process_time
                                   FROM product JOIN product_process ON product_process.product_code = product.product_code 
                                   WHERE product.product_name = @productName";


                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@productName", productName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 콤보박스에 데이터 추가
                                comboBox1.Items.Add(reader["process_name"].ToString());
                            }
                            reader.Close();
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
