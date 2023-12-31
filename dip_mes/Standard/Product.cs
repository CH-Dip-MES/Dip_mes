using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dip_mes
{
    public partial class Product : UserControl
    {
        private MySqlConnection connection;
        private string connectionString = "Server=222.108.180.36; Database=mes_2; User ID=EDU_STUDENT; Password=1234;";
        public static Product staticProduct;

        public Product()
        {
            staticProduct = this;
            InitializeComponent();
            InitializeDatabaseConnection();
            InitializeDataGridViewColumns();
            LoadDataIntocomboBox1();
            LoadDataIntoComboBox2();
            LoadDataIntoComboBox3(); // 추가된 부분
            dataGridView1.CellClick += dataGridView1_CellClick;
            textBox1.KeyDown += textBox1_KeyDown;
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Enter 키가 눌렸을 때, button1 클릭 이벤트를 호출합니다.
                button1.PerformClick();
            }
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
            // DataGridView에 사용자 정의 체크박스 컬럼 추가
            DataGridViewCheckBoxColumn checkBoxColumn1 = new DataGridViewCheckBoxColumn();
            checkBoxColumn1.Name = "checkBoxColumn1";
            checkBoxColumn1.HeaderText = "체크";
            dataGridView1.Columns.Insert(0, checkBoxColumn1); // 첫 번째 열에 추가

            // DateGridView2에 사용자 정의 체크박스 컬럼 추가
            DataGridViewCheckBoxColumn checkBoxColumn2 = new DataGridViewCheckBoxColumn();
            checkBoxColumn2.Name = "checkBoxColumn2";
            checkBoxColumn2.HeaderText = "체크";
            dataGridView2.Columns.Insert(0, checkBoxColumn2); // 첫 번째 열에 추가

            // DateGridView3에 사용자 정의 체크박스 컬럼 추가
            DataGridViewCheckBoxColumn checkBoxColumn3 = new DataGridViewCheckBoxColumn();
            checkBoxColumn3.Name = "checkBoxColumn3";
            checkBoxColumn3.HeaderText = "체크";
            dataGridView3.Columns.Insert(0, checkBoxColumn3); // 첫 번째 열에 추가

            // 체크박스 컬럼의 크기 설정
            dataGridView1.Columns["checkBoxColumn1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns["checkBoxColumn1"].Width = 50;

            dataGridView2.Columns["checkBoxColumn2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView2.Columns["checkBoxColumn2"].Width = 50;

            dataGridView3.Columns["checkBoxColumn3"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView3.Columns["checkBoxColumn3"].Width = 50;


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

            // ComboBox3 추가
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.Items.Add("선택하세요");  // 초기 선택 항목 추가
            comboBox3.SelectedIndex = 0;
            Controls.Add(comboBox3);  // 폼에 컨트롤 추가

            dataGridView2.Columns["Field5Column"].Visible = false;
            dataGridView3.Columns["Field5Column"].Visible = false;
        }

        private void LoadDataIntoComboBox3()
        {
            // ComboBox3에 아이템 추가
            comboBox3.Items.Add("완제품");
            comboBox3.Items.Add("반제품");
        }

        public void LoadDataIntocomboBox1()
        {
            string query = "SELECT process_name FROM process";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            try
            {
                MySqlDataReader dataReader = cmd.ExecuteReader();

                // 기존 아이템 클리어
                comboBox1.Items.Clear();

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


        public void LoadDataIntoComboBox2()
        {
            string query = "SELECT Material_name FROM Material";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            try
            {
                MySqlDataReader dataReader = cmd.ExecuteReader();

                // 기존 아이템 클리어
                comboBox2.Items.Clear();

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
            if (Login.getAuth != 1 && Login.getAuth != 3)
            {
                Console.WriteLine(Login.getAuth);
                Console.WriteLine(Login.getAuth != 1 || Login.getAuth != 3);
                Console.WriteLine(true || false);
                MessageBox.Show("권한이 없습니다.");
                return;
            }
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
                string.IsNullOrWhiteSpace(comboBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("빈 칸을 채워주세요.");
                return; // 빈 칸이 있으면 더 이상 진행하지 않음
            }

            // 등록할 데이터 메시지 생성
            string registerMessage = $"품번: {textBox2.Text}\n이름: {textBox3.Text}\n가격: {comboBox3.Text}\n수량: {textBox5.Text}";

            // 확인/취소 다이얼로그 표시
            DialogResult result = MessageBox.Show($"{registerMessage}\n\n데이터를 등록하시겠습니까?", "확인", MessageBoxButtons.OKCancel);

            // 사용자가 확인을 선택한 경우
            if (result == DialogResult.OK)
            {
                // 새로운 행 생성
                DataGridViewRow newRow = new DataGridViewRow();

                // 행에 데이터 추가
                newRow.CreateCells(dataGridView1, false, textBox2.Text, textBox3.Text, comboBox3.Text, textBox5.Text, ins_date);

                // 행을 데이터그리드뷰의 첫 번째 위치에 삽입
                dataGridView1.Rows.Insert(0, newRow);

                // MySQL에 데이터 삽입
                InsertDataIntoMySQL(productCode, textBox3.Text, comboBox3.Text, textBox5.Text, ins_date);

                // 텍스트 칸 비우기
                textBox2.Clear();
                textBox3.Clear();
                comboBox3.Text = string.Empty;
                textBox5.Clear();
            }
            // 사용자가 취소를 선택한 경우
            else if (result == DialogResult.Cancel)
            {
                MessageBox.Show("데이터 등록이 취소되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }





        private void InsertProductProcess(string processName, int processTime, string productCode)
        {
            if (Login.getAuth != 1 || Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
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
            if (Login.getAuth != 1 || Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
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
            if (Login.getAuth != 1 || Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
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

                // 검색 결과가 없는 경우 메시지 표시
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("입력한 품명과 일치하는 데이터가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // textbox1 초기화
            textBox1.Clear();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Login.getAuth != 1 || Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
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
                    string productCode = row.Cells["Field2Column"].Value.ToString(); // 수정된 부분
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
            dataGridView2.Rows.Clear();

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

                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(dataGridView2, false, getProcessCode, getProcessName, getProcessTime, getId);

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
            dataGridView3.Rows.Clear();

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
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(dataGridView3, false, getProcessCode, getMaterialName, getMaterialNumber, getId);
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
            if (e.RowIndex >= 0)
            {
                string productCode = dataGridView1.Rows[e.RowIndex].Cells["Field2Column"].Value.ToString();
                textbox6.Text = productCode;
                textBox7.Text = productCode;
                SelectProductProcess(productCode);
                SelectProductMaterial(productCode);
                LoadDataIntocomboBox1();
                LoadDataIntoComboBox2();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRegister1_Click(object sender, EventArgs e)
        {
            if (Login.getAuth != 1 || Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }

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
            comboBox1.Text = string.Empty;
            txtInput.Clear();


            SelectProductProcess(textbox6.Text);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (Login.getAuth != 1 || Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
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

            comboBox2.Text = string.Empty;
            textBox8.Clear();


            SelectProductMaterial(textBox7.Text);
        }


        private void btnDelete1_Click(object sender, EventArgs e)
        {
            if (Login.getAuth != 1 || Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            // 체크된 항목을 반복하여 삭제
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;
                bool isChecked = Convert.ToBoolean(checkBoxCell.Value);

                if (isChecked)
                {
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
            if (Login.getAuth != 1 || Login.getAuth != 3)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;
                bool isChecked = Convert.ToBoolean(checkBoxCell.Value);

                if (isChecked)
                {
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
       
            textbox6.Clear();
            textBox7.Clear();    
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            LoadDataIntocomboBox1();
            LoadDataIntoComboBox2();
        }

    }
}