using System.Security.Cryptography;

namespace TradingApp;

class Trade
{
    public User To;
    public User From;
    public Item RequestedItem;
    public Item OfferedItem;
    public TradingStatus Status;

    public Trade(User to, User from, Item requesteditem, Item offereditem)
    {
        To = to;
        From = from;
        RequestedItem = requesteditem;
        OfferedItem = offereditem;
        Status = TradingStatus.Pending;
    }

    public string Info3()
    {
        return To.Items + " -> " + From.Items;
    }


    public enum TradingStatus
    {
        None,
        Pending,
        Denied,
        Accepted,
        Completed,
    }
}