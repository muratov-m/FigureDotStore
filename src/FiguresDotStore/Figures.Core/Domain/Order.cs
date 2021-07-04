using System.Collections.Generic;
using System.Linq;

namespace Figures.Core.Domain
{
    public class Order
    {
        public List<OrderPosition> Positions { get; set; }

        public decimal GetTotal() => Positions.Sum(x => x.GetSubTotal());
    }
}