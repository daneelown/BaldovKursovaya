namespace BaldovKursovaya
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelWelcome;
        private System.Windows.Forms.Button buttonMenu;
        private System.Windows.Forms.ListBox listBoxCategories;
        private System.Windows.Forms.ListBox listBoxMenu;
        private System.Windows.Forms.Button buttonAddToCart;
        private System.Windows.Forms.Button buttonCart;
        private System.Windows.Forms.TextBox textBoxDescription;

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
            this.components = new System.ComponentModel.Container();

            this.labelWelcome = new System.Windows.Forms.Label();
            this.buttonMenu = new System.Windows.Forms.Button();
            this.listBoxCategories = new System.Windows.Forms.ListBox();
            this.listBoxMenu = new System.Windows.Forms.ListBox();
            this.buttonAddToCart = new System.Windows.Forms.Button();
            this.buttonCart = new System.Windows.Forms.Button();
            this.textBoxDescription = new System.Windows.Forms.TextBox();

            this.SuspendLayout();
 
            // labelWelcome
            this.labelWelcome.Font = new System.Drawing.Font("Century Schoolbook", 18F, System.Drawing.FontStyle.Bold);
            this.labelWelcome.ForeColor = System.Drawing.Color.Crimson;
            this.labelWelcome.Location = new System.Drawing.Point(15, 23);
            this.labelWelcome.Size = new System.Drawing.Size(760, 64);
            this.labelWelcome.Text = "♥♥♥Привет!♥♥♥ Мы кафе-кондитерская \"С Собой\"";
            this.labelWelcome.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelWelcome.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
 
            // buttonMenu
            this.buttonMenu.BackColor = System.Drawing.Color.MistyRose;
            this.buttonMenu.Font = new System.Drawing.Font("Century", 9.75F);
            this.buttonMenu.ForeColor = System.Drawing.Color.Firebrick;
            this.buttonMenu.Location = new System.Drawing.Point(320, 100);
            this.buttonMenu.Size = new System.Drawing.Size(160, 34);
            this.buttonMenu.Text = "НАШЕ МЕНЮ";
            this.buttonMenu.Click += new System.EventHandler(this.buttonMenu_Click);
            this.buttonMenu.Anchor = System.Windows.Forms.AnchorStyles.Top;

            // listBoxCategories
            this.listBoxCategories.Location = new System.Drawing.Point(20, 150);
            this.listBoxCategories.Size = new System.Drawing.Size(220, 260);
            this.listBoxCategories.SelectedIndexChanged += new System.EventHandler(this.listBoxCategories_SelectedIndexChanged);
            this.listBoxCategories.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;

            // listBoxMenu
            this.listBoxMenu.Location = new System.Drawing.Point(260, 150);
            this.listBoxMenu.Size = new System.Drawing.Size(300, 260);
            this.listBoxMenu.SelectedIndexChanged += new System.EventHandler(this.listBoxMenu_SelectedIndexChanged);
            this.listBoxMenu.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;

            // buttonAddToCart
            this.buttonAddToCart.Location = new System.Drawing.Point(260, 420);
            this.buttonAddToCart.Size = new System.Drawing.Size(140, 30);
            this.buttonAddToCart.Text = "Добавить в корзину";
            this.buttonAddToCart.Click += new System.EventHandler(this.buttonAddToCart_Click);
            this.buttonAddToCart.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;

            // buttonCart
            this.buttonCart.Location = new System.Drawing.Point(420, 420);
            this.buttonCart.Size = new System.Drawing.Size(140, 30);
            this.buttonCart.Text = "КОРЗИНА";
            this.buttonCart.Click += new System.EventHandler(this.buttonCart_Click);
            this.buttonCart.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;

			// textBoxDescription
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.textBoxDescription.Location = new System.Drawing.Point(580, 150);
			this.textBoxDescription.Size = new System.Drawing.Size(270, 300);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.ReadOnly = true;
			this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxDescription.BackColor = System.Drawing.Color.MistyRose; // фон как у формы
			this.textBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None; // убрали рамку
			this.textBoxDescription.Anchor =
				System.Windows.Forms.AnchorStyles.Top
				| System.Windows.Forms.AnchorStyles.Bottom
				| System.Windows.Forms.AnchorStyles.Left
				| System.Windows.Forms.AnchorStyles.Right;


			// Form1
			this.BackColor = System.Drawing.Color.MistyRose;
            this.ClientSize = new System.Drawing.Size(880, 480);
            this.Controls.Add(this.labelWelcome);
            this.Controls.Add(this.buttonMenu);
            this.Controls.Add(this.listBoxCategories);
            this.Controls.Add(this.listBoxMenu);
            this.Controls.Add(this.buttonAddToCart);
            this.Controls.Add(this.buttonCart);
            this.Controls.Add(this.textBoxDescription);
            this.Name = "Form1";
            this.Text = "Кафе-кондитерская \"С Собой\"";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
