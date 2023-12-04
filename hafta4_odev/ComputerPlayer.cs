using System;
using System.Collections;

namespace hafta4_odev
{
    internal class ComputerPlayer
    {
        public Player player;
        public ArrayList cardMemory;
        public ComputerPlayer() // InnerType yapmış oldum.
                                // (Önceki ödevlerde de bilinçsiz olarak kullanmıştım sanıyorsam ama şuan fark ettiğim için açıklamak istedim.)
                                // Bu OOP özelliği ile bir sınıfta var olan ve bu sınıfta zaten kullanmam gereken noktaları kullanmamı sağladı.
                                // O field ve methot'ları yeniden tanımlamam gerekmedi. 
        {         // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            player = new Player(); // ComputerPlayer nesnesi oluşturduğunda sınıf içi sınıf(player) gibi olduğu temel olarak denebilir.
                                   // Player içinde burada da tanımlamamız gereken field veya metotlar var. Bunları player içinden dahil ettim.
            player.name = "Bilgisayar";
            player.playerDesk = new ArrayList(); 
            cardMemory = new ArrayList(); // Blöfe karar verirken karşılaştıracak o tipteki kaç defa atılmış. Duruma göre mesela: 2A atıldığı iddia ama memory'ye göre 2A atmak mümkün değilse blöf demeli
            foreach (string card in Bluff.cards)
                cardMemory.Add(card);
        }
    }
}
