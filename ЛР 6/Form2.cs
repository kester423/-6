using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ЛР_6.ModelEF;

namespace ЛР_6
{
    public partial class FormAddUPDMD : Form
    {
        public FormAddUPDMD()
        {
            InitializeComponent();
        }
        private string Pic_Name;
        private List<Table_Motorbike> vsMotorbike = FormShowMot.DB.Table_Motorbike.ToList();

        private void FormAddUPDMD_Load(object sender, EventArgs e)
        {
            List<string> dictBrand = new List<string>();
            foreach (Table_Motorbike TB in vsMotorbike)
                dictBrand.Add(TB.Brand);
            dictBrand = dictBrand.Distinct().ToList();
            comboBoxBrand.DataSource = dictBrand;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(comboBoxBrand.Text)||String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Заполните все поля Модель и Марка!");
                return;
            }
            try
            {
                Convert.ToInt32(textBox5.Text);
                Convert.ToInt32(textBox4.Text);
            }
            catch
            {
                MessageBox.Show("В полях Л/С и Пробег, могут быть ьолько целочисленные данные");
                return;
            }
            try
            {
                Convert.ToSingle(textBox3.Text);
            }
            catch(Exception)
            {
                MessageBox.Show("В поле Цена, могут быть только числа с плавующей точкой");
                return;
            }
            if(!File.Exists(Pic_Name))
            {
                MessageBox.Show("Невозможно найти файл!");
                return;
            }
            Table_Motorbike NMotorbike = new Table_Motorbike();
            NMotorbike.ID = FLplus1();
            NMotorbike.Brand = comboBoxBrand.Text;
            NMotorbike.Model = textBox1.Text;
            NMotorbike.Price = Convert.ToSingle(textBox3.Text);
            NMotorbike.Horsepower = Convert.ToInt32(textBox4.Text);
            NMotorbike.Mileage = Convert.ToInt32(textBox5.Text);
            NMotorbike.Picture = $@"{FLplus1()}{Path.GetExtension(Pic_Name)}";
            File.Copy(Pic_Name, $@"{FLplus1()}{Path.GetExtension(Pic_Name)}");
            try
            {
                FormShowMot.DB.Table_Motorbike.Add(NMotorbike);
                FormShowMot.DB.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Данные успешно добавлены!");
            FormShowMot form = new FormShowMot();
            form.Visible = true;
            this.Close();
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
                textBox.Text = null;
            comboBoxBrand.SelectedIndex = 0;
            pictureBox1.Image = null;
            Pic_Name = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormShowMot form = new FormShowMot();
            form.Visible = true;
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp; *.jpg, *.png";
            DialogResult result = openFileDialog.ShowDialog();
            if(DialogResult.OK==result)
            {
                Pic_Name = openFileDialog.FileName;
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ',')
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 46)
                e.Handled = true;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 46)
                e.Handled = true;
        }
        private int FLplus1()
        {
            int max = 0;
            foreach (Table_Motorbike TB in vsMotorbike)
                if (max < TB.ID) max = TB.ID;
            return ++max;
        }
    }
}
