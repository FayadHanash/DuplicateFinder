using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFinder
{
    public class ListManager<T> : IListManager<T>
    {
        /// <summary>
        /// instance list
        /// </summary>
        private List<T> list;
        /// <summary>
        /// property for list
        /// just a getter access
        /// </summary>
        public List<T> List { get { return list; } }
        /// <summary>
        /// defualt constructor
        /// creates a new a nstance
        /// </summary>
        public ListManager()
        {
            list = new List<T>();
        }
        /// <summary>
        /// Return number of items in the list
        /// </summary>
        public int Count => list.Count;
        /// <summary>
        /// Add an object to the collection
        /// </summary>
        /// <param name="item">A type</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool Add(T item)
        {
            bool ok = false;
            if (item != null)
            {
                list.Add(item);
                ok = true;
            }
            return ok;
        }
        /// <summary>
        /// Change the value of object at a given position in the collection
        /// </summary>
        /// <param name="type"> the object to replace an existing at index-position </param>
        /// <param name="index">The postion in the object collection in which the changes is to be done</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool ChangeAt(T type, int index)
        {
            bool ok = false;
            if (CheckIndex(index))
            {
                list[index] = type;
                ok = true;
            }
            return ok;
        }
        /// <summary>
        /// Check if the index is not out of collections's range
        /// </summary>
        /// <param name="index">Input index of the postion to be checked</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool CheckIndex(int index) => (index >= 0 && index < list.Count);
        /// <summary>
        /// Delete the collection
        /// </summary>
        public void DeleteAll() => list = new List<T>();
        /// <summary>
        /// Remove an object from the collection at a given position 
        /// </summary>
        /// <param name="index">Index to object that is to be removed</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool DeleteAt(int index)
        {
            bool ok = false;
            if (CheckIndex(index))
            {
                list.RemoveAt(index);
                ok = true;
            }
            return ok;
        }

        /// <summary>
        /// Return the right object from the collection at a given position
        /// </summary>
        /// <param name="index">input index of the position in the collection</param>
        /// <returns>True if successful, false otherwise</returns>
        public T GetAt(int index)
        {
            T t = default;
            if (CheckIndex(index))
            {
                t = list[index];
            }
            return t;
        }
        /// <summary>
        /// Return the collection as array of string
        /// </summary>
        /// <returns>array</returns>
        public string[] ToStringArray()
        {
            string[] array = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
                array[i] = list[i].ToString();
            return array;
        }
        /// <summary>
        /// return the collection as list of string
        /// </summary>
        /// <returns>list</returns>

        public List<string> ToStringList()
        {
            List<string> stringList = new List<string>();
            foreach (T t in list)
                stringList.Add(t.ToString());
            return stringList;
        }
        /// <summary>
        /// Sorting Method 
        /// </summary>
        /// <param name="sorter"></param>
        public void Sort(IComparer<T> sorter) { list.Sort(sorter); }

        /// <summary>
        /// Method that saves data to a text file
        /// </summary>
        /// <param name="fileName">file name</param>
        public void SaveToFile(string fileName)
        {
        }
        /// <summary>
        /// Method that loads data from a text file
        /// </summary>
        /// <param name="fileName">file name</param>
        public void LoadFromFile(string fileName)
        {
        }

        /// <summary>
        /// Method that saves data to a binary file
        /// </summary>
        /// <param name="fileName">file name</param>
        public void BinarySerialize(string fileName)
        {
        }

        /// <summary>
        /// Method that loads data from a binary file
        /// </summary>
        /// <param name="fileName">file name</param>
        public void BinaryDeserialize(string fileName)
        {
        }



        /// <summary>
        /// Method that saves data to a xml file
        /// </summary>
        /// <param name="fileName">file name</param>
        public void XMLSerialize(string fileName)
        {
        }

        /// <summary>
        /// Method that saves data from a xml file
        /// </summary>
        /// <param name="fileName">file name</param>
        public void XMLDeserialize(string fileName)
        {
        }
    }
}
