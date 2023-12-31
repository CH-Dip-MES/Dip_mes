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
    public partial class Login : Form
    {
        private Join popJoin;
        public static int getAuth;
        string jConn = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        public event Action<int> UserAuthStock;

        public Login()
    {
        InitializeComponent();
        // 이 코드는 로그인 폼의 생성자 또는 초기화 메서드에서 설정합니다.
        this.AcceptButton = button1;

        // 각 컨트롤의 TabIndex 설정
        ID.TabIndex = 1;
        PW.TabIndex = 2;
        button1.TabIndex = 3;
        Find.TabIndex = 4;
        New.TabIndex = 5;
    }
        public class UserAuthCheck //권한 조회
        {
            private static string jConn = "Server=222.108.180.36; Database=mes_2; Uid=EDU_STUDENT; Pwd=1234;";

            public static int GetUserAuth(string userID)
            {
                int userAuth = 0;

                using (MySqlConnection connection = new MySqlConnection(jConn))
                {
                    connection.Open();
                    string query = "SELECT authority FROM user WHERE id = @id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", userID);

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        userAuth = Convert.ToInt32(result);
                    }
                }

                return userAuth;
            }
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
                string log = "SELECT name FROM user WHERE id = @userID AND pwd = @userPW";
                MySqlCommand cmd = new MySqlCommand(log, conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@userPW", userPW);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string userName = result.ToString();
                    int userAuth = UserAuthCheck.GetUserAuth(userID);
                    getAuth = userAuth;
                    UserAuthStock?.Invoke(userAuth);
                    Console.WriteLine(getAuth);
                    MainScreen mainScreen = new MainScreen
                    {
                        UserID = userID,
                        UserName = userName
                    };
                    this.Hide();
                    mainScreen.ShowDialog();
                    this.Show();

                    // MainScreen 폼이 닫히면 로그인 폼을 종료합니다.
                    this.Close();
                }
                else
                {
                    MessageBox.Show("해당하는 회원정보가 없습니다");
                }
            }
        }

        private void Find_Click(object sender, EventArgs e)
        {
            FindUser FindUser = new FindUser();
            FindUser.Show();
        }
        private void New_Click(object sender, EventArgs e)
        {
            popJoin = new Join();
            popJoin.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}