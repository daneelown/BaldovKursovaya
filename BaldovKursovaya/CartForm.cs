using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BaldovKursovaya
{
	public partial class CartForm : Form
	{
		private System.ComponentModel.IContainer components = null;

		private DataGridView dgvCart;
		private Button btnPay;
		private Button btnRemove;
		private Label lblTotal;
		private BindingList<CartItem> bindingCart;
		private List<CartItem> sourceCart;

		private const string DefaultVideoFileName = "jur.mp4";

		public CartForm()
		{
			bindingCart = new BindingList<CartItem>();
			sourceCart = new List<CartItem>();

			InitializeComponent();

			this.FormBorderStyle = FormBorderStyle.Sizable;
			this.MaximizeBox = true;
			this.MinimizeBox = true;
			this.StartPosition = FormStartPosition.CenterScreen;

			bindingCart.ListChanged += (s, e) => UpdateTotal();
			dgvCart.DataSource = bindingCart;

			UpdateTotal();
		}

		public CartForm(List<CartItem> cart) : this()
		{
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
				return;

			sourceCart = cart ?? new List<CartItem>();
			bindingCart = new BindingList<CartItem>(sourceCart);
			bindingCart.ListChanged += (s, e) => UpdateTotal();

			dgvCart.DataSource = bindingCart;
			UpdateTotal();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();

			this.dgvCart = new DataGridView();
			this.btnPay = new Button();
			this.btnRemove = new Button();
			this.lblTotal = new Label();

			this.SuspendLayout();

			// базовый размер
			this.ClientSize = new Size(600, 380);
			this.MinimumSize = new Size(520, 300);

			// dgvCart
			this.dgvCart.Name = "dgvCart";
			this.dgvCart.TabIndex = 0;
			this.dgvCart.AutoGenerateColumns = false;
			this.dgvCart.ReadOnly = true;
			this.dgvCart.Location = new Point(12, 12);
			this.dgvCart.Size = new Size(this.ClientSize.Width - 24, this.ClientSize.Height - 100);
			this.dgvCart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvCart.AllowUserToAddRows = false;
			this.dgvCart.AllowUserToDeleteRows = false;

			// Добавляем колонки
			this.dgvCart.Columns.Add(new DataGridViewTextBoxColumn
			{
				HeaderText = "Название",
				DataPropertyName = "Name",
				Name = "colName",
				FillWeight = 50
			});
			this.dgvCart.Columns.Add(new DataGridViewTextBoxColumn
			{
				HeaderText = "Цена",
				DataPropertyName = "Price",
				Name = "colPrice",
				FillWeight = 15
			});
			this.dgvCart.Columns.Add(new DataGridViewTextBoxColumn
			{
				HeaderText = "Кол-во",
				DataPropertyName = "Quantity",
				Name = "colQuantity",
				FillWeight = 15
			});
			this.dgvCart.Columns.Add(new DataGridViewTextBoxColumn
			{
				HeaderText = "Сумма",
				DataPropertyName = "Total",
				Name = "colTotal",
				FillWeight = 20
			});

			// lblTotal
			this.lblTotal.Name = "lblTotal";
			this.lblTotal.TabIndex = 2;
			this.lblTotal.Size = new Size(this.ClientSize.Width - 200, 30);
			this.lblTotal.Location = new Point(12, this.ClientSize.Height - 70);
			this.lblTotal.Text = "Итого: 0 руб.";
			this.lblTotal.TextAlign = ContentAlignment.MiddleLeft;
			this.lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

			// btnRemove
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.TabIndex = 3;
			this.btnRemove.Size = new Size(120, 34);
			this.btnRemove.Location = new Point(this.ClientSize.Width - 270, this.ClientSize.Height - 72);
			this.btnRemove.Text = "Удалить позицию";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnRemove.Click += BtnRemove_Click;

			// btnPay
			this.btnPay.Name = "btnPay";
			this.btnPay.TabIndex = 1;
			this.btnPay.Size = new Size(120, 34);
			this.btnPay.Location = new Point(this.ClientSize.Width - this.btnPay.Width - 12, this.ClientSize.Height - 72);
			this.btnPay.Text = "Оплатить";
			this.btnPay.UseVisualStyleBackColor = true;
			this.btnPay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnPay.Click += BtnPay_Click;

			// Form
			this.Controls.Add(this.dgvCart);
			this.Controls.Add(this.btnPay);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.lblTotal);
			this.Name = "CartForm";
			this.Text = "Корзина";
			this.ResumeLayout(false);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				components?.Dispose();

			base.Dispose(disposing);
		}

		private void UpdateTotal()
		{
			decimal total = bindingCart?.Sum(x => x?.Total ?? 0m) ?? 0m;
			lblTotal.Text = $"Итого: {total} руб.";
		}

		private void BtnRemove_Click(object sender, EventArgs e)
		{
			if (dgvCart.CurrentRow == null)
			{
				MessageBox.Show("Выберите строку для удаления.", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			int index = dgvCart.CurrentRow.Index;
			if (index < 0 || index >= bindingCart.Count) return;

			var item = bindingCart[index];

			var result = MessageBox.Show(
				$"Удалить одну позицию «{item.Name}»?",
				"Подтверждение удаления",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				if (item.Quantity > 1)
					item.Quantity--;
				else
					bindingCart.Remove(item);

				UpdateTotal();
				dgvCart.Refresh();
			}
		}

		private void BtnPay_Click(object sender, EventArgs e)
		{
			if (bindingCart == null || bindingCart.Count == 0)
			{
				MessageBox.Show("Корзина пуста.");
				return;
			}

			string startupPath = Application.StartupPath;
			string expected = Path.Combine(startupPath, DefaultVideoFileName);
			string videoPath = File.Exists(expected) ? expected : null;

			if (videoPath == null)
			{
				using (OpenFileDialog ofd = new OpenFileDialog())
				{
					ofd.Title = "Выберите видео jur.mp4";
					ofd.Filter = "Видео файлы|*.mp4;*.avi;*.wmv;*.mkv;*.mov|Все файлы|*.*";
					ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

					if (ofd.ShowDialog(this) == DialogResult.OK)
						videoPath = ofd.FileName;
					else
					{
						MessageBox.Show("Видео не найдено, оплата отменена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
				}
			}

			try
			{
				Process.Start(new ProcessStartInfo(videoPath) { UseShellExecute = true });
			}
			catch (Exception ex)
			{
				MessageBox.Show("Не удалось открыть видео: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			sourceCart?.Clear();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
