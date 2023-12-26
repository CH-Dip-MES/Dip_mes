using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

namespace dip_mes
{
    public partial class find_screen : Form
    {



        private const string ConnectionString = "server= 222.108.180.36; Uid=EDU_STUDENT; password=1234; database=mes_2;";
        public find_screen()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 사용자로부터 이름과 이메일을 가져오기
                string name = textBox_name.Text;
                string email = textBox_email.Text;

                // MySQL 연결 및 쿼리 수행
                 MySqlConnection connection = new MySqlConnection(ConnectionString);
                connection.Open();

                string query = "SELECT id FROM test WHERE name = @name AND email = @email";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    MessageBox.Show("아이디는 " + result.ToString() + "입니다.");
                }
                else
                {
                    MessageBox.Show("일치하는 사용자가 없습니다.");
                }

            }

    
            catch (Exception ex)
            {
                MessageBox.Show($"에러 발생: {ex.Message}", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // 사용자로부터 이름과 이메일을 가져오기
                string name = textBox_name2.Text;
                string id = textBox_ID.Text;
                string number = textBox_number.Text;

                // MySQL 연결 및 쿼리 수행
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                connection.Open();

                string query = "SELECT pwd FROM test WHERE name = @name AND id = @id AND number = @number ";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@number", number);


                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    MessageBox.Show("비밀번호는 " + result.ToString() + "입니다.");
                }
                else
                {
                    MessageBox.Show("일치하는 사용자가 없습니다.");
                }

            }


            catch (Exception ex)
            {
                MessageBox.Show($"에러 발생: {ex.Message}", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}



