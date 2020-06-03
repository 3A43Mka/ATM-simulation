using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseWork
{
    // Facade / Observer
    public class ATM
    {
        private Terminal terminal;
        private ConsoleUI cui;
        private CommandFactory factory;
        public EventManager eventManager;
        public Terminal Terminal
        {
            get { return terminal; }
        }
        public ConsoleUI CUI
        {
            get { return cui; }
        }
        public CommandFactory Factory
        {
            get { return factory; }
        }
        public ATM(User user)
        {
            terminal = new Terminal(user);
            factory = new CommandFactory(this);
            cui = new ConsoleUI(factory);
            eventManager = new EventManager(EVENT_TYPE.ADDTOWITHDRAW, EVENT_TYPE.REMOVEFROMWITHDRAW, EVENT_TYPE.WITHDRAW, EVENT_TYPE.CHANGEPIN, EVENT_TYPE.CHOOSESUM);
        }
        public bool ShowMenu()
        {
            return cui.CurrentMenu.ShowMenu();
        }
        public void AddToATM(Item item)
        {
            terminal.AddToATM(item);
        }
        public void AddToWithdraw(Item item)
        {
            bool isAdded = terminal.user.AddToWithdraw(item);
            if (isAdded)
            {
                eventManager.notify(EVENT_TYPE.ADDTOWITHDRAW, "You have added " + item.Name + " to your cart");
            } else
            {
                eventManager.notify(EVENT_TYPE.ADDTOWITHDRAW, "Sorry you are not allowed to buy this");
            }
        }
        public void RemoveFromCart(Item item)
        {
            if (item != null)
            {
                terminal.user.RemoveFromWithdraw(item);
                SetCurrentMenu(new WithdrawMenuCreator());
                eventManager.notify(EVENT_TYPE.REMOVEFROMWITHDRAW, item.Name + " was removed.");
            }
        }
        public void WithdrawMoney()
        {
            bool hasBought = terminal.user.WithdrawMoney();
            SetCurrentMenu(new WithdrawMenuCreator());
            if (hasBought)
            {
                eventManager.notify(EVENT_TYPE.WITHDRAW, "Please, take your banknotes. Have a nice day!");
            } else
            {
                eventManager.notify(EVENT_TYPE.WITHDRAW, "No money were dispatched.");
            }
        }
        public void ChangePIN()
        {
            bool haschanged = terminal.user.ChangePIN();
            SetCurrentMenu(new PINMenuCreator());
            if (haschanged)
            {
                eventManager.notify(EVENT_TYPE.CHANGEPIN, "You have changed PIN!");
            }
            else
            {
                eventManager.notify(EVENT_TYPE.CHANGEPIN, "You haven't changed PIN.");
            }
        }
        public void ChooseSum()
        {
            bool success = true;
            Console.WriteLine("Type in your sum below:\n" +
                "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            string input = Console.ReadLine();
            int sum = 0;
            success = Int32.TryParse(input, out sum);
            int temp = sum;
            if (sum > terminal.user.Balance)
            {
                success = false;
            }
            else {
                List<Item> buffer = new List<Item>();
                while (temp != 0)
                {
                    for (int i = 0; i < terminal.Banknotes.Count; i++)
                    {
                        while ((temp - terminal.Banknotes[i].Facevalue) >= 0)
                        {
                            temp -= terminal.Banknotes[i].Facevalue;
                            buffer.Add(terminal.Banknotes[i]);
                        }
                    }
                    if (temp != 0)
                    {
                        success = false;
                        break;
                    } else
                    {
                        foreach (Banknote banknote in buffer)
                        {
                            terminal.user.AddToWithdraw(banknote);
                        }
                    }
                }
            }
            SetCurrentMenu(new SumMenuCreator());
            if (success)
            {
                eventManager.notify(EVENT_TYPE.CHOOSESUM, "You have chosen sum to withdraw!");
            }
            else
            {
                eventManager.notify(EVENT_TYPE.CHOOSESUM, "Sorry, your entered sum can't be withdrawed.");
            }
        }
        public void SetCurrentMenu()
        {
            cui.CurrentMenu = cui.PreviousMenu;
        }
        public void SetCurrentMenu(IMenuFactory currentMenuCreator)
        {
            cui.CurrentMenu = currentMenuCreator.createMenu(this);
        }
        public void SetPreviousMenu(IMenuFactory previousMenuCreator)
        {
            cui.PreviousMenu = previousMenuCreator.createMenu(this);
        }
        public void PreviousPage()
        {
            IPagination menu = (IPagination)cui.CurrentMenu;
            menu.SetPage(menu.Page - 1, terminal, factory);
        }
        public void NextPage()
        {
            IPagination menu = (IPagination)cui.CurrentMenu;
            menu.SetPage(menu.Page + 1, terminal, factory);
        }
    }
}
