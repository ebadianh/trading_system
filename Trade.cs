using System.Security.Cryptography;

namespace TradingApp;

class Trade
{
    public int TradeID;
    public User From;
    public User To;
    public Item RequestedItem;
    public Item OfferedItem;
    public TradingStatus Status;

    public Trade(User from, User to, Item requesteditem, Item offereditem)
    {
        From = from;
        To = to;
        RequestedItem = requesteditem;
        OfferedItem = offereditem;
        Status = TradingStatus.Pending;
    }

    public Trade(int tradeid, User from, User to, Item requesteditem, Item offereditem, TradingStatus status)
    {
        TradeID = tradeid;
        From = from;
        To = to;
        RequestedItem = requesteditem;
        OfferedItem = offereditem;
        Status = status;
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