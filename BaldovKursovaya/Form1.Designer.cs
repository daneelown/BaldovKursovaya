using System.Windows.Forms;

namespace BaldovKursovaya
{
	partial class Form1
	{
		private System.ComponentModel.IContainer components = null;
		private Label labelWelcome;
		private Button buttonMenu;
		private ListBox listBoxCategories;
		private ListBox listBoxMenu;
		private Button buttonAddToCart;
		private Button buttonCart;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		private void InitializeComponent()
		{
			// Инициализируем контейнер компонентов (на всякий случай, чтобы Dispose работал корректно)
			this.components = new System.ComponentModel.Container();

			this.labelWelcome = new Label();
			this.buttonMenu = new Button();
			this.listBoxCategories = new ListBox();
			this.listBoxMenu = new ListBox();
			this.buttonAddToCart = new Button();
			this.buttonCart = new Button();
			this.SuspendLayout();

			// labelWelcome
			this.labelWelcome.Font = new System.Drawing.Font("Century Schoolbook", 18F, System.Drawing.FontStyle.Bold);
			this.labelWelcome.ForeColor = System.Drawing.Color.Crimson;
			this.labelWelcome.Location = new System.Drawing.Point(15, 23);
			this.labelWelcome.Size = new System.Drawing.Size(550, 64);
			this.labelWelcome.Text = "♥♥♥Привет!♥♥♥ Мы кафе-кондитерская \"С Собой\"";
			this.labelWelcome.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.labelWelcome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

			// buttonMenu
			this.buttonMenu.BackColor = System.Drawing.Color.MistyRose;
			this.buttonMenu.Font = new System.Drawing.Font("Century", 9.75F);
			this.buttonMenu.ForeColor = System.Drawing.Color.Firebrick;
			this.buttonMenu.Location = new System.Drawing.Point(220, 90);
			this.buttonMenu.Size = new System.Drawing.Size(145, 30);
			this.buttonMenu.Text = "НАШЕ МЕНЮ";
			this.buttonMenu.Click += new System.EventHandler(this.buttonMenu_Click);
			this.buttonMenu.Anchor = AnchorStyles.Top;

			// listBoxCategories
			this.listBoxCategories.Location = new System.Drawing.Point(20, 150);
			this.listBoxCategories.Size = new System.Drawing.Size(200, 238);
			this.listBoxCategories.SelectedIndexChanged += new System.EventHandler(this.listBoxCategories_SelectedIndexChanged);
			this.listBoxCategories.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

			// listBoxMenu
			this.listBoxMenu.Location = new System.Drawing.Point(240, 150);
			this.listBoxMenu.Size = new System.Drawing.Size(300, 238);
			this.listBoxMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

			// buttonAddToCart
			this.buttonAddToCart.Location = new System.Drawing.Point(240, 400);
			this.buttonAddToCart.Size = new System.Drawing.Size(140, 30);
			this.buttonAddToCart.Text = "Добавить в корзину";
			this.buttonAddToCart.Click += new System.EventHandler(this.buttonAddToCart_Click);
			this.buttonAddToCart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

			// buttonCart
			this.buttonCart.Location = new System.Drawing.Point(400, 400);
			this.buttonCart.Size = new System.Drawing.Size(140, 30);
			this.buttonCart.Text = "КОРЗИНА";
			this.buttonCart.Click += new System.EventHandler(this.buttonCart_Click);
			this.buttonCart.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			// Form1
			this.BackColor = System.Drawing.Color.MistyRose;
			this.ClientSize = new System.Drawing.Size(600, 450);
			this.Controls.Add(this.labelWelcome);
			this.Controls.Add(this.buttonMenu);
			this.Controls.Add(this.listBoxCategories);
			this.Controls.Add(this.listBoxMenu);
			this.Controls.Add(this.buttonAddToCart);
			this.Controls.Add(this.buttonCart);
			this.Name = "Form1";
			this.Text = "Кафе-кондитерская \"С Собой\"";
			this.ResumeLayout(false);

		}

		#endregion
	}
}
