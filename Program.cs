using System;

namespace CourseWork
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxyUser user = new ProxyUser(0, "0000", new UsedUserState() );
            user.LoadSave();
            ATM atm = new ATM(user);
            atm.AddToATM(new Banknote("Five Hundred Hrivnas", 500, new WithdrawDisplay()));
            atm.AddToATM(new Banknote("Two Hundred Hrivnas", 200, new WithdrawDisplay()));
            atm.AddToATM(new Banknote("One Hundred Hrivnas", 100, new WithdrawDisplay()));
            atm.AddToATM(new Banknote("Fifty Hrivnas", 50, new WithdrawDisplay()));
            atm.AddToATM(new Banknote("Twenty Hrivnas", 20, new WithdrawDisplay()));
            atm.AddToATM(new Banknote("Ten Hrivnas", 10, new WithdrawDisplay()));
            bool inMenu = user.Authenticate();
            if (!inMenu)
            {
                user.State = new FailedUserState();
            }
            while (inMenu)
            {
                inMenu = atm.ShowMenu();
            }
            user.Save();
            Console.Clear();
            user.PrintCheck();
            Console.ReadKey();
        }
    }
}
