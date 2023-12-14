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
    public partial class add_order : Form
    {
        public add_order()
        {
            InitializeComponent();
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
        1
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
                    string query = $"SELECT DISTINCT Process FROM product WHERE Product = @inputValue";
                    //string query = $"SELECT DISTINCT Process, 조회하고자 하는 컬럼 FROM 조회를 원하는 테이블명 WHERE Product = @inputValue";
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

                    // 데이터베이스에 데이터 추가하는 SQL 쿼리
                    string query = "INSERT INTO product (No, Product, Process, Planned, Estimated) " + "VALUES (@textBox1, @textBox2, @comboBox1, @textBox3, @textBox4)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 설정
                        cmd.Parameters.AddWithValue("@textBox1", textBox1Value);
                        cmd.Parameters.AddWithValue("@textBox2", textBox2Value);
                        cmd.Parameters.AddWithValue("@comboBox1", comboBox1Value);
                        cmd.Parameters.AddWithValue("@textBox3", textBox3Value);
                        cmd.Parameters.AddWithValue("@textBox4", textBox4Value);
                        

                        // 쿼리 실행
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("작업지시 등록이 완료되었습니다.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            // 텍스트박스3에서 입력된 숫자 가져오기
            int textBox3Value;
            if (!int.TryParse(textBox3.Text, out textBox3Value))
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

                    // 텍스트 박스2에서 입력된 값 가져오기
                    string inputValue = textBox2.Text.Trim();

                    // MySQL에서 데이터 조회하는 SQL 쿼리
                    string query = "SELECT YourColumn, AnotherColumn FROM YourTable WHERE YourColumn = @inputValue";

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

                        // 조회된 데이터가 없을 경우 메시지 표시 후 리턴
                        if (dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("데이터가 없습니다.");
                            return;
                        }

                        // 텍스트박스4에 표시할 값 계산
                        int result = Convert.ToInt32(dataTable.Rows[0]["AnotherColumn"]) * textBox3Value;

                        // 텍스트박스4에 결과 표시
                        textBox4.Text = result.ToString();
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
