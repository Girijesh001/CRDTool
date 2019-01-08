using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace XYZRefrenceDependencyTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Assembly[] drives = System.Reflection.Assembly.ReflectionOnlyLoadFrom(tempName);
            //foreach (DriveInfo driveInfo in drives)
            //    trvStructure.Items.Add(CreateTreeItem(driveInfo));
            txtPath.Text = @"\\Pathofprojectdependencies";
        }

        public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            // if(!((TreeViewItem)e.Source).Header.ToString().Contains("System"))
            if ((item.Items.Count == 1) && (item.Items[0] is string))
            {
                item.Items.Clear();
                //string tempName = @"\\Pathofprojectdependencies" + ((System.Reflection.AssemblyName)(item.Tag)).Name + ".dll";
                //Assembly a = System.Reflection.Assembly.ReflectionOnlyLoadFrom(tempName);
                //// "Referenced assemblies:");
                //foreach (AssemblyName an in a.GetReferencedAssemblies())
                //{
                //    trvStructure.Items.Add(CreateTreeItem(an));
                //}
                //}
                //if (item.Tag is DriveInfo)
                //    expandedDir = (item.Tag as DriveInfo).RootDirectory;
                //if (item.Tag is DirectoryInfo)
                //    expandedDir = (item.Tag as DirectoryInfo);
                //try
                //{
                //    foreach (DirectoryInfo subDir in expandedDir.GetDirectories())
                //        item.Items.Add(CreateTreeItem(subDir));
                //}
                //catch { }


                Assembly expandedDir = null;
                //if (item.Tag is AssemblyName)
                //  expandedDir = (item.Tag as AssemblyName).;
                if (item.Tag is AssemblyName)
                    expandedDir = (item.Tag as Assembly);

                string tempName = ofd.Tag + ((System.Reflection.AssemblyName)(item.Tag)).Name + ".dll";
                //dll exist in ofd.tag then show green or show red color.
                string pathForX64 = txtPath.Text + ((System.Reflection.AssemblyName)(item.Tag)).Name + ".dll";
                Dictionary<string, string> objConst = new Dictionary<string, string>();
                if (!File.Exists(pathForX64))
                {
                    try
                    {

                        objConst.Add(item.Header.ToString(), item.Header.ToString());
                        trvNotExist.Items.Add(item.Tag.ToString());
                    }
                    catch (Exception)
                    {

                    }
                }
                // string tempName = @"\\Pathofprojectdependencies" + ((System.Reflection.AssemblyName)(item.Tag)).Name + ".dll";
                try
                {
                    Assembly a = System.Reflection.Assembly.ReflectionOnlyLoadFrom(tempName);
                    foreach (AssemblyName subDir in a.GetReferencedAssemblies())
                        item.Items.Add(CreateTreeItem(subDir));
                }
                catch { }

            }
        }

        private TreeViewItem CreateTreeItem(object o)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = ((System.Reflection.AssemblyName)(o)).Name;
            item.Tag = o;
            item.Items.Add("Loading...");
            return item;
        }
        static string sName;
        OpenFileDialog ofd = new OpenFileDialog();
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ofd.ShowDialog();
                trvStructure.Items.Clear();
                sName = ofd.FileName;
                lblAssemblyName.Content += ofd.SafeFileName;
                //lblProjectName.Content = sName; 
                //foreach (DriveInfo driveInfo in drives)
                //    trvStructure.Items.Add(CreateTreeItem(driveInfo));

                //System.Reflection.Assembly myDllAssembly =
                // System.Reflection.Assembly.LoadFile(sName);

                //System.Type MyDLLFormType = myDllAssembly.GetType(sName);
                Assembly a = System.Reflection.Assembly.ReflectionOnlyLoadFrom(sName);
                // "Referenced assemblies:");
                foreach (AssemblyName an in a.GetReferencedAssemblies())
                {
                    trvStructure.Items.Add(CreateTreeItem(an));
                   
                }
                ofd.Tag = ofd.FileName.Remove(ofd.FileName.Length - ofd.SafeFileName.Length);//	"\\\\tahoe\\Solution-Test\\winintel64\\"	string
            }
            catch (Exception)
            {
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            lblAssemblyName.Content = "Loaded Assembly :";
            trvNotExist.Items.Clear();
            trvStructure.Items.Clear();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

}