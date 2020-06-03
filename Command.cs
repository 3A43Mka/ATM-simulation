using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWork
{
    public abstract class Command
    {
        protected ATM _atm;
        public Command(ATM atm)
        {
            _atm = atm;
        }
        public abstract void Execute();
    }
    class RouteCommand: Command
    {
        private IMenuFactory menuCreator;
        private IMenuFactory previousMenuCreator;
        public IMenuFactory MenuCreator
        {
            get { return menuCreator; }
            set { menuCreator = value; }
        }
        public IMenuFactory PreviousMenuCreator
        {
            get { return previousMenuCreator; }
            set { previousMenuCreator = value; }
        }
        public RouteCommand(ATM atm):base(atm)
        {
        }
        public override void Execute()
        {
            if (menuCreator!=null)
            {
                _atm.SetCurrentMenu(menuCreator);
            }
            if (PreviousMenuCreator != null)
            {
                _atm.SetPreviousMenu(previousMenuCreator);
            }
        }
    }
    class AddToWithdrawCommand: Command
    {
        private Item item;
        public Item Item
        {
            get { return item; }
            set { item = value; }
        }
        public AddToWithdrawCommand(ATM atm) :base(atm)
        {
        }
        public override void Execute()
        {
            _atm.AddToWithdraw(item);
        }
    }
    class BackCommand : Command
    {
        private IMenuFactory previousMenuCreator;
        public IMenuFactory PreviousMenuCreator
        {
            get { return previousMenuCreator; }
            set { previousMenuCreator = value; }
        }
        public BackCommand(ATM atm):base(atm)
        {
        }
        public override void Execute()
        {
            _atm.SetCurrentMenu();
            if (PreviousMenuCreator != null)
            {
                _atm.SetPreviousMenu(previousMenuCreator);
            }
        }
    }
    class RemoveFromWithdrawCommand: Command
    {
        private Item item;
        public Item Item
        {
            get { return item; }
            set { item = value; }
        }
        public RemoveFromWithdrawCommand(ATM atm): base(atm)
        {
        }
        public override void Execute()
        {
            _atm.RemoveFromCart(item);
        }
    }
    class NextCommand: Command
    {
        public NextCommand(ATM atm) : base(atm)
        {
        }
        public override void Execute()
        {
            _atm.NextPage();
        }
    }
    class PreviousCommand: Command
    {
        public PreviousCommand(ATM atm) : base(atm)
        {
        }
        public override void Execute()
        {
            _atm.PreviousPage();
        }
    }
    class WithdrawCommand: Command
    {
        public WithdrawCommand(ATM atm) : base(atm)
        {
        }
        public override void Execute()
        {
            _atm.WithdrawMoney();
        }
    }
    class ChangePINCommand : Command
    {
        public ChangePINCommand(ATM atm) : base(atm)
        {
        }
        public override void Execute()
        {
            _atm.ChangePIN();
        }
    }
    class ChooseSumCommand : Command
    {
        public ChooseSumCommand(ATM atm) : base(atm)
        {
        }
        public override void Execute()
        {
            _atm.ChooseSum();
        }
    }
}
