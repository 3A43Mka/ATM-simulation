using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWork
{
    public enum ITEM_TYPE
    {
        ITEM = 0,
        BANKNOTE = 1,
    }
    public interface IDisplay
    {
        string DisplayInfo(string name, int facevalue);
    }
    class WithdrawDisplay : IDisplay
    {
        public string DisplayInfo(string name, int facevalue)
        {
            return $"-- {name} ({facevalue})";
        }
    }
    class OnCheckDisplay : IDisplay
    {
        public string DisplayInfo(string name, int facevalue)
        {
            return $"+ {facevalue} -- {name}";
        }
    }
    public abstract class Item
    {
        private string name;
        private int facevalue;
        protected ITEM_TYPE type;
        private IDisplay _display;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Facevalue
        {
            get { return facevalue; }
            set { facevalue = value; }
        }

        public ITEM_TYPE Type
        {
            get { return type; }
            set { type = value; }
        }
        public void SetDisplay(IDisplay display)
        {
            this._display = display;
        }
        public string DisplayInfo(Item item)
        {
            return this._display.DisplayInfo(item.Name, item.Facevalue);
        }
        public Item(string name, int facevalue, IDisplay display)
        {
            Name = name;
            Facevalue = facevalue;
            type = ITEM_TYPE.ITEM;
            _display = display;
        }
        public override bool Equals(object other)
        {
            Item item = other as Item;
            if (item.Name == name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }
    
    public class Banknote: Item
    {
        
        public Banknote(string name, int facevalue, IDisplay display) : base(name, facevalue, display)
        {
            type = ITEM_TYPE.BANKNOTE;
        }
    }
}
