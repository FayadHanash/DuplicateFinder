using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DuplicateFinder
{
    /// <summary>
    /// class that contains utilities to handle with files 
    /// </summary>
    public class FileUtilities
    {
        /// <summary>
        /// Method that opens a dialog to browser folders 
        /// returns True if successful the browder dialog,false otherwise and returns the path to the folder 
        /// which has been selected
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Tuple<bool,string> OpenFolderBrowserDialog(string message)
        {
            try
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog
                {
                    Description = message,
                    SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    ShowNewFolderButton = true,
                };
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return new Tuple<bool, string>(true, folderBrowserDialog.SelectedPath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return new Tuple<bool, string>(false, string.Empty);
        }
        /// <summary>
        /// Method that uses to take a snapshot of file system 
        /// Rerturns Ienumerable öist of file info
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IEnumerable<FileInfo> GetFiles(string path)
        {
            IEnumerable<FileInfo> temp = null;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                temp = dir.GetFiles("*.*", SearchOption.AllDirectories);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return temp;
        }
        /// <summary>
        /// Method that deletes the files
        /// returns True if successful has been deleted,false otherwise
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool DeleteFile(string fileName)
        {
            bool ok = false;
            try
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException(fileName);
                }
                File.Delete(fileName);
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw new Exception(ex.Message);
            }
            return ok;
        }
        /// <summary>
        /// Method that copies tha files
        /// returns True if successful the file has been copied,false otherwise
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool CopyFile(string fileName, string sourceFileName, string destFileName)
        {
            bool ok = false;
            try
            {
                Directory.CreateDirectory(destFileName);
                File.Copy(sourceFileName,Path.Combine(destFileName,fileName),true);
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw new Exception(ex.Message);
            }
            return ok;
        }
        /// <summary>
        /// Method that moves the files
        /// returns True if successful the file has been moved,false otherwise
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool MoveFile(string sourceFileName, string destFileName)
        {
            bool ok = false;
            try
            {
                File.Move(sourceFileName,destFileName);
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw new Exception(ex.Message);
            }
            return ok;
        }
    }
}
