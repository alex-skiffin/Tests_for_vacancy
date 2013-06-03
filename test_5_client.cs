/*
- Необходимо написать на C# 2 программы: клиент и сервер. Сервер - консольное приложение. Клиент с графическим интерфейсом. 
- Клиент запрашивает сетевой адрес сервера и подключается к нему.
- Сервер пересылает клиенту с интервалом 0.5 секунд последовательно 10 картинок (картинки любые, можете сами нарисовать, берутся из папки, откуда запускается программа). 
- Клиент получает картинки и отображает их на экране.
- Пересылка продолжается до тех пор пока клиент не будет закрыт.
- Клиентов может подключаться несколько одновременно.
*/

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace macroscop_client_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PuskStopClick(object sender, EventArgs e)
        {
            if (Pusk_stop.Text == "START")
            {
                // отправляем broadcast запрос
                Socket zaprosServ = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
                IPAddress broadcast = IPAddress.Broadcast;
                string args = textBox1.Text;
                byte[] sendbuf = Encoding.UTF8.GetBytes(args);
                IPEndPoint ep = new IPEndPoint(broadcast, 13171);
                zaprosServ.EnableBroadcast = true;
                zaprosServ.SendTo(sendbuf, ep);
                zaprosServ.Close();

                // запускаем поток получения файла
                Pusk_stop.Text = "STOP";
                _potok = new Thread(FileReceiver);
                _potok.Start();
            }
            else
            {
                Pusk_stop.Text = "START";
                try
                {
                    _potok.Abort();
                }
                catch { }
            }
            
        }

        static Thread _potok;
        private static bool _done1;
        protected void FileReceiver()
        {
            _done1 = true;

            // создаем Listener на порт 13171
            TcpListener listen = new TcpListener(13171);

            // начинаем прослушку
            listen.Start();
            Socket receiveSocket = listen.AcceptSocket();
            
            while (_done1&&Pusk_stop.Text=="STOP")
            {
                try
                {
                    // ждем получения сообщения
                    Byte[] Receive = new Byte[256]; 
                    MemoryStream memPotok = new MemoryStream();
                    
                    // количество считанных байт
                    Int32 receivedBytes;
                    do
                    {
                        // начинаем чтение в поток
                        receivedBytes = receiveSocket.Receive(Receive, Receive.Length, 0);
                        memPotok.Write(Receive, 0, receivedBytes);

                        // читаем до конца
                    } while (receivedBytes == Receive.Length);

                    // выводим рисунок
                    imgScreen.Image = new Bitmap(memPotok);
                    memPotok.Close();
                    memPotok.Dispose();
                }
                catch
                {
                    listen.Stop();
                }
            }
            listen.Stop();
        }
    }
}
