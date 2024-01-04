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
using static Google.Protobuf.WellKnownTypes.Field.Types;
namespace dip_mes
{
    public partial class Join : Form
    {
        //테스트용 주석
        /* string _server = "EDU"; //DB 서버 주소, 로컬일 경우 localhost
        int _port = 3306; //DB 서버 포트
        string _database = "mes_2"; //DB 이름
        string _id = "EDU_STUDENT"; //계정 아이디
        string _pw = "1234"; //계정 비밀번호
        string _connectionAddress = ""; */

        public Join()
        {
            InitializeComponent();
            txtbox_name.TabIndex = 1;
            txtbox_id.TabIndex = 2;
            txtbox_pwd.TabIndex = 3;
            number.TabIndex = 4;
            email.TabIndex = 5;
            Department.TabIndex = 6;
            button1.TabIndex = 7;
            this.AcceptButton = button1;
        }
        
        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // 입력값이 비어 있는지 확인
            if (string.IsNullOrWhiteSpace(txtbox_name.Text) ||
                string.IsNullOrWhiteSpace(txtbox_id.Text) ||
                string.IsNullOrWhiteSpace(txtbox_pwd.Text) ||
                string.IsNullOrWhiteSpace(number.Text) ||
                string.IsNullOrWhiteSpace(email.Text) ||
                string.IsNullOrWhiteSpace(Department.Text))
            {
                MessageBox.Show("모든 정보를 입력하세요.");
                return; // 등록 중단
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=222.108.180.36; Database=mes_2; Uid=EDU_STUDENT;Pwd=1234;"))
                {
                    connection.Open();

                    // 아이디 중복 체크
                    string checkIdQuery = "SELECT COUNT(*) FROM user WHERE id = @id";
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
                    int authority;
                    if(txtbox_name.Text == "송상욱") // 특정 관리자 이름 일 시
                    {
                        authority = 3;
                    }
                    else
                    {
                        switch(Department.Text)
                        {
                            case "영업부":
                                authority = 2;
                                break;
                            case "생산부":
                                authority = 1; // 생산부 권한
                                break;
                            default:
                                authority = 0; // 기본 권한 (접근 제한)
                                break;
                        }
                    }
                    string insertQuery = "INSERT INTO user (name, id, pwd, number, email, department, authority) VALUES (@name, @id, @pwd, @number, @email, @department, @authority)";
                    MySqlCommand command = new MySqlCommand(insertQuery, connection);

                    // 매개변수 추가
                    command.Parameters.AddWithValue("@name", txtbox_name.Text);
                    command.Parameters.AddWithValue("@id", txtbox_id.Text);
                    command.Parameters.AddWithValue("@pwd", txtbox_pwd.Text);
                    command.Parameters.AddWithValue("@number", number.Text);
                    command.Parameters.AddWithValue("@email", email.Text);
                    command.Parameters.AddWithValue("@department", Department.Text);
                    command.Parameters.AddWithValue("@authority", authority);

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

        private void Join_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
