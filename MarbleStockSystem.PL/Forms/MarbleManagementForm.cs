using MarbleStockSystem.BLL.Interfaces;
using MarbleStockSystem.DAL.Entities;

namespace MarbleStockSystem.PL.Forms
{
    /// <summary>
    /// Mermer yönetim formu - CRUD işlemleri
    /// </summary>
    public partial class MarbleManagementForm : Form
    {
        private readonly IMarbleService _marbleService;
        private DataGridView? dgvMarbles;
        private TextBox? txtName, txtType, txtColor, txtThickness, txtPricePerM2, txtStockQuantity;
        private Button? btnAdd, btnUpdate, btnDelete, btnClear;
        private int? selectedMarbleId = null;

        /// <summary>
        /// Constructor - Dependency Injection ile service alır
        /// </summary>
        public MarbleManagementForm(IMarbleService marbleService)
        {
            _marbleService = marbleService;
            InitializeComponent();
            LoadMarbles();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form ayarları
            this.Text = "Mermer Yönetimi";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // DataGridView
            dgvMarbles = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(940, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvMarbles.SelectionChanged += DgvMarbles_SelectionChanged;
            dgvMarbles.CellDoubleClick += DgvMarbles_CellDoubleClick;

            // Label ve TextBox'lar
            int yPos = 340;
            int labelWidth = 120;
            int textBoxWidth = 200;
            int spacing = 30;

            var lblName = new Label { Text = "Mermer Adı:", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            txtName = new TextBox { Location = new Point(150, yPos), Size = new Size(textBoxWidth, 23) };
            yPos += spacing;

            var lblType = new Label { Text = "Tip:", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            txtType = new TextBox { Location = new Point(150, yPos), Size = new Size(textBoxWidth, 23) };
            yPos += spacing;

            var lblColor = new Label { Text = "Renk:", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            txtColor = new TextBox { Location = new Point(150, yPos), Size = new Size(textBoxWidth, 23) };
            yPos += spacing;

            var lblThickness = new Label { Text = "Kalınlık (cm):", Location = new Point(20, yPos), Size = new Size(labelWidth, 23) };
            txtThickness = new TextBox { Location = new Point(150, yPos), Size = new Size(textBoxWidth, 23) };
            yPos += spacing;

            var lblPricePerM2 = new Label { Text = "Fiyat/m²:", Location = new Point(400, 340), Size = new Size(labelWidth, 23) };
            txtPricePerM2 = new TextBox { Location = new Point(530, 340), Size = new Size(textBoxWidth, 23) };

            var lblStockQuantity = new Label { Text = "Stok (m²):", Location = new Point(400, 370), Size = new Size(labelWidth, 23) };
            txtStockQuantity = new TextBox { Location = new Point(530, 370), Size = new Size(textBoxWidth, 23) };

            // Butonlar
            btnAdd = new Button
            {
                Text = "Ekle",
                Location = new Point(800, 340),
                Size = new Size(80, 30),
                BackColor = Color.LightGreen
            };
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button
            {
                Text = "Güncelle",
                Location = new Point(800, 380),
                Size = new Size(80, 30),
                BackColor = Color.LightBlue
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button
            {
                Text = "Sil",
                Location = new Point(800, 420),
                Size = new Size(80, 30),
                BackColor = Color.LightCoral
            };
            btnDelete.Click += BtnDelete_Click;

            btnClear = new Button
            {
                Text = "Temizle",
                Location = new Point(800, 460),
                Size = new Size(80, 30),
                BackColor = Color.LightGray
            };
            btnClear.Click += BtnClear_Click;

            // Kontrolleri forma ekle
            this.Controls.Add(dgvMarbles);
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblType);
            this.Controls.Add(txtType);
            this.Controls.Add(lblColor);
            this.Controls.Add(txtColor);
            this.Controls.Add(lblThickness);
            this.Controls.Add(txtThickness);
            this.Controls.Add(lblPricePerM2);
            this.Controls.Add(txtPricePerM2);
            this.Controls.Add(lblStockQuantity);
            this.Controls.Add(txtStockQuantity);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnClear);

            this.ResumeLayout(false);
        }

        private void LoadMarbles()
        {
            try
            {
                var marbles = _marbleService.GetAllMarbles();
                dgvMarbles!.DataSource = marbles.ToList();
                dgvMarbles.Columns["MarbleId"]!.Visible = false;
                dgvMarbles.Columns["Sales"]!.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMarbles_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvMarbles!.SelectedRows.Count > 0)
            {
                var row = dgvMarbles.SelectedRows[0];
                selectedMarbleId = (int)row.Cells["MarbleId"].Value;
                txtName!.Text = row.Cells["Name"].Value.ToString();
                txtType!.Text = row.Cells["Type"].Value.ToString();
                txtColor!.Text = row.Cells["Color"].Value.ToString();
                txtThickness!.Text = row.Cells["Thickness"].Value.ToString();
                txtPricePerM2!.Text = row.Cells["PricePerM2"].Value.ToString();
                txtStockQuantity!.Text = row.Cells["StockQuantity"].Value.ToString();
            }
        }

        private void DgvMarbles_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            DgvMarbles_SelectionChanged(sender, e);
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            try
            {
                var marble = new Marble
                {
                    Name = txtName!.Text,
                    Type = txtType!.Text,
                    Color = txtColor!.Text,
                    Thickness = decimal.Parse(txtThickness!.Text),
                    PricePerM2 = decimal.Parse(txtPricePerM2!.Text),
                    StockQuantity = decimal.Parse(txtStockQuantity!.Text)
                };

                _marbleService.AddMarble(marble);
                MessageBox.Show("Mermer başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadMarbles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (!selectedMarbleId.HasValue)
            {
                MessageBox.Show("Lütfen güncellenecek bir mermer seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var marble = new Marble
                {
                    MarbleId = selectedMarbleId.Value,
                    Name = txtName!.Text,
                    Type = txtType!.Text,
                    Color = txtColor!.Text,
                    Thickness = decimal.Parse(txtThickness!.Text),
                    PricePerM2 = decimal.Parse(txtPricePerM2!.Text),
                    StockQuantity = decimal.Parse(txtStockQuantity!.Text)
                };

                _marbleService.UpdateMarble(marble);
                MessageBox.Show("Mermer başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadMarbles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (!selectedMarbleId.HasValue)
            {
                MessageBox.Show("Lütfen silinecek bir mermer seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bu mermeri silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _marbleService.DeleteMarble(selectedMarbleId.Value);
                    MessageBox.Show("Mermer başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadMarbles();
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
            selectedMarbleId = null;
            txtName!.Clear();
            txtType!.Clear();
            txtColor!.Clear();
            txtThickness!.Clear();
            txtPricePerM2!.Clear();
            txtStockQuantity!.Clear();
            dgvMarbles!.ClearSelection();
        }
    }
}



