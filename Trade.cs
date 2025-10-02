using System.Security.Cryptography;

namespace TradingApp;

class Trade
{
    public User To;
    public User From;
    public Item Item;
    public TradingStatus Status;

    public Trade(User to, User from, Item item)
    {
        To = to;
        From = from;
        Item = item;
        Status = TradingStatus.Pending;
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