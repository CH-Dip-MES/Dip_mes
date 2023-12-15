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

namespace dip_mes.product
{
    public partial class product01 : UserControl
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";

        public product01()
        {
            InitializeComponent();
            LoadDataToDataGridView();
        }

        private void product01_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadDataToDataGridView()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // MySQL에서 데이터 조회하는 SQL 쿼리 (Status가 '작업대기'인 데이터만 조회)
                    string query = "SELECT No, Product, Material, Planned, Input, Residue, Duration FROM product WHERE Status = '작업대기'";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 데이터를 담을 DataTable 생성
                        DataTable dataTable = new DataTable();

                        // MySQLDataAdapter를 사용하여 데이터 가져오기
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }

                        // DataGridView에 데이터 바인딩
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dataTable;

                        // Duration 컬럼의 데이터를 포맷팅하여 표시
                        FormatDurationColumn();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 텍스트 박스에서 입력된 값 가져오기
                    string inputValue = textBox1.Text.Trim();

                    // MySQL에서 데이터 조회하는 SQL 쿼리
                    string query = "SELECT No, Product, Material, Planned,Input, Residue, Duration FROM product WHERE No LIKE @inputValue";

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
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dataTable;

                        // Duration 컬럼의 데이터를 포맷팅하여 표시
                        FormatDurationColumn();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void FormatDurationColumn()
        {
            // Duration 컬럼이 존재하면서 DateTime 형식으로 변환 가능한 경우
            if (dataGridView1.Columns.Contains("Duration") && dataGridView1.Columns["Duration"] is DataGridViewTextBoxColumn durationColumn)
            {
                durationColumn.DefaultCellStyle.Format = "M월 d일 H:mm";
            }
        }
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
