using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Homework8
{
    internal class tasks_8_home
    {
        public class AgileLinkedList<T>
        {
            public class Node<TNode>
            {
                public TNode Data { get; set; }
                public Node<TNode> Next { get; set; }
                public Node<TNode> Previous { get; set; }
            }
            public Node<T> First { get; set; }
            public Node<T> Last { get; set; }
            public int count_node { get; set; }
            public override string ToString()
            {
                var current = First;
                string node_text = "";
                for (int i = 0; i < count_node; i++)
                {
                    if (current.Next == null)
                    {
                        node_text += $"{current.Data}";
                    }
                    else
                    {
                        node_text += $"{current.Data} <-> ";
                    }
                    current = current.Next;
                }
                return node_text;
            }
            public AgileLinkedList(List<T> data)
            {
                foreach (T value in data)
                {
                    AddLast(value);
                }
            }
            public void AddLast(T data)
            {
                var new_node = new Node<T> { Data = data };
                if (First == null)
                {
                    First = new_node;
                    Last = new_node;
                }
                else
                {
                    new_node.Previous = Last;
                    Last.Next = new_node;
                    Last = new_node;
                }
                count_node++;
            }
            public void AddFirst(T data)
            {
                var new_node = new Node<T> { Data = data };
                if (First == null)
                {
                    First = new_node;
                    Last = new_node;
                }
                else
                {
                    new_node.Next = First;
                    First.Previous = new_node;
                    First = new_node;
                }
                count_node++;
            }
            public bool IsSimmetryc(AgileLinkedList<T> data)
            {
                if (data == null) return true;
                var current_first = First;
                var current_last = Last;
                while (current_first != null && current_last != null && current_last != current_first && current_first.Next != current_last)
                {
                    if (!EqualityComparer<T>.Default.Equals(current_first.Data, current_last.Data)) return false;
                    current_first = current_first.Next;
                    current_last = current_last.Previous;
                }
                return true;
            }
            public void add_next_n(int N, T new_elem)
            {
                if (First == null || N < 0) return;

                var current = First;
                for (int i = 0; i < N; i++)
                {
                    current = current.Next;
                }
                if (current == null) return;
                var new_node = new Node<T>() { Data = new_elem };
                new_node.Next = current.Next;
                new_node.Previous = current;
                if (current != null)
                {
                    current.Next.Previous = new_node;
                }
                else
                {
                    Last = new_node;
                }
                current.Next = new_node;
                count_node++;
            }
            public void remove_n_elem(int N)
            {
                if (First == null || N < 0 || N >= count_node) return;
                var current = First;
                for (int i = 0; i < N; i++)
                {
                    current = current.Next;
                }
                if (current == null) return;
                if (current.Previous == null)
                {
                    First = current.Next;
                    if (First != null) First.Previous = null;
                    else
                    {
                        Last = null;
                    }
                }
                else if (current.Next == null)
                {
                    Last = current.Previous;
                    if (Last != null) Last.Next = null;
                }
                else 
                {
                    current.Previous.Next = current.Next;
                    current.Next.Previous = current.Previous;
                }
             
                count_node--;
            }




        }
    }
}