using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberInteractiveTestTask
{
    class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random; // произвольный элемент внутри списка
        public string Data;
    }
    class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        /// <summary>
        /// Serialize this <see cref="ListRandom"/> to memory stream.
        /// </summary>
        /// <param name="s">Stream to serialize to.</param>
        public void Serialize(Stream s)
        {
            //i tried to get O(n) but i think with Random parameter it is not possible. Currently serializing is O(n^2)
            var list = new List<ListNode>();

            ListNode currentNode = Head;

            while (currentNode != null)
            {
                if (list.Contains(currentNode))
                    throw new ArgumentException("List has a loop.");

                list.Add(currentNode);

                currentNode = currentNode.Next;
            }

            StringBuilder dataBuilder = new StringBuilder();

            //using "Data=" and "Random=" as separators
            foreach (ListNode node in list)
            {
                dataBuilder.Append($"Data={node.Data}Random={list.IndexOf(node.Random)}");
            }

            byte[] bytes = new UTF8Encoding(true).GetBytes(dataBuilder.ToString());
            s.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Deserialize <see cref="ListRandom"/> from memory stream.
        /// </summary>
        /// <param name="s">Stream to desirialize from.</param>
        public void Deserialize(Stream s)
        {
            List<ListNode> list = new List<ListNode>();
            s.Position = 0;
            string data;

            using (StreamReader reader = new StreamReader(s, Encoding.UTF8))
                data = reader.ReadToEnd();

            string[] arr = data.Split(new string[] { "Data=" }, StringSplitOptions.RemoveEmptyEntries);

            Head = new ListNode();
            ListNode currentNode = Head;

            Count = arr.Length;

            for (int i = 0; i < arr.Length; i++)
            {
                currentNode.Data = arr[i];
                list.Add(currentNode);

                ListNode next = new ListNode();
                currentNode.Next = next;
                next.Previous = currentNode;

                currentNode = next;
            }

            Tail = currentNode.Previous;
            Tail.Next = null;

            foreach (ListNode n in list)
            {
                var tempArr = n.Data.Split(new string[] { "Random=" }, StringSplitOptions.None);
                n.Random = list[Convert.ToInt32(tempArr[1])];
                n.Data = tempArr[0];
            }

        }


    }
}
