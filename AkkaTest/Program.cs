//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2020 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2020 .NET Foundation <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using Akka.Actor;
using System;
using System.Collections.Immutable;
using QDFeedParser;
using SymbolLookup.Actors;
using SymbolLookup.Actors.Messages;
using SymbolLookup.YahooFinance;
using System.Threading;

namespace SymbolLookup
{
    class Program
    {
        public static event EventHandler<FullStockData> DataAvailable;
        public static event EventHandler<string> StatusChange;

        private void OnDataAvailable(object sender, FullStockData fullStockData)
        {
            Console.WriteLine("OnDataAvailable");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ActorSystem ActorSystem;
            IActorRef StockActor;

            DataAvailable += (sender, s) =>
            {
                //Console.WriteLine("{0}", s.Headlines.Items);
                //foreach(BaseFeedItem item in s.Headlines.Items)
                //{
                //    Console.WriteLine("{0}", item.Title);
                //}
                Console.WriteLine("--------");
            };
            StatusChange += (sender, s) =>
            {
                Console.WriteLine("Status changed {0}", s);
            };

            ActorSystem = ActorSystem.Create("stocks");
            StockActor =
                ActorSystem
                .ActorOf(
                    Props.Create(() => new DispatcherActor(DataAvailable, StatusChange)));

            while (true)
            {
                StockActor.Tell("MSFT", ActorRefs.NoSender);
                Random rand = new Random();
                int sec = rand.Next(5, 11);
                Console.WriteLine("> Sleeping for {0} seconds", sec);
                Thread.Sleep(sec * 1000);
            }

            Console.ReadLine();
            ActorSystem.Terminate();
        }
    }
}