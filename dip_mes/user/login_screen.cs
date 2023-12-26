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
    public partial class login_screen : Form
    {
        string jConn = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";

        public login_screen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userID = ID.Text;
            string userPW = PW.Text;
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(userPW))
            {
                MessageBox.Show("ID/PW를 입력해주세요");
                return;
            }
            using (MySqlConnection conn = new MySqlConnection(jConn))
            {
                conn.Open();
                string log = "SELECT COUNT(*) FROM test WHERE id = @userID AND pwd = @userPW";
                MySqlCommand cmd = new MySqlCommand(log, conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@userPW", userPW);

                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if (result > 0)
                {
                    // MainScreen 인스턴스를 생성합니다.
                    main_screen mainScreen = new main_screen();

                    // 현재 로그인 폼을 숨깁니다. (또는 this.Close();를 사용해 닫을 수도 있습니다.)
                    this.Hide();

                    // MainScreen 폼을 표시합니다.
                    mainScreen.ShowDialog();

                    // MainScreen 폼이 닫히면 로그인 폼을 다시 표시합니다.
                    this.Show();
                }
                else
                {
                    MessageBox.Show("해당하는 회원정보가 없습니다");
                }
            }
        }
        private void Find_Click(object sender, EventArgs e)
        {
            find_screen find_Screen = new find_screen();
            find_Screen.Show();
        }
        private void New_Click(object sender, EventArgs e)
        {
            join_screen join_screen = new join_screen();
            join_screen.Show();
        }
    }
}