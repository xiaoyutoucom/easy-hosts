using System.Linq;
using System.Windows.Forms;

namespace HostsEditor.Item
{
    public class Container
    {
        public string ParseIdByControl(Control control)
        {
            return control.Name.Split('_').Last();
        }

        public object Closest(Control child, string name)
        {
            Control control = child;
            while((control = control.Parent) != null)
            {
                if(control.Name == name) {
                    return control;
                }
            }
            return null;
        }
    }
}
