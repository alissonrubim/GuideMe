using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySnake
{
    class TrackQueue //Lista ordenada
    {
        private LinkedList<Track> queue = new LinkedList<Track>();

        public Track First
        {
            get
            {
                return queue.First != null ? queue.First.Value : null;
            }
        }

        public bool HasVertex(Vertex v)
        {
            LinkedListNode<Track> node = queue.First;
            while(node != null)
            {
                if(node.Value.CurrentVertex.Equals(v))
                    return true;
                node = node.Next;
            }
            return false;
        }

        public void Add(Track obj)
        {
            LinkedListNode<Track> node = queue.First;
            while(node != null)
            {
                if (node.Value.TotalCost > obj.TotalCost)
                    break;
                node = node.Next;
            }

            if (node == null)
                queue.AddLast(obj);
            else
                queue.AddBefore(node, obj);
        }

        public void Remove()
        {
            queue.RemoveLast();
        }

        public void Remove(Track obj)
        {
            queue.Remove(obj);
        }

        public Track Get(Vertex v)
        {
            LinkedListNode<Track> no = queue.First;

            while (no != null)
            {
                if (no.Value.CurrentVertex.Equals(v))
                    return no.Value;

                no = no.Next;
            }

            return null;
        }
    }
}
