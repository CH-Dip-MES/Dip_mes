using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace dip_mes
{
    public partial class standard : UserControl
    {
        private MySqlConnection connection;
        private string connectionString = "Server=222.108.180.36; Database=mes_2; User ID=EDU_STUDENT; Password=1234;";

        public standard()
        {
            InitializeComponent();
            InitializeDatabaseConnection();

            // 폼이 로드될 때 DataGridView에 컬럼 추가
            InitializeDataGridViewColumns();
        }

        private void InitializeDatabaseConnection()
        {
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection error: " + ex.Message);
            }
        }

        private void InitializeDataGridViewColumns()
        {
            // 체크박스 컬럼 추가
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "checkBoxColumn";
            checkBoxColumn.HeaderText = "체크";
            dataGridView1.Columns.Add(checkBoxColumn);

            // DataGridView에 컬럼 추가
            dataGridView1.Columns.Add("Field2Column", "품번");
            dataGridView1.Columns.Add("Field3Column", "품명");
            dataGridView1.Columns.Add("Field4Column", "제품구분");
            dataGridView1.Columns.Add("Field5Column", "제품규격");
            dataGridView1.Columns.Add("ins_dateColumn", "등록 시간");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // 데이터 그리드뷰에 데이터 추가
            DateTime ins_date = DateTime.Now;

            // 중복 품번 체크
            string productCode = textBox2.Text;
            if (IsProductCodeDuplicate(productCode))
            {
                MessageBox.Show("중복된 품번입니다. 확인 부탁드립니다.");
                return; // 중복이면 더 이상 진행하지 않음
            }

            // 새로운 행 생성
            DataGridViewRow newRow = new DataGridViewRow();

            // 행에 데이터 추가
            newRow.CreateCells(dataGridView1, false, productCode, textBox3.Text, textBox4.Text, textBox5.Text, ins_date);

            // 행을 데이터그리드뷰의 첫 번째 위치에 삽입
            dataGridView1.Rows.Insert(0, newRow);

            // MySQL에 데이터 삽입
            InsertDataIntoMySQL(productCode, textBox3.Text, textBox4.Text, textBox5.Text, ins_date);
        }

        private bool IsProductCodeDuplicate(string productCode)
        {
            // 품번이 중복되는지 데이터베이스에서 확인하는 로직을 구현
            string query = "SELECT COUNT(*) FROM product WHERE product_code = @product_code";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@product_code", productCode);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private void InsertDataIntoMySQL(string product_code, string product_name, string category, string product_size, DateTime ins_date)
        {
            string query = "INSERT INTO product (product_code, product_name, category, product_size, ins_date) VALUES (@product_code, @product_name, @category, @product_size, @ins_date)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@product_code", product_code);
                cmd.Parameters.AddWithValue("@product_name", product_name);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@product_size", product_size);
                cmd.Parameters.AddWithValue("@ins_date", ins_date);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("제품 데이터가 성공적으로 등록되었습니다");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting data into MySQL: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                // 전체 조회
                RetrieveAllData();
            }
            else
            {
                // 해당 품번에 대한 조회
                RetrieveDataByProductCode(textBox1.Text);
            }

            // textbox1 초기화
            textBox1.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 체크된 항목 삭제
            DeleteCheckedItems();
        }

        private void DeleteCheckedItems()
        {
            // 체크된 항목을 반복하여 삭제
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // 체크된 항목인지 확인
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;
                bool isChecked = Convert.ToBoolean(checkBoxCell.Value);

                if (isChecked)
                {
                    // 체크된 경우, 데이터 그리드와 MySQL에서 삭제
                    string productCode = row.Cells["Field2Column"].Value.ToString();
                    DeleteRowFromDataGridView(row);
                    DeleteRowFromMySQL(productCode);
                }
            }
        }

        private void DeleteRowFromDataGridView(DataGridViewRow row)
        {
            // 데이터 그리드에서 특정 행 삭제
            dataGridView1.Rows.Remove(row);
        }

        private void DeleteRowFromMySQL(string productCode)
        {
            // MySQL에서 특정 품번 데이터 삭제
            string query = "DELETE FROM product WHERE product_code = @product_code";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@product_code", productCode);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("데이터가 성공적으로 삭제되었습니다");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting data from MySQL: " + ex.Message);
                }
            }
        }

        private void RetrieveAllData()
        {
            // 데이터그리드뷰 초기화
            dataGridView1.Rows.Clear();

            // MySQL에서 데이터 조회
            string query = "SELECT product_code, product_name, category, product_size, ins_date FROM product";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string productCode = reader.GetString("product_code");
                        string productName = reader.GetString("product_name");
                        string category = reader.GetString("category");
                        string productSize = reader.GetString("product_size");
                        DateTime insDate = reader.GetDateTime("ins_date");

                        // 조회된 데이터를 DataGridView에 추가
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(dataGridView1, false, productCode, productName, category, productSize, insDate);

                        // 체크박스를 제외한 나머지 셀들을 읽기 전용으로 설정
                        // 체크박스를 제외한 나머지 셀들을 읽기 전용으로 설정
                        for (int i = 1; i < newRow.Cells.Count; i++)
                        {
                            newRow.Cells[i].ReadOnly = true;
                        }

                        dataGridView1.Rows.Add(newRow);
                    }
                }
            }
        }

        private void RetrieveDataByProductCode(string productCode)
        {
            // 데이터그리드뷰 초기화
            dataGridView1.Rows.Clear();

            // MySQL에서 해당 품번 데이터 조회
            string query = "SELECT product_code, product_name, category, product_size, ins_date FROM product WHERE product_code = @product_code";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@product_code", productCode);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string productName = reader.GetString("product_name");
                        string category = reader.GetString("category");
                        string productSize = reader.GetString("product_size");
                        DateTime insDate = reader.GetDateTime("ins_date");

                        // 조회된 데이터를 DataGridView에 추가
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(dataGridView1, false, productCode, productName, category, productSize, insDate);

                        // 체크박스를 제외한 나머지 셀들을 읽기 전용으로 설정
                        for (int i = 1; i < newRow.Cells.Count; i++)
                        {
                            newRow.Cells[i].ReadOnly = true;
                        }

                        dataGridView1.Rows.Add(newRow);
                    }
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 체크박스의 값이 변경되었을 때의 이벤트 처리
            if (e.ColumnIndex == dataGridView1.Columns["checkBoxColumn"].Index && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell checkBoxCell = dataGridView1.Rows[e.RowIndex].Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;
                bool isChecked = Convert.ToBoolean(checkBoxCell.Value);

                if (isChecked)
                {
                    // 체크된 경우, 데이터 그리드와 MySQL에서 삭제
                    string productCode = dataGridView1.Rows[e.RowIndex].Cells["Field2Column"].Value.ToString();
                    DeleteRowFromMySQL(productCode);
                }
            }
        }
    }
}