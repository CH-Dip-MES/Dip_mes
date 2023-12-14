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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dip_mes.product
{
    public partial class product02 : UserControl
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        private DataGridViewComboBoxColumn statusComboColumn;

        public product02()
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
                    string query = "SELECT No, Product, Process, Planned, Actual, Estimated, Status FROM product";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // "Status" 열이 없으면 추가
                        if (!dataTable.Columns.Contains("Status"))
                        {
                            DataColumn statusColumn = new DataColumn("Status", typeof(string));
                            dataTable.Columns.Add(statusColumn);
                        }

                        // DataGridView에 "Status" 열을 콤보박스로 설정
                        statusComboColumn = new DataGridViewComboBoxColumn();
                        statusComboColumn.HeaderText = "Status";
                        statusComboColumn.Name = "Status";
                        statusComboColumn.DataSource = GetStatusOptions(); // 콤보박스의 옵션을 설정하는 메서드 호출
                        statusComboColumn.DisplayMember = "StatusOption"; // 표시될 멤버 설정
                        statusComboColumn.DataPropertyName = "Status"; // 데이터 소스의 컬럼과 매핑 

                        statusComboColumn.DefaultCellStyle.NullValue = "작업대기";

                        // "Status" 열이 이미 있으면 제거
                        if (dataGridView1.Columns.Contains("Status"))
                        {
                            dataGridView1.Columns.Remove("Status");
                        }
                        // DataGridView에 "Status" 열 추가
                        dataGridView1.Columns.Add(statusComboColumn);

                        // DataGridView에 데이터 바인딩
                        dataGridView1.DataSource = dataTable;

                        // "Status" 열의 DisplayIndex를 설정하여 원하는 위치로 이동
                        dataGridView1.Columns["Status"].DisplayIndex = 6; // "Status" 열이 6번째에 표시되도록 설정
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private DataTable GetStatusOptions()
        {
            DataTable statusTable = new DataTable();
            statusTable.Columns.Add("StatusOption");

            // 기본값 "작업대기" 추가
            statusTable.Rows.Add("작업대기");
            // 추가 옵션들
            statusTable.Rows.Add("작업시작");
            statusTable.Rows.Add("작업완료");

            // 기본값 "작업대기"를 설정하려면 데이터가 비어있을 때 추가
            if (statusTable.Rows.Count == 0)
            {
                statusTable.Rows.Add("작업대기");
            }

            return statusTable;
        }


        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dataGridView1.Columns["Status"].Index && e.RowIndex >= 0)
                {
                    // 변경된 값 가져오기
                    string newStatusValue = dataGridView1.Rows[e.RowIndex].Cells["Status"].Value?.ToString();

                    if (string.IsNullOrEmpty(newStatusValue))
                    {
                        // 처리할 내용이 없으면 리턴
                        return;
                    }

                    // 해당 행의 기본 키(예: No) 가져오기
                    string primaryKeyValue = dataGridView1.Rows[e.RowIndex].Cells["No"].Value.ToString();

                    // 데이터베이스 업데이트
                    UpdateStatusInDatabase(primaryKeyValue, newStatusValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in DataGridView1_CellValueChanged: {ex.Message}");
            }
        }

        private void UpdateStatusInDatabase(string primaryKeyValue, string newStatusValue)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 데이터베이스 업데이트 쿼리 작성
                    string updateQuery = "UPDATE product SET Status = @newStatus WHERE No = @primaryKey";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@newStatus", newStatusValue);
                        cmd.Parameters.AddWithValue("@primaryKey", primaryKeyValue);

                        // 쿼리 실행
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void UserControl1_Load(object sender, EventArgs e)
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // 텍스트 박스에서 입력된 값 가져오기
                        string inputValue = textBox1.Text.Trim();

                        // MySQL에서 데이터 조회하는 SQL 쿼리
                        string query = "SELECT No, Product, Process, Planned, Actual, Estimated, Status FROM product WHERE Product = @inputValue";

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
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}

