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

        public static void GetACount(object url)
        {
            WebClient wc = new WebClient();
            _mutex.WaitOne();
            html = wc.DownloadString((string)url);
            int counter = 0;
            for (int i = 0; i < html.Length; i++)
            {
                if (html[i] == 'a')
                {
                    counter++;
                }
            }
            _mutex.ReleaseMutex();
            Console.WriteLine("the letter 'a' appeared on '{0}' {1} times", url, counter);
        }
    }

    public class Method3
    {

        public async Task getACountAsync(string url1, string url2)
        {
            Task<int> t1 = CalcAAsync(url1);
            Task<int> t2 = CalcAAsync(url2);
            await Task.WhenAll(t1, t2);
            Console.WriteLine("the letter 'a' appeared at '{0}' {1} times", url1, t1.Result);
            Console.WriteLine("the letter 'a' appeared at '{0}' {1} times", url2, t2.Result);
        }

        private static async Task<int> CalcAAsync(string url)
        {
            WebClient wc = new WebClient();
            string html = wc.DownloadString(url);
            int counter = 0;
            for (int i = 0; i < html.Length; i++)
            {
                if (html[i] == 'a')
                {
                    counter++;
                }
            }
            Console.WriteLine("finished calculation of " + url);
            return counter;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ////method 1
            //Console.WriteLine("-----METHOD-1-----");
            //Method1 meth1 = new Method1();
            //meth1.GetACount("http://www.ynet.co.il");
            //meth1.GetACount("https://www.twitch.tv");

            ////Method 2
            //Console.WriteLine("-----METHOD-2-----");
            //Thread getYnetHtml = new Thread(new ParameterizedThreadStart(Method2.GetACount));
            //Thread getTwitchHtml = new Thread(new ParameterizedThreadStart(Method2.GetACount));

            //getYnetHtml.Start("http://www.ynet.co.il");
            //getTwitchHtml.Start("https://www.twitch.tv");

            //getYnetHtml.Join();
            //getTwitchHtml.Join();

            //Method 3
            Console.WriteLine("-----METHOD-3-----");
            Method3 meth3 = new Method3();
            meth3.getACountAsync("http://www.ynet.co.il", "https://www.twitch.tv").Wait();
        }
    }
}
