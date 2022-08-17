using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFinder
{
    /// <summary>
    /// file class 
    /// </summary>
    public class MyFile : IEqualityComparer<MyFile>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MyFile()
        {
        }

        /// <summary>
        /// Fildes with read and write access
        /// </summary>
        public string Name { get; set; } // file name
        public long Size { get; set; } // file length (Bytes)
        public DateTime DateCreated { get; set; } // file's date creation
        public DateTime DateModified { get; set; } // file's last write date
        public string Type { get; set; } // file's type (extension)
        public string FilePath { get; set; } // path to the file
        public bool IsChecked { get; set; } // used in the list view to select files

        /// <summary>
        /// Method that returns true if two files is equal, otherwise false
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => Equals(obj as MyFile);
        
        public bool Equals(MyFile other) => other != null && this.Name == other.Name && this.Size == other.Size && this.Type == other.Type && this.DateCreated == other.DateCreated && this.DateModified == other.DateModified;
        
        /// <summary>
        /// Method that returns true if two files is equal, otherwise false
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(MyFile x, MyFile y) => x.Name == y.Name && x.Size == y.Size && x.Type == y.Type && x.DateCreated == y.DateCreated && x.DateModified == y.DateModified;

        /// <summary>
        /// Method that returns a hash
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => $"{this.Name}{this.Size}{this.Type}{this.DateCreated}{this.DateModified}".GetHashCode();
        
        /// <summary>
        /// Method that returns a hash
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(MyFile obj) => $"{this.Name}{this.Size}{this.Type}{this.DateCreated}{this.DateModified}".GetHashCode();

        /// <summary>
        /// This method prepares a format string that is in sync with the ToString method. 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{this.Name} {this.Size} {this.Type} {this.DateCreated} {this.DateModified}";
    }
}
