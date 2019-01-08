using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XYZRefrenceDependencyTool
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        static string sName;
        OpenFileDialog ofd = new OpenFileDialog();
        Dictionary<string, string> objConst = new Dictionary<string, string>();

        /// <summary>
        /// Deault windows load.
        /// </summary>
        public HomePage()
        {
            InitializeComponent();
            txtPath.Text = @"Path from where you want to match this project dependency is available or not like \\Path\update_dll_are_here";
            btnClear_Click(null, null);
        }



        /// <summary>
        /// Load all preojct refrences in treeview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            // if ((item.Items.Count == 1) && (item.Items[0] is string))
            {
                item.Items.Clear();
                Assembly expandedDir = null;
                if (item.Tag is AssemblyName)
                    expandedDir = (item.Tag as Assembly);

                string tempName = ofd.Tag + ((System.Reflection.AssemblyName)(item.Tag)).Name + ".dll";
                //dll exist in ofd.tag then show green or show red color.
                string pathForX64 = txtPath.Text + ((System.Reflection.AssemblyName)(item.Tag)).Name + ".dll";
                var file = Directory.GetFiles(txtPath.Text, ((System.Reflection.AssemblyName)(item.Tag)).Name + ".dll", SearchOption.AllDirectories)
                    .FirstOrDefault();
                if (file == null)
                {
                //    // Handle the file not being found
                //}
                //else
                //{
                //    // The file variable has the *first* occurrence of that filename
                //}

                //if (!File.Exists(pathForX64))
                //{
                    try
                    {
                        string tempHeader = item.Header.ToString();
                        if (!objConst.ContainsKey(tempHeader))
                        {
                            objConst.Add(tempHeader, tempHeader);
                            trvNotExist.Items.Add(item.Tag.ToString());
                        }
                        lblStatus.Content = "Not Available in x64 bit.";
                        lblStatus.Foreground = Brushes.Red;
                    }
                    catch (Exception)
                    {

                    }
                    lblNot.Visibility = System.Windows.Visibility.Visible;
                    lblDep.Visibility = System.Windows.Visibility.Visible;
                    trvNotExist.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    lblStatus.Foreground = Brushes.Green;
                    lblStatus.Content = "Available in x64 bit.";
                }
                try
                {
                    Assembly a = System.Reflection.Assembly.ReflectionOnlyLoadFrom(tempName);
                    foreach (AssemblyName subDir in a.GetReferencedAssemblies())
                        item.Items.Add(CreateTreeItem(subDir));

                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// Create tree view structure for peoject dependencies
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private TreeViewItem CreateTreeItem(object o)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = ((System.Reflection.AssemblyName)(o)).Name;
            item.Tag = o;
            item.Items.Add("Loading...");
            return item;
        }

        /// <summary>
        /// Select prjoect dll.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ofd.ShowDialog();
                trvStructure.Items.Clear();
                sName = ofd.FileName;
                lblLoad.Content += ofd.SafeFileName;
                lblDep.Content += ofd.SafeFileName + ":";
                Assembly a = System.Reflection.Assembly.ReflectionOnlyLoadFrom(sName);
                foreach (AssemblyName an in a.GetReferencedAssemblies())
                {
                    trvStructure.Items.Add(CreateTreeItem(an));
                }
                ofd.Tag = ofd.FileName.Remove(ofd.FileName.Length - ofd.SafeFileName.Length);
                trvStructure.Visibility = System.Windows.Visibility.Visible;
                lblDep.Visibility = System.Windows.Visibility.Visible;
                lblLoad.Visibility = System.Windows.Visibility.Visible;
                lblNot.Visibility = System.Windows.Visibility.Visible;
                lblStatus.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception)
            {
                if (!string.IsNullOrWhiteSpace(ofd.SafeFileName))
                {
                    MessageBox.Show(string.Format("{0} is not a c# Dll/Exe. We are working on other's language.", ofd.SafeFileName), "XYZ Corporation", MessageBoxButton.OK, MessageBoxImage.Information);
                    btnClear_Click(null, null);
                }

            }
        }

        /// <summary>
        /// select tree view item from tree view list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;

            if (tvi == null || e.Handled) return;

            tvi.IsExpanded = !tvi.IsExpanded;
            e.Handled = true;
        }

        /// <summary>
        /// Reest all control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lblLoad.Content = "Loaded Assembly :";
            lblDep.Content = "Dependency DLL's for ";
            trvNotExist.Items.Clear();
            trvStructure.Items.Clear();
            trvStructure.Visibility = System.Windows.Visibility.Hidden;
            trvNotExist.Visibility = System.Windows.Visibility.Hidden;
            lblNot.Visibility = System.Windows.Visibility.Hidden;
            lblLoad.Visibility = System.Windows.Visibility.Hidden;
            lblDep.Visibility = System.Windows.Visibility.Hidden;
            lblStatus.Visibility = System.Windows.Visibility.Hidden;
            lblStatus.Content = "";
            if(objConst != null)
            {
                objConst.Clear();
            }
        }

        /// <summary>
        /// Close application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

}