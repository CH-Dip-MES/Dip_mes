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
    public partial class Client : UserControl
    {
        private string connectionString = "Server=222.108.180.36; Database=mes_2; User ID=EDU_STUDENT; Password=1234;";
        private int pageSize = 25;  // 한 페이지에 보여질 행 수
        private int currentPage = 1; // 현재 페이지

        public Client()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void Client_Load(object sender, EventArgs e)
        {
            RetrieveDataAndBindToGridView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Login.getAuth < 2)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            // bnss_registration 폼 인스턴스 생성
            AddClient registrationForm = new AddClient();

            // ShowDialog를 사용하여 팝업 창으로 표시
            registrationForm.ShowDialog();
        }

        private void RetrieveDataAndBindToGridView()
        {
            int offset = (currentPage - 1) * pageSize;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $"SELECT *, DATE_FORMAT(registrationdate, '%Y년 %m월 %d일') AS formattedDate FROM business ORDER BY registrationdate DESC LIMIT {offset}, {pageSize}";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // DataGridView 초기화
                            dataGridView1.Columns.Clear();

                            // DataGridView에 체크박스 열 추가
                            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                            checkBoxColumn.Name = "체크";
                            dataGridView1.Columns.Add(checkBoxColumn);

                            // 체크박스 열의 헤더 텍스트 및 크기 조절
                            dataGridView1.Columns["체크"].HeaderText = "체크"; // 헤더 텍스트 지정
                            dataGridView1.Columns["체크"].Width = 40; // 원하는 크기로 조절

                            // DataGridView에 "companycode" 열 추가
                            dataGridView1.Columns.Add("companycode", "업체코드");

                            // 나머지 열은 그대로 유지
                            dataGridView1.Columns.Add("division", "구분");
                            dataGridView1.Columns.Add("companyname", "업체명");
                            dataGridView1.Columns.Add("phonenumber", "전화번호");
                            dataGridView1.Columns.Add("address", "주소");
                            dataGridView1.Columns.Add("formattedDate", "등록일"); // 변경된 등록일

                            // formattedDate 열에 대한 정렬 비활성화
                            dataGridView1.Columns["formattedDate"].SortMode = DataGridViewColumnSortMode.NotSortable;

                            // 첫 번째 체크박스 열을 제외한 모든 열을 읽기 전용으로 설정
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                if (column.Index != 0) // 첫 번째 체크박스 열은 제외
                                {
                                    column.ReadOnly = true;
                                }
                            }

                            // 데이터로 행 채우기
                            int rowIndex = 0;
                            foreach (DataRow row in dataTable.Rows)
                            {
                                // "companycode" 열에 순서를 변경하여 데이터 삽입
                                dataGridView1.Rows.Add(false, row["companycode"],
                                    row["division"], row["companyname"], row["phonenumber"], row["address"], row["formattedDate"]);

                                rowIndex++;
                            }
                        }
                    }

                    // label3 업데이트
                    label3.Text = $"페이지 {currentPage}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 선택된 행을 기준으로 데이터베이스에서 삭제
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // DataGridView 첫 번째 열이 체크되어 있는지 확인
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["체크"] as DataGridViewCheckBoxCell;

                if (checkBoxCell != null && Convert.ToBoolean(checkBoxCell.Value))
                {
                    // 첫 번째 열(체크박스 열)이 체크된 행의 데이터를 기반으로 삭제 쿼리 실행
                    string companyname = row.Cells["companyname"].Value.ToString(); // 회사명을 기준으로 예시
                    DeleteRowFromDatabase(companyname); // 실제로 데이터베이스에서 삭제하는 메서드 호출
                }
            }

            // 삭제 후 DataGridView 다시 로드
            RetrieveDataAndBindToGridView();
        }

        private void DeleteRowFromDatabase(string companyname)
        {
            // 데이터베이스 연결 및 삭제 쿼리 실행
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM business WHERE companyname = @companyname";

                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@companyname", companyname);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("삭제 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // textBox1에 입력된 값 가져오기
            string searchValue = textBox1.Text.Trim();

            // 검색 값이 비어있으면 모든 데이터 조회
            if (string.IsNullOrEmpty(searchValue))
            {
                RetrieveDataAndBindToGridView();
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 데이터베이스에서 입력된 값과 일치하는 행들을 조회하는 쿼리
                    string query = "SELECT *, DATE_FORMAT(registrationdate, '%Y년 %m월 %d일') AS formattedDate FROM business WHERE companyname LIKE @searchValue ORDER BY registrationdate DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%"); // 부분 일치 검색을 위해 LIKE 사용

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // 검색된 데이터가 없으면 메시지 박스 표시
                            if (dataTable.Rows.Count == 0)
                            {
                                MessageBox.Show("없는 업체명입니다. 다시 확인해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            // DataGridView 초기화
                            dataGridView1.Columns.Clear();

                            // DataGridView에 체크박스 열 추가
                            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                            checkBoxColumn.Name = "체크";
                            dataGridView1.Columns.Add(checkBoxColumn);

                            // 체크박스 열의 헤더 텍스트 및 크기 조절
                            dataGridView1.Columns["체크"].HeaderText = "체크"; // 헤더 텍스트 지정
                            dataGridView1.Columns["체크"].Width = 50; // 원하는 크기로 조절

                            // DataGridView에 "companycode" 열 추가
                            dataGridView1.Columns.Add("companycode", "업체코드");

                            // 나머지 열은 그대로 유지
                            dataGridView1.Columns.Add("division", "구분");
                            dataGridView1.Columns.Add("companyname", "회사명");
                            dataGridView1.Columns.Add("phonenumber", "전화번호");
                            dataGridView1.Columns.Add("address", "주소");
                            dataGridView1.Columns.Add("formattedDate", "등록일"); // 변경된 등록일

                            // formattedDate 열에 대한 정렬 비활성화
                            dataGridView1.Columns["formattedDate"].SortMode = DataGridViewColumnSortMode.NotSortable;

                            // 첫 번째 체크박스 열을 제외한 모든 열을 읽기 전용으로 설정
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                if (column.Index != 0) // 첫 번째 체크박스 열은 제외
                                {
                                    column.ReadOnly = true;
                                }
                            }

                            // 데이터로 행 채우기
                            int rowIndex = 0;
                            foreach (DataRow row in dataTable.Rows)
                            {
                                // "companycode" 열에 순서를 변경하여 데이터 삽입
                                dataGridView1.Rows.Add(false, row["companycode"],
                                    row["division"], row["companyname"], row["phonenumber"], row["address"], row["formattedDate"]);

                                rowIndex++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 오류가 발생한 경우 메시지를 표시
                    MessageBox.Show("오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    textBox1.Clear(); // 예외 발생 여부와 관계없이 textBox1 초기화
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                RetrieveDataAndBindToGridView();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int totalRowCount = GetTotalRowCount();
            int maxPage = (int)Math.Ceiling((double)totalRowCount / pageSize);

            if (currentPage < maxPage)
            {
                currentPage++;
                RetrieveDataAndBindToGridView();
            }
        }

        // GetTotalRowCount 메서드 추가
        private int GetTotalRowCount()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string countQuery = "SELECT COUNT(*) FROM business";
                    using (MySqlCommand countCmd = new MySqlCommand(countQuery, connection))
                    {
                        return Convert.ToInt32(countCmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("데이터 수 조회 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
        }
    }
}
