using System.Reflection.Metadata.Ecma335;

namespace TradingApp;

class Item
{
    string ItemName;
    string Description;

    public Item(string itemname, string description)
    {
        ItemName = itemname;
        Description = description;
    }

    public string Info()
    {
        return ItemName + ":" + " " + Description;
    }
}
