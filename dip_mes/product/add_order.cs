using System;
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

        private void Form2_Load(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=test;Uid=root;Pwd=1234;";
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
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=test;Uid=root;Pwd=1234;"))
            {
                try
                {
                    connection.Open();

                    // 입력된 컬럼 이름을 기반으로 데이터 조회
                    string query = $"SELECT DISTINCT process FROM t12 WHERE product = @inputValue";
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
            string connectionString = "Server=localhost;Database=test;Uid=root;Pwd=1234;"; //테이블 변경과 패스워드 설정

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
                    string query = "INSERT INTO pfm (No, product, process, plan) " + "VALUES (@textBox1, @textBox2, @comboBox1, @textBox3)";

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

                        MessageBox.Show("데이터가 성공적으로 저장되었습니다.");
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
