using System.Drawing;
using System.Windows.Forms;

namespace HostsEditor.Controls.Head
{
    public class ItemPanel
    {
        /// <summary>
        /// 实例化并生成输入框组件
        /// </summary>
        /// <param name="id"></param>
        public Panel CreateElement(int id, Panel headPanel, Panel bodyPanel)
        {
            Panel panel = new Panel
            {
                Name = BuildElementId(id),
                Width = ShareData.HeadSize.Width,
                Height = bodyPanel.Height + ShareData.HeadSize.Height + 1,
                BackColor = Color.FromArgb(255, 234, 234, 234),
                Location = new Point(0, 0),
                Margin = new Padding(ShareData.ContainerMargin, ShareData.ContainerMargin, ShareData.ContainerMargin, 0)
            };

            panel.Controls.Add(headPanel);
            panel.Controls.Add(bodyPanel);

            return panel;
        }

        /// <summary>
        /// 生成元素ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string BuildElementId(int id)
        {
            return string.Format("Item_Panel_" + id);
        }
    }
}
