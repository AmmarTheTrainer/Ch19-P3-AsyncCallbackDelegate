using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ch19_P3_AsyncCallbackDelegate
{
    public delegate int BinaryOp(int x, int y);
    class Program
    {
        private static bool isDone = false;
        static void Main(string[] args)
        {
            Console.WriteLine("***** AsyncCallbackDelegate Example *****");
            Console.WriteLine("Main() invoked on thread {0}.",Thread.CurrentThread.ManagedThreadId);
            BinaryOp b = new BinaryOp(Add);
            //IAsyncResult ar = b.BeginInvoke(10, 10,new AsyncCallback(AddComplete), null);
            IAsyncResult ar = b.BeginInvoke(10, 10,new AsyncCallback(AddComplete) , 
                                        " Main() thanks for adding these numbers ");
            // Assume other work is performed here...
            while (!isDone)
            {
                Console.WriteLine("Working....");
                Thread.Sleep(500);
            }
            Console.ReadLine();
        }

        private static void AddComplet(IAsyncResult ar)
        {
            Console.WriteLine(" task completed ");
            Console.WriteLine("IsCompleted : {0} ", ar.IsCompleted);



            isDone = true;
        }

        static int Add(int x, int y)
        {
            Console.WriteLine("Add() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(5000); // lengthy task is in progress...
            return x + y;
        }
        static void AddComplete(IAsyncResult iar)
        {
            Console.WriteLine("AddComplete() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Your addition is complete");

            // now get the results.
            AsyncResult asyncResult = (AsyncResult)iar;
            BinaryOp b = (BinaryOp)asyncResult.AsyncDelegate;
            Console.WriteLine(b.EndInvoke(iar));

            // Receive the information object and cast it to relevant type.
            string message = (string)iar.AsyncState;
            Console.WriteLine(message);

            isDone = true;
        }
    }
}
