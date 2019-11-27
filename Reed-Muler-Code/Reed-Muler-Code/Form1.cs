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
        private static Bitmap _picture;
        private static VectorService _vectorService = new VectorService();
        private static StringService _stringService = new StringService();
        private static ImageService _imageService = new ImageService();
        private int CountVectorLength(int m, int r) => m.CountCombination(r);
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void encodingButton_Click(object sender, EventArgs e)
        {
            if (!Validate(RtextBox, MtextBox, errorRateBox, errorTextBox))
                return;

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
            if (_encodedVector == null)
            {
                errorTextBox.Text = "No encoded vector could be found";
                return;
            }

            if (!Validate(RtextBox, MtextBox, errorRateBox, errorTextBox))
                return;

            List<int> errors;
            (_vectorFromChannel, errors) = _vectorService.SendThroughChannel(_encodedVector, double.Parse(errorRateBox.Text.Replace(',', '.')));

            errorCountLabel.Text = $"Error count: {errors.Count()}";
            errorPositionsLabel.Text = $"Error positions: {string.Join(", ", errors)}";

            evFromChannelBox.Text = _vectorFromChannel.ToString();
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            if (_vectorFromChannel == null)
            {
                errorTextBox.Text = "No vector passed through channel could be found";
                return;
            }

            Vector vector = _vectorService.DecodeVector(_vectorFromChannel);
            decodedEncodedTextBox.Text = vector.Bits.ArrayToString();
        }

        private void stringEncode_Click(object sender, EventArgs e)
        {
            if (!Validate(stringRbox, stringMbox, stringErrorRateBox, stringErrorBox))
                return;
            double.TryParse(stringErrorRateBox.Text, out var errorRate);

            int r = int.Parse(stringRbox.Text);
            int m = int.Parse(stringMbox.Text);
            string message = stringBox.Text;

            if(message == "")
            {
                stringErrorBox.Text = "Please provide a string.";
                return;
            }

            stringErrorBox.Text = "In progress...";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            stringDecodedBox.Text = _stringService.HandleStringWithEncoding(message, m, r, errorRate);
            stringPassedBox.Text = _stringService.HandleString(message, errorRate);

            stopwatch.Stop();
            stringErrorBox.Text = $"Elapsed time: {stopwatch.Elapsed}";
        }

        private void imageUploadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "BMP|*.bmp" };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                _picture = new Bitmap(openFileDialog.FileName);

            uploadedImagePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            uploadedImagePictureBox.Image = _picture;
        }

        private void imageChannelButton_Click(object sender, EventArgs e)
        {
            if(_picture == null)
            {
                imageErrorBox.Text = "Please provide a bitmap image.";
                return;
            }

            if (!Validate(imageRbox, imageMbox, imageErrorRateBox, imageErrorBox))
                return;
            double.TryParse(imageErrorRateBox.Text, out var errorRate);

            int r = int.Parse(imageRbox.Text);
            int m = int.Parse(imageMbox.Text);

            imageErrorBox.Text = "In progress...";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            passedImagePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            passedImagePictureBox.Image = _imageService.HandlePicture(_picture, errorRate);
            encodedDecodedImagePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            encodedDecodedImagePictureBox.Image = _imageService.HandlePictureWithEncoding(_picture, m, r, errorRate);

            stopwatch.Stop();
            imageErrorBox.Text = $"Elapsed time: {stopwatch.Elapsed}";
        }

        private void validateButton_Click(object sender, EventArgs e)
        {
            if (!Validate(RtextBox, MtextBox, errorRateBox, errorTextBox))
                return;

            int r = int.Parse(RtextBox.Text);
            int m = int.Parse(MtextBox.Text);
            int vectorLength = CountVectorLength(m, r);

            errorTextBox.Text = $"Vector length should be: {vectorLength} symbols long";
        }

        private bool Validate(TextBox rBox, TextBox mBox, TextBox errorRateBox, RichTextBox errorBox)
        {
            if (!Regex.IsMatch(rBox.Text, "^[1-9]{1,}$") || !Regex.IsMatch(mBox.Text, "^[1-9]{1,}$"))
            {
                errorBox.Text = $"M and R have to be numeric and more than 0!";
                return false;
            }

            int r = int.Parse(rBox.Text);
            int m = int.Parse(mBox.Text);

            if (r > m)
            {
                errorBox.Text = $"R and M should be: R <= M";
                return false;
            }
            if (!Regex.IsMatch(errorRateBox.Text.Replace(',', '.'), "^(0)$|^([0].[0-9]{1,})|^(1)$|^(1.(0){1,})$"))
            {
                errorBox.Text = "Error rate should be a value between 0.0 and 1.0";
                return false;
            }
            if (!double.TryParse(errorRateBox.Text, out var errorRate))
            {
                errorBox.Text = "Error rate must be a number.";
                return false;
            }

            return true;
        }
    }
}
