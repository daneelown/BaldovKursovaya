using KonditerskayaApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
namespace BaldovKursovaya
{
	public partial class Form1 : Form
	{


		private List<CartItem> cart = new List<CartItem>();

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
			if (listBoxCategories.SelectedValue is int categoryId)
			{
				string sql = $"SELECT Position, Price FROM Menu WHERE CategoryId = {categoryId}";
				DataTable dt = DatabaseHelper.GetData(sql);

				listBoxMenu.Items.Clear();
				foreach (DataRow row in dt.Rows)
				{
					listBoxMenu.Items.Add($"{row["Position"]} — {row["Price"]} руб.");
				}
			}
		}

		// Добавить выбранный товар в корзину
		private void buttonAddToCart_Click(object sender, EventArgs e)
		{
			if (listBoxMenu.SelectedItem == null)
			{
				MessageBox.Show("Выберите товар в меню, чтобы добавить в корзину.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}


			string itemText = listBoxMenu.SelectedItem.ToString();
			// Ожидаемый формат: "Название — 120.5 руб."
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

			MessageBox.Show("Товар добавлен в корзину", "Ок", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


	}
}