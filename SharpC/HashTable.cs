using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace SharpC
{
    public unsafe ref struct HashTable
    {
        public HashTableSlot* Slots;
        public int Size;
        public int Capacity;
        public Func<byte, int, int> Hasher;

        public unsafe HashTable(int size, Func<byte, int, int> hasher)
        {
            Slots = (HashTableSlot*)Marshal.AllocHGlobal(sizeof(HashTableSlot) * size).ToPointer();
            Size = 0;
            Capacity = size;
            Hasher = hasher;
        }
    }

    public unsafe ref struct HashTableSlot
    {
        public Node *Head;
    }

    public unsafe ref struct Node
    {
        public byte* Key;
        public byte* Value;
        public Node* Next;
    }

    public static unsafe class HashTableUtil
    {
        public static void Initialize(HashTable* table)
        {
            for(int i = 0; i < (*table).Capacity; i++)
                (*table).Slots[i].Head = (Node*)IntPtr.Zero;
        }

        public static void Add(HashTable* table, byte* key)
        {
            int hash = (*table).Hasher(*key, (*table).Capacity);
            if ((IntPtr)(*table).Slots[hash].Head == IntPtr.Zero)
            {
                var node = (Node*) Marshal.AllocHGlobal(sizeof(Node)).ToPointer();
                if (node == null)
                    throw new InsufficientMemoryException("Unable to allocate node space");
                node->Key = key;
                node->Value = key;
                node->Next = null;
                (*table).Slots[hash].Head = node;
            }
            else
            {
                var node = (Node*)Marshal.AllocHGlobal(sizeof(Node)).ToPointer(); ;
                node->Key = key;
                node->Value = key;
                var temp = (*table).Slots[hash].Head;
                node->Next = temp;
                (*table).Slots[hash].Head = node;
            }
            (*table).Size++;
        }

        public static void Destroy(HashTable* table)
        {
            if((*table).Size > 0)
                Clear(table);
            Marshal.FreeHGlobal((IntPtr)(*table).Slots);
            Marshal.FreeHGlobal((IntPtr)table);
        }

        public static void Clear(HashTable* table)
        {
            for(int i = 0; i < (*table).Capacity; i++)
            {
                Node* node = (*table).Slots[i].Head;
                while((IntPtr)node != IntPtr.Zero)
                {
                    Node* temp = node;
                    node = node->Next;
                    Marshal.FreeHGlobal((IntPtr)node);
                }
            }
            (*table).Size = 0;
        }
    }
}
