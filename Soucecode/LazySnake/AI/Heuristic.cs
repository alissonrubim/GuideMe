using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySnake
{
    class Heuristic
    {
        public void CalcuatesCost(Track currentTrack, Vertex targetVertex, double value)
        {
            if (currentTrack.Preview == null)
                currentTrack.CostG = value;
            else
                currentTrack.CostG = currentTrack.Preview.CostG + value;

            double a = currentTrack.CurrentVertex.RowIndex - targetVertex.RowIndex;
            double b = currentTrack.CurrentVertex.ColIndex - targetVertex.ColIndex;

            currentTrack.CostH = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }
    }
}
