using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseWork
{
    public class Terminal
    {
        private List<Banknote> banknotes = new List<Banknote>();
        public User user;
        public List<Banknote> Banknotes
        {
            get { return banknotes; }
        }
        public Terminal(User user)
        {
            this.user = user;
        }
        public Terminal(User user, List<Banknote> banknotes) : this(user)
        {
            this.banknotes = banknotes;
        }
        public void AddToATM(Item item)
        {
            switch (item.Type)
            {
                case ITEM_TYPE.BANKNOTE:
                    if (!banknotes.Contains(item))
                    {
                        banknotes.Add((Banknote)item);
                        banknotes.Sort((x, y) => y.Facevalue.CompareTo(x.Facevalue));
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
