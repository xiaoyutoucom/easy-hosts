using System.Drawing;

namespace HostsEditor.Controls
{
    public class ShareData
    {
        static public string HostsFileName = @"C:\Windows\System32\drivers\etc\hosts";

        static public string XmlFileName = @"C:\Windows\System32\drivers\etc\hosts.xml";

        static public Color DisabledColor = Color.FromArgb(255, 220, 220, 220);

        static public Font DefaultFont = new Font("Consolas", 14F, FontStyle.Regular, GraphicsUnit.Pixel, (byte)134);

        // 顶级容器尺寸
        static public Size ContainerSize
        {
            get
            {
                return new Size(640, 396);
            }
        }
        // 边距
        static public readonly int ContainerMargin = 10;
        // 水平边距(左+右)
        static public readonly int ContainerMarginHorizontal = 20;

        // 文本域初始行数
        static public readonly int TextBoxInitLines = 4;

        // 文字高度
        static public int FontHeight = DefaultFont.Height;

        // HeadPanel尺寸
        static public Size HeadSize
        {
            get
            {
                // 顶层容器宽度 - 顶层容器水平边距10*2 = 20
                return new Size(ContainerSize.Width - ContainerMarginHorizontal, 30);
            }
        }

        static public Size RemoveButtonSize
        {
            get
            {
                return new Size(45, HeadSize.Height);
            }
        }
    }
}
