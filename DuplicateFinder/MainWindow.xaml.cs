using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace DuplicateFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileManager fileMgr; //declaration of FileManager
        List<System.IO.FileInfo> filesInfoList; // list of FileInfo
        IEnumerable<MyFile> query = null; //IEnumerable list of MyFile
        Tuple<bool, bool, bool> IfButtonsClicked;// tuple of three booleans
        /// <summary>
        /// Default constructor
        /// Calls InitializeComponent and InitializeGUI methods
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeGUI();
        }
        /// <summary>
        /// Method that initializes the GUI.
        /// Creates instances of List of FileInfo, FileManager and Tuple
        /// Calls UpdateGUI method
        /// </summary>
        void InitializeGUI()
        {
            filesInfoList = new List<System.IO.FileInfo>();
            fileMgr = new FileManager();
            IfButtonsClicked = new Tuple<bool, bool, bool>(false,false,false);
            UpdateGUI();
            this.Title = "Duplicate Finder";
        }

        /// <summary>
        /// Method that checks the check boxes
        /// </summary>
        /// <returns>True if successful,false otherwise</returns>
        bool IsCheckBoxesChecked () => (chBoxName.IsChecked == false && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == false && chBoxDateModified.IsChecked == false && chBoxDateCreated.IsChecked == false) ? false : true;
        /// <summary>
        /// Method that manages the buttons
        /// </summary>
        void ManageButtons()
        {
            if(lstFolders.Items.Count > 0)
            {
                btnRemove.IsEnabled=true;
                btnRemoveAll.IsEnabled=true;
                btnFindDuplicate.IsEnabled=true;
                btnListAllFiles.IsEnabled = true;
            }
            else
            {
                btnRemove.IsEnabled=false;
                btnRemoveAll.IsEnabled=false;
                btnFindDuplicate.IsEnabled =false;
                btnListAllFiles.IsEnabled=false;
            }
            if (lstFolders.Items.Count == 2)
                btnFindIdentical.IsEnabled = true;
            else btnFindIdentical.IsEnabled = false;
            if (lstResult.Items.Count > 0 && ControllPath() != true)
            {
                btnCopyTo.IsEnabled = true;
                btnMoveTo.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnCopyTo.IsEnabled = false;
                btnMoveTo.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
            if (lstResult.Items.Count > 0 )
            {
                btnSelectAll.IsEnabled = true;
            }
            else
            {
                btnSelectAll.IsEnabled = false;
            }


        }
        /// <summary>
        /// Method that adds the folders in folders listview
        /// Calls OpenFolderBrowserDialog to get folders and UpdateGUI() method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            (bool Valid, string path) = FileUtilities.OpenFolderBrowserDialog("Select a folder");
            if(Valid)
            {
                lstFolders.Items.Add(new Folder(path));
                lstResult.Items.Clear();
                UpdateGUI();
            }
        }
        /// <summary>
        /// Method that removes a folder from listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            int index = lstFolders.SelectedIndex;
            if(index >= 0)
            {
                lstFolders.Items.RemoveAt(index);
                lstResult.Items.Clear();
                UpdateGUI();
            }
        }
        /// <summary>
        /// Method that updates the GUI
        /// Calls ManageButtons method
        /// </summary>
        void UpdateGUI()
        {
            if (lstResult.Items.Count > 0)
            {
                lblCount.Visibility = Visibility.Visible;
                lblCount.Content = $"Count: {lstResult.Items.Count}";
            }
            else
            {
                lblCount.Visibility = Visibility.Hidden;
            }
            ManageButtons();
        }
        /// <summary>
        /// Method that removes all folders from listview
        /// Calls UpdateGUI method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            lstFolders.Items.Clear();
            lstResult.Items.Clear();
            UpdateGUI();
        }
        /// <summary>
        /// A help method that it be calls if findAll buttons clicked 
        /// Calls GetAndCopyFiles, ManageSorting and UpdateGUI methods
        /// </summary>
        void FindAllButtonClicked()
        {
            GetAndCopyFiles();
            ManageSorting();
            UpdateGUI();
        }
        /// <summary>
        /// Method that lists all files when the ListAllFiles is clicked
        /// Calls IsCheckBoxesChecked and FindAllButtonClicked methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListAllFiles_Click(object sender, RoutedEventArgs e)
        {

            if (IsCheckBoxesChecked())
            {
                IfButtonsClicked = new Tuple<bool, bool, bool>(true,false,false);
                FindAllButtonClicked();
            }
            else
                MessageBox.Show("Ckeck at least one attribute to be compared");
        }
        /// <summary>
        /// Method that gets and copies files from harddisk
        /// </summary>
        void GetAndCopyFiles()
        {
            lstResult.Items.Clear();
            filesInfoList = new List<System.IO.FileInfo>();
            fileMgr = new FileManager();
            foreach (var item in lstFolders.Items)
            {
                filesInfoList.AddRange(FileUtilities.GetFiles(item.ToString()));
            }
            foreach (var f in filesInfoList)
            {
                fileMgr.Add(new MyFile { Name = f.Name, Size = f.Length, Type = f.Extension, DateCreated = f.CreationTime, DateModified = f.LastWriteTime, FilePath = f.FullName });
            }
        }

        /// <summary>
        /// A help method that it be calls if FindDuplicate buttons clicked 
        /// Calls GetAndCopyFiles, ManageComparing and UpdateGUI methods
        /// </summary>
        void FindDuplicateClicked()
        {
            GetAndCopyFiles();
            MangageComparing();
            UpdateGUI();

        }
        /// <summary>
        /// Method that lists duplicate files when the FindDuplicate is clicked
        /// Calls IsCheckBoxesChecked and FindDuplicateClicked methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindDuplicate_Click(object sender, RoutedEventArgs e)
        {
            if (IsCheckBoxesChecked())
            {
                IfButtonsClicked = new Tuple<bool, bool, bool>(false,true,false);
                FindDuplicateClicked();
            }
            else
                MessageBox.Show("Ckeck at least one attribute to be compared");
        }

        /// <summary>
        /// Method that compares files to define duplicates files based on checked file attributes
        /// Calls CheckCheckBoxes method and updates the result listview
        /// </summary>
        void MangageComparing()
        {
            (bool name, bool size, bool type, bool created, bool modified) = CheckCheckBoxes();
             query = null;
            // 1
            if (name && !size && !type && !created && !modified)
                query = fileMgr.List.GroupBy(x => x.Name).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && size && !type && !created && !modified)
                query = fileMgr.List.GroupBy(x => x.Size).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && !size && type && !created && !modified)
                query = fileMgr.List.GroupBy(x => x.Type).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && !size && !type && created && !modified)
                query = fileMgr.List.GroupBy(x => x.DateCreated).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && !size && !type && !created && modified)
                query = fileMgr.List.GroupBy(x => x.DateModified).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            //2
            else if (name && !size && type && !created && !modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Type)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (name && size && !type && !created && !modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Size)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (name && !size && !type && !created && modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (name && !size && !type && created && !modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.DateCreated)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());

            else if (!name && size && type && !created && !modified)
                query = fileMgr.List.GroupBy(x => (x.Type, x.Size)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && !size && type && !created && modified)
                query = fileMgr.List.GroupBy(x => (x.Type, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && !size && type && created && !modified)
                query = fileMgr.List.GroupBy(x => (x.Type, x.DateCreated)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());

            else if (!name && size && !type && created && !modified)
                query = fileMgr.List.GroupBy(x => (x.DateCreated, x.Size)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && !size && !type && created && modified)
                query = fileMgr.List.GroupBy(x => (x.DateCreated, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());

            else if (!name && size && !type && !created && modified)
                query = fileMgr.List.GroupBy(x => (x.DateModified, x.Size)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            //3

            else if (name && size && type && !created && !modified)
                query = fileMgr.List.GroupBy(x=> (x.Name, x.Size, x.Type)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (name && size && !type && created && !modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Size, x.DateCreated)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (name && size && !type && !created && modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Size, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());

            else if (name && !size && type && created && !modified)
                query = fileMgr.List.GroupBy(x => (x.Name,x.Type, x.DateCreated)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (name && !size && type && !created && modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Type, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());

            else if (name && !size && !type && created && modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.DateCreated, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());

            else if (!name && size && type && created && !modified)
                query = fileMgr.List.GroupBy(x => (x.Size, x.Type, x.DateCreated)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && size && type && !created && modified)
                query = fileMgr.List.GroupBy(x => (x.Size, x.Type, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && size && !type && created && modified)
                query = fileMgr.List.GroupBy(x => (x.Size, x.DateCreated, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());

            else if (!name && !size && type && created && modified)
                query = fileMgr.List.GroupBy(x => (x.Type, x.DateCreated, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            //4

            else if (name && size && type && created && !modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Size, x.Type, x.DateCreated)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (name && size && type && !created && modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Size, x.Type, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (name && size && !type && created && modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Size, x.DateCreated, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (name && !size && type && created && modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Type, x.DateCreated, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            else if (!name && !size && type && created && modified)
                query = fileMgr.List.GroupBy(x => (x.Size, x.Type, x.DateCreated, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            //5
            else if (name && size && type && created && modified)
                query = fileMgr.List.GroupBy(x => (x.Name, x.Size, x.Type, x.DateCreated, x.DateModified)).Where(g => g.Count() > 1).SelectMany(g => g.ToList());
            if (query != null)
            {
                lstResult.Items.Clear();
                foreach (var q in query)
                    lstResult.Items.Add(q);
            }
        }

        /// <summary>
        /// Methods that returns the checked attributes 
        /// </summary>
        /// <returns></returns>
        Tuple<bool, bool, bool, bool, bool> CheckCheckBoxes()
        {
            // name, size, type, created, modifiied
            if (chBoxName.IsChecked == true && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(true,false,false,false,false);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(false, true, false, false, false);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(false, false, true, false, false);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(false, false, false, true, false);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(false, false, false, false, true);
            //2
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(true, false, true, false, false);
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(true, true, false, false, false);
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(true, false, false, false, true);
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(true, false, false, true, false);

            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(false, true, true, false, false);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(false, false, true, false, true);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(false, false, true, true, false);

            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(false, true, false, true, false);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(false, false, false, true, true);

            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(false, true, false, false, true);
            //3
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(true, true, true, false, false);
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(true, true, false, true, false);
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(true, true, false, false, true);

            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(true, false, true, true, false);
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(true, false, true, false, true);

            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(true, false, false, true, true);

            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(false, true, true, true, false);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(false, true, true, false, true);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(false, true, false, true, true);

            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(false, false, true, true, true);
            //4

            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == false)
                return new Tuple<bool, bool, bool, bool, bool>(true, true, true, true, false);
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == false && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(true, true, true, false, true);
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == false && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(true, true, false, true, true);
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == false && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(true, false, true, true, true);
            else if (chBoxName.IsChecked == false && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(false, true, true, true, true);


            //5
            else if (chBoxName.IsChecked == true && chBoxSize.IsChecked == true && chBoxFileType.IsChecked == true && chBoxDateCreated.IsChecked == true && chBoxDateModified.IsChecked == true)
                return new Tuple<bool, bool, bool, bool, bool>(true, true, true, true, true);

            return new Tuple<bool, bool, bool, bool, bool>(false, false, false, false, false);
        }

        /// <summary>
        /// Method that lists and sorts files based on checked file attributes
        /// Calls CheckCheckBoxes method and updates the result listview
        /// </summary>
        void ManageSorting()
        {
            query = null;
            (bool name, bool size, bool type, bool created, bool modified) = CheckCheckBoxes();
            // 1
            if (name && !size && !type && !created && !modified)
                query = fileMgr.List.OrderBy(x => x.Name).ToList();
            else if (!name && size && !type && !created && !modified)
                query = fileMgr.List.OrderBy(x => x.Size).ToList();
            else if (!name && !size && type && !created && !modified)
                query = fileMgr.List.OrderBy(x => x.Type).ToList();
            else if (!name && !size && !type && created && !modified)
                query = fileMgr.List.OrderBy(x => x.DateCreated).ToList();
            else if (!name && !size && !type && !created && modified)
                query = fileMgr.List.OrderBy(x => x.DateModified).ToList();
            //2
            else if (name && !size && type && !created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Type)).ToList();
            else if (name && size && !type && !created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Size)).ToList();
            else if (name && !size && !type && !created && modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.DateModified)).ToList();
            else if (name && !size && !type && created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.DateCreated)).ToList();

            else if (!name && size && type && !created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Type, x.Size)).ToList();
            else if (!name && !size && type && !created && modified)
                query = fileMgr.List.OrderBy(x => (x.Type, x.DateModified)).ToList();
            else if (!name && !size && type && created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Type, x.DateCreated)).ToList();

            else if (!name && size && !type && created && !modified)
                query = fileMgr.List.OrderBy(x => (x.DateCreated, x.Size)).ToList();
            else if (!name && !size && !type && created && modified)
                query = fileMgr.List.OrderBy(x => (x.DateCreated, x.DateModified)).ToList();

            else if (!name && size && !type && !created && modified)
                query = fileMgr.List.OrderBy(x => (x.Size, x.DateModified)).ToList();
            //3
            else if (name && size && type && !created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Size, x.Type)).ToList();
            else if (name && size && !type && created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Size, x.DateCreated)).ToList();
            else if (name && size && !type && !created && modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Size, x.DateModified)).ToList();

            else if (name && !size && type && created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Type, x.DateCreated)).ToList();
            else if (name && !size && type && !created && modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Type, x.DateModified)).ToList();

            else if (name && !size && !type && created && modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.DateCreated, x.DateModified)).ToList();

            else if (!name && size && type && created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Size, x.Type, x.DateCreated)).ToList();
            else if (!name && size && type && !created && modified)
                query = fileMgr.List.OrderBy(x => (x.Size, x.Type, x.DateModified)).ToList();
            else if (!name && size && !type && created && modified)
                query = fileMgr.List.OrderBy(x => (x.Size, x.DateCreated, x.DateModified)).ToList();

            else if (!name && !size && type && created && modified)
                query = fileMgr.List.OrderBy(x => (x.Type, x.DateCreated, x.DateModified)).ToList();
            //4

            else if (name && size && type && created && !modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Size, x.Type, x.DateCreated)).ToList();
            else if (name && size && type && !created && modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Size, x.Type, x.DateModified)).ToList();
            else if (name && size && !type && created && modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Size, x.DateCreated, x.DateModified)).ToList();
            else if (name && !size && type && created && modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Type, x.DateCreated, x.DateModified)).ToList();
            else if (!name && size && type && created && modified)
                query = fileMgr.List.OrderBy(x => (x.Size, x.Type, x.DateCreated, x.DateModified)).ToList();


            //5
            else if (name && size && type && created && modified)
                query = fileMgr.List.OrderBy(x => (x.Name, x.Size, x.Type, x.DateCreated, x.DateModified)).ToList();
            if (query != null)
            {
                lstResult.Items.Clear();
                foreach (var q in query)
                    lstResult.Items.Add(q);
            }
        }

        /// <summary>
        /// A help method that it be calls if FindIdentical buttons clicked 
        /// Gets the files from two folders
        /// Calls ManageIdentical and UpdateGUI methods
        /// </summary>
        void FindIdenticalClicked()
        {
            IEnumerable<System.IO.FileInfo> list1 = FileUtilities.GetFiles(lstFolders.Items.GetItemAt(0).ToString());
            IEnumerable<System.IO.FileInfo> list2 = FileUtilities.GetFiles(lstFolders.Items.GetItemAt(1).ToString());
            List<MyFile> l1 = new List<MyFile>();
            List<MyFile> l2 = new List<MyFile>();
            foreach (var f1 in list1)
            {
                l1.Add(new MyFile { Name = f1.Name, Size = f1.Length, Type = f1.Extension, DateCreated = f1.CreationTime, DateModified = f1.LastWriteTime, FilePath = f1.FullName });
            }
            foreach (var f2 in list2)
            {
                l2.Add(new MyFile { Name = f2.Name, Size = f2.Length, Type = f2.Extension, DateCreated = f2.CreationTime, DateModified = f2.LastWriteTime, FilePath = f2.FullName });
            }

            bool areIdentical = l1.SequenceEqual(l2, new MyFile());
            if (areIdentical) MessageBox.Show("Folders are identical");
            ManageIdentical(l1, l2);
            UpdateGUI();
        }
        /// <summary>
        /// Method that lists identical files when the FindIdentical button is clicked
        /// Calls IsCheckBoxesChecked and FindIdenticalClicked methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindIdentical_Click(object sender, RoutedEventArgs e)
        {
    
            if (IsCheckBoxesChecked())
            {
                lstResult.Items.Clear();
                if (lstFolders.Items.Count == 2)
                {
                    IfButtonsClicked = new Tuple<bool, bool, bool>(false,false,true);
                    FindIdenticalClicked();
                }
                else
                    MessageBox.Show("The list should has just two folders");
            }
            else
                MessageBox.Show("Ckeck at least one attribute to be compared");
        }
        /// <summary>
        /// Method that updates the GUI if Delete, CopyTo or MoveTo buttons is clicked
        /// Calls the IfButtonsClicked tuple to define which button was clicked before the delete, copy or move buttons clicked
        /// Calls FindAllButtonClicked, FindDuplicateClicked and FindIdenticalClicked
        /// </summary>
        void UpdateEfterDeleteCopyMove()
        {
            (bool fAll, bool fDupl, bool fIden) = IfButtonsClicked;
            if (fAll && !fDupl && !fIden)
                FindAllButtonClicked();
            else if (!fAll && fDupl && !fIden)
                FindDuplicateClicked();
            else if (!fAll && !fDupl && fIden)
                FindIdenticalClicked();
            else lstResult.Items.Clear();
        }
        /// <summary>
        /// Method that gets identical files based on checked file attributes
        /// Calls CheckCheckBoxes method and updates the result listview
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        void ManageIdentical(List<MyFile> list1, List<MyFile> list2)
        {
            (bool name, bool size, bool type, bool created, bool modified) = CheckCheckBoxes();
            List<MyFile> l1 = new List<MyFile>();
            List<MyFile> l2 = new List<MyFile>();
            query = null; // query common files

            if (name && !size && !type && !created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (!name && size && !type && !created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Size = i1.Size, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Size = i2.Size, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && !size && type && !created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Type = i1.Type, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Type = i2.Type, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && !size && !type && created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && !size && !type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            //2
            else if (name && !size && type && !created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Type = i1.Type, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Type = i2.Type, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (name && size && !type && !created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Size = i1.Size, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Size = i2.Size, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (name && !size && !type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && !size && created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && size && type && !created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Type = i1.Type, Size = i1.Size, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Type = i2.Type, Size = i2.Size, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && !size && type && !created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Type = i1.Type, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Type = i2.Type, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && !size && type && created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Type = i1.Type, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Type = i2.Type, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && size && !type && created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Size = i1.Size, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Size = i2.Size, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && !size && !type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { DateModified = i1.DateModified, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { DateModified = i2.DateModified, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (!name && size && !type && !created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Size = i1.Size, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Size = i2.Size, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            //3
            else if (name && size && type && !created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Size = i1.Size, Type = i1.Type, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Size = i2.Size, Type = i2.Type, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (name && size && !type && created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Size = i1.Size, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Size = i2.Size, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (name && size && !type && !created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Size = i1.Size, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Size = i2.Size, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }

            else if (name && !size && type && created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Type = i1.Type, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Type = i2.Type, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (name && !size && type && !created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Type = i1.Type, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Type = i2.Type, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (name && !size && !type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, DateCreated = i1.DateCreated, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, DateCreated = i2.DateCreated, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (!name && size && type && created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Size = i1.Size, Type = i1.Type, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Size = i2.Size, Type = i2.Type, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (!name && size && type && !created && modified)

            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Size = i1.Size, Type = i1.Type, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Size = i2.Size, Type = i2.Type, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (!name && size && !type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Size = i1.Size, DateModified = i1.DateModified, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Size = i2.Size, DateModified = i2.DateModified, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (!name && !size && type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { DateModified = i1.DateModified, Type = i1.Type, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { DateModified = i2.DateModified, Type = i2.Type, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            //4
            else if (name && size && type && created && !modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Size = i1.Size, Type = i1.Type, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Size = i2.Size, Type = i2.Type, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (name && size && type && !created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Size = i1.Size, Type = i1.Type, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Size = i2.Size, Type = i2.Type, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (name && size && !type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Size = i1.Size, DateModified = i1.DateModified, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Size = i2.Size, DateModified = i2.DateModified, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (name && !size && type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, DateModified = i1.DateModified, Type = i1.Type, DateCreated = i1.DateCreated, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, DateModified = i2.DateModified, Type = i2.Type, DateCreated = i2.DateCreated, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            else if (!name && size && type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Size = i1.Size, Type = i1.Type, DateCreated = i1.DateCreated, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Size = i2.Size, Type = i2.Type, DateCreated = i2.DateCreated, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            //5
            else if (name && size && type && created && modified)
            {
                l1 = new List<MyFile>();
                l2 = new List<MyFile>();
                foreach (var i1 in list1)
                    l1.Add(new MyFile { Name = i1.Name, Size = i1.Size, Type = i1.Type, DateCreated = i1.DateCreated, DateModified = i1.DateModified, FilePath = i1.FilePath });
                foreach (var i2 in list2)
                    l2.Add(new MyFile { Name = i2.Name, Size = i2.Size, Type = i2.Type, DateCreated = i2.DateCreated, DateModified = i2.DateModified, FilePath = i2.FilePath });
                query = l1.Intersect(l2, new MyFile());
            }
            if (query.Any())
            {
                foreach (var q in query)
                {
                    lstResult.Items.Add(q);
                }
            }
            else
                MessageBox.Show("No common files");
        }
        /// <summary>
        /// Method that deletes the files 
        /// Calls UpdateEfterDeleteCopyMove IsCheckedItem methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (IsCheckedItem())
            {
                if (MessageBox.Show($"Are you sure to delete the file:", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool ok = false;
                    foreach (var q in query)
                    {
                        if (q.IsChecked)
                        {
                            if (FileUtilities.DeleteFile(q.FilePath))
                                ok = true;
                            else
                                ok = false;
                        }
                    }
                    if (ok)
                    {
                        MessageBox.Show($"The file has been deleted");
                        UpdateEfterDeleteCopyMove();
                    }

                }
            }
            else MessageBox.Show("check the file to be deleted");


        }
        /// <summary>
        /// Method that copy the files 
        /// Calls UpdateEfterDeleteCopyMove and IsCheckedItem methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyTo_Click(object sender, RoutedEventArgs e)
        {
            if (IsCheckedItem())
            {
                (bool Valid, string destPath) = FileUtilities.OpenFolderBrowserDialog("Select a folder where the file will be copied");
                if (Valid)
                {
                    bool ok = false;
                    foreach (var q in query)
                    {
                        if (q.IsChecked)
                        {
                            if (FileUtilities.CopyFile(q.Name, q.FilePath, destPath))
                                ok = true;
                            else
                                ok = false;
                        }
                    }
                    if (ok)
                    {
                        MessageBox.Show($"The file has been copied");
                        UpdateEfterDeleteCopyMove();
                    }
                }
            }
            else MessageBox.Show("check the file to be copied");
        }
        /// <summary>
        /// Method that returns if a item is checked
        /// </summary>
        /// <returns></returns>
        bool IsCheckedItem()
        {
            bool check = false;
            foreach (var c in query)
            {
                if (c.IsChecked)
                    check = true;
            }
            return check;
        }
        /// <summary>
        /// Method that moves the files 
        /// Calls UpdateEfterDeleteCopyMove and IsCheckedItem methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveTo_Click(object sender, RoutedEventArgs e)
        {
           
             if (IsCheckedItem())
             {
                 (bool Valid, string destPath) = FileUtilities.OpenFolderBrowserDialog("Select a folder where the file will be moved");
                 if (Valid)
                 {
                     bool ok = false;
                     foreach (var q in query)
                     {
                         if (q.IsChecked)
                         {
                             if (FileUtilities.MoveFile(q.FilePath, destPath + "\\" + q.Name))
                                 ok = true;
                             else
                                 ok = false;
                         }
                     }
                     if (ok)
                     {
                         MessageBox.Show($"The file has been moved");
                         UpdateEfterDeleteCopyMove();
                     }
                 }
             }
             else MessageBox.Show("check the file to be moved");
        }
        /// <summary>
        /// Method that selects all item in the result listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (query != null)
            {
                foreach (var i in query)
                    i.IsChecked = true;
                lstResult.Items.Clear();
                foreach (var q in query)
                    lstResult.Items.Add(q);
            }
            ManageButtons();
        }

        List<string> strings = new List<string>(); // list of paths it is used to check if the selected items contains the same paths
        /// <summary>
        /// Method that gets the selected items in the result list view
        /// </summary>
        void GetcheckedItems()
        {
            strings = new List<string>();
            if (query != null)
            {
                foreach (var i in query)
                {
                    if(i.IsChecked == true)
                    {
                        strings.Add(i.FilePath);
                    }
                }
            }
        }
        /// <summary>
        /// Methood that return true if the strings list contain same path , otherwise false
        /// 
        /// </summary>
        /// <returns></returns>
        bool ControllPath()
        {
            bool ok = false;
            GetcheckedItems();
            if(strings.Count != strings.Distinct().Count())
            {
                ok = true;
            }
            return ok;
        }

        /// <summary>
        /// methód that manages buttons every time an item in the list view is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ManageButtons();
        }
        /// <summary>
        /// methód that manages buttons every time an item in the list view is unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ManageButtons();
        }
    }
}
