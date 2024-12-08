using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

/*
dotnet add package itext --version <REPLACE_WITH_DESIRED_ITEXT_VERSION>
dotnet add package itext --iText Core/Community 9.0.0
dotnet add package itext.bouncy-castle-adapter --version <REPLACE_WITH_DESIRED_ITEXT_VERSION>

dotnet add package itext.commons --version 8.0.3         HH hat funktioniert
*/

namespace PDFErstellen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Test2_Click(object sender, RoutedEventArgs e)
        {  // aus Dateinertrachter

            List<String> Dateiliste = new List<string>();
            List<int> DateilisteNum = new List<int>();

            var folderBrowser = new FolderBrowserDialog();

            var result = folderBrowser.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            
            var ordnerInfo = new DirectoryInfo(folderBrowser.SelectedPath);

            if (!ordnerInfo.Exists)
                return;
            
            foreach (var dateiInfo in ordnerInfo.GetFiles())
            {
                if (dateiInfo.Extension == ".jpg")
                {
                    string s1 = dateiInfo.Name;
                    s1 = s1.Substring(2, s1.IndexOf(".jpg")-2);
                    Dateiliste.Add(s1);

                    int xx = 0;
                    if (!Int32.TryParse(s1, out xx))
                        Message1();
                    else
                        DateilisteNum.Add(xx);
                }                
            }

            DateilisteNum.Sort();

            ImageData data;
            string pdfPath = ordnerInfo.FullName + "\\AllImg.pdf";
            //pdfPath = "C:\\C#\\T1\\MyPDF.pdf";
            using (PdfWriter writer = new PdfWriter(pdfPath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    iText.Layout.Document document = new iText.Layout.Document(pdf);
                    // document.Add(new iText.Layout.Element.Paragraph("Hello, World!"));
                    foreach (var ii in DateilisteNum)
                    {
                        string fname = ordnerInfo.FullName + "\\PH" + ii.ToString() + ".jpg";
                        data = ImageDataFactory.Create(fname);
                        iText.Layout.Element.Image image = new iText.Layout.Element.Image(data);
                        document.Add(image);
                    }
                    document.Close();
                }
            }
        }
        private void Message1()
        {
            string messageBoxText = "keine Nummer";
            string caption = "Kovertierung";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        private void Test1_Click(object sender, RoutedEventArgs e)
        {
            string pdfPath = "C:\\C#\\T1\\MyPDF.pdf";
            using (PdfWriter writer = new PdfWriter(pdfPath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    iText.Layout.Document document = new iText.Layout.Document(pdf);
                   // document.Add(new iText.Layout.Element.Paragraph("Hello, World!"));

                    ImageData data = ImageDataFactory.Create("C:\\C#\\T1\\PH0.JPG");
                    iText.Layout.Element.Image image = new iText.Layout.Element.Image(data);
                    document.Add(image);

                    data = ImageDataFactory.Create("C:\\C#\\T1\\PH1.JPG");
                    image = new iText.Layout.Element.Image(data);
                    document.Add(image);

                    document.Close();
                }
            }

           // Console.WriteLine("PDF generated successfully using iText 7.");

        }
    }
}