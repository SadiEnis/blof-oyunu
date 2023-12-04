using System;
using System.Collections;

namespace hafta4_odev
{
    internal class Player
    {
        public string name;
        public ArrayList playerDesk;
        public Player(string _name)
        {
            name = _name;
            playerDesk = new ArrayList();
        }
        public Player()
        {}
        public void AddCard(object _card)
        { // Oyuncunun destesine bir kart ekler.
            playerDesk.Add(_card);
        }
        public void RemoveCard(object _card)
        { // Oyuncunun destesinden bir kart siler.
            playerDesk.Remove(_card);
        }
        public bool SayBluff(ArrayList _placedCards, int _bluffCardsPcs, string _bluffCardsType) // Doğru mu yanış mı geri dönüş sağlamalı.
        { // Blöf dendiği zaman doğru olup olmadığını kontrol eder.
            bool _bluff = false;
            for (int i = 1; i <= _bluffCardsPcs; i++)
            { // i 1'ken count-1 oluyor ArrayList'in son değerine bakar, 2 olduğunda ise bir önceki vs.
              // Sonuç olarak son atılan kart sayısı kadar for dönecek ve kontrol edilecek blöf mü.
                if (_placedCards[_placedCards.Count - i].ToString() == _bluffCardsType)
                    _bluff = true;
                else
                {
                    _bluff = false;
                    break;
                    // Blöf sonucu şayet atan kişinin attığını iddia ettiği kart ile aynı ise true dönecek ama bir tane bile olsa farklı ise false olup for'dan çıkacak
                    // Bu durumda dönecek sonuç hepsi true olmadıkça true dönemeyecek.
                }
            }return _bluff;
        }
    }
}
