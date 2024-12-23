using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using Image = SixLabors.ImageSharp.Image;
using Path = System.IO.Path;

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
        }

        List<string> befor_ch = new List<string>();
        //string[] befor_ch = [];
        List<string> after_ch = new List<string>();
        List<string> largeFiles = new List<string>();
        //string[] after_ch = [];
        int isch = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isch = 0;
            ListBox1.Items.Clear();
            TextBox1.Clear();
            largeFiles.Clear();
            after_ch.Clear();
            try
            {
                // Диалоговое окно настройки открытия папки
                Microsoft.Win32.OpenFolderDialog dialog = new();

                dialog.Multiselect = false;
                dialog.Title = "Select a folder";

                // Показать диалоговое окно "Открыть папку"
                bool? result = dialog.ShowDialog();
                Console.WriteLine();
                // Обработать результаты работы диалогового окна "Открыть папку"
                if (result == true)
                {
                    //// Получить выбранную папку
                    TextBox1.Text = dialog.FolderName;
                }
                string path = TextBox1.Text + "\\";
                string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

                // Ищем файлы больше 800 КБ
                var largeFiles = GetLargeFiles(path, 800 * 1024); // 800 KB в байтах

            }
            catch (Exception ex)
            {
                MessageBox.Show("Не выбран путь");
            }
        }
        private List<string> GetLargeFiles(string directoryPath, long minSize)
        {
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
                        ListBox1.Items.Add($"{i}: {fileInfo.FullName} ({fileInfo.Length / (1024.0):F2} КБ);");
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
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox1.Text))
            {
                MessageBox.Show("Сначала выберите папку!");
                return;
            }

            string folderPath = TextBox1.Text;

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Указанная папка не существует!");
                return;
            }

            if (isch == 1)
            {
                MessageBox.Show("Сначала выберите папку!");
                return;
            }
            // Очищаем ListBox перед началом работы
            isch = 1;
            ListBox1.Items.Clear();

            // Сжимаем изображения в указанной папке
            CompressImagesInFolder(folderPath, ListBox1);

            MessageBox.Show("Сжатие завершено!");
        }

        private void CompressImagesInFolder(string folderPath, ListBox outputListBox)
        {
            string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            int i = 0;

            foreach (string filePath in files)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long originalSize = fileInfo.Length;

                // Пропуск файлов меньше 800 КБ
                if (originalSize <= 800 * 1024)
                {
                    //outputListBox.Items.Add($"Пропущен файл: {filePath} - размер меньше 800 КБ.");
                    continue;
                }

                long compressedSize = 0;

                try
                {
                    i++;
                    string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));

                    using (var image = Image.Load(filePath))
                    {
                        // Определяем формат изображения
                        var format = SixLabors.ImageSharp.Image.DetectFormat(filePath);

                        if (format == null)
                        {
                            //outputListBox.Items.Add($"{i}: Пропущен файл с неподдерживаемым форматом: {filePath}");
                            continue;
                        }

                        // Сжатие для формата JPEG
                        if (format.DefaultMimeType == "image/jpeg")
                        {
                            var encoder = new JpegEncoder()
                            {
                                Quality = 5 // Максимально низкое качество для сильного сжатия
                            };
                            image.Save(tempFilePath, encoder);
                        }
                        // Сжатие для формата PNG
                        else if (format.DefaultMimeType == "image/png")
                        {
                            var encoder = new PngEncoder
                            {
                                CompressionLevel = PngCompressionLevel.BestCompression // Максимальный уровень сжатия
                            };
                            image.Save(tempFilePath, encoder);
                        }
                        // Конвертация BMP в PNG с максимальным сжатием
                        else if (format.DefaultMimeType == "image/bmp")
                        {
                            string pngTempFilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".png");
                            var encoder = new JpegEncoder()
                            {
                                Quality = 90 // Максимально низкое качество для сильного сжатия
                            };
                            image.Save(pngTempFilePath, encoder);

                            compressedSize = new FileInfo(pngTempFilePath).Length;

                            outputListBox.Items.Add($"{i}: {pngTempFilePath}");
                            outputListBox.Items.Add($"Размер до: {originalSize / 1024} КБ");
                            outputListBox.Items.Add($"Размер после: {compressedSize / 1024} КБ");
                            outputListBox.Items.Add("---------------------------------------------------");
                            // Удаляем оригинальный BMP файл после успешной конвертации
                            File.Delete(filePath);
                            continue;
                        }
                        else
                        {
                            //outputListBox.Items.Add($"{i}: Пропущен файл с неподдерживаемым форматом: {filePath}");
                            continue;
                        }

                        compressedSize = new FileInfo(tempFilePath).Length;

                        if (compressedSize < originalSize)
                        {
                            File.Copy(tempFilePath, filePath, true); // Заменить оригинал на сжатый
                            outputListBox.Items.Add($"{i}: {filePath}");
                            outputListBox.Items.Add($"Размер до: {originalSize / 1024} КБ");
                            outputListBox.Items.Add($"Размер после: {compressedSize / 1024} КБ");
                        }
                        else
                        {
                            //outputListBox.Items.Add($"{i}: {filePath} - сжатие не дало результата.");
                        }
                    }

                    outputListBox.Items.Add("---------------------------------------------------");
                }
                catch (Exception ex)
                {
                    //outputListBox.Items.Add($"{i}: Ошибка обработки файла: {filePath}");
                    //outputListBox.Items.Add($"Ошибка: {ex.Message}");
                    //outputListBox.Items.Add("---------------------------------------------------");
                }
            }
        }


    }


}
