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
using iText.Kernel.Geom;
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
        private string glOrdner = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HallöHexCode(object sender, RoutedEventArgs e)
        {
            PrintChars("Hallö");
        }
        
        void PrintChars(string s)
        {
            //Ausgabe.Text = ($"\"{s}\".Length = {s.Length}");
            Ausgabe.Text = s +".Length =" +s.Length.ToString();
            for (int i = 0; i < s.Length; i++)
            {
                //Ausgabe.Text += ($"s[{i}] = '{s[i]}' ('\\u{(int)s[i]:x4}')");
                //Ausgabe.Text +="\n" + s[i] + $"{(int)s[i]:x4}";
                Ausgabe.Text += " " + s[i] + ((int)s[i]).ToString("X4");
            }          
        }
        void PrintChars(string s1, byte[] bb)
        {
            Ausgabe.FontSize = 30;
            Ausgabe.FontFamily = new System.Windows.Media.FontFamily("Courier New");
            //Ausgabe.Text = ($"\"{s}\".Length = {s.Length}");
            Ausgabe.Text = "ZeichenAnz:" +s1.Length.ToString() +"\n";
            Ausgabe.Text += "ByteAnz:" + bb.Length.ToString() + "\n";

            
            for(int ii=0;ii<bb.Length;ii++)
            {
                Ausgabe.Text += s1[ii] + "  ";
                Ausgabe.Text += bb[ii].ToString("X4") +"\n"; 
            }
            
            /*
            for (int i = 0; i < s1.Length; i++)
            {
                //Ausgabe.Text += ($"s[{i}] = '{s[i]}' ('\\u{(int)s[i]:x4}')");
                //Ausgabe.Text +="\n" + s[i] + $"{(int)s[i]:x4}";
                Ausgabe.Text += " " + s1[i] + ((int)s1[i]).ToString("X4") + " " +bb[2*i+1].ToString("X2") +"\n";
            } */         
        }
        private void untitled1Ausgeben(object sender, RoutedEventArgs e)
        {
            String fn= "C:\\C#\\T1\\T1\\Untitled 1.txt";
           // string tx = File.ReadAllText(fn, Encoding.UTF8);


            Ausgabe.Text = File.ReadAllText(fn);

        }
        private void untitled1InBuchPDF(object sender, RoutedEventArgs e)
        {

            //String fn= "C:\\C#\\T1\\T1\\Untitled 1.txt";
            String fn = glOrdner + "\\Untitled 1.txt";
            //string tx = File.ReadAllText(fn);

            if (!File.Exists(fn))
            {
                Ausgabe.Text += fn +" exisitiert nicht";
                return;
            }

            string tx = File.ReadAllText(fn, Encoding.GetEncoding("iso-8859-1"));

            string tx2 = "";
            int letztes = 10;
            foreach (char c in tx)
            {
                int aktcode = (int)c;
                if (aktcode == 10)
                    if (letztes == 10)
                        tx2+=c;
                    else
                        tx2 +=" ";
                else
                    tx2 += c;

                letztes =aktcode;
            }            

            ImageData data;
            //string pdfPath = "C:\\C#\\T1\\T1\\Buch.pdf";
            string pdfPath = glOrdner +"\\Buch.pdf";
            if (File.Exists(pdfPath))
            {
                Ausgabe.Text += "\n" + pdfPath + " exisitiert bereits";
                return;
            }
            using (PdfWriter writer = new PdfWriter(pdfPath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    iText.Layout.Document document = new iText.Layout.Document(pdf);
                    //document.Add(new iText.Layout.Element.Paragraph("Hello, World!"));
                    document.Add(new iText.Layout.Element.Paragraph(tx2));
                    document.Close();
                    Ausgabe.Text += "\n" + pdfPath + " wurde erstellt";
                }
            }


        }
        private void untitledHexCode(object sender, RoutedEventArgs e)
        {
            String fn= "C:\\C#\\T1\\T1\\Untitled 1.txt";
            string tx = File.ReadAllText(fn);
            byte[] bb =File.ReadAllBytes(fn);

            PrintChars(tx);

        }
        private void CharByteWert(object sender, RoutedEventArgs e)
        {
            String fn = "C:\\C#\\T1\\T1\\tx1.txt";
            byte[] bb = File.ReadAllBytes(fn);
            //string tx = File.ReadAllText(fn);
            string tx= File.ReadAllText(fn, Encoding.GetEncoding("iso-8859-1"));
            PrintChars(tx,bb);
        }
        
        private void Empty(object sender, RoutedEventArgs e)
        {
            Message1("noch manuell ausführem");
        }
        private void AllJPGsInPDF(object sender, RoutedEventArgs e)
        {  // Liste .JPGs aus Ordner,Sort,in PDF-Datei
            Ausgabe.Text = "";
            List<String> Dateiliste = new List<string>();
            List<int> DateilisteNum = new List<int>();

            var folderBrowser = new FolderBrowserDialog();

            var result = folderBrowser.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                Ausgabe.Text += "Kein Directory ausgewählt";
                return;
            }
            
            var ordnerInfo = new DirectoryInfo(folderBrowser.SelectedPath);

            if (!ordnerInfo.Exists)
            {
                Ausgabe.Text += "Keine ordnerInfo!";
                return;
            }
            
            foreach (var dateiInfo in ordnerInfo.GetFiles())
            {
                if (dateiInfo.Extension == ".jpg")
                {
                    string s1 = dateiInfo.Name;
                    string s2 = s1.Substring(0, 2);
                    s1 = s1.Substring(2, s1.IndexOf(".jpg")-2);
                    if (s2 != "PH")
                        continue;

                    Dateiliste.Add(s1);

                    int xx = 0;
                    if (!Int32.TryParse(s1, out xx))
                        Message1("keine Nummer");
                    else
                        DateilisteNum.Add(xx);
                }                
            }
            if(DateilisteNum.Count == 0)
            {
                Ausgabe.Text += "Keine PH*.JPG-Datei im Directory";
                    return;
            }

            string pdfPath = ordnerInfo.FullName + "\\AllImg.pdf";

            if (File.Exists(pdfPath)) {
                Ausgabe.Text += "AllImg.pdf exisitiert bereits";
                return;
            }

            DateilisteNum.Sort();

            glOrdner= ordnerInfo.FullName;

            ImageData data;
            
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
            Ausgabe.Text += DateilisteNum.Count.ToString() + " JPG-Dateien in AllImg.pdf Kopiert";
        }
        private void Message1(string messageBoxText)
        {
            string caption = "Kovertierung";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        private void CreateSimplePDF(object sender, RoutedEventArgs e)
        { //macht Pdf aus einem Text und 2 Photos
            string pdfPath = "C:\\C#\\T1\\MyPDF.pdf";
            using (PdfWriter writer = new PdfWriter(pdfPath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    iText.Layout.Document document = new iText.Layout.Document(pdf);
                    document.Add(new iText.Layout.Element.Paragraph("Hello, World!"));

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