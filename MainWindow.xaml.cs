using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using iText.Kernel.Pdf;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Drawing;
using PdfReader = iText.Kernel.Pdf.PdfReader;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using SixLabors.ImageSharp.Formats.Png;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using iText.Kernel.Pdf;
//using iText.Layout;
//using iText.Layout.Element;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using System.IO;
using System.Text;
using PdfiumViewer;
using Size = SixLabors.ImageSharp.Size;
using PdfiumViewer; // Для конвертации PDF в изображения
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using System.Linq;
using System.Reflection.Metadata;
using PdfSharpCore.Pdf.Advanced;
namespace image_compression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            isch = 0;
            ListBox1.Items.Clear();
            befor_ch.Clear();
            after_ch.Clear();

            //Button2.IsEnabled = false;
        }

        List<string> befor_ch = new List<string>();
        //string[] befor_ch = [];
        List<string> after_ch = new List<string>();
        List<string> largeFiles = new List<string>();
        //string[] after_ch = [];
        int isch = 0;
        int kon = 0;

        bool blok_SelectionChanged = false; //блокировщик для выполнения сжатия
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            ListBox1.Items.Clear();
            ListBox2.Items.Clear();
            ListBox3.Items.Clear();
        }
        private void ListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //если сжатие выполняется, то не выполняется код
            if (blok_SelectionChanged == true) return;

            if (ListBox2.SelectedItem != null)
            {
                //очистка
                ListBox3.Items.Clear();
                // Получаем выбранный объект
                var folderPath = ListBox2.SelectedItem;

                string path = folderPath + "\\";
                // Ищем файлы больше 800 КБ
                var largeFiles = GetLargeFiles(path, 800 * 1024); // 800 KB в байтах

                // Если нужно сбросить выделение после обработки:
                // MyListBox.SelectedItem = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isch = 0;
            //TextBox1.Clear();
            largeFiles.Clear();
            after_ch.Clear();
            try
            {
                // Диалоговое окно настройки открытия папки
                Microsoft.Win32.OpenFolderDialog dialog = new();

                dialog.Multiselect = false;
                dialog.Title = "Select a folder";
                //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Показать диалоговое окно "Открыть папку"
                bool? result = dialog.ShowDialog();
                Console.WriteLine();
                // Обработать результаты работы диалогового окна "Открыть папку"
                if (result == true)
                {
                    //// Получить выбранную папку
                    ListBox2.Items.Add($"{dialog.FolderName}");
                }
                //string path = TextBox1.Text + "\\";
                //string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

                //// Ищем файлы больше 800 КБ
                //var largeFiles = GetLargeFiles(path, 800 * 1024); // 800 KB в байтах

            }
            catch (Exception ex)
            {
                MessageBox.Show("Не выбран путь");
            }
            //Button1.IsEnabled = false;
            //Button2.IsEnabled = true;
            File.AppendAllText("Log_image_compression.txt", "---------------------------------------------------" + "\r\n");
        }
        private List<string> GetLargeFiles(string directoryPath, long minSize)
        {
            File.WriteAllText("Log_image_compression.txt", string.Empty); //Открытие файла и полностью его очистить. Если файла нет, то создаст
            //List<string> largeFiles = new List<string>();
            int i = 0;
            if (Directory.Exists(directoryPath))
            {
                // Получаем все файлы в папке и подкаталогах
                string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (fileInfo.Length > minSize)
                    {
                        i++;
                        largeFiles.Add(file);  // Добавляем файл в список, если его размер больше 800 КБ
                        ListBox3.Items.Add($"{i}: {fileInfo.FullName} ({fileInfo.Length / (1024.0):F2} КБ);");
                        File.AppendAllText("Log_image_compression.txt", "Найденные файлы:" + "\r\n");
                        File.AppendAllText("Log_image_compression.txt", $"{i}: {fileInfo.FullName} ({fileInfo.Length / (1024.0):F2} КБ);" + "\r\n");
                        //ListBox1.Items.Add($"Размер - {fileInfo.Length / (1024.0):F2} КБ");
                    }
                }
            }
            else
            {
                MessageBox.Show("Директория не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return largeFiles;
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string path = @"pdfium.dll";

            if (!File.Exists(path))
            {
                MessageBox.Show("Файла pdfium.dll нет");
                return;
            }

            if (ListBox2.Items.Count == 0)
            {
                MessageBox.Show("Список путей пуст");
                return;
            }

            Button1.IsEnabled = false;

            blok_SelectionChanged = true;

            kon = 0;

            try
            {
                for (int i = 0; i < ListBox2.Items.Count; i++)
                {
                    ListBox2.SelectedIndex = i;
                    ListBox2.ScrollIntoView(ListBox2.Items[i]);

                    string folderPath = ListBox2.Items[i].ToString();

                    ListBox3.Items.Clear();

                    largeFiles.Clear();

                    GetLargeFiles(folderPath, 800 * 1024);

                    // ОБРАБОТКА ФАЙЛОВ ПО ОЧЕРЕДИ
                    foreach (string filePath in largeFiles)
                    {
                        await CompressImagesInFolder(filePath, ListBox1);
                    }

                    ListBox1.Items.Add(
                        "=================================");
                }

                MessageBox.Show($"Всего обработано: {kon}");
            }
            finally
            {
                Button1.IsEnabled = true;
                blok_SelectionChanged = false;
            }
        }

        private async Task CompressImagesInFolder(string filePath, ListBox outputListBox)
        {
            await Task.Run(() =>
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    if (!fileInfo.Exists)
                        return;

                    long originalSize = fileInfo.Length;

                    // Пропуск маленьких файлов
                    if (originalSize <= 800 * 1024)
                        return;

                    bool isPdf = false;

                    // =========================
                    // Проверка PDF
                    // =========================
                    try
                    {
                        byte[] header = new byte[4];

                        using (FileStream fs = new FileStream(
                            filePath,
                            FileMode.Open,
                            FileAccess.Read))
                        {
                            int bytesRead = fs.Read(header, 0, 4);

                            if (bytesRead == 4)
                            {
                                string headerStr =
                                    Encoding.ASCII.GetString(header);

                                if (headerStr.StartsWith("%PDF"))
                                {
                                    isPdf = true;
                                }
                            }
                        }
                    }
                    catch
                    {
                        return;
                    }

                    // =====================================================
                    // PDF
                    // =====================================================
                    if (isPdf)
                    {
                        try
                        {
                            string fileDirectory =
                                Path.GetDirectoryName(filePath);

                            string tempFolder =
                                Path.Combine(
                                    fileDirectory,
                                    "TempImages_" + Guid.NewGuid());

                            Directory.CreateDirectory(tempFolder);

                            List<string> imagePaths =
                                new List<string>();

                            // ======================================
                            // PDF -> JPG
                            // ======================================
                            using (var pdfDoc =
                                   PdfiumViewer.PdfDocument.Load(filePath))
                            {
                                for (int pageIndex = 0;
                                     pageIndex < pdfDoc.PageCount;
                                     pageIndex++)
                                {
                                    string outputImagePath =
                                        Path.Combine(
                                            tempFolder,
                                            $"Page_{pageIndex + 1}.jpg");

                                    using (var bitmap =
                                           pdfDoc.Render(
                                               pageIndex,
                                               1200,
                                               1200,
                                               true))
                                    {
                                        bitmap.Save(
                                            outputImagePath,
                                            System.Drawing.Imaging
                                                .ImageFormat.Jpeg);

                                        imagePaths.Add(outputImagePath);
                                    }
                                }
                            }

                            // ======================================
                            // Сжатие JPG
                            // ======================================
                            foreach (string imagePath in imagePaths)
                            {
                                using (var image =
                                       SixLabors.ImageSharp.Image.Load(imagePath))
                                {
                                    image.Mutate(x =>
                                        x.Resize(
                                            new ResizeOptions
                                            {
                                                Mode = SixLabors.ImageSharp.Processing.ResizeMode.Stretch,
                                                Size = new Size(800, 800)
                                            }));

                                    var encoder =
                                        new JpegEncoder()
                                        {
                                            Quality = 90
                                        };

                                    image.Save(imagePath, encoder);
                                }
                            }

                            // ======================================
                            // Сборка PDF
                            // ======================================
                            string compressedPdfPath =
                                Path.Combine(
                                    fileDirectory,
                                    "Compressed_" +
                                    Path.GetFileName(filePath));

                            using (var writer =
                                   new PdfWriter(compressedPdfPath))

                            using (var pdfDoc =
                                   new PdfDocument(writer))
                            {
                                foreach (string imagePath in imagePaths)
                                {
                                    byte[] imageBytes =
                                        File.ReadAllBytes(imagePath);

                                    var imageData =
                                        iText.IO.Image.ImageDataFactory
                                            .Create(imageBytes);

                                    var pageSize =
                                        new iText.Kernel.Geom.PageSize(
                                            imageData.GetWidth(),
                                            imageData.GetHeight());

                                    var page =
                                        pdfDoc.AddNewPage(pageSize);

                                    var pdfCanvas =
                                        new PdfCanvas(page);

                                    pdfCanvas.AddImage(
                                        imageData,
                                        0,
                                        0,
                                        false);
                                }
                            }

                            long compressedPdfSize =
                                new FileInfo(compressedPdfPath).Length;

                            // ======================================
                            // Замена файла
                            // ======================================
                            if (compressedPdfSize < originalSize)
                            {
                                File.Delete(filePath);

                                File.Move(
                                    compressedPdfPath,
                                    filePath);

                                int currentNumber =
                                    Interlocked.Increment(ref kon);


                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    
                                    outputListBox.Items.Add($"{currentNumber}:{currentNumber}. {filePath}");

                                    outputListBox.Items.Add($"Размер до: {originalSize / 1024} КБ");

                                    outputListBox.Items.Add($"Размер после: {compressedPdfSize / 1024} КБ");

                                    outputListBox.Items.Add("---------------------------------------------------");
                                });
                            }
                            else
                            {
                                File.Delete(compressedPdfPath);
                            }

                            // Удаление temp
                            if (Directory.Exists(tempFolder))
                            {
                                Directory.Delete(tempFolder, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                outputListBox.Items.Add(
                                    $"Ошибка PDF:");

                                outputListBox.Items.Add(filePath);

                                outputListBox.Items.Add(ex.Message);

                                outputListBox.Items.Add("---------------------------------------------------");
                            });
                        }
                    }

                    // =====================================================
                    // IMAGE
                    // =====================================================
                    else
                    {
                        try
                        {
                            string tempFilePath =
                                Path.Combine(
                                    Path.GetTempPath(),
                                    Guid.NewGuid() +
                                    Path.GetExtension(filePath));

                            using (var image = Image.Load(filePath))
                            {
                                var format =
                                    Image.DetectFormat(filePath);

                                if (format == null)
                                    return;

                                // JPG
                                if (format.DefaultMimeType == "image/jpeg")
                                {
                                    var encoder =
                                        new JpegEncoder()
                                        {
                                            Quality = 5
                                        };

                                    image.Save(tempFilePath, encoder);
                                }

                                // PNG
                                else if (
                                    format.DefaultMimeType == "image/png")
                                {
                                    var encoder =
                                        new PngEncoder()
                                        {
                                            CompressionLevel =
                                                PngCompressionLevel
                                                    .BestCompression
                                        };

                                    image.Save(tempFilePath, encoder);
                                }

                                // BMP/TIFF
                                else if (
                                    format.DefaultMimeType == "image/bmp" ||
                                    format.DefaultMimeType == "image/tiff")
                                {
                                    var encoder =
                                        new JpegEncoder()
                                        {
                                            Quality = 70
                                        };

                                    image.Save(tempFilePath, encoder);
                                }
                                else
                                {
                                    return;
                                }
                            }

                            long compressedSize =
                                new FileInfo(tempFilePath).Length;

                            if (compressedSize < originalSize)
                            {
                                File.Copy(tempFilePath,filePath,true);

                                int currentNumber =
                                    Interlocked.Increment(ref kon);

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    outputListBox.Items.Add(@$"{currentNumber}:{filePath}");

                                    outputListBox.Items.Add(
                                        $"Размер до: {originalSize / 1024} КБ");

                                    outputListBox.Items.Add(
                                        $"Размер после: {compressedSize / 1024} КБ");

                                    outputListBox.Items.Add(
                                        "---------------------------------------------------");
                                });
                            }

                            if (File.Exists(tempFilePath))
                            {
                                File.Delete(tempFilePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                outputListBox.Items.Add(
                                    $"Ошибка изображения:");

                                outputListBox.Items.Add(filePath);

                                outputListBox.Items.Add(ex.Message);

                                outputListBox.Items.Add(
                                    "---------------------------------------------------");
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        outputListBox.Items.Add(
                            $"Глобальная ошибка:");

                        outputListBox.Items.Add(filePath);

                        outputListBox.Items.Add(ex.Message);

                        outputListBox.Items.Add(
                            "---------------------------------------------------");
                    });
                }
            });
        }

    }


}
