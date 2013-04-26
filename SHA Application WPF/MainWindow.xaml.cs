using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace SHA_Application_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SHA_Console.SHA256 sha256Object;

        public MainWindow()
        {
            InitializeComponent();
            sha256Object = new SHA_Console.SHA256();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                Nullable<bool> res = dialog.ShowDialog();
                if (res == true)
                {
                    txtFileName.Text = dialog.FileName;
                    txtMessage.Text = File.ReadAllText(txtFileName.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dialog.ShowDialog();
                if (result == true)
                {
                    StreamWriter sw = new StreamWriter(dialog.FileName);
                    sw.Write(txtMessage.Text);
                    sw.Close();
                    txtFileName.Text = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCreateDigest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string digest;
                if (txtFileName.Text != string.Empty)
                    digest = sha256Object.GetMessageDigestFromFile(txtFileName.Text);
                else
                {
                    //btnSaveMessage_Click(null, null);
                    digest = sha256Object.GetMessageDigest(txtMessage.Text);
                    MessageBox.Show(sha256Object.GetMessageDigestFromString(txtMessage.Text).ToString());
                }
                txtDigest.Text = digest;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveDigest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filename = txtFileName.Text;
                DateTime dateModified = File.GetLastWriteTime(filename);
                string content = filename + ";" + dateModified.ToString() + ";" + txtDigest.Text;
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dialog.ShowDialog();
                if (result == true)
                {
                    StreamWriter sw = new StreamWriter(dialog.FileName);
                    sw.Write(content);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBrowseFile1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                Nullable<bool> res = dialog.ShowDialog();
                if (res == true)
                {
                    txtFileName1.Text = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBrowseFile2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                Nullable<bool> res = dialog.ShowDialog();
                if (res == true)
                {
                    txtFileName2.Text = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtFileName1.Text != string.Empty)
                    txtDigestFile1.Text = sha256Object.GetMessageDigestFromFile(txtFileName1.Text);
                else
                    throw new Exception("File Name 1 must not be empty!");
                if (txtFileName2.Text != string.Empty)
                    txtDigestFile2.Text = sha256Object.GetMessageDigestFromFile(txtFileName2.Text);
                else
                    throw new Exception("File Name 2 must not be empty!");
                if (txtDigestFile1.Text == txtDigestFile2.Text)
                    imgResult.Source = new BitmapImage(new Uri(@"Assets\valid.png",UriKind.Relative));
                else
                    imgResult.Source = new BitmapImage(new Uri(@"Assets\invalid.png",UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
