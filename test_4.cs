/*Написать программу на C#, предназначенную для сжатия и расжатия файлов с помощью System.IO.Compression.GzipStream.
Параметры программы, имена исходного и результирующего файлов задаются в командной строке.
Программа должна эффективно распараллеливать и синхронизировать задачи в многопроцессорной среде и уметь обрабатывать файлы, 
размер которых превышает объем доступной оперативной памяти.
Код должен корректно обрабатывать все исключения, 
а при работе с потоками не должны использоваться стандартные классы и библиотеки 
ThreadPool, BackgroundWorker, TPL, continuations, async/await и т.п.).
Код программы должен следовать принципам ООП и ООД (читаемость, разбиение на классы и тд).
*/


using System.IO;
using System.IO.Compression;
using System.Threading;
using System;

namespace veeam_test
{
    class Program
    {
        static void Main(string[] args)
        {
            const string help = "\r\n\nKонсольный архиватор by Alex Skiffin\r\n\nКлючи:\r\n      -z      Упаковка файла\r\n      -uz     Распаковка файла\r\nПримеры:\r\n      -z test.file\r\n      -uz  zipped.gz  test.file\r\n      test.file  zipped.gz";
            
            #region проверка входных данных

            switch (args.Length)
            {
                case 0:
                    // Если приложение запущено без аргументов
                    Console.WriteLine("Это консольное проложение, тут нужны аргументы!"+help);
                    break;

                case 1:
                    // Если аргумент один
                    if (File.Exists(args[0]))
                    {
                        ZapPotokov(args[0],true);               
                    }
                    else { Console.WriteLine("Неправильное имя файла"+help); }
                    break;

                case 2:
                    // Если аргументов два
                    if ((args[0] == "-z") && File.Exists(args[1])) 
                    {
                        ZapPotokov(args[1],true);
                    }
                    else if (args[0] == "-uz" && File.Exists(args[1])) 
                    {
                        ZapPotokov(args[1],false); 
                    }
                    else if (File.Exists(args[0]))
                    {
                        ZapPotokov(args[0],true, args[1]);
                    }
                    else { Console.WriteLine("Неправильное имя файла" + help); }
                    break;

                case 3:
                    // Если аргументов три
                    if ((args[0] == "-z") && File.Exists(args[1]))
                    {
                        ZapPotokov(args[1],true, args[2]);
                    }
                    else if ((args[0] == "-uz") && File.Exists(args[1]))
                    {
                        ZapPotokov(args[1],false, args[2]);
                    }
                    else { Console.WriteLine("Неправильное имя файла"+help); }
                    break;

                default:
                    Console.WriteLine(help);
                    break;
            }

            #endregion
        }

        // Количество потоков будет равно двойному количеству процесссоров (ядер)
        static int _potokov = Environment.ProcessorCount * 2;

        // Запуск архивирования-разархивирования в потоках
        public static void ZapPotokov(string incom, bool comDecom, string reName=null)
        {
            FileInfo fi = new FileInfo(incom);
            long fileLength = (fi.Length);

            long fileCount = Environment.ProcessorCount * 2;
            if (comDecom)
            {
                for (int i = 1; i < fileCount + 1; i++)
                {
                    long byteCounter = fileLength / fileCount;
                    var i1 = i;
                    // Поток на архивацию
                    new Thread(() => VPotok(byteCounter, incom, i1,reName)).Start();
                }
            }
            else
            {
                FileStream fromStream = new FileStream(incom, FileMode.Open, FileAccess.Read, FileShare.Read);
                long byteCounter = 0;
                int i1 = 1;
                while (fromStream.CanRead)
                {
                    if ((byte)fromStream.ReadByte() == 31)
                    {
                        if ((byte)fromStream.ReadByte() == 139)
                        {
                            if ((byte)fromStream.ReadByte() == 8)
                            {
                                if ((byte)fromStream.ReadByte() == 0)
                                {
                                    int toStream = (int)byteCounter - (i1 - 1);
                                    int i2 = i1;
                                    // Поток на разархивацию
                                    new Thread(() => IzPotok(toStream, incom, i2, reName)).Start();
                                    byteCounter++;
                                    i1++;
                                }
                                byteCounter++;
                            }
                            byteCounter++;
                        }
                        byteCounter++;
                    }
                    byteCounter++;
                }
                fromStream.Close();
                fromStream.Dispose();
            }


        }
        
        // Поток архивации
        static void VPotok(long byteCounters, string name, int count,string reName=null)
        {
            FileStream fromStream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read);
            fromStream.Position = byteCounters * (count - 1);
            FileStream targetFile = File.Open(string.Format(name + ".gz{0}", count), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
            GZipStream gzipStream = new GZipStream(targetFile, CompressionMode.Compress, true);
            while (fromStream.CanRead && byteCounters > 0)
            {
                byteCounters--;
                gzipStream.WriteByte((byte)fromStream.ReadByte());
            }

            fromStream.Dispose();
            gzipStream.Dispose();
            targetFile.Dispose();
            fromStream.Close();
            gzipStream.Close();
            targetFile.Close();
            _potokov--;
            if (_potokov == 0)
            {
                Sborka(name + ".gz", reName);
            }
        }
        
        // Поток разархивации
        static void IzPotok(long byteCounters, string name, int count, string reName=null)
        {
            FileStream fromStream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read);
            fromStream.Position = byteCounters;

            try
            {
                FileStream decompressedFileStream = File.Open(string.Format(name.Replace(".gz", "") + "{0}", count),
                                                              FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                GZipStream decompressionStream = new GZipStream(fromStream, CompressionMode.Decompress, false);
                decompressionStream.CopyTo(decompressedFileStream);
                decompressedFileStream.Close();
                decompressedFileStream.Dispose();
                decompressionStream.Close();
                decompressionStream.Dispose();
            }
            catch
            {
                Console.WriteLine("Возможно архив поврежден. Разархивация невозможна");
            }
            fromStream.Close();
            fromStream.Dispose();

            _potokov--;
            if (_potokov == 0)
            {
                Sborka(name.Replace(".gz", ""), reName);
            }
        }
        
        // Сборка временных файлов в один
        static void Sborka(string nameFile, string newName=null)
        {

            FileStream targetFile = File.Open((newName ?? nameFile), FileMode.Append, FileAccess.Write, FileShare.Write);

            for (int i = 1; i < (Environment.ProcessorCount * 2) + 1; i++)
            {

                var fromStream = new FileStream(string.Format(nameFile + "{0}", i), FileMode.Open, 
                                                                            FileAccess.Read, FileShare.Delete);
                fromStream.CopyTo(targetFile);
                fromStream.Close();
                fromStream.Dispose();
                File.Delete(string.Format(nameFile + "{0}", i));
            }
            targetFile.Close();
            targetFile.Dispose(); 

            Console.WriteLine("\r\nНажмите любую клавишу для завершения работы");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}
