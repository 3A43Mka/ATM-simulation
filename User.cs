using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace CourseWork
{
    // Proxy / State / Memento
    public class User
    {
        private int balance;
        private string pin;
        private List<Item> inventory = new List<Item>();
        private List<Item> withdraw = new List<Item>();
        public virtual int Balance
        {
            get { return balance; }
        }
        public virtual string PIN
        {
            get { return pin; }
            set { pin = value; }
        }
        public virtual List<Item> Withdraw
        {
            get { return withdraw.ToList(); }
        }
        public virtual List<Item> Inventory
        {
            get { return inventory.ToList(); }
        }
        public User()
        {
            balance = 0;
            pin = "0000";
        }
        public User(int money, string pin)
        {
            this.balance = money;
            this.pin = pin;
        }
        public virtual bool Authenticate()
        {
            return true;
        }
        public virtual void Save()
        {
            UserSave newSave = new UserSave(balance, pin);
            string saveString;
            saveString = System.Text.Json.JsonSerializer.Serialize(newSave);
            File.WriteAllText("savefile.txt", saveString);
        }
        public virtual void LoadSave()
        {
            string saveString = File.ReadAllText("savefile.txt");
            UserSave loadedSave = JsonConvert.DeserializeObject<UserSave>(saveString);
            this.balance = loadedSave.Balance;
            this.pin = loadedSave.PIN;
        }
        public virtual bool AddToWithdraw(Item item)
        {
            withdraw.Add(item);
            return true;
        }
        public virtual void RemoveFromWithdraw(Item item)
        {
            withdraw.Remove(item);
        }
        public virtual bool WithdrawMoney()
        {
            if (withdraw.Count == 0)
            {
                return false;
            }
            else
            {
                int counter = 0;
                foreach (Item item in withdraw)
                {
                    counter += item.Facevalue;
                }
                if (balance < counter)
                {
                    return false;
                }
                else
                {
                    balance -= counter;
                    inventory.AddRange(withdraw);
                    withdraw.Clear();
                    return true;
                }
            }
        }
        public virtual bool ChangePIN()
        {
            Console.WriteLine("\nOK. Type your new PIN below:\n" +
                "(Note, that PIN must consist only of 4 digits)\n" +
                "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            string newPIN = Console.ReadLine();
            bool check = true;
            check = Int32.TryParse(newPIN, out _);
            if ((newPIN.Length != 4) || (!check))
            {
                return false;
            }
            else
            {
                PIN = newPIN;
                Console.WriteLine(PIN);
                return true;
            }
        }
        public virtual void PrintCheck()
        {
            Console.WriteLine("Empty");
        }
    }
    // Memento
    public class UserSave
    {
        public int Balance { get; private set; }
        public string PIN { get; private set; }
        public UserSave(int balance, string pin)
        {
            this.Balance = balance;
            this.PIN = pin;
        }
    }
    // State
    public interface IUserState
    {
        void PrintCheck(ProxyUser user);
    }
    class UsedUserState : IUserState
    {
        public void PrintCheck(ProxyUser user)
        {
            Console.WriteLine("\n");
            Console.WriteLine("Money on a balance left:");
            Console.WriteLine("--" + user.Balance);
            Console.WriteLine("Banknotes on hands:");
            if (user.Inventory.Count == 0)
            {
                Console.WriteLine("--Empty--");
            }
            else
            {
                int total = 0;
                foreach (Item item in user.Inventory)
                {
                    total += item.Facevalue;
                    item.SetDisplay(new OnCheckDisplay());
                    Console.WriteLine(item.DisplayInfo(item));
                }
                Console.WriteLine("-----------------\n");
                Console.WriteLine(total+ " hrivnas total");
            }
            Console.WriteLine("-----------------\n");
            Console.WriteLine("Thank you for choosing BetaBank!\n");
        }
    }
    class FailedUserState : IUserState
    {
        public void PrintCheck(ProxyUser user)
        {
            Console.WriteLine("\n");
            Console.WriteLine("You are empty handed. You failed to get correct login.");
        }
    }
    // Proxy
    public class ProxyUser : User
    {
        private User user;
        public IUserState State { get; set; }
        public override int Balance
        {
            get { return user.Balance; }
        }
        public override List<Item> Withdraw
        {
            get { return user.Withdraw; }
        }
        public override List<Item> Inventory
        {
            get { return user.Inventory; }
        }
        public override string PIN
        {
            get { return user.PIN; }
        }
        public ProxyUser(int balance, string pin, IUserState state)
        {
            user = new User(balance, pin);
            this.State = state;
        }
        public ProxyUser(User user)
        {
            this.user = user;
            this.State = new FailedUserState();
        }
        public override bool Authenticate()
        {
            Console.WriteLine("Welcome.\nEnter your PIN code, please.");
            string check = "";
            char inpchar;
            while (check.Length <= 3)
            {
                inpchar = Console.ReadKey(true).KeyChar;
                check += inpchar;
                Console.Write("*");
            }
            Console.WriteLine();
            if (check == user.PIN)
            {
                return user.Authenticate();
            }
            else
            {
                Console.WriteLine("Wrong PIN.");
                Console.ReadKey();
                return false;
            }
        }
        public override void Save()
        {
            user.Save();
        }
        public override void LoadSave()
        {
            user.LoadSave();
        }
        public override bool AddToWithdraw(Item item)
        {
            if (item.Type == ITEM_TYPE.BANKNOTE)
            {
                return user.AddToWithdraw(item);
            }
            return false;
        }
        public override void RemoveFromWithdraw(Item item)
        {
            user.RemoveFromWithdraw(item);
        }
        public override bool WithdrawMoney()
        {
            return user.WithdrawMoney();
        }
        public override bool ChangePIN()
        {
            return user.ChangePIN();
        }
        public override void PrintCheck()
        {
            State.PrintCheck(this);
        }
    }
}
