using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWork
{
    public enum COMMAND
    {
        ROUTE,
        BACK,
        ADD_TO_WITHDRAW,
        REMOVE_FROM_WITHDRAW,
        NEXT,
        PREVIOUS,
        CHOOSE_SUM,
        WITHDRAW,
        CHANGE_PIN,
        QUIT,
    }
    public class CommandFactory
    {
        private ATM _atm;
        public CommandFactory(ATM atm)
        {
            this._atm = atm;
        }
        public Command CreateCommand(COMMAND command)
        {
            switch (command)
            {
                case COMMAND.ROUTE:
                    return new RouteCommand(_atm);
                case COMMAND.ADD_TO_WITHDRAW:
                    return new AddToWithdrawCommand(_atm);
                case COMMAND.BACK:
                    return new BackCommand(_atm);
                case COMMAND.REMOVE_FROM_WITHDRAW:
                    return new RemoveFromWithdrawCommand(_atm);
                case COMMAND.NEXT:
                    return new NextCommand(_atm);
                case COMMAND.PREVIOUS:
                    return new PreviousCommand(_atm);
                case COMMAND.WITHDRAW:
                    return new WithdrawCommand(_atm);
                case COMMAND.CHANGE_PIN:
                    return new ChangePINCommand(_atm);
                case COMMAND.CHOOSE_SUM:
                    return new ChooseSumCommand(_atm);
                default:
                    throw new ArgumentException("Wrong command");
            }
        }
    }
}
