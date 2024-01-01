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
using System.Windows.Forms.DataVisualization.Charting;

namespace dip_mes
{
    public partial class OrderList : UserControl
    {
        private const string connectionString = "Server = 222.108.180.36; Database=mes_2; Uid=EDU_STUDENT;Pwd=1234;";

        public OrderList()
        {
            InitializeComponent();

        }

        private void OrderList_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView3();
            InitializeNumericUpDown();
            LoadDataToDataGridView1((int)numericUpDown1.Value);
            LoadBusinessData();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        private void LoadDataToDataGridView3()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // buy3 테이블에서 데이터 조회
                string selectQuery = @"
                SELECT 
                buy1.Deliverydate AS '납기일자',
                buy1.code AS '업체코드',
                buy1.Companyname AS '업체명',
                buy2.itemname AS '품명',
                buy2.itemnumber AS '품번',
                buy2.weight AS '입고수량',
                buy1.orderingcode AS '발주코드'
                FROM buy1
                INNER JOIN buy2 ON buy1.orderingcode = buy2.orderingcode";

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // DataGridView3에 데이터 바인딩
                    dataGridView3.DataSource = dataTable;
                }

                connection.Close();
            }

            // DataGridView3에서 첫 번째 행을 선택하도록 설정
            if (dataGridView3.Rows.Count > 0)
            {
                dataGridView3.Rows[0].Selected = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // DataGridView 클리어
            dataGridView3.DataSource = null;
            dataGridView3.Rows.Clear();

            // 년월 형식을 MySQL의 DATE_FORMAT 함수에 맞게 변환
            string deliveryYearMonth = dateTimePicker1.Value.ToString("yyyyMM");

            // MySQL 연결 및 명령어 생성
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // buy1, buy2 테이블에서 데이터 조회
                string selectQuery = @"
            SELECT 
                buy1.Deliverydate AS '납기일자',
                buy1.code AS '업체코드',
                buy1.Companyname AS '업체명',
                buy2.itemname AS '품명',
                buy2.itemnumber AS '품번',
                buy2.weight AS '입고수량',
                buy1.orderingcode AS '발주코드'
            FROM buy1
            INNER JOIN buy2 ON buy1.orderingcode = buy2.orderingcode";

                MySqlCommand command;

                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    // TextBox1에 값이 없으면 해당 년월의 행들 조회
                    selectQuery += " WHERE DATE_FORMAT(buy1.DeliveryDate, '%Y%m') = @DeliveryYearMonth";
                    command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@DeliveryYearMonth", deliveryYearMonth);
                }
                else
                {
                    // TextBox1에 값이 있으면 해당 년월 및 companycode가 일치한 행들 조회
                    selectQuery += " WHERE DATE_FORMAT(buy1.DeliveryDate, '%Y%m') = @DeliveryYearMonth AND buy1.code = @code";
                    command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@DeliveryYearMonth", deliveryYearMonth);
                    command.Parameters.AddWithValue("@code", textBox1.Text);
                }

                // 데이터 가져오기
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // DataGridView에 데이터 바인딩
                    dataGridView3.DataSource = dataTable;
                }

                connection.Close();
            }
        }

        private void InitializeNumericUpDown()
        {
            // 년도 범위 설정 (예: 2000년부터 현재 년도까지)
            numericUpDown1.Minimum = 2000;
            numericUpDown1.Maximum = DateTime.Now.Year;

            // 현재 년도로 초기화
            numericUpDown1.Value = DateTime.Now.Year;
        }

        private void LoadDataToDataGridView1(int selectedYear)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 특정연도의 월별 입고내역과 출고내역 합계를 가져오기
                string selectQuery = @"SELECT MONTH(buy1.Orderdate) AS OrderMonth, 
                                       SUM(buy2.weight) AS TotalWeight 
                              FROM buy2 
                              JOIN buy1 ON buy2.Orderingcode = buy1.Orderingcode 
                              WHERE YEAR(buy1.Orderdate) = @Year
                              GROUP BY OrderMonth";

                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Year", selectedYear);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // DataGridView1 초기화
                        dataGridView1.Columns.Clear();
                        dataGridView1.Rows.Clear();

                        // 1행 추가 (월 표시)
                        dataGridView1.Columns.Add("년도", "년도");

                        for (int month = 1; month <= 12; month++)
                        {
                            dataGridView1.Columns.Add("입고", "입고");
                            dataGridView1.Columns.Add("출고", "출고");
                        }

                        // 2행 추가 (DB 데이터)
                        if (dataTable.Rows.Count > 0)
                        {
                            object[] rowData = new object[25];
                            rowData[0] = selectedYear; // "년도" 열에 년도 데이터 설정

                            foreach (DataRow row in dataTable.Rows)
                            {
                                int orderMonth = Convert.ToInt32(row["OrderMonth"]);
                                rowData[2 * orderMonth - 1] = row["TotalWeight"];
                            }

                            // manufacture 테이블에서 월별 inputQty 합계를 가져오기
                            string manufactureQuery = @"SELECT MONTH(StartTime) AS Month, 
                                                SUM(inputQty) AS TotalInputQty 
                                         FROM manufacture 
                                         WHERE YEAR(StartTime) = @Year
                                         GROUP BY Month";

                            using (MySqlCommand manufactureCommand = new MySqlCommand(manufactureQuery, connection))
                            {
                                manufactureCommand.Parameters.AddWithValue("@Year", selectedYear);

                                using (MySqlDataAdapter manufactureAdapter = new MySqlDataAdapter(manufactureCommand))
                                {
                                    DataTable manufactureDataTable = new DataTable();
                                    manufactureAdapter.Fill(manufactureDataTable);

                                    // 2행 추가 (manufacture 테이블 데이터)
                                    if (manufactureDataTable.Rows.Count > 0)
                                    {

                                        foreach (DataRow row in manufactureDataTable.Rows)
                                        {
                                            int month = Convert.ToInt32(row["Month"]);
                                            rowData[2 * month] = row["TotalInputQty"];
                                        }
                                    }
                                }
                            }
                            dataGridView1.Rows.Add(rowData);
                        }
                    }
                }
                connection.Close();
            }
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // NumericUpDown의 값이 변경될 때마다 데이터 다시 불러오기
            LoadDataToDataGridView1((int)numericUpDown1.Value);
            LoadDataToChart((int)numericUpDown1.Value);
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            string[] strHeaders = { "1월", "2월", "3월", "4월", "5월", "6월", "7월", "8월", "9월", "10월", "11월", "12월" };
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            for (int i = 0; i < strHeaders.Length; i++)
            {
                if (gv.Rows.Count > 0)
                {
                    int nextColumnIndex = i * 2 + 1; // Calculate the next column index based on the month index
                    Rectangle r = gv.GetCellDisplayRectangle(nextColumnIndex, -1, false);
                    int width = gv.GetCellDisplayRectangle(nextColumnIndex + 1, -1, false).Width;
                    r.X += 1;
                    r.Y += 1;
                    r.Width = r.Width + width - 2;
                    r.Height = (r.Height / 2) - 2;

                    e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r);
                    e.Graphics.FillRectangle(new SolidBrush(gv.ColumnHeadersDefaultCellStyle.BackColor), r);
                    e.Graphics.DrawString(strHeaders[i], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r, format);
                }
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }


        private void LoadDataToChart(int selectedYear)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // "입고내역" 시리즈 생성 및 설정
                Series enterSeries = chart2.Series.FirstOrDefault(s => s.Name == "입고내역");
                if (enterSeries == null)
                {
                    enterSeries = new Series("입고내역");
                    enterSeries.ChartType = SeriesChartType.Column;
                    enterSeries.Color = Color.FromArgb(9, 4, 58); // Set custom color
                    chart2.Series.Add(enterSeries);
                }
                else
                {
                    // 기존 "입고내역" 시리즈를 초기화
                    enterSeries.Points.Clear();
                }

                // buy2 테이블에서 데이터 조회 (입고내역)
                string selectBuyQuery = @"
            SELECT MONTH(buy1.Orderdate) AS OrderMonth, 
                   SUM(buy2.weight) AS TotalWeight 
            FROM buy2 
            JOIN buy1 ON buy2.Orderingcode = buy1.Orderingcode 
            WHERE YEAR(buy1.Orderdate) = @Year
            GROUP BY OrderMonth";

                using (MySqlCommand buyCommand = new MySqlCommand(selectBuyQuery, connection))
                {
                    buyCommand.Parameters.AddWithValue("@Year", selectedYear);

                    using (MySqlDataReader buyReader = buyCommand.ExecuteReader())
                    {
                        // 데이터 바인딩 (입고내역)
                        while (buyReader.Read())
                        {
                            int orderMonth = Convert.ToInt32(buyReader["OrderMonth"]);
                            int totalWeight = Convert.ToInt32(buyReader["TotalWeight"]);

                            // 데이터 포인트 추가 (입고내역)
                            enterSeries.Points.AddXY(orderMonth, totalWeight);
                        }
                    }
                }

                // "출고내역" 시리즈 생성 및 설정
                Series deliverySeries = chart2.Series.FirstOrDefault(s => s.Name == "출고내역");
                if (deliverySeries == null)
                {
                    deliverySeries = new Series("출고내역");
                    deliverySeries.ChartType = SeriesChartType.Column;
                    deliverySeries.Color = Color.FromArgb(108, 189, 182); // Set custom color
                    chart2.Series.Add(deliverySeries);
                }
                else
                {
                    // 기존 "출고내역" 시리즈를 초기화
                    deliverySeries.Points.Clear();
                }

                // manufacture 테이블에서 월별 inputQty 합계를 가져오기
                string selectManufactureQuery = @"SELECT MONTH(StartTime) AS Month, 
                                            SUM(inputQty) AS TotalInputQty 
                                        FROM manufacture 
                                        WHERE YEAR(StartTime) = @Year
                                        GROUP BY Month";

                using (MySqlCommand manufactureCommand = new MySqlCommand(selectManufactureQuery, connection))
                {
                    manufactureCommand.Parameters.AddWithValue("@Year", selectedYear);

                    using (MySqlDataReader manufactureReader = manufactureCommand.ExecuteReader())
                    {
                        // 데이터 바인딩 (출고내역)
                        while (manufactureReader.Read())
                        {
                            int month = Convert.ToInt32(manufactureReader["Month"]);
                            int totalInputQty = Convert.ToInt32(manufactureReader["TotalInputQty"]);

                            // 데이터 포인트 추가 (출고내역)
                            deliverySeries.Points.AddXY(month, totalInputQty);
                        }
                    }
                }

                connection.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // comboBox1에서 선택된 companycode 가져오기
            string selectedCompanyCode = comboBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedCompanyCode))
            {
                // MySQL 연결 및 명령어 생성
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 선택된 companycode에 해당하는 companyname 가져오기
                    string selectQuery = "SELECT companyname FROM business WHERE companycode = @CompanyCode";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CompanyCode", selectedCompanyCode);

                        // 결과 가져오기
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            // companyname을 textBox2에 표시
                            textBox1.Text = result.ToString();
                        }
                        else
                        {
                            // 선택된 companycode에 해당하는 데이터가 없을 경우 textBox2를 초기화
                            textBox1.Text = string.Empty;
                        }
                    }

                    connection.Close();
                }
            }
        }

        private void LoadBusinessData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // division이 '협력사'인 행들의 companycode를 선택
                string selectQuery = "SELECT companycode FROM business WHERE division = '협력사'";

                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // comboBox1를 초기화
                        comboBox1.Items.Clear();

                        // 데이터를 comboBox1에 추가
                        while (reader.Read())
                        {
                            // 각 행의 "companycode"를 가져와서 comboBox1에 추가
                            string companyCode = reader["companycode"].ToString();
                            comboBox1.Items.Add(companyCode);
                        }
                    }
                }

                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadDataToDataGridView3();
        }
    }
}