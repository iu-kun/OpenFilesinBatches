using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFilesinBatches
{
    internal class OpenFilesButton : Button
    {
        protected override async void OnClick()
        {
            await OpenTIFF();
        }

        /// <summary>
        /// 快速打开多个文件夹的同名文件
        /// </summary>
        /// <returns></returns>
        public static async Task OpenTIFF()
        {
            if (MapView.Active == null) return;
            Map map = MapView.Active.Map;
            if (map == null) return;

            try
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.Description = "请选择文件路径";
                dialog.SelectedPath = "C:\\";
                string foldPath = "";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    foldPath = dialog.SelectedPath;
                }

                DirectoryInfo folder = new DirectoryInfo(foldPath);//根据路径创建一个文件操作对象
                DirectoryInfo[] filefolds = folder.GetDirectories();//获取对象下的子文件夹

                for (int i = 0; i < filefolds.Length; i++)
                {
                    //同名文件名
                    string weekend_name = filefolds[i].FullName + "\\" + "name.tif";
                    Uri uri = new Uri(weekend_name);
                    await QueuedTask.Run(() => LayerFactory.Instance.CreateLayer(uri, map));                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

    }
}
