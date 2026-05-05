using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace lub_19
{
    internal class lub_19_tasks
    {


        public class Graph
        {
            /// <summary>
            /// Список смежностей вершин. 
            /// </summary>
            /// <example>
            /// Например:
            /// 1 -> 2, 3
            /// 2 -> 1
            /// 3 -> 1
            /// Означает, что существуют дуги из вершины 1 в вершины 2 и 3
            /// </example>
            private Dictionary<string, List<string>> _adjacencyList;

            public Graph(Dictionary<string, List<string>> adjacencyList)
            {
                _adjacencyList = adjacencyList;
            }

            /// <summary>
            /// Строит остовное дерево графа
            /// </summary>
            /// <returns>
            /// Список строк, описывающих дуги в полученном остовном дереве, например:
            /// ["1->2","2->3","3->4"]
            /// </returns>
            public List<string> BuildSpanningTree()
            {

                // TODO: реализовать DepthFirstSearch
                List<string> alredy_have = new List<string>();
                List<string> answer_Graph = new List<string>();
                void DFS(string data) 
                {
                    alredy_have.Add(data);
                    if (_adjacencyList.ContainsKey(data)) 
                    {
                        foreach (var neighbor in _adjacencyList[data]) 
                        {
                            if (!alredy_have.Contains(neighbor))
                            {
                                answer_Graph.Add($"{data} -> {neighbor}");
                                DFS(neighbor);
                            }
                        }
                    }
                    
                }
                foreach (var key in _adjacencyList.Keys) 
                {
                    if (!alredy_have.Contains(key)) 
                    {
                        DFS(key);
                    }
                }
                return answer_Graph;
                
            }

            // ↓============================== Примеры графов ==============================↓

            public static Graph GetSampleGraph1() => new Graph(new Dictionary<string, List<string>>
    {
        // Для поддержки .NET 2.1
        {"1", new List<string>() {"3", "5", "6", "9"}},
        {"2", new List<string>() {"4", "6", "8"}},
        {"3", new List<string>() {"1", "5", "6", "7", "8"}},
        {"4", new List<string>() {"2", "10"}},
        {"5", new List<string>() {"1", "3", "7"}},
        {"6", new List<string>() {"1", "2", "3"}},
        {"7", new List<string>() {"3", "5"}},
        {"8", new List<string>() {"2", "3"}},
        {"9", new List<string>() {"1", "11", "12"}},
        {"10", new List<string>() {"4"}},
        {"11", new List<string>() {"9", "12"}},
        {"12", new List<string>() {"9", "11"}},
    });

            public static Graph GetSampleGraph2() => new Graph(new Dictionary<string, List<string>>
    {
        // Для поддержки .NET 2.1
        {"Б", new List<string>() {"Д", "К", "М"}},
        {"Д", new List<string>() {"Б", "К"}},
        {"К", new List<string>() {"Б", "Д", "Р"}},
        {"М", new List<string>() {"Б"}},
        {"Р", new List<string>() {"К"}},
    });

            public static Graph GetSampleGraph3() => new Graph(new Dictionary<string, List<string>>
    {
        // Для поддержки .NET 2.1
        {"1", new List<string>() {"4"}},
        {"2", new List<string>() {"4"}},
        {"3", new List<string>() {"4"}},
        {"4", new List<string>() {"1", "2", "3", "5"}},
        {"5", new List<string>() {"4", "6"}},
        {"6", new List<string>() {"5"}},
    });
        }
        static void Main() 
        {
            var example = Graph.GetSampleGraph1();
            var example_1 = Graph.GetSampleGraph2();
            var example_2 = Graph.GetSampleGraph3();
            Console.WriteLine(String.Join(", ",example.BuildSpanningTree()));
            Console.WriteLine(String.Join(", ", example_1.BuildSpanningTree()));
            Console.WriteLine(String.Join(", ", example_2.BuildSpanningTree()));
        }
    }
}
