using HostsEditor.Item;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HostsEditor.Controls.Head
{
    public class HeadCheckBox : Container
    {
        readonly int ElementWidth = 45;

        // 元素位置
        Point ElementPoint
        {
            get
            {
                return new Point(ShareData.HeadSize.Width - ElementWidth - 3 - ShareData.RemoveButtonSize.Width, 0);
            }
        }

        /// <summary>
        /// 实例化并生成输入框组件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public CheckBox CreateElement(int id, string status)
        {
            CheckBox cb = new CheckBox
            {
                Name = BuildElementId(id),
                Width = ElementWidth,
                Height = ShareData.HeadSize.Height,
                Location = ElementPoint,
                AutoSize = false,
                FlatStyle = FlatStyle.System,
                Text = "启用",
                ForeColor = Color.FromArgb(255, 45, 45, 48),
                Checked = status == "active"
            };

            cb.CheckedChanged += Cb_CheckedChanged;

            return cb;
        }

        private void Cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            string id = ParseIdByControl(cb);

            string name = string.Format("Item_Panel_{0}", id);

            Panel itemPanel = (Panel)Closest(cb, name);

            Control[] tbs = itemPanel.Controls.Find("Head_Content_" + id, true);
            Control[] tbs2 = itemPanel.Controls.Find("Body_Content_" + id, true);

            if (cb.Checked)
            {
                tbs.First().ForeColor = tbs2.First().ForeColor = Color.Black;
            }
            else
            {
                tbs.First().ForeColor = tbs2.First().ForeColor = ShareData.DisabledColor;
            }
        }

        /// <summary>
        /// 生成元素ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string BuildElementId(int id)
        {
            return string.Format("Head_Status_" + id);
        }
    }
}
