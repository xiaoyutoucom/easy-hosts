using System;
using System.Drawing;
using System.Windows.Forms;
using HostsEditor.Item;

namespace HostsEditor.Controls.Head
{
    public class HeadButton : Container
    {
        readonly int ElementWidth = 45;

        // 元素位置
        Point ElementPoint
        {
            get
            {
                return new Point(ShareData.HeadSize.Width - ElementWidth - 3, 0);
            }
        }
        /// <summary>
        /// 实例化并生成输入框组件
        /// </summary>
        /// <param name="id"></param>
        public Label CreateElement(int id)
        {
            Label label = new Label
            {
                Name = BuildElementId(id),
                Width = ElementWidth,
                Height = ShareData.HeadSize.Height,
                Location = ElementPoint,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(255, 241, 112, 122),
                Text = "删除",
            };

            label.Click += Label_Click;

            return label;
        }

        private void Label_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定删除此组配置?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if(dr == DialogResult.OK)
            {
                Label label = (Label)sender;

                string id = ParseIdByControl(label);

                string name = string.Format("Item_Panel_{0}", id);

                Panel itemPanel = (Panel) Closest(label, name);

                if(itemPanel != null)
                {
                    itemPanel.Parent.Controls.RemoveByKey(name);
                }
            }
        }

        /// <summary>
        /// 生成元素ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string BuildElementId(int id)
        {
            return string.Format("Head_Button_" + id);
        }
    }
}
