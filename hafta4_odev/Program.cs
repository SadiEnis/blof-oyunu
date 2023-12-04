using System;
using System.Collections;

namespace hafta4_odev
{
    internal class Program : Bluff // Aşağıda defalarca belirtmek yerine kalıttım (Inheritance)
    {
        public static void Main(string[] args)
        {
            BluffMenu();
            Console.ReadKey(); // Oyun başlıyor...
            Console.ForegroundColor = ConsoleColor.White; // Bunlar estetil :)
            ArrayList cardDesk = new ArrayList(); // Oyun içindeki 52lik iskambil destemiz olacak.
                                                  // Neden ArrayList: Çünkü ArrayList object içerik tutar. Kullanımı esnektir.
            MixDesk(cardDesk); // Destemizi karıştırıp oyuna başlayacağız.

            Player player1 = new Player("Sadi");
            Player player2 = new Player("Enis");
            ComputerPlayer computer = new ComputerPlayer();

            DistributeCards(player1, player2, computer, cardDesk); // Kartları sırayla dağıtır.

            while (true)
            {
                PlayBluff(player1, player2, computer);

                if (player1.playerDesk.Count == 0 || player2.playerDesk.Count == 0 || computer.player.playerDesk.Count == 0)
                    break; // Kullanıcılardan birinin destesi bittiğinde diğer iki kişi kaybetmiş olacak.
                           // Destesi ilk biten kişi kazanacak.
            }

            // Aşağısı bitiş ekranı ↓↓↓
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("                           -------------------------- ");
            Console.WriteLine("                           |  ♣♠♦♥ Blöf Oyunu ♣♠♦♥  |  ");
            Console.WriteLine("                           --------------------------  ");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Oyun Sonucu:\n");

            Console.Write("{0} sona kalan kartları: ", player1.name);
            foreach (var item in player1.playerDesk)
                Console.Write(item + " ");
            Console.WriteLine();

            Console.Write("{0} sona kalan kartları: ", player2.name);
            foreach (var item in player2.playerDesk)
                Console.Write(item + " ");
            Console.WriteLine();

            Console.Write("{0} sona kalan kartları: ", computer.player.name);
            foreach (var item in computer.player.playerDesk)
                Console.Write(item + " ");
            Console.WriteLine();

            if (player1.playerDesk.Count == 0) // Kaybeden kişi ekranı ↓↓↓
                Console.WriteLine("Tebrikler {0} Kazandı. Diğer oyuncular kaybetti.", player1.name);
            else if (player2.playerDesk.Count == 0)
                Console.WriteLine("Tebrikler {0} Kazandı. Diğer oyuncular kaybetti.", player2.name);
            else if (computer.player.playerDesk.Count == 0)
                Console.WriteLine("Tebrikler {0} Kazandı. Diğer oyuncukar kaybetti.", computer.player.name);
            Console.WriteLine();
            Console.Write("Çıkmak için herhangi bir tuşa basınız...");
            Console.ReadKey();
        }
    }
}
