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

namespace dip_mes.register
{

    
    public partial class signup : UserControl

    {
        
        private MySqlConnection connection;
        private string ConnectionString = "server=222.108.180.36; Uid=EDU_STUDENT; password=1234; database=mes_2;";

        public signup()
        {
            InitializeComponent();
            

        }

        private void serch_btn_Click(object sender, EventArgs e)
        {
            using (MySqlConnection sConn = new MySqlConnection(ConnectionString))
            {
                sConn.Open();
                string searchName = textBox_name.Text.Trim(); // 검색창 텍스트
                string findName = "select name,id,pwd,number,email,department from test";
                if (!string.IsNullOrEmpty(searchName)) // 검색창에 입력한 문자 있을 시 활성화 없으면 위의 fItem 문구 그대로
                {
                    findName += $" WHERE name = '{searchName}'";
                }
                MySqlCommand cmd = new MySqlCommand(findName, sConn);
                try
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable fManage = new DataTable();
                        adapter.Fill(fManage);

                        dataGridView1.Columns.Clear();
                        DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                        checkBoxColumn.HeaderText = "선택";
                        dataGridView1.Columns.Insert(0, checkBoxColumn);

                        // Check if any rows are returned
                        if (fManage.Rows.Count > 0)
                        {
                            // DataGridView에 데이터 설정
                            dataGridView1.DataSource = fManage;

                            // DataGridView 컬럼 헤더 텍스트 설정
                            dataGridView1.Columns["name"].HeaderText = "이름";
                            dataGridView1.Columns["id"].HeaderText = "아이디";
                            dataGridView1.Columns["pwd"].HeaderText = "비밀번호";
                            dataGridView1.Columns["number"].HeaderText = "주민번호";
                            dataGridView1.Columns["email"].HeaderText = "이메일";
                            dataGridView1.Columns["department"].HeaderText = "부서";

                            // Make all columns read-only except the checkbox column
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                if (column.Index != 0) // Skip the checkbox column
                                {
                                    column.ReadOnly = true;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("해당 이름의 데이터가 없습니다.");
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"오류 발생: {ex.Message}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            login.join_screen joinsc = new login.join_screen();
            joinsc.ShowDialog();
        }
    }
}