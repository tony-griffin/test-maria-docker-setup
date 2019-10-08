using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Http;

namespace MariaExpoDocker
{
    class Program
    {
        static HttpListener _httpListener = new HttpListener();
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
           // $"http://+:{System.Environment.GetEnvironmentVariable("PORT")}/"
            //_httpListener.Prefixes.Add("http://localhost:5000/"); // add prefix 
            _httpListener.Prefixes.Add($"http://+:{System.Environment.GetEnvironmentVariable("PORT")}/"); // add prefix 
            _httpListener.Start(); // start server (Run application as Administrator!)
            Console.WriteLine("Server started.");
            Thread responseThread = new Thread(ResponseThread);
            responseThread.Start(); // start the response thread
        }
        
        static void ResponseThread()
        {
            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext(); // get a context
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                
                string text;
                using (var reader = new StreamReader(request.InputStream,
                    request.ContentEncoding))
                {
                    text = reader.ReadToEnd();
                }
                
                Console.Write(text);
                Console.WriteLine("\n");
                
                string hello = "I'm WORKING !!!'";
                Console.WriteLine(hello);
                byte[] responseArray = Encoding.UTF8.GetBytes(hello); // get the bytes to response
                
                
                
                response.AddHeader("Content-type", "application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length); // write bytes to the output stream
                response.KeepAlive = false; // set the KeepAlive bool to false
                response.Close(); // close the connection
                Console.WriteLine("Response given to a request.");
            }
        }
    }
}