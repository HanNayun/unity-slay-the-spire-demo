public class DrawCardGA : GameAction
{
    public int Amount { get; set; }

    public DrawCardGA(int count)
    {
        Amount = count;
    }
}