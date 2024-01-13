using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataPeople_RVP_Yurl
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        
        public Form1()
        {
            InitializeComponent();
            InitDataGridView();
        }

        private void InitDataGridView()
        {

            // Создаем столбцы
            dataGridView.Columns.Add("Name", "Имя");
            dataGridView.Columns.Add("Height", "Рост");
            dataGridView.Columns.Add("Weight", "Вес");

            // Создаем столбец для группы крови
            var bloodGroupColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Группа крови",
                Name = "BloodGroup",
                DataSource = new string[] { "A", "B", "AB" }
            };

            dataGridView.Columns.Add(bloodGroupColumn);

            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            dataGridView.Rows.Add(getRandomName(), random.Next(140, 200), random.Next(50, 120), getRandomBloodGroup());
        }
        private string getRandomName()
        {
            string[] names = { "Alice", "Bob", "Charlie", "David", "Eve" };
            return names[random.Next(names.Length)];
        }

        private string getRandomBloodGroup()
        {
            string[] bloodGroups = { "A", "B", "AB" };
            return bloodGroups[random.Next(bloodGroups.Length)];
        }

        private void buttonFilling_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            timer.Stop();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv";
                sfd.Title = "Save as CSV File";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var sw = new StreamWriter(sfd.FileName))
                    {
                        // Записываем заголовки столбцов
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            sw.Write(column.HeaderText + ";");
                        }
                        sw.WriteLine();

                        // Записываем данные из DataGridView
                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                sw.Write(cell.Value + ";");
                            }
                            sw.WriteLine();
                        }
                    }
                }
            }
            MessageBox.Show("Файл успешно сохранён!");
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            timer.Stop();
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV files (*.csv)|*.csv";
                ofd.Title = "Load from CSV File";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    dataGridView.Rows.Clear(); // Очистка существующих данных в DataGridView

                    using (var sr = new StreamReader(ofd.FileName))
                    {
                        string[] headers = sr.ReadLine().Split(';'); // Прочитываем заголовки столбцов

                        while (!sr.EndOfStream)
                        {
                            string[] rows = sr.ReadLine().Split(';');
                            dataGridView.Rows.Add(rows);
                        }
                    }
                }
            }
        }
    }
}
