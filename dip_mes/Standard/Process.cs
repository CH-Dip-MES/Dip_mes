using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Google.Protobuf.WellKnownTypes.Field.Types;

namespace dip_mes
{
    public partial class Process : UserControl
    {
        private MySqlConnection connection;
        private string connectionString = "Server=222.108.180.36; Database=mes_2; User ID=EDU_STUDENT; Password=1234;";
        private DataTable dataTable;

        public Process()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);

            // 데이터그리드뷰 초기화
            InitializeDataGridView();
            InitializeComboBox(); // comboBox1 초기화 추가

            textBox1.TabIndex = 1;
            textBox2.TabIndex = 2;
            SearchButton.TabIndex = 3;
            comboBox1.TabIndex = 4; 
            button1.TabIndex = 5;

            textBox1.KeyPress += textBox1_KeyPress;
            textBox2.KeyPress += textBox2_KeyPress;
            comboBox1.KeyPress += comboBox1_KeyPress; // comboBox1의 KeyPress 이벤트 추가
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
            dataTable.Columns.Add("process_code", typeof(string));
            dataTable.Columns.Add("process_name", typeof(string));

            // 데이터그리드뷰에 데이터 테이블 연결
            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns["process_code"].HeaderText = "공정코드";
            dataGridView1.Columns["process_name"].HeaderText = "공정명";
            // 모든 열에 대해 ReadOnly 속성 설정
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }

            // 특정 열에 대해 수정 가능하도록 예외적으로 설정
            dataGridView1.Columns["CheckBoxColumn"].ReadOnly = false;
        }
        private void InitializeComboBox()
        {
            // comboBox1에 데이터베이스에서 가져온 product_name 추가
            string query = "SELECT product_name FROM product";

            try
            {
                connection.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["product_name"].ToString());
                        }
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
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 엔터 키를 누르면
            if (e.KeyChar == (char)Keys.Enter)
            {
                // button1 클릭
                SearchButton.PerformClick();
                e.Handled = true; // 이벤트 처리 완료
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 엔터 키를 누르면
            if (e.KeyChar == (char)Keys.Enter)
            {
                // button2 클릭
                SearchButton.PerformClick();
                e.Handled = true; // 이벤트 처리 완료
            }
        }
        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 엔터 키를 누르면
            if (e.KeyChar == (char)Keys.Enter)
            {
                // button1 클릭
                button1.PerformClick();
                e.Handled = true; // 이벤트 처리 완료
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
                string query;

                if (string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    // textBox1과 textBox2가 모두 비어 있으면 전체 조회
                    query = "SELECT process_code, process_name FROM process";
                }
                else if (string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    // textBox1이 비어 있고 textBox2가 값이 있으면 해당 값으로 검색
                    query = "SELECT process_code, process_name FROM process WHERE process_name = @value";
                }
                else if (!string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    // textBox2가 비어 있고 textBox1이 값이 있으면 해당 값으로 검색
                    query = "SELECT process_code, process_name FROM process WHERE process_code = @value";
                }
                else
                {
                    // textBox1과 textBox2가 모두 값이 있으면 OR 조건으로 검색
                    query = "SELECT process_code, process_name FROM process WHERE process_code = @value OR process_name = @value";
                }

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // textBox1이나 textBox2 중 하나라도 값이 있을 때만 검색어를 파라미터에 추가
                    if (!string.IsNullOrWhiteSpace(textBox1.Text) || !string.IsNullOrWhiteSpace(textBox2.Text))
                    {
                        cmd.Parameters.AddWithValue("@value", !string.IsNullOrWhiteSpace(textBox1.Text) ? textBox1.Text : textBox2.Text);
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                // 검색 결과가 없을 때 메시지 표시
                if (dataTable.Rows.Count == 0)
                {
                    string message = string.IsNullOrWhiteSpace(textBox2.Text) ? "정확한 코드을 입력해주세요." : "정확한 공정을 입력해주세요.";
                    MessageBox.Show(message, "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            // 텍스트 박스 초기화
            textBox1.Clear();
            textBox2.Clear();
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
                        string processCode = row.Cells["process_code"].Value.ToString();
                        string deleteQuery = "DELETE FROM process WHERE process_code = @processCode";

                        using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection))
                        {
                            deleteCmd.Parameters.AddWithValue("@processCode", processCode);
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
                string searchItemNo = comboBox1.Text.Trim(); // 검색창 텍스트
                string fItem = @"SELECT product.product_name,product.product_code,product_process.process_name,product_process.process_time
                        FROM product
                        JOIN product_process ON product_process.product_code = product.product_code
                        WHERE product.product_name = @searchItemNo";
                MySqlCommand cmd = new MySqlCommand(fItem, sConn);
                cmd.Parameters.AddWithValue("@searchItemNo", searchItemNo); // 파라미터 추가

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
                            dataGridView2.Columns["process_name"].HeaderText = "공정명";
                            dataGridView2.Columns["process_time"].HeaderText = "공정시간";
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

        private void RegisterButton_Click_1(object sender, EventArgs e)
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

                }
                else
                {
                    // SQL 쿼리를 작성하여 데이터베이스에 값을 삽입
                    string query = "INSERT INTO process (process_code, process_name) VALUES (@value1, @value2)";

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
                            newRow["process_code"] = value1;
                            newRow["process_name"] = value2;
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

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}