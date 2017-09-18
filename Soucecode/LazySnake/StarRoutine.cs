using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySnake
{
    class StarRoutine
    {
        public bool Start(Map map, Vertex origin, Vertex target, Heuristic heuristic, out List<Vertex> path)
        {
            TrackQueue opened = new TrackQueue();
            Dictionary<Vertex, Track> closed = new Dictionary<Vertex, Track>();

            path = null;

            if (origin == null || target == null || heuristic == null)                
                return false;

            Track currentTrack = new Track(origin, null);
            heuristic.CalcuatesCost(currentTrack, target, 0);

            opened.Add(currentTrack);

            while(currentTrack != null)
            {
                if (currentTrack.CurrentVertex.Equals(target))
                {
                    //path = 
                    return true;
                }
                else
                {
                    closed.Add(currentTrack.CurrentVertex, currentTrack);
                    opened.Remove(currentTrack);
                }
            }

            return false;
        }
    }
}
