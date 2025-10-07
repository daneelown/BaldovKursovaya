using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BaldovKursovaya
{
	public partial class OrderInfoForm : Form
	{
		public string CustomerName { get; private set; }
		public string Phone { get; private set; }

		public OrderInfoForm(decimal total)
		{
			InitializeComponent();

			labelTitle.Text = "💖 Укажите, для кого заказ:";
			labelTotal.Text = $"Сумма заказа: {total} руб.";
		}

		private void InitializeComponent()
		{
			this.labelTitle = new Label();
			this.labelName = new Label();
			this.textName = new TextBox();
			this.labelPhone = new Label();
			this.textPhone = new TextBox();
			this.labelTotal = new Label();
			this.buttonOk = new Button();
			this.buttonCancel = new Button();

			this.SuspendLayout();

			// labelTitle
			this.labelTitle.Font = new Font("Century Schoolbook", 15F, FontStyle.Bold);
			this.labelTitle.ForeColor = Color.MediumVioletRed;
			this.labelTitle.Location = new Point(10, 15);
			this.labelTitle.Size = new Size(380, 45);
			this.labelTitle.TextAlign = ContentAlignment.MiddleCenter;

			// labelName
			this.labelName.Text = "Имя:";
			this.labelName.Font = new Font("Century Gothic", 10F);
			this.labelName.Location = new Point(35, 75);
			this.labelName.Size = new Size(80, 25);

			// textName
			this.textName.Font = new Font("Century Gothic", 10F);
			this.textName.Location = new Point(120, 75);
			this.textName.Size = new Size(220, 25);

			// labelPhone
			this.labelPhone.Text = "Телефон:";
			this.labelPhone.Font = new Font("Century Gothic", 10F);
			this.labelPhone.Location = new Point(35, 115);
			this.labelPhone.Size = new Size(80, 25);

			// textPhone
			this.textPhone.Font = new Font("Century Gothic", 10F);
			this.textPhone.Location = new Point(120, 115);
			this.textPhone.Size = new Size(220, 25);

			// labelTotal
			this.labelTotal.Font = new Font("Century Gothic", 11F, FontStyle.Italic);
			this.labelTotal.ForeColor = Color.DarkSlateGray;
			this.labelTotal.Location = new Point(35, 160);
			this.labelTotal.Size = new Size(300, 25);
			this.labelTotal.TextAlign = ContentAlignment.MiddleCenter;

			// buttonOk
			this.buttonOk.Text = "Оплатить заказ 💌";
			this.buttonOk.BackColor = Color.LightPink;
			this.buttonOk.Font = new Font("Century Gothic", 9F, FontStyle.Bold);
			this.buttonOk.Location = new Point(55, 200);
			this.buttonOk.Size = new Size(130, 38);
			this.buttonOk.Click += ButtonOk_Click;

			// buttonCancel
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.Font = new Font("Century Gothic", 9F, FontStyle.Bold);
			this.buttonCancel.BackColor = Color.White;
			this.buttonCancel.Location = new Point(210, 200);
			this.buttonCancel.Size = new Size(130, 38);
			this.buttonCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

			// Form
			this.ClientSize = new Size(400, 270);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.labelPhone);
			this.Controls.Add(this.textPhone);
			this.Controls.Add(this.labelTotal);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.buttonCancel);
			this.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Данные заказчика";
			this.BackColor = Color.MistyRose;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;

			this.ResumeLayout(false);
		}

		private void ButtonOk_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(textName.Text))
			{
				MessageBox.Show("Введите имя, пожалуйста 💕", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			CustomerName = textName.Text.Trim();
			Phone = textPhone.Text.Trim();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private Label labelTitle;
		private Label labelName;
		private Label labelPhone;
		private TextBox textName;
		private TextBox textPhone;
		private Label labelTotal;
		private Button buttonOk;
		private Button buttonCancel;
	}
}
