using NUnit.Framework;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpC.Tests
{
    public class HashTableTest
    {
        private unsafe int HashFunction(byte element, int capacity)
        {
            return element.GetHashCode() % capacity;
        }

        [Test]
        public unsafe void TestHeadNull()
        {
            HashTable table = new(35, HashFunction);
            HashTableUtil.Initialize(&table);
            Assert.IsTrue(table.Slots[0].Head == null);

            HashTableUtil.Destroy(&table);
           Assert.IsTrue(((IntPtr)table.Slots[0].Head) == IntPtr.Zero);
        }

        [Test]
        public unsafe void TestInsert()
        {
            HashTable table = new(125, HashFunction);
            HashTableUtil.Initialize(&table);
            string key_value = "125";
            byte key = byte.Parse(key_value);

            int hash = table.Hasher(key, table.Capacity);
            HashTableUtil.Add(&table, &key);
            Assert.That(*(table.Slots[hash].Head->Key), Is.EqualTo(key));

            string key_value_2 = "250";
            byte key_2 = byte.Parse(key_value_2);

            int hash2 = table.Hasher(key_2, table.Capacity);
            HashTableUtil.Add(&table, &key_2);
            Assert.That(*(table.Slots[hash2].Head->Key), Is.EqualTo(key_2));
            HashTableUtil.Destroy(&table);
        }

        [Test]
        public unsafe void TestClear()
        {
            HashTable table = new(125, HashFunction);

            HashTableUtil.Initialize(&table);
            string key_value = "125";
            byte key = byte.Parse(key_value);

            int hash = table.Hasher(key, table.Capacity);
            HashTableUtil.Add(&table, &key);
            Assert.That(*(table.Slots[hash].Head->Key), Is.EqualTo(key));

            string key_value_2 = "250";
            byte key_2 = byte.Parse(key_value_2);

            int hash2 = table.Hasher(key_2, table.Capacity);
            HashTableUtil.Add(&table, &key_2);
            Assert.That(*(table.Slots[hash2].Head->Key), Is.EqualTo(key_2));
            HashTableUtil.Clear(&table);

            Assert.That(table.Size, Is.EqualTo(0));

            HashTableUtil.Destroy(&table);
        }
    }
}