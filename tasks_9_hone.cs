using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hometasks9
{
    internal class tasks_9_hone
    {
        public class TreeNode<T>
        {
            /// <summary>
            /// Поле данных
            /// </summary>
            public T Data;
            /// Левое поддерево
            /// </summary>
            public TreeNode<T> Left;

            /// <summary>
            /// Правое поддерево
            /// </summary>
            public TreeNode<T> Right;

            /// <summary>
            /// Инициализирует узел бинарного дерева значением data поля данных
            /// и поддеревьями left, right
            /// </summary>
            /// <param name="data">Значение поля данных узла</param>
            /// <param name="left">Левое поддерево</param>
            /// <param name="right">Правое поддерево</param>
            public TreeNode(T data, TreeNode<T> left = null, TreeNode<T> right = null)
            {
                Data = data;
                Left = left;
                Right = right;
            }
        }
        public static class TreeUtils
        {
            private static Random _random = new Random();

            /// <summary>
            /// Печатает дерево инфиксным обходом. Если дерево пустое, выводится &lt;empty tree&gt;
            /// </summary>
            /// <param name="root"></param>
            public static void PrintTreeInfix<T>(TreeNode<T> root)
            {
                if (root == null)
                {
                    Console.WriteLine("<empty tree>");
                    return;
                }
                PrintNodeInfix(root);
                Console.WriteLine();

                void PrintNodeInfix(TreeNode<T> node)
                {
                    if (node == null)
                        return;
                    PrintNodeInfix(node.Left);
                    Console.Write($"{node.Data} ");
                    PrintNodeInfix(node.Right);
                }
            }

            /// <summary>
            /// Создаёт рандомное бинарное дерево.
            /// </summary>
            /// <param name="count">Количество узлов в дереве</param>
            /// <param name="minVal">Левая граница значений узлов</param>
            /// <param name="maxVal">Правая граница значений узлов (не включая)</param>
            public static TreeNode<int> GetRandomTree(int count, int minVal = -10, int maxVal = 10)
            {
                if (count < 0)
                    throw new ArgumentException("Count must be >= 0.");
                if (minVal > maxVal)
                    throw new ArgumentException("Min value must be greater than or equal to Max value.");
                if (count == 0)
                    return null;
                var leftNodesCount = _random.Next(0, count - 1);
                return new TreeNode<int>(_random.Next(minVal, maxVal),
                    GetRandomTree(leftNodesCount, minVal, maxVal),
                    GetRandomTree(count - leftNodesCount - 1, minVal, maxVal));
            }

            /// <summary>
            /// Создаёт бинарное дерево из 6 целых чисел
            /// </summary>
            ///      7
            ///    /   \
            ///   -6    32
            ///  /  \     \
            /// 0   11     -5
            public static TreeNode<int> GetSampleIntTree1()
            {
                return new TreeNode<int>(7,
                    new TreeNode<int>(-6,
                        new TreeNode<int>(0),
                        new TreeNode<int>(11)
                    ),
                    new TreeNode<int>(32,
                        right: new TreeNode<int>(-5)
                    )
                );
            }

            /// <summary>
            /// Создаёт бинарное дерево из 5 первых натуральных чисел
            /// </summary>
            ///      1
            ///    /   \
            ///   2     3
            ///  	   / \
            ///       4   5
            public static TreeNode<int> GetSampleIntTree2()
            {
                return new TreeNode<int>(1,
                    new TreeNode<int>(2),
                    new TreeNode<int>(3,
                        new TreeNode<int>(4),
                        new TreeNode<int>(5)
                    )
                );
            }

            /// <summary>
            /// Создаёт бинарное дерево из 4 целых чисел,
            /// расположенных в левой крайней ветви
            /// </summary>
            ///       -1001
            ///       /   
            ///     999   
            ///     /  
            ///    0
            ///   / 
            /// -57 
            public static TreeNode<int> GetSampleIntTree3()
            {
                return new TreeNode<int>(-1001,
                    new TreeNode<int>(999,
                        new TreeNode<int>(0,
                            new TreeNode<int>(-57)
                        )
                    )
                );
            }

            /// <summary>
            /// Создаёт бинарное дерево из 3 целых чисел,
            /// расположенных в правой крайней ветви
            /// </summary>
            ///      3
            ///    	  \
            ///  	   5
            ///  	    \ 
            ///      	 8 
            public static TreeNode<int> GetSampleIntTree4()
            {
                return new TreeNode<int>(3,
                    right: new TreeNode<int>(5,
                        right: new TreeNode<int>(8)
                    )
                );
            }


            /// <summary>
            /// Создаёт бинарное дерево из 6 целых чисел,
            /// расположенных в крайних ветвях
            /// </summary>
            ///      18
            ///    	/  \
            ///   -20  35
            ///   /	     \ 
            /// -75       66
            ///	           \
            ///				94
            public static TreeNode<int> GetSampleIntTree5()
            {
                return new TreeNode<int>(18,
                    new TreeNode<int>(-20,
                        new TreeNode<int>(-75)
                    ),
                    new TreeNode<int>(35,
                        right: new TreeNode<int>(66,
                            right: new TreeNode<int>(94)
                        )
                    )
                );
            }
        }
        public class BinarySearchTree
        {

            public TreeNode<int> root = null;
            public BinarySearchTree(params int[] numbers)
            {
                foreach (int x in numbers)
                {
                    Add(x);
                }
            }

            public void Add(int x)
            {
                root = AddRecursive(root, x);
            }
            public static bool Cointains_Tree(TreeNode<int> root, int elem)
            {
                if (root == null) return false;
                if (elem == root.Data)
                {
                    return true;
                }
                else if (root.Data > elem)
                {
                    return Cointains_Tree(root.Left, elem);
                }
                else
                {
                    return Cointains_Tree(root.Right, elem);
                }

            }
            public static int sum_of_binray_tree(TreeNode<int> root)
            {
                if (root == null) return 0;
                if (root.Left == null && root.Right == null) return root.Data;
                return sum_of_binray_tree(root.Left) + sum_of_binray_tree(root.Right);

            }
            public static TreeNode<T> Copy<T>(TreeNode<T> root)
            {
                var answer_Tree = new TreeNode<T>(root.Data);
                if (root.Left != null) answer_Tree.Left = Copy(root.Left);
                if (root.Right != null) answer_Tree.Right = Copy(root.Right);
                return answer_Tree;

            }
            public static int level_width(TreeNode<int> root,int n )
            {
                if (root == null) return 0;
                if (n == 0) return 1;
                return level_width(root.Left, n - 1) + level_width(root.Right, n - 1);            
            }

            public static bool is_sum_tree(TreeNode<int> root) 
            {
                if (root == null) return false;
                int sum_under_tree(TreeNode<int> node) 
                {
                    if (node == null) return 0;
                    return sum_under_tree(node.Left) + sum_under_tree(node.Right) + node.Data;
                }
                if (ddd)
            }



            private TreeNode<int> AddRecursive(TreeNode<int> node, int x)
            {
                if (node == null)
                {
                    return new TreeNode<int>(x);
                }
                if (x < node.Data)
                {
                    node.Left = AddRecursive(node.Left, x);
                }
                else if (x > node.Data)
                {
                    node.Right = AddRecursive(node.Right, x);
                }
                return node;
            }
            public static void print_tree(TreeNode<int> node)
            {
                TreeUtils.PrintTreeInfix(node);
            }


        }

        static void Main()
        {
            var node = TreeUtils.GetSampleIntTree5();
            BinarySearchTree.print_tree(node);
            if (BinarySearchTree.Cointains_Tree(node, 94))
            {
                Console.WriteLine("Число найдено");
            }
            else { Console.WriteLine("Число не найдено"); }
            var new_node = BinarySearchTree.Copy(node.Right);
            BinarySearchTree.print_tree(new_node);
            Console.WriteLine(BinarySearchTree.sum_of_binray_tree(node));
            Console.WriteLine(BinarySearchTree.level_width(node,1));
        }
    }
}