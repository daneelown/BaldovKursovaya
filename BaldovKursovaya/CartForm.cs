using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using KonditerskayaApp;
using Newtonsoft.Json; 

namespace BaldovKursovaya
{
	public partial class CartForm : Form
	{
		private System.ComponentModel.IContainer components = null;

		private DataGridView dgvCart;
		private Button btnPay;
		private Button btnRemove;
		private Label lblTotal;
		private Label lblHeader;
		private BindingList<CartItem> bindingCart;
		private List<CartItem> sourceCart;

		private const string DefaultVideoFileName = "jur.mp4";

		public CartForm()
		{
			bindingCart = new BindingList<CartItem>();
			sourceCart = new List<CartItem>();

			InitializeComponent();

			this.BackColor = Color.MistyRose;
			this.Font = new Font("Century Gothic", 10F, FontStyle.Regular);
			this.FormBorderStyle = FormBorderStyle.Sizable;
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
			this.lblHeader = new Label();

			this.SuspendLayout();

			//заголовок
			this.lblHeader.Text = "🧁 Ваша корзина";
			this.lblHeader.Font = new Font("Century Schoolbook", 15F, FontStyle.Bold);
			this.lblHeader.ForeColor = Color.MediumVioletRed;
			this.lblHeader.TextAlign = ContentAlignment.MiddleCenter;
			this.lblHeader.Dock = DockStyle.Top;
			this.lblHeader.Height = 45;
			this.lblHeader.BackColor = Color.FromArgb(255, 240, 245);

			//DataGridView
			this.dgvCart.Name = "dgvCart";
			this.dgvCart.AutoGenerateColumns = false;
			this.dgvCart.ReadOnly = true;
			this.dgvCart.Location = new Point(15, 65);
			this.dgvCart.Size = new Size(650, 230);
			this.dgvCart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.dgvCart.BackgroundColor = Color.White;
			this.dgvCart.GridColor = Color.LightPink;
			this.dgvCart.BorderStyle = BorderStyle.None;
			this.dgvCart.RowHeadersVisible = false;
			this.dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvCart.DefaultCellStyle.Font = new Font("Century Gothic", 10F);
			this.dgvCart.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10F, FontStyle.Bold);
			this.dgvCart.ColumnHeadersDefaultCellStyle.BackColor = Color.MistyRose;
			this.dgvCart.EnableHeadersVisualStyles = false;

			this.dgvCart.Columns.Add(new DataGridViewTextBoxColumn
			{
				HeaderText = "Название",
				DataPropertyName = "Name",
				FillWeight = 50
			});
			this.dgvCart.Columns.Add(new DataGridViewTextBoxColumn
			{
				HeaderText = "Цена",
				DataPropertyName = "Price",
				FillWeight = 15
			});
			this.dgvCart.Columns.Add(new DataGridViewTextBoxColumn
			{
				HeaderText = "Кол-во",
				DataPropertyName = "Quantity",
				FillWeight = 15
			});
			this.dgvCart.Columns.Add(new DataGridViewTextBoxColumn
			{
				HeaderText = "Сумма",
				DataPropertyName = "Total",
				FillWeight = 20
			});

			//lblTotal
			this.lblTotal.Font = new Font("Century Gothic", 11F, FontStyle.Bold);
			this.lblTotal.ForeColor = Color.Firebrick;
			this.lblTotal.Location = new Point(15, 305);
			this.lblTotal.Size = new Size(300, 30);
			this.lblTotal.Text = "Итого: 0 руб.";
			this.lblTotal.TextAlign = ContentAlignment.MiddleLeft;
			this.lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

			//btnRemove
			this.btnRemove.Text = "🗑 Удалить позицию";
			this.btnRemove.Font = new Font("Century Gothic", 9F, FontStyle.Bold);
			this.btnRemove.BackColor = Color.LightPink;
			this.btnRemove.FlatStyle = FlatStyle.Flat;
			this.btnRemove.FlatAppearance.BorderSize = 0;
			this.btnRemove.Size = new Size(170, 36);
			this.btnRemove.Location = new Point(320, 300);
			this.btnRemove.Click += BtnRemove_Click;
			this.btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			//btnPay
			this.btnPay.Text = "💖 Оформить заказ";
			this.btnPay.Font = new Font("Century Gothic", 9F, FontStyle.Bold);
			this.btnPay.BackColor = Color.LightCoral;
			this.btnPay.ForeColor = Color.White;
			this.btnPay.FlatStyle = FlatStyle.Flat;
			this.btnPay.FlatAppearance.BorderSize = 0;
			this.btnPay.Size = new Size(170, 36);
			this.btnPay.Location = new Point(510, 300);
			this.btnPay.Click += BtnPay_Click;
			this.btnPay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			//Form
			this.ClientSize = new Size(700, 360);
			this.Controls.Add(this.lblHeader);
			this.Controls.Add(this.dgvCart);
			this.Controls.Add(this.btnPay);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.lblTotal);
			this.Text = "Корзина";
			this.ResumeLayout(false);
		}

		private void BtnRemove_Click(object sender, EventArgs e)
		{
			if (dgvCart.SelectedRows.Count == 0)
			{
				MessageBox.Show("Выберите позицию для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var item = dgvCart.SelectedRows[0].DataBoundItem as CartItem;
			if (item == null) return;

			var result = MessageBox.Show(
				$"Удалить {item.Name} (×{item.Quantity}) из корзины?",
				"Подтверждение",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				if (item.Quantity > 1)
					item.Quantity--;
				else
					bindingCart.Remove(item);

				UpdateTotal();
			}
		}

		private void UpdateTotal()
		{
			decimal total = bindingCart?.Sum(x => x.Total) ?? 0m;
			lblTotal.Text = $"Итого: {total} руб.";
		}

		private void BtnPay_Click(object sender, EventArgs e)
		{
			if (bindingCart == null || bindingCart.Count == 0)
			{
				MessageBox.Show("Корзина пуста 💔", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			decimal total = bindingCart.Sum(x => x.Total);

			// Форма данных заказчика
			using (var orderForm = new OrderInfoForm(total))
			{
				if (orderForm.ShowDialog(this) != DialogResult.OK)
					return;

				try
				{
					// сериализуем состав заказа в JSON
					var orderItems = bindingCart.Select(ci => new
					{
						Name = ci.Name,
						Price = ci.Price,
						Quantity = ci.Quantity,
						Total = ci.Total
					}).ToList();

					string orderJson = JsonConvert.SerializeObject(orderItems, Formatting.None);

					string sql = @"
						INSERT INTO Orders (CustomerName, CustomerPhone, OrderDate, Total, OrderItems)
						VALUES (@name, @phone, GETDATE(), @total, @json)";

					var parameters = new Dictionary<string, object>
					{
						{"@name", orderForm.CustomerName},
						{"@phone", orderForm.Phone},
						{"@total", total},
						{"@json", orderJson}
					};

					DatabaseHelper.ExecuteNonQuery(sql, parameters);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Ошибка при сохранении заказа: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			// Видео "оплата"
			string startupPath = Application.StartupPath;
			string expected = Path.Combine(startupPath, DefaultVideoFileName);
			string videoPath = null;

			if (File.Exists(expected))
			{
				videoPath = expected;
			}
			else
			{
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

			sourceCart?.Clear();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				components?.Dispose();
			base.Dispose(disposing);
		}
	}
}
