using Figures.Core.Domain;

namespace Figures.Web.Models
{
    public class Position
    {
        public FigureType Type { get; set; }

        public float SideA { get; set; }
        public float SideB { get; set; }
        public float SideC { get; set; }

        public int Count { get; set; }
    }
}