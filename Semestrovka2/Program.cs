using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{

    class Program
    {
        
        bool alive = false; // будет ли работать поток для приема
        UdpClient client;
        const int LOCALPORT = 8001; // порт для приема сообщений 
        const string HOST = "127.0.0.1"; // хост для групповой рассылки

        static void Main(string[] args)
        {
            // адрес для групповой рассылки //получаем адреса для запуска сокета
            IPAddress groupAddress = IPAddress.Parse(HOST);

            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint remotePoint = new IPEndPoint(IPAddress.Parse(HOST), LOCALPORT);

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listeningSocket.Bind(remotePoint);

                // начинаем прослушивание
                listeningSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listeningSocket.Accept();

                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();

                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    while (handler.Available > 0)
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    // отправляем ответ
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);

                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
