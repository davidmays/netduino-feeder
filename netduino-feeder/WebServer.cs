using System;
using Microsoft.SPOT;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;

namespace NetduinoFeeder
{
    class WebServer
    {
        private Socket _socket;

        private int Backlog = 10;
        private Thread _listenerThread;
        private RadioShackMicroServo _servo;
        private bool _waiting = true;
        private Buzzer _buzzer;

        public WebServer(int port, RadioShackMicroServo servo, Buzzer buzzer)
        {
            _servo = servo;
            _buzzer = buzzer;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Listen(Backlog);

            _listenerThread = new Thread(new ThreadStart(ListenForClients));
            _listenerThread.Start();
        }

        public void Wait() 
        {
            while (_waiting)
            {
                Thread.Sleep(1);
            }
        }


        private void ListenForClients()
        {
            while (true)
            {
                using (Socket client = _socket.Accept())
                {
                    // Wait for data to become available
                    while (!client.Poll(10, SelectMode.SelectRead)) ;

                    int bytesSent = client.Available;
                    if (bytesSent > 0)
                    {
                        byte[] buffer = new byte[bytesSent];
                        int bytesReceived = client.Receive(buffer, bytesSent, SocketFlags.None);

                        if (bytesReceived == bytesSent)
                        {
                            string request = new string(Encoding.UTF8.GetChars(buffer));
                            Debug.Print(request);

                            Respond(client, request);
                        }
                    }
                }
            }
        }

        private void Respond(Socket client, string request)
        {
            var lines = request.Split('\n');

            var requestLine = new RequestLine(lines[0]);

            if(requestLine.Verb.Equals(RequestLine.Verbs.GET))
            {
                RespondToQuery(client, requestLine.Query);
            }
            else
            {
                SendError(client,"request was:\n" + request);
            }

            
        }

        private void RespondToQuery(Socket client, string query)
        {
            

            switch (query)
            {
                case "/":
                    SendIndex(client);
                    break;
                case "/beep":
                    Beep();
                    SendOK(client, "beep");
                    break;
                case "/left":
                    SendOK(client, "setting to:" + query);
                    _servo.Left();
                    break;
                case "/right":
                    SendOK(client, "setting to:" + query);
                    _servo.Right();
                    break;
                case "/center":
                    SendOK(client, "setting to:" + query);
                    _servo.Center();
                    break;
                case "/quit":
                    SendOK(client, "shutting down");
                    _servo.Release();
                    _waiting = false;
                    _listenerThread.Abort();
                    _socket.Close();
                    break;
                default:
                    SendError(client, "bad query:" + query);
                    break;
            }
        }

        private void Beep()
        {
            _buzzer.Buzz(250, 880);
        }

        private void SendIndex(Socket client)
        {
            SendOK(client, GetIndexHtml());
        }


        private const string Response200Begin = "HTTP/1.0 200 OK\r\nContent-Type: text; charset=utf-8\r\nContent-Length: ";
        private const string Response400Begin = "HTTP/1.0 400 BAD REQUEST\r\nContent-Type: text; charset=utf-8\r\nContent-Length: ";

        private const string ResponseEnd = "\r\nConnection: close\r\n\r\n";

        private void SendOK(Socket client, string content)
        {
            long fileSize = content.Length;

            string header = Response200Begin + fileSize.ToString() + ResponseEnd;
            client.Send(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
            client.Send(Encoding.UTF8.GetBytes(content), SocketFlags.None);
        }

        private void SendError(Socket client, string content)
        {
            long fileSize = content.Length;

            string header = Response400Begin + fileSize.ToString() + ResponseEnd;
            client.Send(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
            client.Send(Encoding.UTF8.GetBytes(content), SocketFlags.None);
        }

        private string GetIndexHtml()
        {
            return "<html>" +
                "<head>" +
                "<title> Control </title>" +
                "<script type='text/javascript'>" +
                "function myclick(action){var myRequest = new XMLHttpRequest();" + 
                "myRequest.open(\"GET\", \"http://192.168.1.199/\"+action, true);" + 
                "myRequest.send();};" +
                "</script></head><body><form>" +
                "<input type='button' value='left' onclick=\"myclick('left');\">" +
                "<input type='button' value='center' onclick=\"myclick('center');\">" +
                "<input type='button' value='right' onclick=\"myclick('right');\">" +
                "<p>"+
                "<input type='button' value='beep' onclick=\"myclick('beep');\">" +
                "<p>" +
                "<input type='button' value='quit' onclick=\"myclick('quit');\"></form>"+
                "</body></html>";
        }
    }


   
}
