using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace Async_Homework
{
    public class Method1
    {
        public void GetACount(string url)
        {
            WebClient wc = new WebClient();
            string htmlYnet = wc.DownloadString(url);
            int counter = 0;
            for (int i = 0; i < htmlYnet.Length; i++)
            {
                if (htmlYnet[i] == 'a')
                {
                    counter++;
                }
            }
            Console.WriteLine("the letter 'a' appeared at '{0}' {1} times", url, counter);
        }
    }

    public class Method2
    {
        public static string html;
        public static Mutex _mutex = new Mutex();

        public static void Ynet()
        {
            WebClient wc = new WebClient();
            _mutex.WaitOne();
            html = wc.DownloadString("http://www.ynet.co.il");
            int counter = 0;
            for (int i = 0; i < html.Length; i++)
            {
                if (html[i] == 'a')
                {
                    counter++;
                }
            }
            _mutex.ReleaseMutex();
            Console.WriteLine("the letter 'a' appeared on Ynet {0} times", counter);
        }

        public static void Twitch()
        {
            WebClient wc = new WebClient();
            _mutex.WaitOne();
            html = wc.DownloadString("https://www.twitch.tv");
            int counter = 0;
            for (int i = 0; i < html.Length; i++)
            {
                if (html[i] == 'a')
                {
                    counter++;
                }
            }
            _mutex.ReleaseMutex();
            Console.WriteLine("the letter 'a' appeared on Twitch {0} times", counter);
        }
    }

    public class Method3
    {
        public static string html = "";

        public async Task getACount(string url1, string url2)
        {
            Task<int> t1 = CalcAAsync(url1);
            Task<int> t2 = CalcAAsync(url2);
            await Task.WhenAll(t1, t2);
            Console.WriteLine("the letter 'a' appeared at '{0}' {1} times", url1, t1.Result);
            Console.WriteLine("the letter 'a' appeared at '{0}' {1} times", url2, t2.Result);
        }

        async private static Task<int> CalcAAsync(string url)
        {
            WebClient wc = new WebClient();
            html = wc.DownloadString(url);
            int counter = 0;
            for (int i = 0; i < html.Length; i++)
            {
                if (html[i] == 'a')
                {
                    counter++;
                }
            }

            return counter;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //method 1

            //Method1 meth1 = new Method1();
            //meth1.GetACount("http://www.ynet.co.il");
            //meth1.GetACount("https://www.twitch.tv");

            //Method 2

            //Thread getYnetHtml = new Thread(new ThreadStart(Method2.Ynet));
            //Thread getTwitchHtml = new Thread(new ThreadStart(Method2.Twitch));
            //getYnetHtml.Start();
            //getTwitchHtml.Start();

            //Method 3

            Method3 meth3 = new Method3();
            meth3.getACount("http://www.ynet.co.il", "https://www.twitch.tv");
            
            
        }
    }
}
