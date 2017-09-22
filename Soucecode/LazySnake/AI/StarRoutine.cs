using LazySnake.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySnake
{
    class StarRoutine
    {
        public bool Start(GameMap map, Vertex origin, Vertex target, Heuristic heuristic, out List<Vertex> path)
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
                    path = getPath(currentTrack);
                    return true;
                }
                else
                {
                    closed.Add(currentTrack.CurrentVertex, currentTrack);
                    opened.Remove(currentTrack);

                    for(int i = currentTrack.CurrentVertex.RowIndex - 1; i <= currentTrack.CurrentVertex.RowIndex + 1; i++)
                    {
                        if (i >= 0 && i < map.GetSize().Rows)
                            for (int j = currentTrack.CurrentVertex.ColIndex - 1; j <= currentTrack.CurrentVertex.ColIndex + 1; j++)
                                if (j >= 0 && j < map.GetSize().Cols)
                                    if (currentTrack.CurrentVertex.RowIndex != i || currentTrack.CurrentVertex.ColIndex != j) {
                                        GameObject gameObject = map.GetGameObjectAt(i, j);
                                        if (gameObject == null || gameObject.Type != GameObject.GameObjectType.Wall)
                                            addVertex(opened, closed, currentTrack, target, heuristic, i, j);
                                    }
                    }
                    currentTrack = opened.First;
                }
            }

            return false;
        }

        private List<Vertex> getPath(Track currentTrack)
        {
            List<Vertex> list = new List<Vertex>();
            Track aux = currentTrack;

            while(aux != null)
            {
                list.Add(aux.CurrentVertex);
                aux = aux.Preview;
            }
            return list;
        }

        private void addVertex(TrackQueue opened, Dictionary<Vertex, Track> closed, Track currentTrack, Vertex target, Heuristic heuristic, int i, int j)
        {
            Vertex v = new Vertex(i, j);
            if (!closed.ContainsKey(v))
            {
                Track t = new Track(v, currentTrack);
                if (i != currentTrack.CurrentVertex.RowIndex && j != currentTrack.CurrentVertex.ColIndex)
                    heuristic.CalcuatesCost(t, target, 14.14); //diagonal
                else
                    heuristic.CalcuatesCost(t, target, 10); //andando

                if (!opened.HasVertex(v))
                    opened.Add(t);
                else
                {
                    Track aux = opened.Get(v);
                    if(aux.TotalCost > t.TotalCost)
                    {
                        opened.Remove(aux);

                        aux.Preview = t.Preview;
                        aux.CostG = t.CostG;
                        aux.CostH = t.CostH;

                        opened.Add(aux);
                    }
                }
            }
        }
    }
}
