using System.Collections.Generic;
using Zenject;

namespace Items
{
    public class DefaultItemProvider : IItemProvider
    {
        [Inject]
        private ItemConfig Config { get; }

        public IEnumerable<Item> Items => Config.Items;
    }

    public interface IItemProvider
    {
        IEnumerable<Item> Items { get; }
    }
}