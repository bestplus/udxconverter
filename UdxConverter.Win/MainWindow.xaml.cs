using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using UdxConverter.Win.Model;

namespace UdxConverter.Win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Regex _vcardInfoRegex = new Regex("<vCardInfo>.*?</vCardInfo>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private Regex _nameRegex = new Regex("<N>(.*?)</N>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private Regex _phoneRegex = new Regex("<TEL>(.*?)</TEL>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private string _vCardPattern = @"BEGIN:VCARD
VERSION:2.1
N;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:{%NAME%}.
TEL;CELL:{%PHONE%}
END:VCARD";

        private OpenFilePageModel _openFilePageModel = new OpenFilePageModel();

        public MainWindow()
        {
            InitializeComponent();

            OpenFilePage.DataContext = _openFilePageModel;
        }

        private void btnSelectUdx_Click_1(object sender, RoutedEventArgs e)
        {
            var fileOpenDialog = new OpenFileDialog();
            fileOpenDialog.DefaultExt = "*.udx";
            fileOpenDialog.Filter = "Philips phone contacts file (.udx)|*.udx";

            if (fileOpenDialog.ShowDialog() == true)
            {
                _openFilePageModel.FileName = fileOpenDialog.FileName;

                FillPhoneNumbers();
                OpenFilePage.CanSelectNextPage = true;
            }
        }

        private void FillPhoneNumbers()
        {
            _openFilePageModel.Phones.Clear();

            var fileContent = File.ReadAllText(_openFilePageModel.FileName);

            foreach (Match data in _vcardInfoRegex.Matches(fileContent))
            {
                _openFilePageModel.Phones.Add(new PhoneNumber
                {
                    Name = _nameRegex.Match(data.Value).Groups[1].Value.TrimStart(';'),
                    Number = _phoneRegex.Match(data.Value).Groups[1].Value
                });
            }
        }

        private void btnConvert_Click_1(object sender, RoutedEventArgs e)
        {
            var openDirectoryDialog = new VistaFolderBrowserDialog();
            if (openDirectoryDialog.ShowDialog() == true)
            {
                CreateVCardFiles(openDirectoryDialog.SelectedPath);

                btnConvert.Visibility = System.Windows.Visibility.Hidden;
                txtConvertationFinished.Visibility = System.Windows.Visibility.Visible;
                pageConvert.CanFinish = true;
            }
        }

        private void CreateVCardFiles(string vcardOutputDirectory)
        {
            foreach (var phone in _openFilePageModel.Phones)
            {
                var output = _vCardPattern;
                output = output.Replace("{%NAME%}", phone.Name);
                output = output.Replace("{%PHONE%}", phone.Number);

                File.WriteAllText(Path.Combine(vcardOutputDirectory, phone.Name + ".vcf"), output);
            }
        }
    }
}
