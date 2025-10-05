using System.Reflection.Metadata.Ecma335;

namespace TradingApp;

class Item
{
    public string ItemName;
    public string Description;
    public User Owner;


    public Item(string itemname, string description, User owner)
    {
        ItemName = itemname;
        Description = description;
        Owner = owner;
    }


    public string Info()
    {
        return Owner.Email + ": " + ItemName + " - " + Description;
    }

    public string Info2()
    {
        return ItemName + " " + Description;
    }

}
