using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySnake
{
    class Track
    {
        public Vertex CurrentVertex;
        public Track Preview;
        public double CostG;
        public double CostH;

        public Track(Vertex vertex, Track preview)
        {
            this.CurrentVertex = vertex;
            this.Preview = preview;
        }

        public double TotalCost
        {
            get
            {
                return CostG + CostH;
            }
        }

    }
}
