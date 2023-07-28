namespace Items
{
    public class Transaction
    {
        public string Source { get; }
        public IPack Pack { get; }

        public Transaction(string source, IPack pack)
        {
            Source = source;
            Pack = pack;
        }
    }
}