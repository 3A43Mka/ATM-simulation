using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWork
{
    //Factory Method / Observer
    public interface IMenuFactory
    {
        Menu createMenu(ATM atm);
    }
    public class MainMenuCreator : IMenuFactory
    {
        public Menu createMenu(ATM atm)
        {
            return new MainMenu(atm.Factory);
        }
    }
    public class WithdrawMenuCreator : IMenuFactory
    {
        public Menu createMenu(ATM atm)
        {
            WithdrawMenu menu = new WithdrawMenu(atm.Terminal, atm.Factory);
            NotifyMenu notifyMenu = new NotifyMenu(menu);
            atm.eventManager.subscribe(EVENT_TYPE.REMOVEFROMWITHDRAW, notifyMenu);
            atm.eventManager.subscribe(EVENT_TYPE.WITHDRAW, notifyMenu);
            return notifyMenu;
        }
    }
    public class PINMenuCreator : IMenuFactory
    {
        public Menu createMenu(ATM atm)
        {
            PINMenu menu = new PINMenu(atm.Terminal, atm.Factory);
            NotifyMenu notifyMenu = new NotifyMenu(menu);
            atm.eventManager.subscribe(EVENT_TYPE.CHANGEPIN, notifyMenu);
            return notifyMenu;
        }
    }
    public class SumMenuCreator : IMenuFactory
    {
        public Menu createMenu(ATM atm)
        {
            SumMenu menu = new SumMenu(atm.Terminal, atm.Factory);
            NotifyMenu notifyMenu = new NotifyMenu(menu);
            atm.eventManager.subscribe(EVENT_TYPE.CHOOSESUM, notifyMenu);
            return notifyMenu;
        }
    }
}
