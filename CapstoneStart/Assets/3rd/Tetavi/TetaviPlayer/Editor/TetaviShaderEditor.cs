using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class TetaviShaderEditor : ShaderGUI
{
    
    override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        bool first_properties = true;
        int expend_level = 0;
        // Goes all over the properties of the shader
        for (int i = 0; i < properties.Length;i++)
        {
            
            
            // makes arrow toggle from every property with name Show in it
            if (properties[i].name.Contains("Expend") && !properties[i].name.Contains("Stop"))
            {
                
                first_properties = false;
                
                properties[i].floatValue = GUILayout.Toggle(properties[i].floatValue > 0, properties[i].displayName, "Foldout", GUILayout.ExpandWidth(true)) ? 1 : 0;
                
                
                expend_node(ref materialEditor, ref properties, ref i, properties[i].floatValue > 0,1);
               
                           
               

            }
            else
            {
                if (first_properties)
                {
                    if(!properties[i].flags.HasFlag(MaterialProperty.PropFlags.HideInInspector))
                        materialEditor.ShaderProperty(properties[i], properties[i].name);
                }
            }
            
        }

        // get the render queue , enable gpu inctancing and double sided global illumination on the material
        base.OnGUI(materialEditor,new MaterialProperty[0]);
    }
    
    void expend_node(ref MaterialEditor materialEditor, ref MaterialProperty[] properties,ref int property,bool show,int depth)
    {
        if (!properties[property].name.Contains("Expend")) return;
        if (show)
        {
            
            for (int j = property + 1; j < properties.Length; j++)
            {
                if(properties[j].name.Contains("Expend") && !properties[j].name.Contains("Stop"))
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                    GUILayout.Label("", GUILayout.Width(depth * 10));
                    properties[j].floatValue = GUILayout.Toggle(properties[j].floatValue > 0, properties[j].displayName, "Foldout", GUILayout.ExpandWidth(false)) ? 1 : 0;
                    GUILayout.EndHorizontal();
                    expend_node(ref materialEditor, ref properties, ref j, properties[j].floatValue > 0, depth+1);
                }
                else
                {
                    if (properties[j].name.Contains("Stop"))
                    {
                        property = j;
                        return;
                    }
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                    GUILayout.Label("", GUILayout.Width(depth * 10));
                    materialEditor.ShaderProperty( properties[j], properties[j].name);
                    GUILayout.EndHorizontal();
                }
                
            }

        }
        else
        {
            int expend_level = 1;
            for (int j = property + 1; j < properties.Length; j++)
            {
                if (properties[j].name.Contains("Expend") && !properties[j].name.Contains("Stop"))
                {
                    expend_level++;
                }
                if (properties[j].name.Contains("Stop"))
                {
                    expend_level--;
                }
                if (expend_level == 0)
                {
                    property = j;
                    break;
                }
            }
        }
        
    }

}