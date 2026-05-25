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
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            ListBox1.Items.Clear();
            ListBox2.Items.Clear();
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
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

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
        //private List<string> GetLargeFiles(string directoryPath, long minSize)
        //{
        //    File.WriteAllText("Log_image_compression.txt", string.Empty); //Открытие файла и полностью его очистить. Если файла нет, то создаст
        //    //List<string> largeFiles = new List<string>();
        //    int i = 0;
        //    if (Directory.Exists(directoryPath))
        //    {
        //        // Получаем все файлы в папке и подкаталогах
        //        string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

        //        foreach (var file in files)
        //        {
        //            FileInfo fileInfo = new FileInfo(file);
        //            if (fileInfo.Length > minSize)
        //            {
        //                i++;
        //                largeFiles.Add(file);  // Добавляем файл в список, если его размер больше 800 КБ
        //                ListBox1.Items.Add($"{i}: {fileInfo.FullName} ({fileInfo.Length / (1024.0):F2} КБ);");
        //                File.AppendAllText("Log_image_compression.txt", "Найденные файлы:" + "\r\n");
        //                File.AppendAllText("Log_image_compression.txt", $"{i}: {fileInfo.FullName} ({fileInfo.Length / (1024.0):F2} КБ);" + "\r\n");
        //                //ListBox1.Items.Add($"Размер - {fileInfo.Length / (1024.0):F2} КБ");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Директория не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }

        //    return largeFiles;
        //}
        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    string path = @"pdfium.dll"; //Расположение файла с указанием папок бекапа
        //    bool fileExist = File.Exists(path); //Запись пути нахождение файла с указанием папок бекапа
        //    if (!fileExist)
        //    {
        //        MessageBox.Show("Файла: pdfium.dll нет в папке с программой");
        //        return;
        //    }
        //    if (string.IsNullOrWhiteSpace(TextBox1.Text))
        //    {
        //        MessageBox.Show("Сначала выберите папку!");
        //        return;
        //    }

        //    string folderPath = TextBox1.Text;

        //    if (!Directory.Exists(folderPath))
        //    {
        //        MessageBox.Show("Указанная папка не существует!");
        //        return;
        //    }

        //    if (isch == 1)
        //    {
        //        MessageBox.Show("Сначала выберите папку!");
        //        return;
        //    }
        //    // Очищаем ListBox перед началом работы
        //    isch = 1;
        //    ListBox1.Items.Clear();

        //    // Сжимаем изображения в указанной папке
        //    CompressImagesInFolder(folderPath, ListBox1);

        //    //MessageBox.Show("Сжатие завершено!");
        //}

        //private async void CompressImagesInFolder(string folderPath, ListBox outputListBox)
        //{
        //    Button1.IsEnabled = false;
        //    Button2.IsEnabled = false;
        //    int i = 0;
        //    string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

        //    await Task.Run(() =>
        //    {
        //        //MessageBox.Show("0.1");
        //        foreach (string filePath in files)
        //        {
        //            //MessageBox.Show("0.2");
        //            FileInfo fileInfo = new FileInfo(filePath);
        //            long originalSize = fileInfo.Length;
        //            //MessageBox.Show("0.3");
        //            // Пропуск файлов меньше 800 КБ
        //            if (originalSize <= 800 * 1024)
        //                continue;
        //            //MessageBox.Show("0.4");
        //            try
        //            {
        //                //bool isImage = false;
        //                bool isPdf = false;
        //                //MessageBox.Show("0.5");
        //                //MessageBox.Show($"1122221Файл: {filePath}");
        //                // Проверка для PDF
        //                try
        //                {
        //                    //MessageBox.Show("3");
        //                    // Чтение первых 4 байтов файла
        //                    byte[] header = new byte[4];
        //                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //                    {
        //                        int bytesRead = fs.Read(header, 0, 4); // Читаем первые 4 байта
        //                        if (bytesRead < 4)
        //                        {
        //                            //MessageBox.Show("Файл слишком маленький для проверки на PDF.");
        //                            return;
        //                        }
        //                    }
        //                    //MessageBox.Show("4");
        //                    // Преобразуем байты в строку (ASCII)
        //                    string headerStr = Encoding.ASCII.GetString(header);

        //                    // Диагностика: выводим байты и строку
        //                    string byteRepresentation = string.Join(" ", header.Select(b => $"0x{b:X2}"));
        //                    //MessageBox.Show($"Первые байты: {byteRepresentation}");
        //                    //MessageBox.Show($"Строка из байтов: {headerStr}");

        //                    // Проверка на PDF
        //                    if (headerStr.StartsWith("%PDF"))
        //                    {
        //                        isPdf = true;
        //                        //Application.Current.Dispatcher.Invoke(() =>
        //                        //{
        //                        //    outputListBox.Items.Add($"Это PDF файл: {filePath}");
        //                        //});
        //                    }
        //                    else
        //                    {
        //                        //Application.Current.Dispatcher.Invoke(() =>
        //                        //{
        //                        //    outputListBox.Items.Add($"Это не PDF. Начало файла: {headerStr}");
        //                        //});
        //                    }
        //                    //MessageBox.Show("5");
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show("Ошибка");
        //                    Application.Current.Dispatcher.Invoke(() =>
        //                    {
        //                        //outputListBox.Items.Add($"Ошибка при проверке файла: {ex.Message}. Файл: {filePath}");
        //                    });
        //                }
        //                //MessageBox.Show("Конец");
        //                if (isPdf) // Проверка, является ли файл PDF
        //                {
        //                    try
        //                    {
        //                        // 1. Получаем путь к папке, где находится исходный файл
        //                        string fileDirectory = Path.GetDirectoryName(filePath);

        //                        // 2. Создание временной папки для хранения изображений
        //                        string tempFolder = Path.Combine(fileDirectory, "TempImages"); // Папка для изображений рядом с исходным файлом
        //                        Directory.CreateDirectory(tempFolder);  // Создаем папку, если ее нет

        //                        // 3. Конвертация страниц PDF в изображения
        //                        List<string> imagePaths = new List<string>();  // Список для хранения путей к изображениям
        //                        using (var pdfDoc = PdfiumViewer.PdfDocument.Load(filePath))  // Загружаем PDF документ с помощью библиотеки Pdfium
        //                        {
        //                            // Проходим по каждой странице в PDF
        //                            for (int pageIndex = 0; pageIndex < pdfDoc.PageCount; pageIndex++)
        //                            {
        //                                // Формируем путь для сохранения изображения страницы
        //                                string outputImagePath = Path.Combine(tempFolder, $"Page_{pageIndex + 1}.jpg");

        //                                // Рендерим страницу в изображение с разрешением 300 DPI
        //                                using (var bitmap = pdfDoc.Render(pageIndex, 1200, 1200, true)) // Отрисовка страницы в Bitmap (картинка)
        //                                {
        //                                    bitmap.Save(outputImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);  // Сохраняем картинку как JPEG
        //                                    imagePaths.Add(outputImagePath);  // Добавляем путь изображения в список
        //                                }
        //                            }
        //                        }

        //                        // 4. Сжатие изображений
        //                        foreach (var imagePath in imagePaths)  // Для каждого изображения в списке
        //                        {
        //                            using (var image = SixLabors.ImageSharp.Image.Load(imagePath))  // Загружаем изображение с помощью библиотеки ImageSharp
        //                            {
        //                                // Изменяем размер изображения до 800x800 пикселей с сохранением пропорций
        //                                image.Mutate(x => x.Resize(new SixLabors.ImageSharp.Processing.ResizeOptions
        //                                {
        //                                    Mode = SixLabors.ImageSharp.Processing.ResizeMode.Max,  // Максимальное изменение размера
        //                                    Size = new SixLabors.ImageSharp.Size(800, 800)  // Устанавливаем максимальный размер 800x800
        //                                }));

        //                                // Применяем сильное сжатие (качество 70%)
        //                                var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder { Quality = 90 }; // Настройка сжатия JPEG
        //                                image.Save(imagePath, encoder);  // Перезаписываем изображение с сжатием
        //                            }
        //                        }

        //                        // 5. Сборка изображений обратно в PDF
        //                        string compressedPdfPath = Path.Combine(fileDirectory, "Compressed_" + Path.GetFileName(filePath));  // Путь для сжатого PDF
        //                        using (var writer = new iText.Kernel.Pdf.PdfWriter(compressedPdfPath))  // Открываем новый файл PDF для записи
        //                        using (var pdfDoc = new iText.Kernel.Pdf.PdfDocument(writer))  // Создаем новый PDF документ
        //                        {
        //                            foreach (var imagePath in imagePaths)  // Для каждого изображения в списке
        //                            {
        //                                byte[] imageBytes = File.ReadAllBytes(imagePath);  // Читаем данные изображения в байтовый массив
        //                                var imageData = iText.IO.Image.ImageDataFactory.Create(imageBytes);  // Создаем объект изображения для PDF

        //                                var pageSize = new iText.Kernel.Geom.PageSize(imageData.GetWidth(), imageData.GetHeight());  // Создаем страницу с размерами, соответствующими изображению
        //                                var page = pdfDoc.AddNewPage(pageSize);  // Добавляем новую страницу с нужными размерами

        //                                var pdfCanvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);  // Получаем холст для рисования на странице
        //                                pdfCanvas.AddImage(imageData, 0, 0, false);  // Добавляем изображение на страницу
        //                            }
        //                        }

        //                        // 6. Проверка на уменьшение размера PDF
        //                        long compressedPdfSize = new FileInfo(compressedPdfPath).Length;  // Получаем размер сжатого PDF
        //                        long originalPdfSize = new FileInfo(filePath).Length;  // Получаем размер оригинального PDF

        //                        if (compressedPdfSize < originalPdfSize)  // Если размер сжатого PDF меньше оригинала
        //                        {
        //                            File.Copy(compressedPdfPath, filePath, true);  // Заменяем оригинальный файл на сжатый
        //                            Application.Current.Dispatcher.Invoke(() =>
        //                            {
        //                                i++;  // Увеличиваем счетчик
        //                                outputListBox.Items.Add($"{i}: {filePath}");  // Добавляем информацию о процессе в ListBox
        //                                outputListBox.Items.Add($"Размер до: {originalPdfSize / 1024} КБ");  // Показываем размер до сжатия
        //                                outputListBox.Items.Add($"Размер после: {compressedPdfSize / 1024} КБ");  // Показываем размер после сжатия
        //                                outputListBox.Items.Add("---------------------------------------------------");  // Разделительная линия
        //                                File.AppendAllText("Log_image_compression.txt", $"{i}: {filePath}" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", $"Размер до: {originalPdfSize / 1024} КБ" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", $"Размер после: {compressedPdfSize / 1024} КБ" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", "---------------------------------------------------" + "\r\n");
        //                            });
        //                        }

        //                        Directory.Delete(tempFolder, true);
        //                        File.Delete(filePath);
        //                        File.Move(compressedPdfPath, filePath);

        //                    }
        //                    catch (Exception ex)  // Обработка ошибок
        //                    {
        //                        Application.Current.Dispatcher.Invoke(() =>
        //                        {
        //                            outputListBox.Items.Add($"Ошибка при обработке PDF: {ex.Message}. Файл: {filePath}");  // Выводим сообщение об ошибке
        //                            File.AppendAllText("Log_image_compression.txt", $"Ошибка при обработке PDF: {ex.Message}. Файл: {filePath}");
        //                        });
        //                    }
        //                }


        //                else
        //                {
        //                    i++;
        //                    string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));

        //                    using (var image = Image.Load(filePath))
        //                    {
        //                        // Определяем формат изображения
        //                        var format = SixLabors.ImageSharp.Image.DetectFormat(filePath);

        //                        if (format == null)
        //                            continue;

        //                        // Сжатие для формата JPEG
        //                        if (format.DefaultMimeType == "image/jpeg")
        //                        {
        //                            var encoder = new JpegEncoder() { Quality = 5 };
        //                            image.Save(tempFilePath, encoder);
        //                        }
        //                        // Сжатие для формата PNG
        //                        else if (format.DefaultMimeType == "image/png")
        //                        {
        //                            var encoder = new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression };
        //                            image.Save(tempFilePath, encoder);
        //                        }
        //                        // Конвертация BMP в PNG
        //                        else if (format.DefaultMimeType == "image/bmp")
        //                        {
        //                            var encoder = new JpegEncoder()
        //                            {
        //                                Quality = 5 // Уровень качества JPEG
        //                            };

        //                            // Временный файл для сжатия
        //                            string bmpTempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));
        //                            image.Save(bmpTempFilePath, encoder);

        //                            long bmpCompressedSize = new FileInfo(bmpTempFilePath).Length; // Переименовано для исключения конфликта

        //                            Application.Current.Dispatcher.Invoke(() =>
        //                            {
        //                                outputListBox.Items.Add($"{i}: {filePath}");
        //                                outputListBox.Items.Add($"Размер до: {originalSize / 1024} КБ");
        //                                outputListBox.Items.Add($"Размер после: {bmpCompressedSize / 1024} КБ");
        //                                outputListBox.Items.Add("---------------------------------------------------");
        //                                File.AppendAllText("Log_image_compression.txt", $"{i}: {filePath}" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", $"Размер до: {originalSize / 1024} КБ" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", $"Размер после: {bmpCompressedSize / 1024} КБ" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", "---------------------------------------------------" + "\r\n");
        //                            });

        //                            // Заменяем оригинальный BMP файл на сжатый JPEG
        //                            File.Copy(bmpTempFilePath, filePath, true); // true позволяет перезаписать файл
        //                            File.Delete(bmpTempFilePath); // Удаляем временный файл
        //                            continue;
        //                        }
        //                        else if (format.DefaultMimeType == "image/tiff")
        //                        {
        //                            var encoder = new JpegEncoder()
        //                            {
        //                                Quality = 70 // Уровень качества JPEG
        //                            };

        //                            // Временный файл для сжатия
        //                            string bmpTempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));
        //                            image.Save(bmpTempFilePath, encoder);

        //                            long bmpCompressedSize = new FileInfo(bmpTempFilePath).Length; // Переименовано для исключения конфликта

        //                            Application.Current.Dispatcher.Invoke(() =>
        //                            {
        //                                outputListBox.Items.Add($"{i}: {filePath}");
        //                                outputListBox.Items.Add($"Размер до: {originalSize / 1024} КБ");
        //                                outputListBox.Items.Add($"Размер после: {bmpCompressedSize / 1024} КБ");
        //                                outputListBox.Items.Add("---------------------------------------------------");
        //                                File.AppendAllText("Log_image_compression.txt", $"{i}: {filePath}" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", $"Размер до: {originalSize / 1024} КБ" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", $"Размер после: {bmpCompressedSize / 1024} КБ" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", "---------------------------------------------------" + "\r\n");
        //                            });

        //                            // Заменяем оригинальный BMP файл на сжатый JPEG
        //                            File.Copy(bmpTempFilePath, filePath, true); // true позволяет перезаписать файл
        //                            File.Delete(bmpTempFilePath); // Удаляем временный файл
        //                            continue;
        //                        }





        //                        else
        //                            continue;

        //                        long compressedSize = new FileInfo(tempFilePath).Length;

        //                        if (compressedSize < originalSize)
        //                        {
        //                            File.Copy(tempFilePath, filePath, true); // Заменяем оригинал на сжатый
        //                            Application.Current.Dispatcher.Invoke(() =>
        //                            {
        //                                outputListBox.Items.Add($"{i}: {filePath}");
        //                                outputListBox.Items.Add($"Размер до: {originalSize / 1024} КБ");
        //                                outputListBox.Items.Add($"Размер после: {compressedSize / 1024} КБ");
        //                                File.AppendAllText("Log_image_compression.txt", $"{i}: {filePath}" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", $"Размер до: {originalSize / 1024} КБ" + "\r\n");
        //                                File.AppendAllText("Log_image_compression.txt", $"Размер после: {compressedSize / 1024} КБ" + "\r\n");

        //                            });
        //                        }

        //                        Application.Current.Dispatcher.Invoke(() =>
        //                        {
        //                            outputListBox.Items.Add("---------------------------------------------------");
        //                            File.AppendAllText("Log_image_compression.txt", "---------------------------------------------------" + "\r\n");
        //                        });
        //                    }

        //                }


        //            }


        //            catch (Exception ex)
        //            {
        //                //Application.Current.Dispatcher.Invoke(() =>
        //                //{
        //                //    outputListBox.Items.Add($"{i}: Ошибка обработки файла: {filePath}");
        //                //    outputListBox.Items.Add($"Ошибка: {ex.Message}");
        //                //    outputListBox.Items.Add("---------------------------------------------------");
        //                //});
        //            }
        //        }
        //    });

        //    MessageBox.Show($"Всего обработалось - {i}");
        //    File.AppendAllText("Log_image_compression.txt", $"Всего обработалось - {i}" + "\r\n");
        //    Button1.IsEnabled = true;
        //    Button2.IsEnabled = false;
        //}

    }


}
