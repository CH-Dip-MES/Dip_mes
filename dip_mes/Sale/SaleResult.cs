using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;

namespace dip_mes.sale
{
    public partial class SaleResult : UserControl
    {
        string jConn = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        public SaleResult()
        {
            InitializeComponent();
            LoadYearSalesData();
        }
        private void CheckButton1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection sConn = new MySqlConnection(jConn))
            {
                sConn.Open();
                string lookSaleNo = lookNo.Text.Trim(); // 검색창 텍스트
                string lSale = @"SELECT sale2.salecode,sale2.buyername,SUM(sale3.sellprice) AS tSell,SUM(sale3.vat) AS tVat
                                FROM sale2
                                JOIN sale3 ON sale2.salecode = sale3.salecode
                                WHERE sale2.procstat = '완료' AND sale2.delistat = '완료'";
                if (!string.IsNullOrEmpty(lookSaleNo)) // 검색창에 입력한 문자 있을 시 활성화
                {
                    lSale += $" AND sale2.salecode = '{lookSaleNo}'";
                }

                lSale += " GROUP BY sale2.salecode, sale2.buyername";

                MySqlCommand cmd = new MySqlCommand(lSale, sConn);
                try
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable tManage = new DataTable();
                        adapter.Fill(tManage);

                        if (tManage.Rows.Count > 0)
                        {
                            // DataGridView에 데이터 설정
                            dataGridView1.DataSource = tManage;

                            // DataGridView 컬럼 헤더 텍스트 설정
                            dataGridView1.Columns["salecode"].HeaderText = "판매번호";
                            dataGridView1.Columns["buyername"].HeaderText = "구매자명";
                            dataGridView1.Columns["tSell"].HeaderText = "판매금액";
                            dataGridView1.Columns["tVat"].HeaderText = "부가세";
                        }
                        else
                        {
                            MessageBox.Show("해당 판매번호의 데이터가 없습니다.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"오류 발생: {ex.Message}");
                }
            }
        }
        private void LoadYearSalesData()
        {
            using (MySqlConnection sConn = new MySqlConnection(jConn))
            {
                sConn.Open();
                // 연간 고객별 매출액을 집계하는 쿼리
                string query = @"SELECT sale2.buyername, SUM(sale3.sellprice) AS TotalSales
                        FROM sale2
                        JOIN sale3 ON sale2.salecode = sale3.salecode
                        WHERE sale2.procstat = '완료' AND sale2.delistat = '완료'
                        GROUP BY sale2.buyername";
                MySqlCommand cmd = new MySqlCommand(query, sConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                GenerateAnnualSalesChart(dataTable);
            }
        }
        private void GenerateAnnualSalesChart(DataTable dataTable)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.Title = "고객명";
            chart1.ChartAreas[0].AxisY.Title = "연간 매출액";

            var series = new Series("연간 매출")
            {
                ChartType = SeriesChartType.Column
            };
            chart1.Series.Add(series);

            foreach (DataRow row in dataTable.Rows)
            {
                string buyerName = row["buyername"].ToString();
                decimal totalSales = row["TotalSales"] != DBNull.Value ? Convert.ToDecimal(row["TotalSales"]) : 0;

                series.Points.AddXY(buyerName, totalSales);
            }
        }
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            // 차트의 히트 테스트를 수행하여 클릭된 요소를 찾습니다.
            HitTestResult result = chart1.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                var series = result.Series;
                int pointIndex = result.PointIndex;
                string selectedBuyerName = series.Points[pointIndex].AxisLabel;

                // 선택된 고객명으로 데이터 로드
                LoadCustomerProductSalesData(selectedBuyerName);
            }
        }
        private void LoadCustomerProductSalesData(string buyerName)
        {
            using (MySqlConnection sConn = new MySqlConnection(jConn))
            {
                sConn.Open();
                string query = @"SELECT MONTH(sale2.delidate) AS SaleMonth, sale3.ItemName, SUM(sale3.sellprice) AS ProductSales
                        FROM sale2
                        JOIN sale3 ON sale2.salecode = sale3.salecode
                        WHERE sale2.buyername = @buyerName AND sale2.procstat = '완료' AND sale2.delistat = '완료'
                        GROUP BY MONTH(sale2.delidate), sale3.ItemName";
                MySqlCommand cmd = new MySqlCommand(query, sConn);
                cmd.Parameters.AddWithValue("@buyerName", buyerName);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                GenerateCustomerProductSalesChart(dataTable);
            }
        }
        private void GenerateCustomerProductSalesChart(DataTable dataTable)
        {
            chart2.Series.Clear();
            chart2.ChartAreas[0].AxisX.Interval = 1;
            chart2.ChartAreas[0].AxisX.Title = "월";
            chart2.ChartAreas[0].AxisY.Title = "제품별 매출";

            // 제품별로 시리즈 추가
            var productNames = dataTable.AsEnumerable().Select(row => row.Field<string>("ItemName")).Distinct();
            foreach (var productName in productNames)
            {
                Series series = new Series(productName) { ChartType = SeriesChartType.Column };
                chart2.Series.Add(series);

                for (int month = 1; month <= 12; month++)
                {
                    var monthlySales = dataTable.AsEnumerable()
                        .Where(row => row.Field<string>("ItemName") == productName && row.Field<int>("SaleMonth") == month)
                        .Sum(row => row.IsNull("ProductSales") ? 0 : row.Field<decimal>("ProductSales"));

                    series.Points.AddXY(month, monthlySales);
                }
            }
        }
    }
}
