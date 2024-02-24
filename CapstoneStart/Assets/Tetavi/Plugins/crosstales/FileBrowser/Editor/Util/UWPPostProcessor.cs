#if UNITY_EDITOR && !CT_DJ
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using Enumerable = System.Linq.Enumerable;

namespace Crosstales.FB.EditorUtil
{
   /// <summary>Post processor for UWP (WSA).</summary>
   public static class UWPPostProcessor
   {
      [PostProcessBuildAttribute(1)]
      public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
      {
         if (EditorHelper.isWSAPlatform && EditorConfig.WSA_MODIFY_MANIFEST)
         {
            string file = $"{pathToBuiltProject}/{Application.productName}/Package.appxmanifest";

            Debug.Log($"File: {file}");

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(file);

            bool exists = Enumerable.Any(Enumerable.Cast<System.Xml.XmlAttribute>(xmlDoc.DocumentElement.Attributes), child => child.Name.CTEquals("xmlns:rescap"));

            if (!exists)
            {
               xmlDoc.DocumentElement.SetAttribute("xmlns:rescap", "http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities");
               xmlDoc.DocumentElement.SetAttribute("IgnorableNamespaces", "uap uap2 uap3 uap4 mp mobile iot rescap");
            }

            System.Xml.XmlNode capabilities = Enumerable.FirstOrDefault(Enumerable.Cast<System.Xml.XmlNode>(xmlDoc.DocumentElement.ChildNodes), child => child.Name.CTEquals("Capabilities"));

            if (capabilities == null)
            {
               capabilities = xmlDoc.CreateElement("Capabilities");
               xmlDoc.DocumentElement.AppendChild(capabilities);

               System.Xml.XmlElement capabilityBfsa = xmlDoc.CreateElement("rescap", "Capability", "");
               capabilityBfsa.SetAttribute("Name", "broadFileSystemAccess");
               capabilities.AppendChild(capabilityBfsa);

               System.Xml.XmlElement capabilityRemoveableStorage = xmlDoc.CreateElement("uap", "Capability", "");
               capabilityRemoveableStorage.SetAttribute("Name", "removableStorage");
               capabilities.AppendChild(capabilityRemoveableStorage);
            }
            else
            {
               exists = Enumerable.Any(Enumerable.Cast<System.Xml.XmlNode>(capabilities.ChildNodes), child => child.Name.CTEquals("rescap:Capability"));

               if (!exists)
               {
                  System.Xml.XmlElement capabilityBfsa = xmlDoc.CreateElement("rescap:Capability");
                  capabilityBfsa.SetAttribute("Name", "broadFileSystemAccess");
                  capabilities.AppendChild(capabilityBfsa);
               }
            }

            xmlDoc.Save(file);

            //TODO dirty hack, improve in the future!
            string content = System.IO.File.ReadAllText(file);
            content = content.Replace("<Capabilities xmlns=\"\">", "<Capabilities>");
            content = content.Replace("<Capability Name=\"broadFileSystemAccess\" />", "<rescap:Capability Name=\"broadFileSystemAccess\" />");
            content = content.Replace("<Capability Name=\"removableStorage\" />", "<uap:Capability Name=\"removableStorage\" />");
            System.IO.File.WriteAllText(file, content);
         }
      }
   }
}
#endif
// © 2021 crosstales LLC (https://www.crosstales.com)