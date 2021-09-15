using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            FillDriveNodes();
            treeView1.BeforeExpand += treeView1_BeforeExpand;
            treeView1.BeforeSelect += treeView1_BeforeSelect;

            //panel1.Paint += Example_GetThumb;
            pictureBox1.Paint+= Example_GetThumb;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;





        }

        Bitmap image1;
        
        string fullPath = null;
        //bool bDrawThumbnails = false;
        
        int x = 10;
        int y = 10;
        

        public bool ThumbnailCallback()
        {
            return false;
        }

        public void Example_GetThumb(object sender, PaintEventArgs e)
        {
            x = 10; y = 10;
            //this.Text = panel1.Height.ToString();
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            if (fullPath != null)
            {
                string[] files = Directory.GetFiles(fullPath);


                if (files.Length > 0)
                {
                    //panel1.SuspendLayout();
                    pictureBox1.SuspendLayout();
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i].EndsWith("jpg") || files[i].EndsWith("bmp"))
                        {
                            
                            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                            Bitmap myBitmap = new Bitmap(files[i]);
                            Image myThumbnail = myBitmap.GetThumbnailImage(
                                60, 60, myCallback, IntPtr.Zero);
                            e.Graphics.DrawImage(myThumbnail, x, y);

                            x += 70;

                            //if (panel1.Width - x < 70)
                            //{
                            //    x = 10;
                            //    y += 70;
                            //}

                            if (pictureBox1.Width - x < 70)
                            {
                                x = 10;
                                y += 70;
                            }
                        }
                    }
                    //panel1.ResumeLayout();
                    pictureBox1.ResumeLayout();
                }
            }

            

        }

        // событие перед раскрытием узла
        void treeView1_BeforeExpand(object sender,TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {
                
                if (Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if(dirs.Length!=0)
                    {
                        for(int i=0;i<dirs.Length;i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);

                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        //
        void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

            fullPath = e.Node.FullPath;

            //panel1.Invalidate();
            pictureBox1.Invalidate();


            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {

                if (Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if (dirs.Length != 0)
                    {
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);

                        }
                    }
                }
            }
            catch (Exception ex) { }


        }


        // получаем все диски на компьюьтере
        private void FillDriveNodes()
        {
            try
            {
                foreach(DriveInfo drive in DriveInfo.GetDrives())
                {
                    TreeNode driveNode = new TreeNode { Text = drive.Name };
                    FillTreeNode(driveNode, drive.Name);
                    treeView1.Nodes.Add(driveNode);

                }
            }
            catch(Exception e) 
            {
                //MessageBox.Show(e.Message);
            }
        }

        // получаем дочерние узлы для определенного узла
        private void FillTreeNode(TreeNode driveNode, string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach(string dir in dirs)
                {
                    TreeNode dirNode = new TreeNode();
                    dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                    driveNode.Nodes.Add(dirNode);
                }
            }
            catch (Exception e) 
            {
                //MessageBox.Show(e.Message);
            }
        }

       
    }
}
