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
    public partial class join_screen : Form
    {

        /* string _server = "EDU"; //DB 서버 주소, 로컬일 경우 localhost
        int _port = 3306; //DB 서버 포트
        string _database = "mes_2"; //DB 이름
        string _id = "EDU_STUDENT"; //계정 아이디
        string _pw = "1234"; //계정 비밀번호
        string _connectionAddress = ""; */

        public join_screen()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 입력값이 비어 있는지 확인
            if (string.IsNullOrWhiteSpace(txtbox_name.Text) ||
                string.IsNullOrWhiteSpace(txtbox_id.Text) ||
                string.IsNullOrWhiteSpace(txtbox_pwd.Text) ||
                string.IsNullOrWhiteSpace(number.Text) ||
                string.IsNullOrWhiteSpace(email.Text) ||
                string.IsNullOrWhiteSpace(department.Text))
            {
                MessageBox.Show("모든 필드를 입력하세요.");
                return; // 등록 중단
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=222.108.180.36; Database=mes_2; Uid=EDU_STUDENT;Pwd=1234;"))
                {
                    connection.Open();

                    // 아이디 중복 체크
                    string checkIdQuery = "SELECT COUNT(*) FROM test WHERE id = @id";
                    MySqlCommand checkIdCommand = new MySqlCommand(checkIdQuery, connection);
                    checkIdCommand.Parameters.AddWithValue("@id", txtbox_id.Text);

                    int count = Convert.ToInt32(checkIdCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        // 중복된 아이디가 존재하는 경우
                        MessageBox.Show("중복된 아이디입니다. 다른 아이디를 사용해주세요.");
                        return; // 등록 중단
                    }

                    // 중복된 아이디가 없으면 회원가입 처리
                    string insertQuery = "INSERT INTO test (name, id, pwd, number, email, department) VALUES (@name, @id, @pwd, @number, @email, @department)";
                    MySqlCommand command = new MySqlCommand(insertQuery, connection);

                    // 매개변수 추가
                    command.Parameters.AddWithValue("@name", txtbox_name.Text);
                    command.Parameters.AddWithValue("@id", txtbox_id.Text);
                    command.Parameters.AddWithValue("@pwd", txtbox_pwd.Text);
                    command.Parameters.AddWithValue("@number", number.Text);
                    command.Parameters.AddWithValue("@email", email.Text);
                    command.Parameters.AddWithValue("@department", department.Text);

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show(txtbox_name.Text + "님 회원가입 완료, 사용할 아이디는" + txtbox_id.Text + "입니다");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("비정상 입력 정보, 재확인 요망");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}