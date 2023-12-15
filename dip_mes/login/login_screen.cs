using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_mes.login
{
    public partial class login_screen : Form
    {
        public login_screen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userID = ID.Text;
            string userPW = PW.Text;

            if (userID.Equals("admin") && userPW.Equals("1234"))
            {
                MessageBox.Show("로그인에 성공했습니다.", "로그인");
            }
            else
            {
                MessageBox.Show("로그인에 실패했습니다", "로그인");
            }
        }
    }
}