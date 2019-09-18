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
using Microsoft.Win32;
using System.Diagnostics;

namespace Demo
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Wikipedia:Database_download
    /// https://dumps.wikimedia.org/enwiki/latest/enwiki-latest-pages-articles.xml.bz2
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime startTime;
        int redirectsCount = 0;
        int pageCount = 0;
        int articleCount = 0;
        int lastCount = 0;

        CancellationTokenSource cancelTokenSource;

        public MainWindow()
        {
            InitializeComponent();
            panelControls.Visibility = Visibility.Visible;
            panelProgress.Visibility = Visibility.Hidden;
        }

        private async Task ProcessDump(Stream inputStream)
        {
            cancelTokenSource?.Cancel();
            cancelTokenSource = new CancellationTokenSource();

            panelControls.Visibility = Visibility.Hidden;
            panelProgress.Visibility = Visibility.Visible;

            try
            {
                using (BZip2InputStream stream = new BZip2InputStream(inputStream))
                {
                    Parser parser = Parser.Create(stream);

                    CancellationToken token = cancelTokenSource.Token;

                    await Task.Run(() =>
                    {
                        startTime = DateTime.UtcNow;
                        redirectsCount = 0;
                        pageCount = 0;
                        articleCount = 0;
                        lastCount = 0;

                        foreach (var page in parser.ReadPages())
                        {
                            token.ThrowIfCancellationRequested();

                            pageCount++;

                            if (page.IsRedirect)
                            {
                                redirectsCount++;
                            }
                            else
                            {
                                if (page.Namespace == 0)
                                    articleCount++;
                            }

                            // update status every 100 articles
                            if (pageCount % 100 == 0)
                            {
                                Dispatcher.BeginInvoke(() => { UpdateStatus(); });
                            }
                        }
                    }, token);

                    UpdateStatus();
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                panelControls.Visibility = Visibility.Visible;
                panelProgress.Visibility = Visibility.Hidden;
            }
        }

        private void UpdateStatus()
        {
            txtProgress.Text = $"Pages: {pageCount}";
        }

        private async void BtnParse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "latest articles bz2|*.bz2",
            };

            if (dlg.ShowDialog() != true)
            {
                return;
            }

            using (Stream fs = dlg.OpenFile())
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource?.Cancel();
        }
    }

}
