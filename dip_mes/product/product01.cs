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
    public partial class product01 : UserControl
    {
        public product01()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 텍스트 박스에서 입력된 값 가져오기
                    string inputValue = textBox1.Text.Trim();

                    // MySQL에서 데이터 조회하는 SQL 쿼리
                    string query = "SELECT * FROM 제품명 테이블 WHERE 제품명 컬럼 = @inputValue";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 설정
                        cmd.Parameters.AddWithValue("@inputValue", inputValue);

                        // 데이터를 담을 DataTable 생성
                        DataTable dataTable = new DataTable();

                        // MySQLDataAdapter를 사용하여 데이터 가져오기
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }

                        // DataGridView에 데이터 바인딩
                        dataGridView1.DataSource = dataTable;

                        if (dataTable.Rows.Count > 0)
                        {
                            // DataGridView에 데이터 바인딩
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void product01_Load(object sender, EventArgs e)
        {

        }
    }
}
