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

namespace dip_mes.goods
{
    public partial class bnss_registration : Form
    {
        private string connectionString = "Server=222.108.180.36; Database=mes_2; User ID=EDU_STUDENT; Password=1234;";

        public bnss_registration()
        {
            InitializeComponent();
        }

        private void bnss_registration_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 각 TextBox에서 텍스트 가져오기
            string companyName = textBox1.Text;
            string phoneNumber = textBox2.Text;
            string address = textBox3.Text;
            string companycode = textBox4.Text;

            // 텍스트박스가 비어있는지 확인
            if (string.IsNullOrEmpty(companyName) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(address))
            {
                // 하나라도 비어있으면 메시지 박스 표시 후 함수 종료
                MessageBox.Show("내용을 적으세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // MySQL 연결
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // MySQL에 데이터 삽입하는 SQL 쿼리
                    string query = "INSERT INTO business (division, companyname, phonenumber, address, registrationdate, companycode) VALUES (@division, @companyname, @phonenumber, @address, NOW(),@companycode)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 추가
                        cmd.Parameters.AddWithValue("@division", "협력사");
                        cmd.Parameters.AddWithValue("@companyname", companyName);
                        cmd.Parameters.AddWithValue("@phonenumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@companycode", companycode);

                        // 쿼리 실행
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("데이터가 성공적으로 저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 각 TextBox에서 텍스트 가져오기
            string companyName = textBox1.Text;
            string phoneNumber = textBox2.Text;
            string address = textBox3.Text;
            string companycode = textBox4.Text;

            // 텍스트박스가 비어있는지 확인
            if (string.IsNullOrEmpty(companyName) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(address))
            {
                // 하나라도 비어있으면 메시지 박스 표시 후 함수 종료
                MessageBox.Show("내용을 적으세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // MySQL 연결
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // MySQL에 데이터 삽입하는 SQL 쿼리
                    string query = "INSERT INTO business (division, companyname, phonenumber, address, registrationdate,companycode) VALUES (@division, @companyname, @phonenumber, @address, NOW(),@companycode)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 추가
                        cmd.Parameters.AddWithValue("@division", "고객사");
                        cmd.Parameters.AddWithValue("@companyname", companyName);
                        cmd.Parameters.AddWithValue("@phonenumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@companycode", companycode);


                        // 쿼리 실행
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("데이터가 성공적으로 저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
