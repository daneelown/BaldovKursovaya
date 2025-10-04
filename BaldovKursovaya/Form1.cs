using KonditerskayaApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaldovKursovaya
{
	public partial class Form1 : Form
	{
		private List<CartItem> cart = new List<CartItem>();

		// для управления уведомлением (чтобы перезапускать таймер)
		private CancellationTokenSource messageCts;

		public Form1()
		{
			InitializeComponent();

			// Разрешаем изменять размер формы
			this.FormBorderStyle = FormBorderStyle.Sizable;
			this.MaximizeBox = true;
			this.MinimizeBox = true;
			this.StartPosition = FormStartPosition.CenterScreen;
			this.MinimumSize = new System.Drawing.Size(620, 450);
		}

		// Загрузка категорий
		private void buttonMenu_Click(object sender, EventArgs e)
		{
			string sql = "SELECT Id, Name FROM Category";
			DataTable dt = DatabaseHelper.GetData(sql);

			listBoxCategories.DataSource = dt;
			listBoxCategories.DisplayMember = "Name";
			listBoxCategories.ValueMember = "Id";
		}

		// Загрузка позиций выбранной категории
		private void listBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
		{
			// если DataSource у listBoxCategories — DataTable, SelectedValue может быть DBNull/строкой и т.д.
			if (listBoxCategories.SelectedValue == null) return;

			int categoryId;
			try
			{
				categoryId = Convert.ToInt32(listBoxCategories.SelectedValue);
			}
			catch
			{
				return;
			}

			string sql = $"SELECT Position, Price FROM Menu WHERE CategoryId = {categoryId}";
			DataTable dt = DatabaseHelper.GetData(sql);

			listBoxMenu.Items.Clear();
			if (dt != null)
			{
				foreach (DataRow row in dt.Rows)
				{
					listBoxMenu.Items.Add($"{row["Position"]} — {row["Price"]} руб.");
				}
			}
		}

		// При выборе блюда показываем описание в textBoxDescription
		private void listBoxMenu_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxMenu.SelectedItem == null)
			{
				textBoxDescription.Clear();
				return;
			}

			string itemText = listBoxMenu.SelectedItem.ToString();
			string name = itemText.Split('—')[0].Trim();

			string sql = $"SELECT Description, Price FROM Menu WHERE Position = N'{name.Replace("'", "''")}'";
			DataTable dt = DatabaseHelper.GetData(sql);

			if (dt != null && dt.Rows.Count > 0)
			{
				string description = dt.Rows[0]["Description"] == DBNull.Value ? "" : dt.Rows[0]["Description"].ToString();
				string price = dt.Rows[0]["Price"].ToString();

				if (string.IsNullOrWhiteSpace(description))
					description = "Описание пока отсутствует.";

				textBoxDescription.Text = $"{name}\r\n\r\n{description}\r\n\r\nЦена: {price} руб.";
			}
			else
			{
				textBoxDescription.Text = "Описание не найдено.";
			}
		}

		// Добавить выбранный товар в корзину
		private void buttonAddToCart_Click(object sender, EventArgs e)
		{
			// если ничего не выбрано — показываем уведомление и выходим
			if (listBoxMenu.SelectedItem == null)
			{
				ShowMessage("Выберите товар в меню", Color.Firebrick);
				return;
			}

			string itemText = listBoxMenu.SelectedItem.ToString();
			var parts = itemText.Split('—');
			string name = parts[0].Trim();
			string pricePart = parts.Length > 1 ? parts[1].Trim() : "0";

			// Получим только цифры и разделитель
			string priceDigits = new string(pricePart.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()).Replace(',', '.');
			if (!decimal.TryParse(priceDigits, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
			{
				price = 0m;
			}

			// Если такой товар уже в корзине — увеличим количество, иначе добавим новый
			var existing = cart.FirstOrDefault(x => x.Name == name && x.Price == price);
			if (existing != null)
			{
				existing.Quantity++;
			}
			else
			{
				cart.Add(new CartItem { Name = name, Price = price, Quantity = 1 });
			}

			// ВАЖНО: вместо MessageBox — показываем ненавязчивое уведомление на форме
			ShowMessage("Товар добавлен в корзину!", Color.DarkGreen);
		}

		private void buttonCart_Click(object sender, EventArgs e)
		{
			using (var cartForm = new CartForm(cart))
			{
				var result = cartForm.ShowDialog(this);
				// Если оплата прошла успешно, CartForm вернёт DialogResult.OK — очистим корзину
				if (result == DialogResult.OK)
				{
					cart.Clear();
				}
			}
		}

		// Показывает уведомление в labelMessage и скрывает через 3 секунды
		// Если показать новое сообщение раньше предыдущее отменяется и таймер перезапускается
		private async void ShowMessage(string text, Color? foreColor = null)
		{
			// Отменяем предыдущий таймер
			try
			{
				messageCts?.Cancel();
			}
			catch { }

			messageCts = new CancellationTokenSource();
			var token = messageCts.Token;

			// Настроим внешний вид
			labelMessage.Text = text;
			labelMessage.Font = new System.Drawing.Font("Century Gothic", 11F, System.Drawing.FontStyle.Bold);
			labelMessage.ForeColor = foreColor ?? System.Drawing.Color.DarkGreen;
			labelMessage.Visible = true;

			try
			{
				await Task.Delay(3000, token); // ждём 3 секунды
			}
			catch (TaskCanceledException)
			{
				// если отменено — просто выйти (новое сообщение, предыдущий таймер прерван)
				return;
			}

			// скрываем, если не отменено
			if (!token.IsCancellationRequested)
				labelMessage.Visible = false;
		}
	}
}
