using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Edytor_zdjec
{
    public partial class Form1 : Form
    {
        public Bitmap image;
        Bitmap greyGlobal;
        Bitmap monoGlobal;
        Bitmap bitGlobal;
        public static int[] histogram = new int[256];
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp;)|*.jpg; *.jpeg; *.png; *.gif; *.bmp|All files(*.*)|*.*";
            open.FilterIndex = 0;
            if (open.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(open.FileName);
                pictureBox1.Image = image;
            }
        }

        private void button3_Click(object sender, EventArgs e) //grey
        {
            if (image == null)
            {
                MessageBox.Show("Load the image first!");
                return;
            }
                button7.Visible = true;
                greyGlobal = ConvertToGrey(image);
                pictureBox2.Image = greyGlobal;
                pictureBox1.Image = image;

        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e) //mono
        {
            if (image == null)
            {
                MessageBox.Show("Load the image first!");
                return;
            }
            if (greyGlobal == null)
            {
                greyGlobal = ConvertToGrey(image);
                pictureBox2.Image = greyGlobal;
            }
            label4.Visible = true;
            label1.Visible = true;
            label1.Text = trackBar1.Value.ToString();
            trackBar1.Visible = true;
            monoGlobal = ConvertToMono(greyGlobal);
            pictureBox3.Image = monoGlobal;
            pictureBox3.Update();
        }
        private Bitmap ConvertToGrey(Bitmap grey)
        {
            Bitmap greyCopy = (Bitmap)grey.Clone();
            Color color;
            double colorToGrey;
            //0.299R 0.587G 0.114B
            for (int i = 0; i < greyCopy.Width; i++)
            {
                for (int j = 0; j < greyCopy.Height; j++)
                {
                    color = greyCopy.GetPixel(i, j);
                    colorToGrey = (color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                    Color newColor = Color.FromArgb((int)colorToGrey, (int)colorToGrey, (int)colorToGrey);
                    greyCopy.SetPixel(i, j, newColor);
                }
            }
            return greyCopy;
        }
        private Bitmap ConvertToMono(Bitmap mono)
        {
            Bitmap monoCopy = (Bitmap)mono.Clone();
            int colorBW;
            Color color;
            double black=0;
            double white=0;
            double percentWhite = 0;
            double percentBlack = 0;
            for (int i = 0; i < monoCopy.Width; i++)
            {
                for (int j = 0; j < monoCopy.Height; j++)
                {
                    color = monoCopy.GetPixel(i, j);
                    if(color.R<=trackBar1.Value)
                    {
                        colorBW = 0;
                        black++;
                    }
                    else
                    {
                        colorBW = 255;
                        white++;
                    }
                    Color newColor = Color.FromArgb((int)colorBW, (int)colorBW, (int)colorBW);
                    monoCopy.SetPixel(i, j, newColor);
                }
            }
            percentWhite = Math.Round((white / (white + black)) * 100, 2);
            percentBlack = Math.Round((black / (white + black)) * 100, 2);
            label4.Text = $"The percentage of whites is: {percentWhite}% \nThe percentage of blacks is: {percentBlack}%";
            return monoCopy;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (image == null)
            {
                MessageBox.Show("Load the image first!");
                return;
            }
            if (greyGlobal == null)
            {
                greyGlobal = ConvertToGrey(image);
                pictureBox2.Image = greyGlobal;
            }
            bitGlobal = sqrtBitmap(greyGlobal);
            pictureBox4.Image = bitGlobal;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = true;
            comboBox1.SelectedItem = comboBox1.Items[0];
            comboBox2.SelectedItem = comboBox2.Items[0];
        }
        

        private void button5_Click(object sender, EventArgs e)
        {
                zapis();
        }
        private void zapis()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg|Wszystkie pliki (*.*)|*.*";  //Save options.
            saveFileDialog.FilterIndex = 1;                                                                       //Default index of the write method.
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                switch (comboBox2.SelectedIndex)
                {
                    case 0:
                        this.pictureBox2.Image?.Save(path, ImageFormat.Png);
                        break;
                    case 1:
                        this.pictureBox3.Image?.Save(path, ImageFormat.Png);
                        break;
                    case 2:
                        this.pictureBox4.Image?.Save(path, ImageFormat.Png);
                        break;
                    case 3:
                        this.pictureBox5.Image?.Save(path, ImageFormat.Png);
                        break;
                }
                
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (image == null)
            {
                MessageBox.Show("Load the image first!");
                return;
            }
            if (greyGlobal == null)
            {
                greyGlobal = ConvertToGrey(image);
                pictureBox2.Image = greyGlobal;
            }
            Bitmap sobel = Sobel(greyGlobal);
            pictureBox5.Image = sobel;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            GenerateHistogram(greyGlobal);
            Form Form2 = new Form2();
            Form2.ShowDialog();
        }
        private Bitmap sqrtBitmap(Bitmap bitmap)
        {
            Bitmap bitmapClone = (Bitmap)bitmap.Clone();
            Color color;
            double colorSqrt = 0.0;
            //0.299R 0.587G 0.114B
            for (int i = 0; i < bitmapClone.Width; i++)
            {
                for (int j = 0; j < bitmapClone.Height; j++)
                {
                    color = bitmapClone.GetPixel(i, j);
                    if (comboBox1.SelectedItem == comboBox1.Items[0])
                        colorSqrt = Math.Sqrt(color.R);
                    if (comboBox1.SelectedItem == comboBox1.Items[1])
                        colorSqrt = Math.Log(color.R + 1, 2);

                    colorSqrt = (byte)(colorSqrt / Math.Sqrt(255) * 255);
                    Color newColor = Color.FromArgb((int)colorSqrt, (int)colorSqrt, (int)colorSqrt);
                    bitmapClone.SetPixel(i, j, newColor);
                }
            }
            return bitmapClone;
        }
        private Bitmap Sobel(Bitmap bitmap)
        {
            Bitmap bitmapClone = (Bitmap)bitmap.Clone();
            int[,] sobelX = new int[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] sobelY = new int[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            for (int y = 1; y < bitmapClone.Height - 3; y++)
            {
                for (int x = 1; x < bitmapClone.Width - 3; x++)
                {
                            int pixelValueX = 0;
                            int pixelValueY = 0;

                    for (int j = 0; j < 3; j++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Color pixelColor = bitmapClone.GetPixel(x+1+i, y+1+j);
                            int grayscaleValue = pixelColor.R;
                            pixelValueX += sobelX[i, j] * grayscaleValue;
                            pixelValueY += sobelY[i, j] * grayscaleValue;
                        }
                    }

                    int sobelValue = (int)((pixelValueX+pixelValueY / 18));
                    if (sobelValue < 0)
                        sobelValue = 0;
                    else if (sobelValue > 255)
                        sobelValue = 255;

                    Color sobelColor = Color.FromArgb(sobelValue, sobelValue, sobelValue);
                    bitmapClone.SetPixel(x, y, sobelColor);
                }
            }

            return bitmapClone;
        }

        static int[] GenerateHistogram(Bitmap image)
        {

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int intensity = pixelColor.R; // The image is already in grayscale, so the red channel value is sufficient
                    histogram[intensity]++;
                }
            }

            return histogram;
        }

    }
}