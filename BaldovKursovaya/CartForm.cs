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
	public class CartForm : Form
	{
		private System.ComponentModel.IContainer components = null;

		private DataGridView dgvCart;
		private Button btnPay;
		private Label lblTotal;
		private BindingList<CartItem> bindingCart;
		private List<CartItem> sourceCart;

		private const string DefaultVideoFileName = "jur.mp4";

		// Конструктор без параметров — используется дизайнером
		public CartForm()
		{
			bindingCart = new BindingList<CartItem>();
			sourceCart = new List<CartItem>();

			InitializeComponent();

			// Разрешаем изменять размер формы — динамическая верстка
			this.FormBorderStyle = FormBorderStyle.Sizable;
			this.MaximizeBox = true;
			this.MinimizeBox = true;
			this.StartPosition = FormStartPosition.CenterScreen;

			// подписываемся на изменение списка, чтобы обновлять итог автоматически
			bindingCart.ListChanged += (s, e) => UpdateTotal();

			dgvCart.DataSource = bindingCart;

			UpdateTotal();
		}

		// Конструктор для реального использования
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
			this.lblTotal = new Label();
			this.SuspendLayout();

			// базовый размер и минимальный размер формы
			this.ClientSize = new Size(600, 360);
			this.MinimumSize = new Size(520, 300);

			// dgvCart
			this.dgvCart.Name = "dgvCart";
			this.dgvCart.TabIndex = 0;
			this.dgvCart.AutoGenerateColumns = false;
			this.dgvCart.ReadOnly = true;
			this.dgvCart.Location = new Point(12, 12);
			// оставляем нижний отступ, чтобы поместились lblTotal и btnPay
			this.dgvCart.Size = new Size(this.ClientSize.Width - 24, this.ClientSize.Height - 80);

			// Делаем поведение адаптивным
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
				FillWeight = 50 // относительная ширина
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

			// lblTotal — располагается внизу слева и растягивается вправо
			this.lblTotal.Name = "lblTotal";
			this.lblTotal.TabIndex = 2;
			this.lblTotal.Size = new Size(this.ClientSize.Width - 150, 30);
			this.lblTotal.Location = new Point(12, this.ClientSize.Height - 54);
			this.lblTotal.Text = "Итого: 0 руб.";
			this.lblTotal.TextAlign = ContentAlignment.MiddleLeft;
			this.lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

			// btnPay — приклеена к правому нижнему углу
			this.btnPay.Name = "btnPay";
			this.btnPay.TabIndex = 1;
			this.btnPay.Size = new Size(120, 34);
			this.btnPay.Location = new Point(this.ClientSize.Width - this.btnPay.Width - 12, this.ClientSize.Height - 56);
			this.btnPay.Text = "Оплатить";
			this.btnPay.UseVisualStyleBackColor = true;
			this.btnPay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnPay.Click += BtnPay_Click;

			// Form
			this.Controls.Add(this.dgvCart);
			this.Controls.Add(this.btnPay);
			this.Controls.Add(this.lblTotal);
			this.Name = "CartForm";
			this.Text = "Корзина";
			this.ResumeLayout(false);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				components?.Dispose();
			}
			base.Dispose(disposing);
		}

		private void UpdateTotal()
		{
			decimal total = 0m;
			if (bindingCart != null)
				total = bindingCart.Sum(x => x?.Total ?? 0m);

			if (lblTotal != null)
				lblTotal.Text = $"Итого: {total} руб.";
		}

		private void BtnPay_Click(object sender, EventArgs e)
		{
			if (bindingCart == null || bindingCart.Count == 0)
			{
				MessageBox.Show("Корзина пуста.");
				return;
			}

			// ищем видео в папке запуска
			string startupPath = Application.StartupPath;
			string expected = Path.Combine(startupPath, DefaultVideoFileName);

			string videoPath = null;

			if (File.Exists(expected))
			{
				videoPath = expected;
			}
			else
			{
				// если jur.mp4 нет — попросим выбрать
				using (OpenFileDialog ofd = new OpenFileDialog())
				{
					ofd.Title = "Выберите видео jur.mp4";
					ofd.Filter = "Видео файлы|*.mp4;*.avi;*.wmv;*.mkv;*.mov|Все файлы|*.*";
					ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

					if (ofd.ShowDialog(this) == DialogResult.OK)
					{
						videoPath = ofd.FileName;
					}
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

			// очищаем корзину и закрываем форму
			sourceCart?.Clear();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
