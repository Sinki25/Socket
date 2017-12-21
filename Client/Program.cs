using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        const int LOCALPORT = 8001;  // порт сервера
        const string HOST = "127.0.0.1"; // адрес сервера ??

        static void Main(string[] args)
        {
            try
            {
                EndPoint remotePoint = new IPEndPoint(IPAddress.Parse(HOST), LOCALPORT);

                Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                // подключаемся к удаленному хосту
                listeningSocket.Connect(remotePoint);
                Console.Write("Введите сообщение:");
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                listeningSocket.Send(data);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                while (listeningSocket.Available > 0)
                {
                    bytes = listeningSocket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }

                Console.WriteLine("ответ сервера: " + builder.ToString());

                // закрываем сокет
                listeningSocket.Shutdown(SocketShutdown.Both);
                listeningSocket.Close();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }
    }
}
