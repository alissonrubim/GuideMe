using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySnake
{
    class Vertex
    {
        public int RowIndex;
        public int ColIndex;

        public Vertex(int r, int c)
        {
            this.RowIndex = r;
            this.ColIndex = c;
        }

        public override bool Equals(object obj)
        {
            if(obj is Vertex)
            {
                return ((Vertex)obj).ColIndex == this.ColIndex && ((Vertex)obj).RowIndex == this.RowIndex;
            }
            return false;
        }
    }
}
