using System;
using System.Drawing;
using System.Windows.Forms;

namespace HostsEditor.Controls.Head
{
    public delegate void UpdateFlowContainerHeight();

    public class BodyTextBox
    {
        /// <summary>
        /// 实例化并生成输入框组件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="label"></param>
        public TextBox CreateElement(int id, string text)
        {
            TextBox body = new TextBox
            {
                ReadOnly = false,
                Name = BuildElementId(id),
                Width = ShareData.HeadSize.Width - 20,
                Height = ShareData.FontHeight * ShareData.TextBoxInitLines,
                BackColor = Color.White,
                Location = new Point(10, 10),
                BorderStyle = BorderStyle.None,
                Text = text,
                Font = ShareData.DefaultFont,
                Multiline = true,
                MaxLength = 2000
            };

            if(body.Lines.Length > ShareData.TextBoxInitLines)
            {
                body.Height = ShareData.FontHeight * body.Lines.Length;
            }

            body.TextChanged += FireTextChanged;

            return body;
        }

        /// <summary>
        /// 生成元素ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string BuildElementId(int id)
        {
            return string.Format("Body_Content_" + id);
        }

        /// <summary>
        /// 文本框变高时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FireTextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Lines.Length < ShareData.TextBoxInitLines)
            {
                return;
            }

            int tbHeight = tb.Lines.Length * ShareData.FontHeight;
            int plHeight = tbHeight + 20 + 31;

            tb.Height = tbHeight;
            
            // 设置父级panel高度
            tb.Parent.Height = tbHeight + 20;
            tb.Parent.Parent.Height = plHeight;
        }
    }
}
