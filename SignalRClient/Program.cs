using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SignalRClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder().WithUrl("https://localhost:7185/locationHub")
                                                       .Build();
                
            connection.On<string>("ReceiveMessage", message =>
            {
                Console.WriteLine("Nova pretraga: " + message);
            });

            try
            {
                await connection.StartAsync();
                Console.WriteLine("SignalR klijent spojen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri spajanju na SignalR: " + ex.Message);
            }

            // Ostanite u petlji kako bi konzola ostala otvorena
            Console.WriteLine("Pritisnite Enter za izlaz.");
            await Task.Run(() => Console.ReadLine());

            // Prekid veze s hubom prilikom izlaska
            await connection.StopAsync();
            await connection.DisposeAsync();
        }
    }
}
