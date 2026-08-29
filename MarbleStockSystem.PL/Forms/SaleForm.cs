using MarbleStockSystem.BLL.Interfaces;
using MarbleStockSystem.DAL.Entities;

namespace MarbleStockSystem.PL.Forms
{
    /// <summary>
    /// Satış yapma formu - Satış işlemleri
    /// </summary>
    public partial class SaleForm : Form
    {
        private readonly ISaleService _saleService;
        private readonly IMarbleService _marbleService;
        private readonly ICustomerService _customerService;
        private DataGridView? dgvSales;
        private ComboBox? cmbMarble, cmbCustomer;
        private TextBox? txtQuantity, txtTotalPrice;
        private Button? btnCreateSale, btnRefresh;
        private Label? lblStockInfo;

        /// <summary>
        /// Constructor - Dependency Injection ile service'leri alır
        /// </summary>
        public SaleForm(ISaleService saleService, IMarbleService marbleService, ICustomerService customerService)
        {
            _saleService = saleService;
            _marbleService = marbleService;
            _customerService = customerService;
            InitializeComponent();
            LoadComboBoxes();
            LoadSales();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form ayarları
            this.Text = "Satış Yap";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // DataGridView
            dgvSales = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(940, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            // Satış bilgileri paneli
            int yPos = 340;
            int labelWidth = 120;
            int controlWidth = 300;
            int spacing = 30;

            var lblMarble = new Label { Text = "Mermer:", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            cmbMarble = new ComboBox { Location = new Point(150, yPos), Size = new Size(controlWidth, 23), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMarble.SelectedIndexChanged += CmbMarble_SelectedIndexChanged;
            yPos += spacing;

            var lblCustomer = new Label { Text = "Müşteri:", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            cmbCustomer = new ComboBox { Location = new Point(150, yPos), Size = new Size(controlWidth, 23), DropDownStyle = ComboBoxStyle.DropDownList };
            yPos += spacing;

            var lblQuantity = new Label { Text = "Miktar (m²):", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            txtQuantity = new TextBox { Location = new Point(150, yPos), Size = new Size(controlWidth, 23) };
            txtQuantity.TextChanged += TxtQuantity_TextChanged;
            yPos += spacing;

            var lblTotalPrice = new Label { Text = "Toplam Fiyat:", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            txtTotalPrice = new TextBox { Location = new Point(150, yPos), Size = new Size(controlWidth, 23), ReadOnly = true, BackColor = Color.LightYellow };
            yPos += spacing;

            lblStockInfo = new Label
            {
                Text = "Stok Bilgisi:",
                Location = new Point(20, yPos),
                Size = new Size(controlWidth + 130, 23),
                ForeColor = Color.DarkBlue,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            // Butonlar
            btnCreateSale = new Button
            {
                Text = "Satış Yap",
                Location = new Point(500, 340),
                Size = new Size(120, 40),
                BackColor = Color.LightGreen,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCreateSale.Click += BtnCreateSale_Click;

            btnRefresh = new Button
            {
                Text = "Yenile",
                Location = new Point(500, 390),
                Size = new Size(120, 30),
                BackColor = Color.LightBlue
            };
            btnRefresh.Click += BtnRefresh_Click;

            // Kontrolleri forma ekle
            this.Controls.Add(dgvSales);
            this.Controls.Add(lblMarble);
            this.Controls.Add(cmbMarble);
            this.Controls.Add(lblCustomer);
            this.Controls.Add(cmbCustomer);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(txtQuantity);
            this.Controls.Add(lblTotalPrice);
            this.Controls.Add(txtTotalPrice);
            this.Controls.Add(lblStockInfo);
            this.Controls.Add(btnCreateSale);
            this.Controls.Add(btnRefresh);

            this.ResumeLayout(false);
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Mermerleri yükle
                var marbles = _marbleService.GetAllMarbles();
                cmbMarble!.Items.Clear();
                foreach (var marble in marbles)
                {
                    cmbMarble.Items.Add(new { MarbleId = marble.MarbleId, Display = $"{marble.Name} - {marble.Type} ({marble.Color})" });
                }
                cmbMarble.DisplayMember = "Display";
                cmbMarble.ValueMember = "MarbleId";

                // Müşterileri yükle
                var customers = _customerService.GetAllCustomers();
                cmbCustomer!.Items.Clear();
                foreach (var customer in customers)
                {
                    cmbCustomer.Items.Add(new { CustomerId = customer.CustomerId, Display = $"{customer.FullName} - {customer.Phone}" });
                }
                cmbCustomer.DisplayMember = "Display";
                cmbCustomer.ValueMember = "CustomerId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSales()
        {
            try
            {
                var sales = _saleService.GetAllSales();
                dgvSales!.DataSource = sales.Select(s => new
                {
                    s.SaleId,
                    Mermer = s.Marble?.Name ?? "Bilinmiyor",
                    Müşteri = s.Customer?.FullName ?? "Bilinmiyor",
                    Miktar = $"{s.Quantity} m²",
                    ToplamFiyat = $"{s.TotalPrice:C}",
                    Tarih = s.SaleDate.ToString("dd.MM.yyyy HH:mm")
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbMarble_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateStockInfo();
            CalculateTotalPrice();
        }

        private void TxtQuantity_TextChanged(object? sender, EventArgs e)
        {
            CalculateTotalPrice();
            UpdateStockInfo();
        }

        private void UpdateStockInfo()
        {
            if (cmbMarble!.SelectedItem == null)
            {
                lblStockInfo!.Text = "Stok Bilgisi: Lütfen mermer seçin";
                return;
            }

            try
            {
                dynamic selectedItem = cmbMarble.SelectedItem;
                int marbleId = selectedItem.MarbleId;
                var marble = _marbleService.GetMarbleById(marbleId);

                if (marble != null)
                {
                    lblStockInfo!.Text = $"Stok Bilgisi: Mevcut Stok = {marble.StockQuantity} m² | Fiyat = {marble.PricePerM2:C}/m²";
                    
                    // Miktar girildiyse stok kontrolü yap
                    if (decimal.TryParse(txtQuantity!.Text, out decimal quantity))
                    {
                        if (quantity > marble.StockQuantity)
                        {
                            lblStockInfo.ForeColor = Color.Red;
                            lblStockInfo.Text += $" | ⚠ Yetersiz Stok!";
                        }
                        else
                        {
                            lblStockInfo.ForeColor = Color.DarkGreen;
                        }
                    }
                }
            }
            catch
            {
                lblStockInfo!.Text = "Stok Bilgisi: Hata oluştu";
            }
        }

        private void CalculateTotalPrice()
        {
            if (cmbMarble!.SelectedItem == null || string.IsNullOrWhiteSpace(txtQuantity!.Text))
            {
                txtTotalPrice!.Text = "0,00";
                return;
            }

            try
            {
                dynamic selectedItem = cmbMarble.SelectedItem;
                int marbleId = selectedItem.MarbleId;
                var marble = _marbleService.GetMarbleById(marbleId);

                if (marble != null && decimal.TryParse(txtQuantity.Text, out decimal quantity))
                {
                    decimal totalPrice = marble.PricePerM2 * quantity;
                    txtTotalPrice!.Text = totalPrice.ToString("F2");
                }
                else
                {
                    txtTotalPrice!.Text = "0,00";
                }
            }
            catch
            {
                txtTotalPrice!.Text = "0,00";
            }
        }

        private void BtnCreateSale_Click(object? sender, EventArgs e)
        {
            if (cmbMarble!.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir mermer seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCustomer!.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity!.Text) || !decimal.TryParse(txtQuantity.Text, out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Lütfen geçerli bir miktar girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dynamic selectedMarble = cmbMarble.SelectedItem;
                dynamic selectedCustomer = cmbCustomer.SelectedItem;
                int marbleId = selectedMarble.MarbleId;
                int customerId = selectedCustomer.CustomerId;

                var sale = _saleService.CreateSale(marbleId, customerId, quantity);
                MessageBox.Show($"Satış başarıyla yapıldı!\nToplam Fiyat: {sale.TotalPrice:C}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Formu temizle
                cmbMarble.SelectedIndex = -1;
                cmbCustomer.SelectedIndex = -1;
                txtQuantity.Clear();
                txtTotalPrice!.Text = "0,00";
                lblStockInfo!.Text = "Stok Bilgisi:";
                
                // Listeyi yenile
                LoadComboBoxes();
                LoadSales();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadComboBoxes();
            LoadSales();
        }
    }
}



