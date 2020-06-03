using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseWork
{
    public class ConsoleUI
    {
        private Menu currentMenu;
        private Menu previousMenu;
        public Menu CurrentMenu
        {
            get { return currentMenu; }
            set { currentMenu = value; }
        }
        public Menu PreviousMenu
        {
            get { return previousMenu; }
            set { previousMenu = value; }
        }
        public ConsoleUI(CommandFactory factory)
        {
            currentMenu = new MainMenu(factory);
            previousMenu = null;
        }
    }
    public class MenuOption
    {
        private string description;
        public Command command;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public MenuOption(string description, Command command)
        {
            this.description = description;
            this.command = command;
        }
    }
    public abstract class Menu
    {
        private string text;
        protected List<MenuOption> options = new List<MenuOption>();
        protected Dictionary<string, MenuOption> otherOptions = new Dictionary<string, MenuOption>();
        public virtual string Text
        {
            get { return text; }
            set { text = value; }
        }
        public virtual void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine(text);
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + options[i].Description);
            }
            Console.WriteLine();
            for (int i = 0; i < otherOptions.Count; i++)
            {
                Console.WriteLine(otherOptions.ElementAt(i).Value.Description);
            }
            Console.WriteLine("(Q) - Quit");
        }
        public virtual bool ProcessInput()
        {
            string input = Console.ReadKey(true).KeyChar.ToString().ToUpper();
            if (input == "Q")
            {
                return false;
            }
            else
            {
                if (otherOptions.ContainsKey(input))
                {
                    Command command = otherOptions[input].command;
                    command.Execute();
                }
                else
                {
                    int number;
                    bool isNumber = Int32.TryParse(input, out number);
                    if (isNumber && (number > 0 && number <= options.Count))
                    {
                        Command command = options[number - 1].command;
                        command.Execute();
                    }
                    else
                    {
                        ProcessInput();
                    }
                }
                return true;
            }
        }
        public virtual bool ShowMenu()
        {
            PrintMenu();
            return ProcessInput();
        }
    }
    class MainMenu : Menu
    {
        public MainMenu(CommandFactory factory)
        {
            Text =
                "\n" +
                "Welcome to BetaBank!\n" +
                "===========================\n" +
                "Press [1) to set withdraw sum\n" +
                "Press [2) to view Withdraw options\n" +
                "Press [3) to view PIN options\n" +
                "Press [Q) to quit\n" +
                "\n\n";
            RouteCommand command = (RouteCommand)factory.CreateCommand(COMMAND.ROUTE);
            command.MenuCreator = new SumMenuCreator();
            command.PreviousMenuCreator = new MainMenuCreator();
            options.Add(new MenuOption("Sum To Withdraw", command));
            command = (RouteCommand)factory.CreateCommand(COMMAND.ROUTE);
            command.MenuCreator = new WithdrawMenuCreator();
            command.PreviousMenuCreator = new MainMenuCreator();
            options.Add(new MenuOption("My Withdraw", command));
            command = (RouteCommand)factory.CreateCommand(COMMAND.ROUTE);
            command.MenuCreator = new PINMenuCreator();
            command.PreviousMenuCreator = new MainMenuCreator();
            options.Add(new MenuOption("Change PIN", command));
        }
    }
    public interface IPagination
    {
        int Page
        {
            get; set;
        }
        void SetPage(int page, Terminal termItems, CommandFactory factory);
    }
    class WithdrawMenu : Menu,
        IPagination
    {
        private int page = 0;
        public int Page
        {
            get { return page; }
            set { page = value; }
        }
        public WithdrawMenu(Terminal terminal, CommandFactory factory)
        {
            Text = "Current balance: " + terminal.user.Balance+"\n"+
                "====================\n" +
                "Press [1)-[9) to remove banknote;\n" +
                "Press [W) to withdraw money;\n" +
                "Press [B) to return to Main Menu\n" +
                "====================\n\n";

            SetPage(0, terminal, factory);
            otherOptions.Add("P", new MenuOption("(P) - Prev", (PreviousCommand)factory.CreateCommand(COMMAND.PREVIOUS)));
            otherOptions.Add("N", new MenuOption("(N) - Next", (NextCommand)factory.CreateCommand(COMMAND.NEXT)));
            otherOptions.Add("W", new MenuOption("(W) - Withdraw", (WithdrawCommand)factory.CreateCommand(COMMAND.WITHDRAW)));
            BackCommand backCommand = (BackCommand)factory.CreateCommand(COMMAND.BACK);
            backCommand.PreviousMenuCreator = new MainMenuCreator();
            otherOptions.Add("B", new MenuOption("(B) - Back", backCommand));
        }
        public void SetPage(int page, Terminal term, CommandFactory fact)
        {
            List<Item> items = term.user.Withdraw;
            int sum = 0;
            foreach (Banknote banknote in items)
            {
                sum += banknote.Facevalue;
            }
            if ((page * 9 < items.Count) && (page >= 0))
            {
                Page = page;
                options.Clear();
                int counter = 0;
                for (int i = Page * 9; (i < items.Count && counter < 9); i++)
                {
                    RemoveFromWithdrawCommand command = (RemoveFromWithdrawCommand)fact.CreateCommand(COMMAND.REMOVE_FROM_WITHDRAW);
                    command.Item = items[i];
                    options.Add(new MenuOption(items[i].DisplayInfo(items[i]), command));
                    counter++;
                }
                options.Add(new MenuOption("(Total: " + sum + " )", (RemoveFromWithdrawCommand)fact.CreateCommand(COMMAND.REMOVE_FROM_WITHDRAW)));
            }
        }
    }
    class PINMenu : Menu
    {
        public PINMenu(Terminal terminal, CommandFactory fact)
        {
            Text = "Current balance: " + terminal.user.Balance + "\n" +
                "====================\n" +
                "Press [C) to change PIN;\n" +
                "Press [B) to return to Main Menu\n" +
                "====================\n\n";
            otherOptions.Add("C", new MenuOption("(C) - Change PIN", (ChangePINCommand)fact.CreateCommand(COMMAND.CHANGE_PIN)));
            BackCommand backCommand = (BackCommand)fact.CreateCommand(COMMAND.BACK);
            backCommand.PreviousMenuCreator = new MainMenuCreator();
            otherOptions.Add("B", new MenuOption("(B) - Back", backCommand));
        }
    }
    class SumMenu : Menu
    {
        public SumMenu(Terminal terminal, CommandFactory factory)
        {
            Text = "Current balance: " + terminal.user.Balance + "\n" +
                "====================\n" +
                "Press [E) to choose sum;\n" +
                "Press [B) to return to Main Menu\n" +
                "====================\n\n";

            otherOptions.Add("E", new MenuOption("(E) - Enter sum", (ChooseSumCommand)factory.CreateCommand(COMMAND.CHOOSE_SUM)));

            BackCommand backCommand = (BackCommand)factory.CreateCommand(COMMAND.BACK);
            backCommand.PreviousMenuCreator = new MainMenuCreator();
            otherOptions.Add("B", new MenuOption("(B) - Back", backCommand));
        }
    }

    //Decorator / Observer
    class NotifyMenu : Menu, IPagination, IEventListener
    {
        private Menu menu;
        private string notifier;
        public NotifyMenu(Menu menu)
        {
            this.menu = menu;
            notifier = null;
        }
        public override string Text
        {
            get { return menu.Text; }
            set { menu.Text = value; }
        }
        public int Page
        {
            get
            {
                IPagination pageMenu = (IPagination)menu;
                return pageMenu.Page;
            }
            set
            {
                IPagination pageMenu = (IPagination)menu;
                pageMenu.Page = value;
            }
        }
        public override bool ShowMenu()
        {
            menu.PrintMenu();
            if (notifier != null)
            {
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine(notifier);
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~");
                notifier = null;
            }
            return menu.ProcessInput();
        }
         public void update(EVENT_TYPE ev, string info)
        {
            notifier = info;
        }
        public void SetPage(int page, Terminal terminal, CommandFactory factory)
        {
            IPagination pageMenu = (IPagination)menu;
            pageMenu.SetPage(page, terminal, factory);
        }
    }
}