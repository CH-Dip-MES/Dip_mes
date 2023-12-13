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
            try
            {
                MySqlConnection connection = new MySqlConnection("Server = 222.108.180.36; Database=mes_2; Uid=EDU_STUDENT;Pwd=1234;");
                connection.Open();

                string insertQuery = "INSERT INTO test (name, id, pwd) VALUES ('" + txtbox_name.Text + "', '" + txtbox_id.Text + "', '" + txtbox_pwd.Text + "');";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show(txtbox_name.Text + "님 회원가입 완료, 사용할 아이디는" + txtbox_id + "입니다");
                    connection.Close();
                    Close();
                }
                else
                {
                    MessageBox.Show("비정상 입력 정보, 재확인 요망");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            

            

            }
        }
    }


