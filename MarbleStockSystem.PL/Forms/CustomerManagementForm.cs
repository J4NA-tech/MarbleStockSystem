using MarbleStockSystem.BLL.Interfaces;
using MarbleStockSystem.DAL.Entities;

namespace MarbleStockSystem.PL.Forms
{
    /// <summary>
    /// Müşteri yönetim formu - CRUD işlemleri
    /// </summary>
    public partial class CustomerManagementForm : Form
    {
        private readonly ICustomerService _customerService;
        private DataGridView? dgvCustomers;
        private TextBox? txtFullName, txtPhone, txtAddress;
        private Button? btnAdd, btnUpdate, btnDelete, btnClear;
        private int? selectedCustomerId = null;

        /// <summary>
        /// Constructor - Dependency Injection ile service alır
        /// </summary>
        public CustomerManagementForm(ICustomerService customerService)
        {
            _customerService = customerService;
            InitializeComponent();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form ayarları
            this.Text = "Müşteri Yönetimi";
            this.Size = new Size(1000, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // DataGridView
            dgvCustomers = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(940, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;
            dgvCustomers.CellDoubleClick += DgvCustomers_CellDoubleClick;

            // Label ve TextBox'lar
            int yPos = 290;
            int labelWidth = 120;
            int textBoxWidth = 300;
            int spacing = 30;

            var lblFullName = new Label { Text = "Ad Soyad:", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            txtFullName = new TextBox { Location = new Point(150, yPos), Size = new Size(textBoxWidth, 23) };
            yPos += spacing;

            var lblPhone = new Label { Text = "Telefon:", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            txtPhone = new TextBox { Location = new Point(150, yPos), Size = new Size(textBoxWidth, 23) };
            yPos += spacing;

            var lblAddress = new Label { Text = "Adres:", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            txtAddress = new TextBox { Location = new Point(150, yPos), Size = new Size(textBoxWidth, 60), Multiline = true };

            // Butonlar
            btnAdd = new Button
            {
                Text = "Ekle",
                Location = new Point(500, 290),
                Size = new Size(80, 30),
                BackColor = Color.LightGreen
            };
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button
            {
                Text = "Güncelle",
                Location = new Point(500, 330),
                Size = new Size(80, 30),
                BackColor = Color.LightBlue
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button
            {
                Text = "Sil",
                Location = new Point(500, 370),
                Size = new Size(80, 30),
                BackColor = Color.LightCoral
            };
            btnDelete.Click += BtnDelete_Click;

            btnClear = new Button
            {
                Text = "Temizle",
                Location = new Point(500, 410),
                Size = new Size(80, 30),
                BackColor = Color.LightGray
            };
            btnClear.Click += BtnClear_Click;

            // Kontrolleri forma ekle
            this.Controls.Add(dgvCustomers);
            this.Controls.Add(lblFullName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblPhone);
            this.Controls.Add(txtPhone);
            this.Controls.Add(lblAddress);
            this.Controls.Add(txtAddress);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnClear);

            this.ResumeLayout(false);
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = _customerService.GetAllCustomers();
                dgvCustomers!.DataSource = customers.ToList();
                dgvCustomers.Columns["CustomerId"]!.Visible = false;
                dgvCustomers.Columns["Sales"]!.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCustomers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvCustomers!.SelectedRows.Count > 0)
            {
                var row = dgvCustomers.SelectedRows[0];
                selectedCustomerId = (int)row.Cells["CustomerId"].Value;
                txtFullName!.Text = row.Cells["FullName"].Value.ToString();
                txtPhone!.Text = row.Cells["Phone"].Value.ToString();
                txtAddress!.Text = row.Cells["Address"]?.Value?.ToString() ?? string.Empty;
            }
        }

        private void DgvCustomers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            DgvCustomers_SelectionChanged(sender, e);
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            try
            {
                var customer = new Customer
                {
                    FullName = txtFullName!.Text,
                    Phone = txtPhone!.Text,
                    Address = txtAddress!.Text
                };

                _customerService.AddCustomer(customer);
                MessageBox.Show("Müşteri başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (!selectedCustomerId.HasValue)
            {
                MessageBox.Show("Lütfen güncellenecek bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var customer = new Customer
                {
                    CustomerId = selectedCustomerId.Value,
                    FullName = txtFullName!.Text,
                    Phone = txtPhone!.Text,
                    Address = txtAddress!.Text
                };

                _customerService.UpdateCustomer(customer);
                MessageBox.Show("Müşteri başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (!selectedCustomerId.HasValue)
            {
                MessageBox.Show("Lütfen silinecek bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bu müşteriyi silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _customerService.DeleteCustomer(selectedCustomerId.Value);
                    MessageBox.Show("Müşteri başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            selectedCustomerId = null;
            txtFullName!.Clear();
            txtPhone!.Clear();
            txtAddress!.Clear();
            dgvCustomers!.ClearSelection();
        }
    }
}



