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
    public partial class Form1 : Form
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";

        public Form1()
        {
            InitializeComponent();
            LoadDataToDataGridView1();
        }

        private void LoadDataToDataGridView1()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 두 테이블을 조인하고 원하는 컬럼들을 선택하여 데이터 가져오기
                    string query = "SELECT * FROM product";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // DataGridView에 데이터 바인딩
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 폼 로드 시에 텍스트 박스 속성 설정
            SetDefaultText();

            // 포커스 이벤트 핸들러 등록
            textBox1.GotFocus += TextBox1_GotFocus;
            textBox1.LostFocus += TextBox1_LostFocus;
        }

        private void SetDefaultText()
        {
            // 텍스트 박스에 기본값 설정
            textBox1.Text = "No.입력창";
            textBox1.ForeColor = Color.Gray;
        }

        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            // 포커스가 들어오면 텍스트 지우고 글씨 색 변경
            if (textBox1.Text == "No.입력창")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            // 포커스를 잃으면 텍스트가 비어있으면 다시 기본값 설정
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                SetDefaultText();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            add_order Form = new add_order();
            Form.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
