using System.Drawing;
using System.Windows.Forms;

namespace HostsEditor.Controls.Head
{
    public class HeadPanel
    {
        /// <summary>
        /// 实例化并生成输入框组件
        /// </summary>
        /// <param name="id"></param>
        public Panel CreateElement(int id, TextBox labelTextBox, CheckBox statusCheckBox, Label removeButton)
        {
            Panel panel = new Panel
            {
                Name = BuildElementId(id),
                Width = ShareData.HeadSize.Width,
                Height = ShareData.HeadSize.Height,
                BackColor = Color.White,
                Location = new Point(0, 0)
            };

            panel.Controls.Add(labelTextBox);
            panel.Controls.Add(statusCheckBox);
            panel.Controls.Add(removeButton);

            return panel;
        }

        /// <summary>
        /// 生成元素ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string BuildElementId(int id)
        {
            return string.Format("Head_Panel_" + id);
        }
    }
}
