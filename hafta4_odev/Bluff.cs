using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace hafta4_odev
{
    internal class Bluff
    {
        public static ArrayList placedCards = new ArrayList(); // static metotlar içinde kullanabilmek için static tanımlandı.
        public static string[] cards = new string[] // BU array deste paketi, amacı memory sıfırlamak ve oyun başında ArrayList'e kartları eklemek.
                                             {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K",
                                              "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K",
                                              "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K",
                                              "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
        public static void MixDesk(ArrayList _cardDesk)
        { // Desteyi karıştıran metot
            foreach (string card in cards)
                _cardDesk.Add(card);

            Random random = new Random();
            int c = _cardDesk.Count;
            while (c > 1)
            {
                c--;
                int indx = random.Next(c + 1);
                object temp = _cardDesk[indx];
                _cardDesk[indx] = _cardDesk[c];
                _cardDesk[c] = temp; // swap işlemi yaptık her döndü döndüğümzde bir kez. Defalarce swap yapmak esteyi karmış olur. 
                                     // Gerçekte de aslında deste kararken kartların yerini yer değiştiriyoruz. Oradan yola çıktım.
            }
        }
        public static string RandomCard(ArrayList _cardDesk)
        { // Bize desteden rastgele bir kart verecek
            Random random = new Random();
            int index = random.Next(0, _cardDesk.Count); // Desteden rastgele seçecek
            object card = _cardDesk[index];
            _cardDesk.Remove(index); // o indexteki kartı tüm deste içinden çıkartıyoruz.
            return card.ToString();
        }
        public static void PlayBluff(Player _player1, Player _player2, ComputerPlayer _computer)
        { // Oyunumuzun temelini oluşturan metottur.
            int cardPcs = 0;
            string cardType = null;
            string chs;
             // Tüm kartları ekledim atıldığı iddia edilen kartı çıkartılacak.

            #region Oyuncu1
            Console.Clear();
            if (cardType != null)
                Console.WriteLine("\nYerde en son atıldığı iddia edilen kartlar: {0} tane {1}\n", cardPcs, cardType);
            else
                Console.WriteLine("\nYerde atıldığı iddia edilen kart yok.\n");

            Console.Write("{0} kartları: ", _player1.name);
            for (int i = 0; i < _player1.playerDesk.Count; i++)
                Console.Write(_player1.playerDesk[i] + " ");
            Console.WriteLine();

            Console.WriteLine("Kart atmak için: A");
            Console.WriteLine("Blöf demek için: B");
            Console.WriteLine("Eli pas geçmek için: C");
            Console.Write("Yapmak istediğiniz işlem: ");
            chs = Console.ReadLine();
            switch (chs.ToUpper()) // Oyuncu yapmak istediği seçeneğe göre değerledirilir.
            {
                case "A":
                    Console.Write("Atmak istediğiniz kart sayısı: ");
                    cardPcs = Convert.ToInt32(Console.ReadLine());
                    for (int i = 0; i < cardPcs; i++)
                    {
                        bool bl = false;
                    retry: // Koymak istediği kart onda yoksa goto ile işlemi yeniliyoruz.
                        Console.Write((i + 1) + ".kart: ");
                        cardType = Console.ReadLine();
                        foreach (var item in _player1.playerDesk)
                            if (item.ToString() == cardType)
                                bl = true;
                        if (!bl)
                        {
                            Console.WriteLine("Böyle bir kartın yok yeniden giriş yap.");
                            goto retry; // Kart yoksa goto retry: 'ye taşıyor.
                        }
                        else
                        {
                            placedCards.Add(cardType);
                            _player1.RemoveCard(cardType);
                        }
                    }
                    Console.WriteLine("Diğerlerine ne attığınızı söyleyeceksiniz.");
                    for (int i = 0; i < cardPcs; i++)
                    {
                        Console.Write((i + 1) + ".kart: ");
                        cardType = Console.ReadLine();
                        _computer.cardMemory.Remove(cardType); // Atılan kartı memoryden sileceğiz bu olay aşağıda bilgisayara blöf yapmasının kararını verdirecek.
                    }
                    break;
                case "B":
                    if (placedCards.Count == 0)
                    {
                        Console.WriteLine("Yerde kart yok blöf yapamazsın. Hata yaptın ve pas geçmiş sayıldın.");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        if (_player1.SayBluff(placedCards, cardPcs, cardType)) // Metot true döndüğünde blöf değildir.
                        {
                            Console.WriteLine("Blöf değildi.");
                            foreach (var item in placedCards)
                                _player1.playerDesk.Add(item);
                            _computer.cardMemory.Clear();
                            foreach (string card in cards)
                                _computer.cardMemory.Add(card); // Memory sıfırlandı.
                            Console.WriteLine("Bilemediğin için yerdeki kartları sana eklendi.");
                            placedCards.Clear(); // Yerdeki kartlar silip onları önceki oyuncuya ekler.
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.WriteLine("Gerçekten blöftü.");
                            foreach (var item in placedCards)
                                _computer.player.playerDesk.Add(item);
                            _computer.cardMemory.Clear();
                            foreach (string card in cards)
                                _computer.cardMemory.Add(card); // Memory sıfırlandı.
                            Console.WriteLine("Bildiğin için yerdeki kartlar önceki oyuncuya eklendi.");
                            placedCards.Clear();
                            Thread.Sleep(1000);
                        }
                    }
                    break;
                case "C":
                    Console.WriteLine("Pas geçtin.");
                    Thread.Sleep(500);
                    break;
                default:
                    Console.WriteLine("Hatalı girdi. Pas geçmiş sayıldın.");
                    Thread.Sleep(500);
                    break;
            }
            #endregion

            #region Oyuncu2
            Console.Clear(); // Yukarıda yorum satırları bu oyuncu için de geçerli yeniden yazmıyorum.
            if (cardType != null)
                Console.WriteLine("\nYerde en son atıldığı iddia edilen kartlar: {0} tane {1}\n", cardPcs, cardType);
            else
                Console.WriteLine("\nYerde atıldığı iddia edilen kart yok.\n");

            Console.Write("{0} kartları: ", _player2.name);
            for (int i = 0; i < _player2.playerDesk.Count; i++)
                Console.Write(_player2.playerDesk[i] + " ");
            Console.WriteLine();

            Console.WriteLine("Kart atmak için: A");
            Console.WriteLine("Blöf demek için: B");
            Console.WriteLine("Eli pas geçmek için: C");
            Console.Write("Yapmak istediğiniz işlem: ");
            chs = Console.ReadLine(); 
            switch (chs.ToUpper())
            {
                case "A":
                    Console.Write("Atmak istediğiniz kart sayısı: ");
                    cardPcs = Convert.ToInt32(Console.ReadLine());
                    for (int i = 0; i < cardPcs; i++)
                    {
                        bool bl = false;
                    retry:
                        Console.Write((i + 1) + ".kart: ");
                        cardType = Console.ReadLine();
                        foreach (var item in _player2.playerDesk)
                            if (item.ToString() == cardType)
                                bl = true;
                        if (!bl)
                        {
                            Console.WriteLine("Böyle bir kartın yok yeniden giriş yap.");
                            goto retry;
                        }
                        else
                        {
                            placedCards.Add(cardType);
                            _player2.RemoveCard(cardType);
                        }
                    }
                    Console.WriteLine("Diğerlerine ne attığınızı söyleyeceksiniz.");
                    for (int i = 0; i < cardPcs; i++)
                    {
                        Console.Write((i + 1) + ".kart: ");
                        cardType = Console.ReadLine();
                        _computer.cardMemory.Remove(cardType);
                    }
                    break;
                case "B":
                    if (placedCards.Count == 0)
                    {
                        Console.WriteLine("Yerde kart yok blöf yapamazsın. Hata yaptın ve pas geçmiş sayıldın.");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        if (_player2.SayBluff(placedCards, cardPcs, cardType)) // Metot true döndüğünde kartlar doğrudur anlamına gelir.
                        {
                            Console.WriteLine("Blöf değildi.");
                            foreach (var item in placedCards)
                                _player2.playerDesk.Add(item);
                            _computer.cardMemory.Clear();
                            foreach (string card in cards)
                                _computer.cardMemory.Add(card); // Memory sıfırlandı.
                            Console.WriteLine("Bilemediğin için yerdeki kartları sana eklendi.");
                            placedCards.Clear();
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.WriteLine("Gerçekten blöftü.");
                            foreach (var item in placedCards)
                                _player1.playerDesk.Add(item);
                            _computer.cardMemory.Clear();
                            foreach (string card in cards)
                                _computer.cardMemory.Add(card); // Memory sıfırlandı.
                            Console.WriteLine("Bildiğin için yerdeki kartlar önceki oyuncuya eklendi.");
                            placedCards.Clear();
                            Thread.Sleep(1000);
                        }
                    }
                    break;
                case "C":
                    Console.WriteLine("Pas geçtin.");
                    Thread.Sleep(500);
                    break;
                default:
                    Console.WriteLine("Hatalı girdi. Pas geçmiş sayıldın.");
                    Thread.Sleep(500);
                    break;
            }
            #endregion

            #region Bilgisayar
            // Bilgisyarın işlevleri şunlar olmalı: Blöf olduğunu anlamalı
                                                 // Blöf yapmalı - Pas geçmeli - Kart atmalı
                                                 // Diğer oyuncular tarafından atıldığı iddia edilen kartları memory'den silmeli.
            Console.Clear();
            Console.WriteLine("Bilgisayar hamlesini düşünüyor.");
            Thread.Sleep(1500);
            // Blöf yapması gerektiğini anlaması:
            int move = 0;
            int tempPcs = 0;
            foreach (string item in _computer.cardMemory)
                if (item == cardType)
                    tempPcs++;
            if (tempPcs < cardPcs)
                move = 1; // Blöf yapacak
            else
                move = 2; // Kart atacak
            
            // Blöf mü yapmalı yapmamalı biliyor artık işlem yapabilir.
            switch (move)
            {
                case 0: // Herhangi bir şey yapmayacak
                    Console.WriteLine("Pas Geçti.");
                    Thread.Sleep(1500);
                    break;
                case 1: // Eğer blöf kararı verdiyse blöf diyecek
                    if (_computer.player.SayBluff(placedCards, cardPcs, cardType)) // Metot true döndüğünde kartlar doğrudur anlamına gelir.
                    {
                        Console.WriteLine("Bilgisayar blöf dedi ama değildi.");
                        foreach (var item in placedCards)
                            _computer.player.playerDesk.Add(item);
                        _computer.cardMemory.Clear();
                        foreach (string card in cards)
                            _computer.cardMemory.Add(card); // Memory sıfırlandı.
                        Console.WriteLine("Bilemediği için yerdeki kartları bilgisayara eklendi.");
                        placedCards.Clear();
                        Thread.Sleep(2500);
                    }
                    else // Kartlar yanlışsa kartları ilgili oyuncuya ekler. Temel blöf mekaniği
                    {
                        Console.WriteLine("Bilgisaayar blöf dedi ve gerçekten blöftü.");
                        foreach (var item in placedCards)
                            _player2.playerDesk.Add(item);
                        _computer.cardMemory.Clear();
                        foreach (string card in cards)
                            _computer.cardMemory.Add(card); // Memory sıfırlandı.
                        Console.WriteLine("Bildiği için yerdeki kartlar önceki oyuncuya eklendi.");
                        placedCards.Clear();
                        Thread.Sleep(2500);
                    }
                    break;
                case 2: // Eğer blöf kararı vermediyse kart atacak (random)
                    Random rndm = new Random();
                    int a = rndm.Next(2, 5); // Kart atma mekaniğini tam anlamıyla yazamadım. En azından random sayıda random kartlar atsın
                    for (int i = 0; i < a; i++)
                    {
                        int index;
                        if (_computer.player.playerDesk.Count != 0)
                        {
                            index = rndm.Next(0, _computer.player.playerDesk.Count);
                            placedCards.Add(_computer.player.playerDesk[index]);
                            _computer.player.RemoveCard(_computer.player.playerDesk[index]);
                        }
                    }
                    Console.WriteLine("Bilgisayar {0} tane kart attı.", a);
                    Thread.Sleep(2500); // En son da bilgisayar hakkında ilgili işlem bilgisi veriyoruz.
                    break;
            }
            #endregion
        }
        public static void DistributeCards(Player _player1, Player _player2, ComputerPlayer _comp, ArrayList _cardDesk)
        { // Kartları sıra sıra dağıtan metottur.
            for (int i = 0; i < 52; i++)
            {
                if (i % 3 == 0 && _cardDesk[i] != null)
                {
                    _player1.AddCard(_cardDesk[i]);
                    _cardDesk[i] = null;
                }
                else if (i % 3 == 1 && _cardDesk[i] != null) // dizin aralık dışındaydı sorununu çöz
                {
                    _player2.AddCard(_cardDesk[i]);
                    _cardDesk[i] = null;
                }
                else if (i % 3 == 2 && _cardDesk[i] != null)
                {
                    _comp.player.AddCard(_cardDesk[i]);
                    _cardDesk[i] = null;
                }
            }
        }
        public static void BluffMenu()
        { // Blöf başlangıçta daha hoş görünmesi açısından Menü metodu.
            Console.OutputEncoding = Encoding.UTF8; // Özel karakterler için encoding UTF-8 olarak ayarladım.
            Console.Title = "Blöf Oyunu ♣♠♦♥";
            Console.ForegroundColor = ConsoleColor.Red; // Bunlar tamamem estetik açıdan :)

            Console.WriteLine();
            Console.WriteLine("                                 --------------------------                                 ");
            Console.WriteLine("                                 |  ♣♠♦♥ Blöf Oyunu ♣♠♦♥  |                                 ");
            Console.WriteLine("                                 --------------------------                                 ");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("|------------------------------------------------------------------------------------------|");
            Console.WriteLine("|                                                                                          |");
            Console.WriteLine("|  ► 2 kişilik blöf oyununa hoş geldiniz.                                                  |");
            Console.WriteLine("|  ► Üçüncü olarak bilgisayar oynuyor.                                                     |");
            Console.WriteLine("|  ► Oyun kuralları                                                                        |");
            Console.WriteLine("|  \t■ Birinci oyuncu başlar.                                                           |");
            Console.WriteLine("|  \t■ Oyun ilk kartları tükenen kişi olana kadar devam eder.                           |");
            Console.WriteLine("|  \t■ Her el sırası geldiğinde kişi kart atabilir, atmama hakkı da vardır.             |");
            Console.WriteLine("|  \t■ Her elde ayrıca önceki oyuncunun blöf yaptığını iddia edebilir.                  |");
            Console.WriteLine("|  \t■ Blöf ne zaman yapmalıyız.                                                        |");
            Console.WriteLine("|  \t\to Şayet önceki oyuncunun yalan söylediğini düşünüyorsan blöf demelisin.    |");
            Console.WriteLine("|  \t\to Emin değilsen ya da doğru olduğunu düşünüyorsan pas geç ya da kart at.   |");
            Console.WriteLine("|                                                                                          |");
            Console.WriteLine("|------------------------------------------------------------------------------------------|");
            Console.WriteLine();
            Console.WriteLine("");

            Console.ResetColor(); // BU metodu daha önce hiç kullanmadım biraz Console sınıfını kurcaladım. Rengi default değerine dönderiyor.
        }
    }
    
    // Card için de bir sınıf oluşturup onları da bir nesne haline getirecektim ama blöf oynamak için sayıları yeterli olduğu için yapma gereği duymadım.
}