using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reed_Muler_Code.Channels;
using Reed_Muler_Code.Services;
using Reed_Muler_Code.Extensions;
using System.Diagnostics;

namespace Reed_Muler_Code
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static Vector _encodedVector;
        private static Vector _vectorFromChannel;
        private static VectorService _vectorService = new VectorService();
        private static StringService _stringService = new StringService();
        private static ImageService _imageService = new ImageService();
        private int CountVectorLength(int m, int r) => m.CountCombination(r);
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void encodingButton_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(RtextBox.Text, "^[0-9]{1,}$") || !Regex.IsMatch(MtextBox.Text, "^[0-9]{1,}$"))
            {
                errorTextBox.Text = $"M and R have to be numeric!";
                return;
            }

            if (!Regex.IsMatch(vectorTextBox.Text, "^[0-1]{1,}$"))
            {
                errorTextBox.Text = $"Vector should contain only binary values: 0s and 1s";
                return;
            }

            int r = int.Parse(RtextBox.Text);
            int m = int.Parse(MtextBox.Text);
            int[] vectorBits = vectorTextBox.Text.StringToIntArray();
            int vectorLength = CountVectorLength(m, r);

            if (vectorBits.Length != vectorLength)
            {
                errorTextBox.Text = $"Vector length should be: {vectorLength} symbols long";
                return;
            }

            _encodedVector = _vectorService.EncodeVector(vectorBits, r, m);
            encodedVectorTextBox.Text = _encodedVector.ToString();
        }

        private void channelButton_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(RtextBox.Text, "^[0-9]{1,}$") || !Regex.IsMatch(MtextBox.Text, "^[0-9]{1,}$"))
            {
                errorTextBox.Text = $"M and R have to be numeric!";
                return;
            }

            int r = int.Parse(RtextBox.Text);
            int m = int.Parse(MtextBox.Text);

            if (!Regex.IsMatch(errorRateBox.Text.Replace(',', '.'), "^(0)$|^([0].[0-9]{1,})|^(1)$|^(1.(0){1,})$"))
            {
                errorTextBox.Text = "Error rate should be a value between 0.0 and 1.0";
                return;
            }

            List<int> errors;
            (_vectorFromChannel, errors) = _vectorService.SendThroughChannel(_encodedVector, double.Parse(errorRateBox.Text.Replace(',', '.')));

            errorCountLabel.Text = $"Error count: {errors.Count()}";
            errorPositionsLabel.Text = $"Error positions: {string.Join(", ", errors)}";

            evFromChannelBox.Text = _vectorFromChannel.ToString();
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            if (_encodedVector == null)
            {
                errorTextBox.Text = "No encoded vector could be found";
                return;
            }

            Vector vector = _vectorService.DecodeVector(_vectorFromChannel);
            decodedEncodedTextBox.Text = vector.Bits.ArrayToString();
        }

        private void stringEncode_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(stringRbox.Text, "^[0-9]{1,}$") || !Regex.IsMatch(stringMbox.Text, "^[0-9]{1,}$"))
            {
                errorTextBox.Text = $"M and R have to be numeric!";
                return;
            }
            if (!Regex.IsMatch(stringErrorRateBox.Text.Replace(',', '.'), "^(0)$|^([0].[0-9]{1,})|^(1)$|^(1.(0){1,})$"))
            {
                errorTextBox.Text = "Error rate should be a value between 0.0 and 1.0";
                return;
            }
            if (!double.TryParse(stringErrorRateBox.Text, out var errorRate))
            {
                MessageBox.Show("Error rate must be a number.");
                return;
            }

            int r = int.Parse(stringRbox.Text);
            int m = int.Parse(stringMbox.Text);
            string message = stringBox.Text;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            stringDecodedBox.Text = _stringService.HandleStringWithEncoding(message, m, r, errorRate);
            stringPassedBox.Text = _stringService.HandleString(message, errorRate);

            stopwatch.Stop();
            stringErrorBox.Text = $"Elapsed time: {stopwatch.Elapsed}";
        }

        private void imageUploadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "BMP|*.bmp" };
            Bitmap pictureBitmap = null;

            if (ofd.ShowDialog() == DialogResult.OK)
                pictureBitmap = new Bitmap(ofd.FileName);

            uploadedImagePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            uploadedImagePictureBox.Image = pictureBitmap;
        }

        private void imageChannelButton_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(imageRbox.Text, "^[0-9]{1,}$") || !Regex.IsMatch(imageMbox.Text, "^[0-9]{1,}$"))
            {
                imageErrorBox.Text = $"M and R have to be numeric!";
                return;
            }
            if (!Regex.IsMatch(imageErrorRateBox.Text.Replace(',', '.'), "^(0)$|^([0].[0-9]{1,})|^(1)$|^(1.(0){1,})$"))
            {
                imageErrorBox.Text = "Error rate should be a value between 0.0 and 1.0";
                return;
            }
            if (!double.TryParse(imageErrorRateBox.Text, out var errorRate))
            {
                imageErrorBox.Text = "Error rate must be a number.";
                return;
            }

            int r = int.Parse(imageRbox.Text);
            int m = int.Parse(imageMbox.Text);
            Bitmap image = uploadedImagePictureBox.Image as Bitmap;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            passedImagePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            passedImagePictureBox.Image = _imageService.HandlePicture(image, errorRate);
            encodedDecodedImagePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            encodedDecodedImagePictureBox.Image = _imageService.HandlePictureWithEncoding(image, m, r, errorRate);

            stopwatch.Stop();
            imageErrorBox.Text = $"Elapsed time: {stopwatch.Elapsed}";
        }

    }
}
