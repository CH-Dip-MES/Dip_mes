using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace dip_mes
{
    public partial class Material : UserControl
    {
        private MySqlConnection connection;
        private string connectionString = "Server=222.108.180.36; Database=mes_2; User ID=EDU_STUDENT; Password=1234;";
        private DataTable dataTable;

        public Material()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);

            // 등록 버튼 클릭 이벤트 핸들러 등록
            RegisterButton1.Click += RegisterButton_Click;

            // 조회 버튼 클릭 이벤트 핸들러 등록
            SearchButton1.Click += SearchButton_Click;

            // 삭제 버튼 클릭 이벤트 핸들러 등록
            DeleteButton1.Click += DeleteButton_Click;

            // 데이터그리드뷰 초기화
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            // 데이터그리드뷰에 체크박스 열 추가
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "CheckBoxColumn";
            checkBoxColumn.HeaderText = "선택";
            dataGridView1.Columns.Add(checkBoxColumn);

            // 데이터 테이블 초기화
            dataTable = new DataTable();
            dataTable.Columns.Add("Material_code", typeof(string));
            dataTable.Columns.Add("Material_name", typeof(string));

            // 데이터그리드뷰에 데이터 테이블 연결
            dataGridView1.DataSource = dataTable;

            // 모든 열에 대해 ReadOnly 속성 설정
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }

            // 특정 열에 대해 수정 가능하도록 예외적으로 설정
            dataGridView1.Columns["CheckBoxColumn"].ReadOnly = false;
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                // 텍스트 박스에서 값을 가져옴
                string value1 = textBox1.Text;
                string value2 = textBox2.Text;

                // 입력값이 비어있는지 확인
                if (string.IsNullOrWhiteSpace(value1) || string.IsNullOrWhiteSpace(value2))
                {
                    MessageBox.Show("정보를 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return; // 등록 중단
                }

                // SQL 쿼리를 작성하여 데이터베이스에 값을 삽입
                string query = "INSERT INTO Material (Material_code, Material_name) VALUES (@value1, @value2)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@value1", value1);
                    cmd.Parameters.AddWithValue("@value2", value2);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("데이터가 성공적으로 등록되었습니다.");

                        // 등록한 데이터를 데이터 테이블에 추가
                        DataRow newRow = dataTable.NewRow();
                        newRow["Material_code"] = value1;
                        newRow["Material_name"] = value2;
                        dataTable.Rows.Add(newRow);

                        // 텍스트 박스 초기화
                        textBox1.Text = string.Empty;
                        textBox2.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("데이터 등록에 실패했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                // 데이터그리드뷰를 초기화
                dataTable.Clear();

                // 등록된 데이터를 가져와서 데이터 테이블에 추가
                string query = "SELECT Material_code, Material_name FROM Material";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                // 선택된 행을 삭제
                List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["CheckBoxColumn"].Value))
                    {
                        string MaterialCode = row.Cells["Material_code"].Value.ToString();
                        string deleteQuery = "DELETE FROM Material WHERE Material_code = @MaterialCode";

                        using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection))
                        {
                            deleteCmd.Parameters.AddWithValue("@MaterialCode", MaterialCode);
                            deleteCmd.ExecuteNonQuery();
                        }

                        rowsToDelete.Add(row);
                    }
                }

                foreach (DataGridViewRow row in rowsToDelete)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection sConn = new MySqlConnection(connectionString))
            {
                sConn.Open();
                string searchItemNo = textBox3.Text.Trim(); // 검색창 텍스트
                string fItem = @"SELECT product.product_name,product.product_code,product_Material.Material_name,product_Material.Material_number
                                FROM product
                                JOIN product_Material ON product_Material.product_code = product.product_code
                                WHERE product.product_name = @searchItemNo";
                MySqlCommand cmd = new MySqlCommand(fItem, sConn);
                cmd.Parameters.AddWithValue("@searchItemNo", searchItemNo);
                try
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable fManage = new DataTable();
                        adapter.Fill(fManage);

                        // Check if any rows are returned
                        if (fManage.Rows.Count > 0)
                        {
                            // DataGridView에 데이터 설정
                            dataGridView2.DataSource = fManage;

                            // DataGridView 컬럼 헤더 텍스트 설정
                            dataGridView2.Columns["product_name"].HeaderText = "제품명";
                            dataGridView2.Columns["product_code"].HeaderText = "제품코드";
                            dataGridView2.Columns["Material_name"].HeaderText = "자재명";
                            dataGridView2.Columns["Material_number"].HeaderText = "자재수량";
                        }
                        else
                        {
                            MessageBox.Show("해당 품명의 데이터가 없습니다.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"오류 발생: {ex.Message}");
                }
            }
        }
    }
}
