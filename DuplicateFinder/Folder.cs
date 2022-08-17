using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFinder
{
    public class Folder
    {
        /// <summary>
        /// folder name's field, both read and write access
        /// </summary>
        public string FolderName { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public Folder() : this(String.Empty)
        {
        }
        /// <summary>
        /// Consstructor with one prameter
        /// </summary>
        /// <param name="folderName"></param>
        public Folder(string folderName)
        {
            FolderName = folderName;
        }
        /// <summary>
        /// This method prepares a format string that is in sync with the ToString method. 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{FolderName}";   
    }
}
