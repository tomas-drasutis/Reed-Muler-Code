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
    public partial class ReedMullerForm : Form
    {
        public ReedMullerForm()
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
        private void ReedMullerForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Paspaudus Encoding mygtuka vektoriaus skyriuje kreipiamasi i uzkodavimo funkcijas. Gavus rezultata jis pateikiamas lange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void encodingButton_Click(object sender, EventArgs e)
        {
            if (!Validate(RtextBox, MtextBox, errorRateBox, errorTextBox))
                return;

            int r = int.Parse(RtextBox.Text);
            int m = int.Parse(MtextBox.Text);
            int[] vectorWords = vectorTextBox.Text.StringToIntArray();
            int vectorLength = CountVectorLength(m, r);

            if (vectorWords.Length != vectorLength)
            {
                errorTextBox.Text = $"Vector length should be: {vectorLength} symbols long";
                return;
            }

            _encodedVector = _vectorService.EncodeVector(new Vector(m, r, vectorWords));
            encodedVectorTextBox.Text = _encodedVector.ToString();
        }

        /// <summary>
        /// Paspaudus Channel mygtuka vektoriaus lange uzkoduotas vektorius siunciamas per kanala. Gavus rezultata jis pateikiamas lange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Paspaudus Decode mygtuka vektoriaus lange si funkcija siuncia uzkoduota ir per kanala persiusta vektoriu
        /// i dekodavimo funkcijas. Gavus rezultata jis pateikiamas lange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decodeButton_Click(object sender, EventArgs e)
        {
            if (_vectorFromChannel == null)
            {
                errorTextBox.Text = "No vector passed through channel could be found";
                return;
            }

            Vector vector = _vectorService.DecodeVector(_vectorFromChannel);
            decodedEncodedTextBox.Text = vector.Words.ArrayToString();
        }

        /// <summary>
        /// Paspaudus Send mygtuka simboliu eilutes lange, simboliu eilute siunciama i uzkodavima, per kanala ir yra dekoduojama
        /// taip pat siunciama neuzkoduota simboliu eilute. Gavus rezultata jis pateikiamas lange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            stringErrorBox.Text = $"String will be split into vectors of length: {CountVectorLength(m, r)}";
            stringErrorBox.Text += "\nIn progress...";
            stringPanel.Refresh();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            stringDecodedBox.Text = _stringService.HandleStringWithEncoding(message, m, r, errorRate);
            stringPassedBox.Text = _stringService.HandleString(message, errorRate);

            stopwatch.Stop();
            stringErrorBox.Text = $"Elapsed time: {stopwatch.Elapsed}";
        }

        /// <summary>
        /// Paspaudus Upload mygtuka nuotraukos lange, atidaromas failo pasirinkimo langas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageUploadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "BMP|*.bmp" };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                _picture = new Bitmap(openFileDialog.FileName);

            uploadedImagePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            uploadedImagePictureBox.Image = _picture;
        }

        /// <summary>
        /// Paspaudus Send mygtuka nuotraukos lange nuotrauka siunciama uzkoduoti, persiusti per kanala bei dekoduoti
        /// taip pat siunciama ir neuzkoduota nuotrauka per kanala. Gavus rezultata jis pateikiamas lange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            imageErrorBox.Text = $"Image will be split into vectors of length: {CountVectorLength(m, r)}";
            imageErrorBox.Text += "\nIn progress...";
            imagePanel.Refresh();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            passedImagePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            passedImagePictureBox.Image = _imageService.HandlePicture(_picture, errorRate);
            encodedDecodedImagePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            encodedDecodedImagePictureBox.Image = _imageService.HandlePictureWithEncoding(_picture, m, r, errorRate);

            stopwatch.Stop();
            imageErrorBox.Text = $"Elapsed time: {stopwatch.Elapsed}";
        }


        /// <summary>
        /// Validavimo mygtukas patikrina ar ivesti tinkami m, r ir klaidos tikimybes parametrai
        /// bei pateikia naudotojui kokio ilgo vektoriaus bus tikimasi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void validateButton_Click(object sender, EventArgs e)
        {
            if (!Validate(RtextBox, MtextBox, errorRateBox, errorTextBox))
                return;

            int r = int.Parse(RtextBox.Text);
            int m = int.Parse(MtextBox.Text);
            int vectorLength = CountVectorLength(m, r);

            errorTextBox.Text = $"Vector length should be: {vectorLength} symbols long";
        }


        /// <summary>
        /// Patikrina ar duomenys ivesties laukuose tinkami
        /// </summary>
        /// <param name="rBox">R parametro laukelis</param>
        /// <param name="mBox">M parametro laukelis</param>
        /// <param name="errorRateBox">Klaidos tikimybes laukelis</param>
        /// <param name="errorBox">Klaidos pranesimo laukelis</param>
        /// <returns>Ar duomenys tinkami</returns>
        private bool Validate(TextBox rBox, TextBox mBox, TextBox errorRateBox, RichTextBox errorBox)
        {
            /*if (!Regex.IsMatch(rBox.Text, @"^[1-9]\d*$") || !Regex.IsMatch(mBox.Text, @"^[1-9]\d*$"))
            {
                errorBox.Text = $"M and R have to be numeric, more than 0.";
                return false;
            }
            */
            int r = int.Parse(rBox.Text);
            int m = int.Parse(mBox.Text);

            if (r >= m)
            {
                errorBox.Text = $"R and M should be: R < M";
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
