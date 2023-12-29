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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace dip_mes
{
    public partial class AddInput : Form
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";

        // 이벤트 핸들러를 저장할 델리게이트 선언
        public delegate void EventHandler(object sender, EventArgs e);

        // 부모 폼의 이벤트에 대한 델리게이트 인스턴스
        public event EventHandler RefreshData;

        public AddInput()
        {
            InitializeComponent();
        }

        private void AddInput_Load(object sender, EventArgs e)
        {

        }

        public void DisplayDataInTextBox1(string data)
        {
            textBox1.Text = data;
        }
        public void DisplayDataInTextBox2(string data)
        {
            textBox2.Text = data;
        }
        public void DisplayDataInTextBox3(string data)
        {
            textBox3.Text = data;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void LoadMaterialData(string data)
        {
            // 콤보박스 초기화
            comboBox1.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 입력된 제품명을 기반으로 데이터 조회
                    string query = "SELECT DISTINCT Material FROM manufacture WHERE product_name = @productName";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@productName", data);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 콤보박스에 데이터 추가
                                string material = reader["Material"].ToString();
                                comboBox1.Items.Add(material);
                            }
                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in LoadMaterialData: " + ex.Message);
                }
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            // 텍스트박스1, 텍스트박스2, 텍스트박스3에서 값을 가져옵니다.
            string textBox1Value = textBox1.Text.Trim();
            string textBox2Value = textBox2.Text.Trim();

            // MySQL 연결 문자열
            string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 텍스트박스1, 텍스트박스2에 해당하는 행을 조회하는 SQL 쿼리
                    string query = "SELECT PlannedQty, standard FROM manufacture WHERE No = @textBox1Value AND product_name = @textBox2Value";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 매개변수 설정
                        cmd.Parameters.AddWithValue("@textBox1Value", textBox1Value);
                        cmd.Parameters.AddWithValue("@textBox2Value", textBox2Value);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Planned 및 standard 컬럼 값 가져오기
                                int plannedValue = Convert.ToInt32(reader["Planned"]);
                                double standardValue = Convert.ToDouble(reader["standard"]);

                                // 텍스트박스3에 표시할 값 계산
                                double result = plannedValue * standardValue;

                                // 텍스트박스3에 결과 표시
                                textBox3.Text = result.ToString();
                            }
                            else
                            {
                                MessageBox.Show("Data not found.");
                            }
                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        public event EventHandler DataSaved;

        private void OnDataSaved()
        {
            DataSaved?.Invoke(this, EventArgs.Empty);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // 텍스트박스4의 데이터 가져오기
            string textBox4Data = textBox4.Text;

            // 텍스트박스1의 데이터 가져오기 (이 부분은 필요에 따라 수정해야 할 수 있습니다)
            string textBox1Data = textBox1.Text;

            // 콤보박스에서 선택된 값 가져오기
            string comboBoxSelectedValue = comboBox1.Text;

            // 데이터베이스에 저장하는 코드
            SaveDataToDatabase(textBox1Data, textBox4Data, comboBoxSelectedValue);

            // 이벤트 발생
            OnDataSaved();

            // RefreshData 이벤트 발생
            RefreshData?.Invoke(this, EventArgs.Empty);


            //176 현재 Inventory 값 가져오기
            int currentInventory = GetCurrentInventory(textBox1Data);

            // 텍스트박스4의 값이 Inventory보다 크면 경고창 표시하고 데이터 저장 방지
            if (int.TryParse(textBox4Data, out int inputQuantity) && inputQuantity > currentInventory)
            {
                MessageBox.Show("입력 수량이 재고보다 큽니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
            }

            clear();
        }
        private void clear()
        {
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

        }

        private int GetCurrentInventory(string textBox1Data)
        {
            int currentInventory = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 해당 No에 대한 Inventory 가져오는 SQL 쿼리
                    string selectInventoryQuery = "SELECT Inventory FROM manufacture WHERE No = @TextBox1Data";

                    using (MySqlCommand selectCmd = new MySqlCommand(selectInventoryQuery, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@TextBox1Data", textBox1Data);

                        using (MySqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Inventory 값 가져오기
                                currentInventory = reader["Inventory"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Inventory"]);
                            }
                            else
                            {
                                MessageBox.Show("지정된 No에 대한 데이터를 찾을 수 없습니다.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.Message);
            }

            return currentInventory;
        }
        private void SaveDataToDatabase(string textBox1Data, string textBox4Data, string comboBoxSelectedValue)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 해당 No에 대한 Inventory 가져오는 SQL 쿼리
                    string selectInventoryQuery = "SELECT Inventory FROM manufacture WHERE No = @TextBox1Data";

                    using (MySqlCommand selectCmd = new MySqlCommand(selectInventoryQuery, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@TextBox1Data", textBox1Data);

                        using (MySqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Inventory 값 가져오기
                                int currentInventory = reader["Inventory"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Inventory"]);

                                // 텍스트박스4의 값 만큼 Inventory에서 차감
                                int updatedInventory = currentInventory - Convert.ToInt32(textBox4Data);

                                // 반드시 DataReader를 사용한 후에는 닫아주어야 합니다.
                                reader.Close();
                                // 텍스트박스4의 값이 Inventory보다 큰 경우 저장하지 않음
                                if (int.TryParse(textBox4Data, out int inputQuantity) && inputQuantity > currentInventory)
                                {
                                    MessageBox.Show("입력 수량이 재고보다 큽니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    // 업데이트하는 SQL 쿼리 실행
                                    string updateQuery = "UPDATE manufacture SET Input = @TextBox4Data, Inventory = @UpdatedInventory, Selected = @ComboBoxValue WHERE No = @TextBox1Data";
                                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
                                    {
                                        updateCmd.Parameters.AddWithValue("@TextBox4Data", textBox4Data);
                                        updateCmd.Parameters.AddWithValue("@UpdatedInventory", updatedInventory);
                                        updateCmd.Parameters.AddWithValue("@TextBox1Data", textBox1Data);
                                        updateCmd.Parameters.AddWithValue("@ComboBoxValue", comboBoxSelectedValue);

                                        // 쿼리 실행
                                        updateCmd.ExecuteNonQuery();
                                    }

                                }
                            }
                            else
                            {
                                MessageBox.Show("No data found for the given No.");
                            }
                        }
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
