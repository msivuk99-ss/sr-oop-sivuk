using System;

namespace sw3v8
{
    public class TemporaryFile : IDisposable
    {
        private string _tempFilePath;
        private bool _fileExists;
        private bool _disposed = false;

        public string TempFilePath => _tempFilePath;
        public bool FileExists => _fileExists;

        public TemporaryFile(string tempFilePath)
        {
            _tempFilePath = tempFilePath;
            _fileExists = true;
            Console.WriteLine($"[СТВОРЕНО] Тимчасовий файл за шляхом: '{_tempFilePath}'");
        }

        public void Write(string content)
        {
            if (_disposed || !_fileExists)
            {
                throw new ObjectDisposedException(nameof(TemporaryFile), "Неможливо записати: файл вже видалено/закрыто!");
            }

            Console.WriteLine($"[ЗАПИС] У файл '{_tempFilePath}' записано: \"{content}\"");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine($"[DISPOSE] Звільнення керованих ресурсів для '{_tempFilePath}'...");
                }

                if (_fileExists)
                {
                    Console.WriteLine($"[ОЧИЩЕННЯ] Видаляємо тимчасовий файл '{_tempFilePath}'...");
                    _fileExists = false;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TemporaryFile()
        {
            Console.WriteLine($"[ФІНАЛІЗАТОР] Деструктор викликано для '{_tempFilePath}'!");
            Dispose(false);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("  СЦЕНАРІЙ 1: Використання конструкції using");
            Console.WriteLine("==================================================");
            using (TemporaryFile file1 = new TemporaryFile(@"C:\temp\file1.tmp"))
            {
                file1.Write("Тестові дані для сценарію 1");


                Console.WriteLine("\n==================================================");
                Console.WriteLine("  СЦЕНАРІЙ 2: Явний виклик Dispose()");
                Console.WriteLine("==================================================");
                TemporaryFile file2 = new TemporaryFile(@"C:\temp\file2.tmp");
                file2.Write("Тестові дані для сценарію 2");
                file2.Dispose();

                Console.WriteLine("Спроба повторного виклику Dispose():");


                Console.WriteLine("\n==================================================");
                Console.WriteLine("  СЦЕНАРІЙ 3: Без Dispose (робота GC та деструктора)");
                Console.WriteLine("==================================================");
                CreateUnmanagedFile();

                Console.WriteLine("Викликаємо збирач сміття (Garbage Collector)...");
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Console.WriteLine("\nПрограму завершено успішно.");
            }

            static void CreateUnmanagedFile()
            {
                TemporaryFile file3 = new TemporaryFile(@"C:\temp\file3.tmp");
                file3.Write("Тестові дані для сценарію 3");
            }
        }
    }