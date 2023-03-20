using System.Drawing;
using System.Windows.Forms;

namespace HostsEditor.Controls.Head
{
    public class BodyPanel
    {
        /// <summary>
        /// 实例化并生成输入框组件
        /// </summary>
        /// <param name="id"></param>
        public Panel CreateElement(int id, TextBox dataTextBox)
        {
            Panel panel = new Panel
            {
                Name = BuildElementId(id),
                Width = ShareData.HeadSize.Width,
                Height = dataTextBox.Height + 20,
                BackColor = Color.White,
                // +1个偏移产生的上边框
                Location = new Point(0, ShareData.HeadSize.Height + 1)
            };

            panel.Controls.Add(dataTextBox);

            return panel;
        }

        /// <summary>
        /// 生成元素ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string BuildElementId(int id)
        {
            return string.Format("Body_Panel_" + id);
        }
    }
}
