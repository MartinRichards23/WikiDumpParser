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
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Threading;
using System.Windows.Threading;
using WikiDumpParser.Models;
using WikiDumpParser;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.BZip2;
using System.Net;

namespace Demo
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Wikipedia:Database_download
    /// https://dumps.wikimedia.org/enwiki/latest/enwiki-latest-pages-articles.xml.bz2
    /// </summary>
    public partial class MainWindow : Window
    {
        int redirectsCount = 0;
        int articleCount = 0;
        int lastCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task ProcessDump(Stream inputStream)
        {
            try
            {
                using (BZip2InputStream stream = new BZip2InputStream(inputStream))
                {
                    Parser parser = Parser.Create(stream);

                    await Task.Run(() =>
                    {
                        foreach (Article article in parser.ReadArticles())
                        {
                            if (article.IsRedirect)
                            {
                                redirectsCount++;
                            }
                            else if (article.Namespace == 0)
                            {
                                articleCount++;
                                lastCount++;
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }

        private async void BtnParse_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = File.OpenRead(@"C:\Users\marti\Downloads\enwiki-latest-pages-articles.xml.bz2"))
            {
                await ProcessDump(fs);
            }
        }

        private async void BtnDownloadFile_Click(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://dumps.wikimedia.org/enwiki/latest/enwiki-latest-pages-articles.xml.bz2");
            using (HttpWebResponse response = (HttpWebResponse)(request.GetResponse()))
            using (Stream receiveStream = response.GetResponseStream())
            {
                await ProcessDump(receiveStream);
            }
        }
    }

}
