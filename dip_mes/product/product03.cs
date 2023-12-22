using dip_mes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_mes
{
    public partial class product03 : UserControl
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        private DateTime startDate;
        public product03()
        {
            InitializeComponent();
            // dateTimePicker1 초기화
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker1.Checked = false;
            LoadDataToDataGridView();
        }
        public string GetSearchValue()
        {
            return textBox1.Text.Trim();
        }
        public string TextBox1Value
        {
            get { return textBox1.Text; }
        }
        private void product03_Load(object sender, EventArgs e)
        {
            
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            // textBox1의 값이 비어있지 않은 경우에만 폼을 열기
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                // 실제 데이터베이스 조회 로직을 통해 Lot이 존재하는지 확인
                if (CheckLotExists(textBox1.Text))
                {
                    Lot myForm = new Lot(this);
                    myForm.TopLevel = false; // 폼이 최상위 수준이 아닌 자식으로 설정
                    myForm.FormBorderStyle = FormBorderStyle.None; // 테두리 제거
                    panel1.Controls.Add(myForm); // 패널에 폼 추가
                    myForm.Show(); // 폼을 표시
                }
                else
                {
                    MessageBox.Show("입력된 데이터와 일치하는 Lot이 없습니다. 확인 후 다시 시도하세요.");
                }
            }
            else
            {
                MessageBox.Show("Lot 번호를 먼저 입력해주세요.");
            }
        }
        
            private void LoadDataToDataGridView()
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // My   SQL에서 데이터 조회하는 SQL 쿼리 (Status가 '작업대기'인 데이터만 조회)
                        string query = "SELECT No, Product AS '제품명', Planned AS '계획수량', Actual AS '실적수량', Operator AS '작업자', Worktime AS '작업시간' FROM manufacture WHERE Status = '작업완료'";

                        // 선택한 날짜가 있는 경우에만 날짜 조건을 추가
                        if (dateTimePicker1.Checked)
                        {
                            query += " AND DATE(Duration) = @SelectedDate";
                        }

                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            // 선택한 날짜가 있는 경우에만 파라미터 추가
                            if (dateTimePicker1.Checked)
                            {
                                cmd.Parameters.AddWithValue("@SelectedDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                            }

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
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        private bool CheckLotExists(string lotValue)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 데이터베이스에서 lotValue를 사용하여 일치하는 데이터 조회
                    string query = "SELECT COUNT(*) FROM manufacture WHERE Lot = @lotValue";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@lotValue", lotValue);

                        // ExecuteScalar를 사용하여 결과 개수를 가져옴
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // 결과가 1 이상이면 데이터가 존재함을 의미
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
        
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // 선택한 날짜 저장
            startDate = dateTimePicker1.Value;

            // 데이터 조회 및 그리드 갱신
            LoadDataToDataGridView();
        }
    }
}
