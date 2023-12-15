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
            btnDelete.Click += btnDelete_Click;
        }

        private void serch_btn_Click(object sender, EventArgs e)
        {
            using (MySqlConnection sConn = new MySqlConnection(ConnectionString))
            {
                sConn.Open();
                string searchName = textBox_name.Text.Trim();
                string findName = "select name,id,pwd,number,email,department from test";
                if (!string.IsNullOrEmpty(searchName))
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

                        if (fManage.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = fManage;
                            dataGridView1.Columns["name"].HeaderText = "이름";
                            dataGridView1.Columns["id"].HeaderText = "아이디";
                            dataGridView1.Columns["pwd"].HeaderText = "비밀번호";
                            dataGridView1.Columns["number"].HeaderText = "주민번호";
                            dataGridView1.Columns["email"].HeaderText = "이메일";
                            dataGridView1.Columns["department"].HeaderText = "부서";

                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                if (column.Index != 0)
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection sConn = new MySqlConnection(ConnectionString))
            {
                sConn.Open();

                // 체크된 행을 찾아서 삭제할 목록 생성
                List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // 체크된 행인지 확인
                    bool isChecked = Convert.ToBoolean(row.Cells[0].Value);
                    if (isChecked)
                    {
                        // 삭제할 행을 목록에 추가
                        rowsToRemove.Add(row);

                        // MySQL에서 해당 행 삭제
                        string idToDelete = row.Cells["id"].Value.ToString();
                        string deleteQuery = $"DELETE FROM test WHERE id = '{idToDelete}'";

                        using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, sConn))
                        {
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                }

                // 목록에 있는 행을 역순으로 삭제
                for (int i = rowsToRemove.Count - 1; i >= 0; i--)
                {
                    dataGridView1.Rows.Remove(rowsToRemove[i]);
                }
            }
        }
    }
}