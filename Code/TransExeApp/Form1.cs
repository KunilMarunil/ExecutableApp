using System.Data;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TransExeApp
{
    public partial class Form1 : Form
    {
        private DateTimePicker dtpTanggalAwal;
        private DateTimePicker dtpTanggalAkhir;
        private ComboBox cmbBarang;
        private DataGridView dataGridView;

        public Form1()
        {
            InitializeComponent();
            SetupForm();
            LoadBarangData();
        }

        private void SetupForm()
        {
            // Labels
            Label lblTanggalAwal = new Label { Text = "Tanggal Awal", Location = new Point(20, 20) };
            Label lblTanggalAkhir = new Label { Text = "Tanggal Akhir", Location = new Point(20, 60) };
            Label lblBarang = new Label { Text = "Barang", Location = new Point(20, 100) };

            // TextBoxes
            dtpTanggalAwal = new DateTimePicker { Location = new Point(120, 20), Format = DateTimePickerFormat.Short, MaxDate = DateTime.Today };
            dtpTanggalAwal.ValueChanged += DateOrProductChanged;
            dtpTanggalAkhir = new DateTimePicker { Location = new Point(120, 60), Format = DateTimePickerFormat.Short , MaxDate = DateTime.Today };
            dtpTanggalAkhir.ValueChanged += DateOrProductChanged;

            // ComboBox for Barang selection
            cmbBarang = new ComboBox { Location = new Point(120, 100), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbBarang.SelectedIndexChanged += DateOrProductChanged;

            // Search Button
            Button btnCari = new Button { Text = "Cari", Location = new Point(500, 100), Width = 100};
            btnCari.Click += SearchThat;

            // DataGridView
            dataGridView = new DataGridView
            {
                Location = new Point(20, 140),
                Width = 600,
                Height = 300,
                ColumnCount = 6,
                ReadOnly = true
            };

            // DataGridView Columns
            dataGridView.Columns[0].Name = "Tanggal";
            dataGridView.Columns[1].Name = "No. Trx";
            dataGridView.Columns[2].Name = "Keterangan";
            dataGridView.Columns[3].Name = "Masuk (Qty)";
            dataGridView.Columns[4].Name = "Keluar (Qty)";
            dataGridView.Columns[5].Name = "Saldo (Qty)";

            // Add controls to the form
            this.Controls.Add(lblTanggalAwal);
            this.Controls.Add(lblTanggalAkhir);
            this.Controls.Add(lblBarang);
            this.Controls.Add(dtpTanggalAwal);
            this.Controls.Add(dtpTanggalAkhir);
            this.Controls.Add(cmbBarang);
            this.Controls.Add(btnCari);
            this.Controls.Add(dataGridView);

            // Set form properties
            this.Text = "Inventory Report";
            this.ClientSize = new Size(650, 500);
        }
        private void SearchThat(object sender, EventArgs e)
        {
            string connectionString = "Server=(localdb)\\Locallydumb;Database=WebAppThree;User Id=;Password=;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "EXEC GetTransactionSearch @Barang = @Barang, @StartDate = @StartDate, @EndDate = @EndDate";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Barang", cmbBarang.SelectedItem);
                    command.Parameters.AddWithValue("@StartDate", dtpTanggalAwal.Value);
                    command.Parameters.AddWithValue("@EndDate", dtpTanggalAkhir.Value);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    dataGridView.DataSource = null;
                    dataGridView.Rows.Clear();
                    dataGridView.Columns.Clear();

                    adapter.Fill(dataTable);

                    dataGridView.DataSource = dataTable;
                    dataGridView.AutoGenerateColumns = true;
                    dataGridView.AllowUserToOrderColumns = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadBarangData()
        {
            string connectionString = "Server=(localdb)\\Locallydumb;Database=WebAppThree;User Id=;Password=;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Name FROM Product";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbBarang.Items.Add(reader["Name"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Barang data: " + ex.Message);
                }
            }
        }

        private void DateOrProductChanged(object sender, EventArgs e)
        {
            dataGridView.DataSource = null;
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
        }
    }
}
