using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dip_mes
{
    public partial class Lot : Form
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        private MfgResult parentForm; // 부모폼을 저장하기 위한 필드
        public Lot(MfgResult MfgResult)
        {
            InitializeComponent();
            parentForm = MfgResult; // 부모폼을 필드에 저장
            LoadDataToDataGridView();
            // 폼 로드 시 라벨 설정
            SetLabelValue();
        }
        private void SetLabelValue()
        {
            // 부모 폼의 TextBox1Value를 자식 폼의 Label1.Text로 설정
            label1.Text = parentForm.TextBox1Value;
        }
        private void Lot_Load(object sender, EventArgs e)
        {
            // 부모 폼의 TextBox1Value를 자식 폼의 Label1.Text로 설정
            label1.Text = parentForm.TextBox1Value;
        }
        private void LoadDataToDataGridView()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 부모폼에서 GetSearchValue 메서드를 호출하여 텍스트 박스에서 입력된 값 가져오기
                    string inputValue = parentForm.GetSearchValue();

                    // MySQL에서 데이터 조회하는 SQL 쿼리 (Status가 '작업완료'이고 Lot이나 Part에 입력된 값과 일치하는 데이터만 조회)
                    string query = "SELECT Lot, Part AS '품번', Product AS '제품명', Actual AS '실적수량', Worktime AS '작업시간', Status AS '작업상태' FROM manufacture WHERE Status = '작업완료' AND (Lot LIKE @inputValue OR Part LIKE @inputValue)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 설정
                        cmd.Parameters.AddWithValue("@inputValue", '%' + inputValue + '%');

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
    }
}
