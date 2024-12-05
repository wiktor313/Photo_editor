using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Edytor_zdjec
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ChartArea chartArea = new ChartArea();
            chart1.Dock = DockStyle.Fill;
            chart1.ChartAreas.Add(chartArea);
            chart1.ChartAreas[0].RecalculateAxesScale();
            Series series = new Series();
            series.ChartType = SeriesChartType.Column;
            series.Points.DataBindY(Form1.histogram);
            chart1.Series.Add(series);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                save();
            }
            catch
            {
                MessageBox.Show("Błąd zapisu!");
            }
        }
        void save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg|Wszystkie pliki (*.*)|*.*";  //Opcje zapisu.
            saveFileDialog.FilterIndex = 1;                                                                       //Domyślny indeks spokosu zapisu.
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                this.chart1.SaveImage(path, ChartImageFormat.Png);
            }
        }
    }
}
