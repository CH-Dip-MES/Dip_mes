using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dip_mes
{
    public partial class Product : UserControl
    {
        private MySqlConnection connection;
        private string connectionString = "Server=222.108.180.36; Database=mes_2; User ID=EDU_STUDENT; Password=1234;";

        public Product()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            InitializeDataGridViewColumns();
            LoadDataIntoComboBox1();
            LoadDataIntoComboBox2();
            dataGridView1.CellClick += dataGridView1_CellClick;
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
            DataGridViewCheckBoxColumn checkBoxColumn1 = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn checkBoxColumn2 = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn checkBoxColumn3 = new DataGridViewCheckBoxColumn();
            checkBoxColumn1.Name = "checkBoxColumn";
            checkBoxColumn1.HeaderText = "체크";
            checkBoxColumn2.Name = "checkBoxColumn";
            checkBoxColumn2.HeaderText = "체크";
            checkBoxColumn3.Name = "checkBoxColumn";
            checkBoxColumn3.HeaderText = "체크";
            dataGridView1.Columns.Add(checkBoxColumn1);
            dataGridView2.Columns.Add(checkBoxColumn2);
            dataGridView3.Columns.Add(checkBoxColumn3);

            // DataGridView에 컬럼 추가
            dataGridView1.Columns.Add("Field2Column", "품번");
            dataGridView1.Columns.Add("Field3Column", "품명");
            dataGridView1.Columns.Add("Field4Column", "제품구분");
            dataGridView1.Columns.Add("Field5Column", "제품규격");
            dataGridView1.Columns.Add("ins_dateColumn", "등록 시간");

            // DateGridView2 컬럼 추가
            dataGridView2.Columns.Add("Field2Column", "제품번호");
            dataGridView2.Columns.Add("Field3Column", "공정명");
            dataGridView2.Columns.Add("Field4Column", "공정시간");
            dataGridView2.Columns.Add("Field5Column", "id");

            // DateGridView3 컬럼 추가
            dataGridView3.Columns.Add("Field2Column", "제품번호");
            dataGridView3.Columns.Add("Field3Column", "자재명");
            dataGridView3.Columns.Add("Field4Column", "자재수량");
            dataGridView3.Columns.Add("Field5Column", "id");

            // ComboBox 추가
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Add("선택하세요");  // 초기 선택 항목 추가
            comboBox1.SelectedIndex = 0;
            Controls.Add(comboBox1);  // 폼에 컨트롤 추가
            // ComboBox 추가
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.Add("선택하세요");  // 초기 선택 항목 추가
            comboBox2.SelectedIndex = 0;
            Controls.Add(comboBox2);  // 폼에 컨트롤 추가

            dataGridView2.Columns["Field5Column"].Visible = false;
            dataGridView3.Columns["Field5Column"].Visible = false;
        }

        private void LoadDataIntoComboBox1()
        {
            string query = "SELECT process_name FROM process";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            try
            {
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetString("process_name"));
                }

                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        private void LoadDataIntoComboBox2()
        {
            string query = "SELECT Material_name FROM Material";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            try
            {
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    comboBox2.Items.Add(dataReader.GetString("Material_name"));
                }

                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
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

            // 각 텍스트 상자의 텍스트가 비어 있는지 확인
            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("빈 칸을 채워주세요.");
                return; // 빈 칸이 있으면 더 이상 진행하지 않음
            }

            // 새로운 행 생성
            DataGridViewRow newRow = new DataGridViewRow();

            // 행에 데이터 추가
            newRow.CreateCells(dataGridView1, false, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, ins_date);

            // 행을 데이터그리드뷰의 첫 번째 위치에 삽입
            dataGridView1.Rows.Insert(0, newRow);

            // MySQL에 데이터 삽입
            InsertDataIntoMySQL(productCode, textBox3.Text, textBox4.Text, textBox5.Text, ins_date);
        }


        private void InsertProductProcess(string processName, int processTime, string productCode)
        {
            // product_process 테이블에 사용자에게 입력받은 데이터 DB 저장
            string Query = "INSERT INTO product_process (product_code, process_name, process_time) VALUES (@product_code, @process_name, @process_time)";
            using (MySqlCommand Cmd = new MySqlCommand(Query, connection))
            {
                Cmd.Parameters.AddWithValue("@product_code", productCode);
                Cmd.Parameters.AddWithValue("@process_name", processName);
                Cmd.Parameters.AddWithValue("@process_time", processTime);

                try
                {
                    Cmd.ExecuteNonQuery();
                    MessageBox.Show("product_process 테이블에 데이터가 성공적으로 등록되었습니다");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting data into product_process table: " + ex.Message);
                }
            }
        }
        private void InsertProductMaterial(string MaterialName, int MaterialNumber, string productCode)
        {
            // product_Material 테이블에 사용자에게 입력받은 데이터 DB 저장
            string Query = "INSERT INTO product_Material (product_code, Material_name, Material_number) VALUES (@product_code, @Material_name, @Material_number)";
            using (MySqlCommand Cmd = new MySqlCommand(Query, connection))
            {
                Cmd.Parameters.AddWithValue("@product_code", productCode);
                Cmd.Parameters.AddWithValue("@Material_number", MaterialNumber);
                Cmd.Parameters.AddWithValue("@Material_name", MaterialName);

                try
                {
                    Cmd.ExecuteNonQuery();
                    MessageBox.Show("product_Material 테이블에 데이터가 성공적으로 등록되었습니다");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting data into product_Material table: " + ex.Message);
                }
            }
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
                    string productCode = row.Cells["Field5Column"].Value.ToString();
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
        private void SelectProductProcess(string productCode)
        {
            // 데이터그리드뷰 초기화
            dataGridView2.Rows.Clear();

            // MySQL에서 해당 품번 데이터 조회
            string query = "SELECT * FROM product_process WHERE product_code = @product_code";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@product_code", productCode);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string getId = reader.GetString("product_process_id");
                        string getProcessCode = reader.GetString("product_code");
                        string getProcessName = reader.GetString("process_name");
                        string getProcessTime = reader.GetString("process_time");
                        
                        // 조회된 데이터를 DataGridView에 추가
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(dataGridView2, false, getProcessCode, getProcessName, getProcessTime, getId);

                        // 체크박스를 제외한 나머지 셀들을 읽기 전용으로 설정
                        for (int i = 1; i < newRow.Cells.Count; i++)
                        {
                            newRow.Cells[i].ReadOnly = true;
                        }

                        dataGridView2.Rows.Add(newRow);
                    }
                }
            }
        }

        private void SelectProductMaterial(string productCode)
        {
            // 데이터그리드뷰 초기화
            dataGridView3.Rows.Clear();

            // MySQL에서 해당 품번 데이터 조회
            string query = "SELECT * FROM product_Material WHERE product_code = @product_code";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@product_code", productCode);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string getId = reader.GetString("product_Material_id");
                        string getProcessCode = reader.GetString("product_code");
                        string getMaterialName = reader.GetString("Material_name");
                        string getMaterialNumber = reader.GetString("Material_number");

                        // 조회된 데이터를 DataGridView에 추가
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(dataGridView3, false, getProcessCode, getMaterialName, getMaterialNumber, getId);

                        // 체크박스를 제외한 나머지 셀들을 읽기 전용으로 설정
                        for (int i = 1; i < newRow.Cells.Count; i++)
                        {
                            newRow.Cells[i].ReadOnly = true;
                        }

                        dataGridView3.Rows.Add(newRow);
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 유효한 행이 클릭되었는지 확인 (헤더나 빈 행이 아닌 경우)
            if (e.RowIndex >= 0)
            {
                // 클릭된 행에서 product_code의 값을 가져옵니다.
                string productCode = dataGridView1.Rows[e.RowIndex].Cells["Field2Column"].Value.ToString();

                // product_code의 값을 textbox6에 설정합니다.
                textbox6.Text = productCode;
                textBox7.Text = productCode;
                SelectProductProcess(productCode);
                SelectProductMaterial(productCode);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRegister1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textbox6.Text)) 
            {
                MessageBox.Show("먼저 제품정보를 조회후 선택하세요");
            }
            else if (string.IsNullOrEmpty(txtInput.Text))
            {
                MessageBox.Show("공정시간을 입력해주세요");
            }
            else if (!Char.IsDigit(txtInput.Text, 0))
            {
                MessageBox.Show("공정시간을 분단위로 숫자만 입력해주세요");
            }
            else
            {
                int getTime = int.Parse(txtInput.Text);
                InsertProductProcess(comboBox1.Text, getTime, textbox6.Text);
            }
            SelectProductProcess(textbox6.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox7.Text))
            {
                MessageBox.Show("먼저 제품정보를 조회후 선택하세요");
            }
            else if (string.IsNullOrEmpty(textBox8.Text))
            {
                MessageBox.Show("자재수량을 입력해주세요");
            }
            else if (!Char.IsDigit(textBox8.Text, 0))
            {
                MessageBox.Show("자재수량을 숫자로 입력해주세요");
            }
            else
            {
                int getNumber = int.Parse(textBox8.Text);
                InsertProductMaterial(comboBox2.Text, getNumber, textBox7.Text);
            }
            SelectProductMaterial(textBox7.Text);
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            // 체크된 항목을 반복하여 삭제
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                // 체크된 항목인지 확인
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;
                bool isChecked = Convert.ToBoolean(checkBoxCell.Value);

                if (isChecked)
                {
                    // 체크된 경우, 데이터 그리드와 MySQL에서 삭제
                    string getid = row.Cells["Field5Column"].Value.ToString();
                    int id = int.Parse(getid);

                    dataGridView2.Rows.Remove(row);

                    string query = "DELETE FROM product_process WHERE product_process_id = @product_process_id";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@product_process_id", id);

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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 체크된 항목을 반복하여 삭제
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                // 체크된 항목인지 확인
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;
                bool isChecked = Convert.ToBoolean(checkBoxCell.Value);

                if (isChecked)
                {
                    // 체크된 경우, 데이터 그리드와 MySQL에서 삭제
                    string getid = row.Cells["Field5Column"].Value.ToString();
                    int id = int.Parse(getid);

                    dataGridView3.Rows.Remove(row);

                    string query = "DELETE FROM product_Material WHERE product_Material_id = @product_Material_id";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@product_Material_id", id);

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
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}