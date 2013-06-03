/*
- Необходимо написать на C# 2 программы: клиент и сервер. Сервер - консольное приложение. Клиент с графическим интерфейсом. 
- Клиент запрашивает сетевой адрес сервера и подключается к нему.
- Сервер пересылает клиенту с интервалом 0.5 секунд последовательно 10 картинок (картинки любые, можете сами нарисовать, берутся из папки, откуда запускается программа). 
- Клиент получает картинки и отображает их на экране.
- Пересылка продолжается до тех пор пока клиент не будет закрыт.
- Клиентов может подключаться несколько одновременно.
*/

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace macroscop_serv_test
{
    public class Server
    {
        private const int ListenPort = 13171;

        // прослушивание порта
        private static void StartListener()
        {
            try
            {
                // ожидаем broadcast сообщения
                UdpClient listener = new UdpClient(ListenPort);
                IPEndPoint[] groupEp = {new IPEndPoint(IPAddress.Any, ListenPort)};
                while (true)
                {
                    Console.WriteLine("Ожидание подключения");

                    // собственно ждем сообщения
                    listener.Receive(ref groupEp[0]);

                    Console.WriteLine("Подключился: "+groupEp[0].Address);

                    // отправляем файлы по полученному адресу
                    new Thread(() => Otpravka(groupEp[0].Address.ToString())).Start();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static int Main()
        {
            StartListener();
            return 0;
        }

        // отправка файлов
        public static bool Otpravka(string ipAddr)
        {
            //Коннектимся
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddr), ListenPort);
                Socket connector = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                connector.Connect(endPoint);
                while (connector.Connected)
                {
                    foreach (string nextFile in Directory.GetFiles(@"img"))
                    {
                        Byte[] readedBytes = new Byte[256];
                        var fileStream = new FileStream(nextFile, FileMode.Open,FileAccess.Read);
                        var reader = new BinaryReader(fileStream);

                        Int32 currentReadedBytesCount;
                        do
                        {
                            // отправляем файл
                            currentReadedBytesCount = reader.Read(readedBytes, 0, readedBytes.Length);
                            connector.Send(readedBytes, currentReadedBytesCount, SocketFlags.None);
                        } while (currentReadedBytesCount == readedBytes.Length);
                        fileStream.Close();
                        fileStream.Dispose();
                        reader.Close();
                        reader.Dispose();

                        // ожидаем 0,5 секунды
                        Thread.Sleep(500);
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }
    }
}
