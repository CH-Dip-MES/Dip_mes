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

        // ----------------------- 등록창에 데이터를 불러오는 기능 ----------------------------
        // inputMaterial 클래스에서 데이터그리드 셀 클릭시 실행하는 메서드
        public void DisplayDataInTextBox1(string data)
        {       // 작업지시번호 로드
            textBox1.Text = data;
        }
        public void DisplayDataInTextBox2(string data)
        {       // 제품명 로드
            textBox2.Text = data;
        }

        // 투입화면의 데이터그리드 셀 클릭 시 자재명을 불러오는 메서드
        public void LoadMaterialData(string productName)
        {
            // 콤보박스 초기화
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 입력된 제품명을 기반으로 데이터 조회
                    string query = @"SELECT product.product_name, product.product_code, product_material.Material_name, product_material.Material_number
                                   FROM product JOIN product_material ON product_material.product_code = product.product_code 
                                   WHERE product.product_name = @productName";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@productName", productName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 콤보박스에 데이터 추가
                                string material = reader["Material_name"].ToString();
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

        // 자재 콤보박스 선택 이벤트
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 데이터 그리드에서 선택한 작업지시의 제품
            string orderNo = textBox1.Text.Trim();
            string productName = textBox2.Text.Trim();
            string materialName = comboBox1.Text;

            string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
            if(materialName != "")
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // 제품별 필요 자재 정보를 조회하는 SQL 쿼리
                        string materialQuery = @"SELECT product.product_name, product.product_code, product_material.Material_name, product_material.Material_number
                                   FROM product JOIN product_material ON product_material.product_code = product.product_code 
                                   WHERE product.product_name = @productName";

                        string orderQuery = "SELECT PlannedQty FROM manufacture WHERE OrderNo = @orderNo";

                        using (MySqlCommand cmd = new MySqlCommand(materialQuery, connection))
                        {
                            // 매개변수 설정
                            cmd.Parameters.AddWithValue("@productName", productName);

                            DataTable dataTable = new DataTable();

                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            {
                                adapter.Fill(dataTable);
                            }

                            // 조회된 데이터가 없을 경우 메시지 표시 후 리턴
                            if (dataTable.Rows.Count == 0)
                            {
                                MessageBox.Show("기준수량을 조회할 수 없습니다.");
                                return;
                            }

                            // 해당 제품에 필요한 자재수량 저장
                            int getQty = Convert.ToInt32(dataTable.Rows[comboBox1.SelectedIndex]["Material_number"]);
                            textBox3.Text = getQty.ToString();

                        }

                        using (MySqlCommand cmd = new MySqlCommand(orderQuery, connection))
                        {
                            // 매개변수 설정
                            cmd.Parameters.AddWithValue("@orderNo", orderNo);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // 작업지시에 등록된 제품 생산계획수량
                                    int plannedQty = Convert.ToInt32(reader["PlannedQty"]);
                                    // textBox3 = 필요자재수량 X 생산계획수량
                                    textBox3.Text = (Convert.ToInt32(textBox3.Text) * plannedQty).ToString();
                                }
                                else
                                {
                                    MessageBox.Show("Data not found.");
                                }
                                reader.Close();
                            }
                        }

                        textBox5.Text = GetCurrentQty(materialName).ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------

        // 등록관련 이벤트
        // 자재투입등록 버튼 클릭 이벤트
        private void button1_Click(object sender, EventArgs e)
        {

            // 텍스트박스1 데이터(등록된 작업지시번호) 저장
            string textBox1Data = textBox1.Text;

            // 텍스트박스3 데이터(불러온 기준자재수량) 저장
            string textBox3Data = textBox3.Text;

            // 텍스트박스4 데이터(투입수량) 저장
            string textBox4Data = textBox4.Text;

            // 콤보박스 데이터(선택한자재) 저장
            string comboBoxSelectedValue = comboBox1.Text;

            // 데이터베이스에 저장하는 코드
            SaveDataToDatabase(textBox1Data, comboBoxSelectedValue, textBox3Data, textBox4Data);

            // RefreshData 이벤트 발생
            RefreshData?.Invoke(this, EventArgs.Empty);

        }

        // DB의 자재투입 정보를 UPDATE하고 재고수량을 차감하는 메서드
        private void SaveDataToDatabase(string orderNo, string inputMaterial, string estQty, string inputQty)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // DB에서 현재고 조회 후 currenQty 변수에 저장
                    int currentQty = GetCurrentQty(inputMaterial);

                    // 현재고에서 투입수량을 뺀 값을 updatedQty 변수에 저장
                    int updatedQty = currentQty - Convert.ToInt32(inputQty);

                    // 입력받은 투입수량 값이 현재고보다 큰 경우 저장하지 않음
                    if (int.TryParse(inputQty, out int inputQuantity) && inputQuantity > currentQty)
                    {
                        MessageBox.Show("입력 수량이 재고 수량보다 큽니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // 업데이트하는 SQL 쿼리 실행
                        string updateQuery = "UPDATE manufacture SET inputMaterial = @inputMaterial, estQty = @estQty, inputQty = @inputQty WHERE OrderNo = @orderNo";
                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
                        {
                            updateCmd.Parameters.AddWithValue("@inputMaterial", inputMaterial);
                            updateCmd.Parameters.AddWithValue("@inputQty", inputQty);
                            updateCmd.Parameters.AddWithValue("@estQty", estQty);
                            updateCmd.Parameters.AddWithValue("@orderNo", orderNo);

                            // 쿼리 실행
                            updateCmd.ExecuteNonQuery();
                        }
                        // 투입된 수량만큼 DB의 현재고를 차감하는 쿼리
                        string subtractQty = "UPDATE material SET curQty = @updatedQty WHERE Material_name = @inputMaterial";
                        using (MySqlCommand subtractCmd = new MySqlCommand(subtractQty, connection))
                        {
                            subtractCmd.Parameters.AddWithValue("@inputMaterial", inputMaterial);
                            subtractCmd.Parameters.AddWithValue("@updatedQty", updatedQty);

                            // 쿼리 실행
                            subtractCmd.ExecuteNonQuery();
                        }

                        comboBox1.SelectedIndex = -1;
                        comboBox1.Items.Clear();
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        MessageBox.Show("자재투입 등록 완료");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private int GetCurrentQty(string inputMaterial)
        {
            int currentQty = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 해당 No에 대한 Inventory 가져오는 SQL 쿼리
                    string Query = "SELECT curQty FROM material WHERE Material_name = @inputMaterial";

                    using (MySqlCommand selectCmd = new MySqlCommand(Query, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@inputMaterial", inputMaterial);

                        using (MySqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Inventory 값 가져오기
                                currentQty = reader["curQty"] == DBNull.Value ? 0 : Convert.ToInt32(reader["curQty"]);
                            }
                            else
                            {
                                MessageBox.Show("해당 자재명의 현재고를 조회할 수 없습니다.");
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.Message);
            }

            return currentQty;
        }
    }
}
