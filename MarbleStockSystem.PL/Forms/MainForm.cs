using Microsoft.Extensions.DependencyInjection;
using MarbleStockSystem.PL.Forms;

namespace MarbleStockSystem.PL
{
    /// <summary>
    /// Ana form - Uygulama menüsü ve navigasyon
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor - Dependency Injection ile service provider alır
        /// </summary>
        public MainForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form ayarları
            this.Text = "Mermer Stok ve Satış Takip Sistemi";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Başlık label'ı
            var lblTitle = new Label
            {
                Text = "Mermer Stok ve Satış Takip Sistemi",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(200, 30)
            };

            // Butonlar
            var btnMarbleManagement = new Button
            {
                Text = "Mermer Yönetimi",
                Size = new Size(200, 60),
                Location = new Point(300, 120),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnMarbleManagement.Click += BtnMarbleManagement_Click;

            var btnCustomerManagement = new Button
            {
                Text = "Müşteri Yönetimi",
                Size = new Size(200, 60),
                Location = new Point(300, 200),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            btnCustomerManagement.Click += BtnCustomerManagement_Click;

            var btnSaleManagement = new Button
            {
                Text = "Satış Yap",
                Size = new Size(200, 60),
                Location = new Point(300, 280),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };
            btnSaleManagement.Click += BtnSaleManagement_Click;

            var btnExit = new Button
            {
                Text = "Çıkış",
                Size = new Size(200, 60),
                Location = new Point(300, 360),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            btnExit.Click += (s, e) => Application.Exit();

            // Kontrolleri forma ekle
            this.Controls.Add(lblTitle);
            this.Controls.Add(btnMarbleManagement);
            this.Controls.Add(btnCustomerManagement);
            this.Controls.Add(btnSaleManagement);
            this.Controls.Add(btnExit);

            this.ResumeLayout(false);
        }

        private void BtnMarbleManagement_Click(object? sender, EventArgs e)
        {
            var form = _serviceProvider.GetRequiredService<MarbleManagementForm>();
            form.ShowDialog();
        }

        private void BtnCustomerManagement_Click(object? sender, EventArgs e)
        {
            var form = _serviceProvider.GetRequiredService<CustomerManagementForm>();
            form.ShowDialog();
        }

        private void BtnSaleManagement_Click(object? sender, EventArgs e)
        {
            var form = _serviceProvider.GetRequiredService<SaleForm>();
            form.ShowDialog();
        }
    }
}

