using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SaberInteractiveTestTask
{
    class Program
    {
        static readonly Random rng = new Random();

        static void Main(string[] args)
        {
            int length = 12;

            ListNode head = new ListNode();
            ListNode tail;
            ListNode currentNode;

            head.Data = Guid.NewGuid().ToString();

            tail = head;

            for (int i = 0; i < length - 1; i++)
                tail = AddNode(tail);

            currentNode = head;

            for (int i = 0; i < length; i++)
            {
                currentNode.Random = GetRandomNode(head, length);
                currentNode = currentNode.Next;
            }

            ListRandom randomList = new ListRandom
            {
                Head = head,
                Tail = tail,
                Count = length
            };

            //Will use in-memory stream
            Stream stream = new MemoryStream();
            randomList.Serialize(stream);

            ListRandom resultList = new ListRandom();

            resultList.Deserialize(stream);

            if (DeepCompareLists(randomList, resultList, true))
                Console.WriteLine("Lists are equal.");
            else
                Console.WriteLine("Lists are not equal.");


            Console.Read();

        }

        /// <summary>
        /// Deeply compares two <see cref="ListRandom"/>
        /// </summary>
        /// <param name="first">First list.</param>
        /// <param name="second">Second list.</param>
        /// <param name="print">Print node data to console.</param>
        /// <returns>True if lists are deeply equal, false otherwise.</returns>
        static bool DeepCompareLists(ListRandom first, ListRandom second, bool print)
        {
            ListNode firstNode = first.Head;
            ListNode secondNode = second.Head;

            while (firstNode != null)
            {
                if(print)
                    Console.WriteLine($"{firstNode.Data} | {secondNode.Data}");

                if (secondNode == null)
                    return false;

                if (firstNode.Data != secondNode.Data && firstNode.Random.Data != secondNode.Random.Data)
                    return false;

                firstNode = firstNode.Next;
                secondNode = secondNode.Next;
            }

            return true;
        }

        /// <summary>
        /// Create new node and set it as next to node from input.
        /// </summary>
        /// <param name="node">Parent node.</param>
        /// <returns>New node.</returns>
        static ListNode AddNode(ListNode node)
        {
            ListNode newNode = new ListNode
            {
                Previous = node,
                Next = null,
                Data = Guid.NewGuid().ToString()
            };
            node.Next = newNode;
            return newNode;
        }

        /// <summary>
        /// Returns a random node from linked list that starts with node from input.
        /// </summary>
        /// <param name="head">Head node of linked list.</param>
        /// <param name="length">Linked list length.</param>
        /// <returns>Random node.</returns>
        static ListNode GetRandomNode(ListNode head, int length)
        {
            int random = rng.Next(0, length);

            ListNode node = head;

            for (int i = 0; i < random; i++)
            {
                if (node == null)
                    throw new ArgumentException("Linked list length is incorrect.");

                node = node.Next;
            }

            return node;
        }
    }
}
