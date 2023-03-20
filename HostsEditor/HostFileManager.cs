using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HostsEditor
{
    public class HostFileManager
    {
        static public void Generate(string filename, ArrayList data)
        {
            StringBuilder tmp = new StringBuilder();

            tmp.Append("# +----------------------------+\r\n");
            tmp.Append("# | 此文件由 爆笑小鱼头 生成 |\r\n");
            tmp.Append("# | 原文件已备份为 hosts.bak   |\r\n");
            tmp.Append("# +----------------------------+\r\n");

            foreach(string dd in data)
            {
                // hosts文件内容
                tmp.Append(dd + "\r\n");
            }

            // 保存 hosts
            try
            {
                File.WriteAllText(filename, tmp.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        static public void GenerateString(string filename, String tmp)
        {
            // 保存 hosts
            try
            {
                File.WriteAllText(filename, tmp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
