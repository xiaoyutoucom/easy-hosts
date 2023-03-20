using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HostsEditor
{
    public class DataFileManager
    {
        static public void Generate(string filename, ArrayList groups, ArrayList hosts)
        {
            StringBuilder xmlContent = new StringBuilder();

            xmlContent.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
            xmlContent.Append("<groups>\r\n");
            if (groups.Count > 0)
            {
                int i = 0;
                foreach(Dictionary<string, string> item in groups)
                {
                    xmlContent.Append("  <group-item>\r\n");
                    xmlContent.Append("    <label>"+ item["label"] +"</label>\r\n");
                    xmlContent.Append("    <status>"+ item["status"] +"</status>\r\n");
                    xmlContent.Append("    <hosts>\r\n");

                    if(hosts[i] != null)
                    {
                        ArrayList tmpHosts = (ArrayList)hosts[i];

                        foreach (Dictionary<string, string> r in tmpHosts)
                        {
                            xmlContent.Append("      <host-item>\r\n");
                            xmlContent.Append("        <ip>" + r["ip"] + "</ip>\r\n");
                            xmlContent.Append("        <host>" + r["host"] + "</host>\r\n");
                            xmlContent.Append("        <comment>" + r["comment"] + "</comment>\r\n");
                            xmlContent.Append("      </host-item>\r\n");
                        }
                    }

                    xmlContent.Append("    </hosts>\r\n");
                    xmlContent.Append("  </group-item>\r\n");
                    i++;
                }
            }
            xmlContent.Append("</groups>\r\n");

            // 写入
            try
            {
                File.WriteAllText(filename, xmlContent.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
