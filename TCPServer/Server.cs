using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FanOutPutLib;
using Newtonsoft.Json;

namespace TCPServer
{
    class Server
    {
        public static readonly List<FanOutPut> fanOutPuts = new List<FanOutPut>()
        {
            new FanOutPut(1,"RoomOne", 20, 75),
            new FanOutPut(2,"roomTwo",24,60),
            new FanOutPut(3,"roomThree",23,50),
            new FanOutPut(4,"roomFour",21,70)
        };

        public static void Start()
        {
            try
            {
                TcpListener serverSocket = null;
                IPAddress localIpAddress = null;
                var host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (var ipAddress in host.AddressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine(ipAddress.ToString());
                        localIpAddress = IPAddress.Parse(ipAddress.ToString());
                    }
                }

                int portNumber = 4646;

                serverSocket = new TcpListener(IPAddress.Loopback, portNumber);
                serverSocket.Start();
                Console.WriteLine("Waiting for connection");

                while (true)
                {
                    TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                    Console.WriteLine("Connection establish");

                    Task.Run(() =>
                    {
                        TcpClient temporarySocket = connectionSocket;
                        DoClient(temporarySocket);
                    });
                }

                serverSocket.Stop();
                Console.WriteLine("Server closed");
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        public static void DoClient(TcpClient connectionSocket)
        {
            //Creating a stream of data, that can both been read, and write from a byte stream
            NetworkStream ns = connectionSocket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; //Will auto flush

            while (true)
            {
                sw.Write("You are now connected enter c to close the connection");

                string message = sr.ReadLine();

                if (message.ToLower().Contains("c"))
                {
                    break;
                }

                switch (message.ToLower())
                {
                    case "getall":
                        string allData = JsonConvert.SerializeObject(fanOutPuts);
                        sw.WriteLine(allData);
                        break;

                    case "getid":
                        string idMessage = sr.ReadLine();
                        int id = Convert.ToInt32(idMessage);

                        FanOutPut fanOutPut = fanOutPuts.Find(i => i.Id == id);
                        string data = JsonConvert.SerializeObject(fanOutPut);
                        sw.WriteLine(data);
                        break;

                    //{"Id":5,"Name":"RoomFive","Temp":20,"Moisture":75}
                    case "save":
                        string saveFan = sr.ReadLine();
                        FanOutPut newFan = JsonConvert.DeserializeObject<FanOutPut>(saveFan);
                        fanOutPuts.Add(newFan);
                        break;

                    default:
                        sw.Write("Please select your method");
                        break;

                }

            }

            sw.WriteLine("c");
            ns.Close();
            Console.WriteLine("Net stream is closed");
            connectionSocket.Close();
            Console.WriteLine("Connection to server is closed");
        }
    }
}
