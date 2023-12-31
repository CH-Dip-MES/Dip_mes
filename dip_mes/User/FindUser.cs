using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace dip_mes
{
    public partial class FindUser : Form
    {
        private const string ConnectionString = "server= 222.108.180.36; Uid=EDU_STUDENT; password=1234; database=mes_2;";

        public FindUser()
        {
            InitializeComponent();
            SetTabOrder();
        }

        private void SetTabOrder()
        {
            textBox_name.TabIndex = 1;
            textBox_email.TabIndex = 2;
            button1.TabIndex = 3;
            textBox_name2.TabIndex = 4;
            textBox_ID.TabIndex = 5;
            textBox_number.TabIndex = 6;
            button2.TabIndex = 7;

            // 엔터 키를 누를 때 이벤트 핸들러 등록
            textBox_name.KeyDown += (sender, e) => { if (e.KeyCode == Keys.Enter) button1.PerformClick(); };
            textBox_email.KeyDown += (sender, e) => { if (e.KeyCode == Keys.Enter) button1.PerformClick(); };
            textBox_name2.KeyDown += (sender, e) => { if (e.KeyCode == Keys.Enter) button2.PerformClick(); };
            textBox_ID.KeyDown += (sender, e) => { if (e.KeyCode == Keys.Enter) button2.PerformClick(); };
            textBox_number.KeyDown += (sender, e) => { if (e.KeyCode == Keys.Enter) button2.PerformClick(); };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 사용자로부터 이름과 이메일을 가져오기
                string name = textBox_name.Text;
                string email = textBox_email.Text;

                // MySQL 연결 및 쿼리 수행
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT id FROM user WHERE name = @name AND email = @email";
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

                textBox_name.Clear();
                textBox_email.Clear();
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
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT pwd FROM user WHERE name = @name AND id = @id AND number = @number ";
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

                textBox_name2.Clear();
                textBox_ID.Clear();
                textBox_number.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"에러 발생: {ex.Message}", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
           this.Close();
        }
    }
}
