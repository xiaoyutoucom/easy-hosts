using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

using HostsEditor.Controls;
using HostsEditor.Controls.Head;

namespace HostsEditor
{
    public partial class Main : Form
    {
        int startIndex = 0;

         HeadTextBox HeadTextBoxElement = new HeadTextBox();
         HeadCheckBox HeadCheckBoxElement = new HeadCheckBox();
         HeadButton HeadButtonElement = new HeadButton();
         HeadPanel HeadPanelElement = new HeadPanel();
         BodyPanel BodyPanelElement = new BodyPanel();
         BodyTextBox BodyTextBoxElement = new BodyTextBox();
         ItemPanel ItemPanelElement = new ItemPanel();

        public Main()
        {
            InitializeComponent();

            // 初始化窗体宽度
            this.Width = ShareData.ContainerSize.Width + (this.Width - this.ClientRectangle.Width);
        }
            /// <summary>
            private void AddXMLLoad()
            {
            ArrayList used = new ArrayList();

            // 挑出启用和禁用的
            try
            {
                string[] contents = File.ReadAllLines(ShareData.HostsFileName);
                foreach (string content in contents)
                {
                    if (content.Length == 0 || content.IndexOf("#") == 0)
                    {
                        continue;
                    }

                    if (content.IndexOf('#') == -1)
                    {
                        used.Add(content);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            // 备份文件
            string backupFileName = ShareData.HostsFileName + ".bak";
            if (File.Exists(backupFileName))
            {
                File.Delete(backupFileName);
            }
            File.Copy(ShareData.HostsFileName, backupFileName);

            // 首次使用
            FirstSaveContent(used);
        }
            /// <summary>
            /// 1. 初始化 hosts.xml
            ///  1. 备份 hosts
            /// 2. 解析xml
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Main_Load(object sender, EventArgs e)
        {
            // 初次使用备份原文件
            if(!File.Exists(ShareData.XmlFileName))
            {
                AddXMLLoad();
            }

            // 解析xml数据
            ArrayList hosts = ParseXml();

            // 添加到窗体
            if(hosts.Count > 0)
            {
                foreach(Dictionary<string, string> item in hosts)
                {
                    AppendItemPanel(startIndex, item["label"], item["body"], item["status"]);
                }
            }
            else
            {
                AppendItemPanel(startIndex, "未命名", "", "active");
            }

            // 添加占位布局
            AppendPlaceholderItem();

            // 自动高度
            AutoSizeFlowContainerHeight();
        }

        /// <summary>
        /// 解析每的行映射数据
        /// </summary>
        /// <param name="lineContent"></param>
        /// <returns></returns>
        private Dictionary<string, string> ParseLine(string lineContent)
        {
            string content = Regex.Replace(lineContent.Trim(), @"(\s+)", "@");

            content = content.Replace("@#@", "@# ");

            string[] pieces = content.Split('@');

            string ip = pieces.First();
            string host = null;
            try { 
            host = pieces[1];
            }catch (Exception ex)
            {
                MessageBox.Show("格式错误，保存失败！");
            }
            string comment = "";

            if (pieces.Length == 3)
            {
                comment = pieces[2];
            }
            return new Dictionary<string, string> {
                {"ip", ip },
                {"host", host },
                {"comment", comment },
            };
        }

        /// <summary>
        /// 格式化每组
        /// </summary>
        /// <param name="groupItem"></param>
        /// <param name="hostMaxLength"></param>
        /// <returns></returns>
        private string FormatGroupItem(ArrayList groupItem, int hostMaxLength)
        {
            // ip + 3个空格 + host
            hostMaxLength += 3;

            StringBuilder sBuilder = new StringBuilder();

            foreach (Dictionary<string, string> r in groupItem)
            {
                // ip不足15位时补充剩余的为空格
                string ip = r["ip"].PadRight(15, ' ');
                // 以最长一个host为标准的位数
                string host = r["host"].PadLeft(hostMaxLength, ' ');

                string tmp = ip + host;

                if (r["comment"].Length > 0)
                {
                    // # 后面没有空格时自动添加空格
                    if (r["comment"].Substring(1, 1) != " ")
                    {
                        r["comment"] = "# " + r["comment"].Substring(1);
                    }
                    tmp += "   " + r["comment"];
                }
                sBuilder.Append(tmp + "\r\n");
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 首次存储文件
        /// </summary>
        /// <param name="used"></param>
        /// <param name="notUsed"></param>
        private void FirstSaveContent(ArrayList used)
        {
            // hosts文件数据
            ArrayList hostsLines = new ArrayList();

            ArrayList groups = new ArrayList();
            ArrayList hosts = new ArrayList();

            if (used.Count > 0)
            {
                groups.Add(new Dictionary<string, string>(){
                    { "label", "未命名分组" },
                    { "status", "active" }
                });

                ArrayList groupItem = new ArrayList();

                foreach (string dd in used)
                {
                    // hosts文件数据
                    hostsLines.Add(dd);
                    // xml文件数据
                    groupItem.Add(ParseLine(dd));
                }
                hosts.Add(groupItem);
            }

            // 保存hosts
            HostFileManager.Generate(ShareData.HostsFileName, hostsLines);

            // 保存hosts.xml
            DataFileManager.Generate(ShareData.XmlFileName, groups, hosts);
        }

        /// <summary>
        /// 添加占位的
        /// </summary>
        private void AppendPlaceholderItem()
        {
            // 新增
            Panel addPanel = new Panel
            {
                Name = "Panel_Placeholder",
                Width = ShareData.HeadSize.Width,
                Height = 0,
                BackColor = Color.FromArgb(255, 243, 243, 243),
                Location = new Point(0, 0),
                Margin = new Padding(ShareData.ContainerMargin)
            };

            this.FlowContainer.Controls.Add(addPanel);
        }

        /// <summary>
        /// 追加项目
        /// </summary>
        /// <param name="index"></param>
        /// <param name="label"></param>
        /// <param name="body"></param>
        /// <param name="status"></param>
        private void AppendItemPanel(int index, string label, string body, string status)
        {
            TextBox labelTextBox = HeadTextBoxElement.CreateElement(index, label);
            CheckBox statusCheckBox = HeadCheckBoxElement.CreateElement(index, status);
            Label removeButton = HeadButtonElement.CreateElement(index);
            Panel headPanel = HeadPanelElement.CreateElement(index, labelTextBox, statusCheckBox, removeButton);

            TextBox bodyTextBox = BodyTextBoxElement.CreateElement(index, body);
            Panel bodyPanel = BodyPanelElement.CreateElement(index, bodyTextBox);

            Panel itemPanel = ItemPanelElement.CreateElement(index, headPanel, bodyPanel);

            // 初始禁用状态
            if(status == "inactive")
            {
                labelTextBox.ForeColor = bodyTextBox.ForeColor = ShareData.DisabledColor;
            }

            startIndex++;

            this.FlowContainer.Controls.Add(itemPanel);
        }

        /// <summary>
        /// 容器自增高度
        /// </summary>
        private void AutoSizeFlowContainerHeight()
        {
            // 自动滚动
            Panel flowContainerPanel = this.FlowContainer;

            int count = flowContainerPanel.Controls.Count;

            int h2 = 0;
            for(int i=0; i<count; i++)
            {
                h2 += (flowContainerPanel.Controls[i].Height + ShareData.ContainerMargin);
            }

            if (ShareData.ContainerSize.Height <= h2)
            {
                if (!flowContainerPanel.AutoScroll)
                {
                    flowContainerPanel.AutoScroll = true;
                    flowContainerPanel.Width += SystemInformation.VerticalScrollBarWidth;
                    flowContainerPanel.Parent.Width += SystemInformation.VerticalScrollBarWidth;
                }
            }
            else
            {
                if (flowContainerPanel.AutoScroll)
                {
                    flowContainerPanel.AutoScroll = false;
                    flowContainerPanel.Width -= SystemInformation.VerticalScrollBarWidth;
                    flowContainerPanel.Parent.Width -= SystemInformation.VerticalScrollBarWidth;
                }
            }
        }

        /// <summary>
        /// 解析xml文件
        /// </summary>
        /// <returns></returns>
        private ArrayList ParseXml()
        {
            XmlDocument xml = new XmlDocument();

            xml.Load(ShareData.XmlFileName);

            XmlNode root = xml.SelectSingleNode("groups");

            XmlNodeList groupItemNodes = root.SelectNodes("group-item");

            ArrayList groups = new ArrayList();

            foreach (XmlNode groupItemNode in groupItemNodes)
            {
                XmlNode hostsItemNode = groupItemNode.SelectSingleNode("hosts");
                XmlNodeList hostsItemNodes = hostsItemNode.SelectNodes("host-item");

                ArrayList groupItem = new ArrayList();

                int hostMaxLength = 0;
                for (int j=0; j< hostsItemNodes.Count; j++)
                {
                    XmlNode hostItemNode = hostsItemNodes[j];

                    string ip = hostItemNode.SelectSingleNode("ip").InnerText;//.PadRight(15, ' ');
                    string host = hostItemNode.SelectSingleNode("host").InnerText;
                    string comment = hostItemNode.SelectSingleNode("comment").InnerText;

                    // 如果有注释时
                    if(comment.Length > 0) {
                        // 从xml解析出的comment没有带＃号
                        //comment = "# " + comment;
                    }

                    groupItem.Add(new Dictionary<string, string> {
                        { "ip", ip },
                        { "host", host },
                        { "comment", comment }
                    });

                    // 标记最长的
                    if (host.Length > hostMaxLength)
                    {
                        hostMaxLength = host.Length;
                    }
                }

                groups.Add(new Dictionary<string, string>
                {
                    { "label", groupItemNode.SelectSingleNode("label").InnerText },
                    { "status", groupItemNode.SelectSingleNode("status").InnerText },
                    { "body", FormatGroupItem(groupItem, hostMaxLength) }
                });
            }
            return groups;
        }

        /// <summary>
        /// 格式化窗体中的每组
        /// </summary>
        private void FormatAllItems()
        {
            // 去掉最后一个 Item_Placeholder
            int count = this.FlowContainer.Controls.Count - 1;

            for(int i=0; i < count; i++)
            {
                Panel itemPanel = (Panel) FlowContainer.Controls[i];

                string id = itemPanel.Name.Split('_').Last();

                Control[] bodyTextBoxes = itemPanel.Controls.Find("Body_Content_" + id, true);

                TextBox bodyTextBox = (TextBox)bodyTextBoxes.First();

                string[] lines = bodyTextBox.Lines;

                if(lines.Length == 0)
                {
                    continue;
                }

                ArrayList groupItem = new ArrayList();

                int hostMaxLength = 0;
                foreach (string content in lines)
                {
                    if(content.Length == 0)
                    {
                        continue;
                    }

                    Dictionary<string, string> lineContent = ParseLine(content);
                    if (lineContent == null)
                    {
                        break;
                        return;
                    }
                    groupItem.Add(lineContent);

                    if (lineContent["host"].Length > hostMaxLength)
                    {
                        hostMaxLength = lineContent["host"].Length;
                    }
                }
                // 格式化后赋值
                bodyTextBox.Text = FormatGroupItem(groupItem, hostMaxLength);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            AppendItemPanel(startIndex, "请输入名称", "", "active");

            this.FlowContainer.Controls.RemoveByKey("Panel_Placeholder");

            AppendPlaceholderItem();

            AutoSizeFlowContainerHeight();
        }

        /// <summary>
        /// 美化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatButton_Click(object sender, EventArgs e)
        {
            FormatAllItems();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            // 格式化
            FormatAllItems();

            // 获取每组数据
            // 去掉最后一个 Item_Placeholder
            int count = this.FlowContainer.Controls.Count - 1;

            // hosts文件数据
            ArrayList hostsLines = new ArrayList();

            ArrayList groups = new ArrayList();
            ArrayList hosts = new ArrayList();

            for (int i = 0; i < count; i++)
            {
                Panel itemPanel = (Panel)FlowContainer.Controls[i];

                string id = itemPanel.Name.Split('_').Last();

                // label
                Control[] labelTextBoxes = itemPanel.Controls.Find("Head_Content_" + id, true);
                // status
                Control[] statusCheckBoxes = itemPanel.Controls.Find("Head_Status_" + id, true);
                // 
                Control[] bodyTextBoxes = itemPanel.Controls.Find("Body_Content_" + id, true);

                TextBox labelTextBox = (TextBox)labelTextBoxes.First();
                CheckBox statusCheckBox = (CheckBox)statusCheckBoxes.First();
                TextBox bodyTextBox = (TextBox)bodyTextBoxes.First();

                string[] lines = bodyTextBox.Lines;

                if (lines.Length == 0)
                {
                    continue;
                }

                groups.Add(new Dictionary<string, string> {
                    { "label", labelTextBox.Text },
                    { "status", statusCheckBox.Checked? "active" : "inactive" }
                });

                hostsLines.Add("\r\n\r\n# " + labelTextBox.Text);

                ArrayList groupItem = new ArrayList();
                foreach (string content in lines)
                {
                    if (content.Length == 0)
                    {
                        continue;
                    }

                    groupItem.Add(ParseLine(content));

                    // hosts文件 启用和禁用
                    if(statusCheckBox.Checked)
                    {
                        hostsLines.Add(content);
                    }
                    else
                    {
                        hostsLines.Add("# " + content);
                    }
                }
                hosts.Add(groupItem);
            }

            // 保存 hosts
            HostFileManager.Generate(ShareData.HostsFileName, hostsLines);

            // 保存 hosts.xml
            DataFileManager.Generate(ShareData.XmlFileName, groups, hosts);

            MessageBox.Show("保存成功", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 保存并关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            // 先保存
            SaveButton_Click(sender, e);
            // 关闭
            this.Close();
        }

        /// <summary>
        ///  打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButton_Click(object sender, EventArgs e)
        {
            string editor = @"C:\Program Files (x86)\Notepad++\notepad++.exe";

            if(!File.Exists(editor))
            {
                editor = "notepad";
            }

            Process.Start(editor, ShareData.HostsFileName);
        }

        private void upGithub_Click(object sender, EventArgs e)
        {

            DialogResult d = MessageBox.Show(
            "该操作会重置所有配置信息，是否同意",
            "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (d== DialogResult.OK)
            {
            string baseUrl = "https://gitee.com/fliu2476/github-hosts/raw/main/hosts";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
                request.Method = "GET";

                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var resStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(resStream, System.Text.Encoding.UTF8);
                string result = sr.ReadToEnd();
                Console.Write(result);
                // 保存 hosts
                HostFileManager.GenerateString(ShareData.HostsFileName, result);
                MessageBox.Show("更新成功");
                //重新加载
                File.Delete(ShareData.XmlFileName);
                AddXMLLoad();
                // 解析xml数据
                ArrayList hosts = ParseXml();
                this.FlowContainer.Controls.Clear();
                // 添加到窗体
                if (hosts.Count > 0)
                {
                    foreach (Dictionary<string, string> item in hosts)
                    {
                        AppendItemPanel(startIndex, item["label"], item["body"], item["status"]);
                    }
                }
                else
                {
                    AppendItemPanel(startIndex, "未命名", "", "active");
                }

                // 添加占位布局
                AppendPlaceholderItem();

                // 自动高度
                AutoSizeFlowContainerHeight();

            }
        }
    }
}
