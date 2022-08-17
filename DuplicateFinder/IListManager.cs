using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFinder
{
    /// <summary>
    /// Interface for implementation by manger classes hosting a collection of the type 
    /// List<T> where T can be any object type. In this documentaion, 
    /// the collection is referred to as list.
    /// IListManger can be implemented by different classes passing any type <T> at declaration
    /// but then T must have the same type in all methods included in this interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IListManager<T>
    {
        /// <summary>
        /// Return the number of items
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Add an object to the collection
        /// </summary>
        /// <param name="item">A type</param>
        /// <returns>True if successful, false otherwise.</returns>
        bool Add(T item);

        /// <summary>
        /// Change the value of object at a given position in the collection
        /// </summary>
        /// <param name="type"> the object to replace an existing at index-position </param>
        /// <param name="index">The postion in the object collection in which the changes is to be done</param>
        /// <returns>True if successful, false otherwise</returns>
        bool ChangeAt(T type, int index);
        /// <summary>
        /// Check if the index is not out of collections's range
        /// </summary>
        /// <param name="index">Input index of the postion to be checked</param>
        /// <returns>True if successful, false otherwise</returns>
        bool CheckIndex(int index);
        /// <summary>
        /// Delete the collection
        /// </summary>
        void DeleteAll();
        /// <summary>
        /// Remove an object from the collection at a given position 
        /// </summary>
        /// <param name="index">Index to object that is to be removed</param>
        /// <returns>True if successful, false otherwise.</returns>
        bool DeleteAt(int index);
        /// <summary>
        /// Return the right object from the collection at a given position
        /// </summary>
        /// <param name="index">input index of the position in the collection</param>
        /// <returns>True if successful, false otherwise</returns>
        T GetAt(int index);
        /// <summary>
        /// Return the collection as array of string
        /// </summary>
        /// <returns>array</returns>
        string[] ToStringArray();
        /// <summary>
        /// return the collection as list of string
        /// </summary>
        /// <returns>list</returns>
        List<string> ToStringList();

        /// <summary>
        /// Method that saves data to a text file
        /// </summary>
        /// <param name="fileName"> file path</param>
        void SaveToFile(string fileName);
        /// <summary>
        /// Method that loads data from a text file
        /// </summary>
        /// <param name="fileName">file path</param>
        void LoadFromFile(string fileName);

        /// <summary>
        /// Method that saves data to a binary file
        /// </summary>
        /// <param name="fileName">file path</param>
        void BinarySerialize(string fileName);
        /// <summary>
        /// Method that loads data from a binary file
        /// </summary>
        /// <param name="fileName">file path</param>
        void BinaryDeserialize(string fileName);

        /// <summary>
        /// Method that saves data to a xml file
        /// </summary>
        /// <param name="fileName">file path</param>
        void XMLSerialize(string fileName);

        /// <summary>
        /// Method that loads data from a xml file
        /// </summary>
        /// <param name="fileName">file path</param>
        void XMLDeserialize(string fileName);



    }
}
