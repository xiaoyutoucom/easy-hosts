using System.Drawing;
using System.Windows.Forms;

namespace HostsEditor.Controls.Head
{
    public class HeadTextBox
    {
        /// <summary>
        /// 实例化并生成输入框组件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="label"></param>
        public TextBox CreateElement(int id, string label)
        {
            return new TextBox
            {
                Name = BuildElementId(id),
                Width = 400,
                Text = label,
                BackColor = Color.White,
                Location = new Point(7, 7),
                BorderStyle = BorderStyle.None
                //Font = new Font("Consolas", 14, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134))),
            };
        }

        /// <summary>
        /// 生成元素ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string BuildElementId(int id)
        {
            return string.Format("Head_Content_" + id);
        }
    }
}
