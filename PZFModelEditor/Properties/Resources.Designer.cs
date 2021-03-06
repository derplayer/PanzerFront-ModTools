//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PZFModelEditor.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PZFModelEditor.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 440 core
        ///
        ///in vec2 v_uv;
        ///in vec4 v_color;
        ///in vec3 v_norm;
        ///in mat4 v_mv;
        ///in vec3 v_fragPos;
        ///
        ///out vec4 color;
        ///
        ///uniform sampler2D u_texture;
        ///uniform vec4 u_stripColor;
        ///uniform vec3 u_lightPos;
        ///uniform vec3 u_lightColor;
        ///
        ///// 0 = shaded
        ///// 1 = flat
        ///// 2 = normal
        ///// 3 = uv
        ///// 4 = color
        ///// 5 = colorOverride
        ///uniform int u_drawMode;
        ///
        ///void main(void)
        ///{
        ///	vec3 n = normalize(mat3(v_mv) * v_norm);
        ///	vec2 uv = v_uv;
        ///
        ///	if (u_drawMode == 0)
        ///	{
        ///		//ambient
        ///		float ambientStrength = 0.2;        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string fragmentShader {
            get {
                return ResourceManager.GetString("fragmentShader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 440 core
        ///
        ///layout(location = 0) in vec3 position;
        ///layout(location = 1) in vec3 normal;
        ///layout(location = 2) in vec2 uv;
        ///layout(location = 3) in vec4 color;
        ///
        ///out vec2 v_uv;
        ///out vec4 v_color;
        ///out vec3 v_norm;
        ///out mat4 v_mv;
        ///out vec3 v_fragPos;
        ///
        ///uniform mat4 u_model;
        ///uniform mat4 u_view;
        ///uniform mat4 u_projection;
        ///
        ///void main(void)
        ///{
        ///	v_norm = normal;
        ///	v_mv = /* u_view *  */ u_model;
        ///	v_uv = uv;
        ///	v_color = color;
        ///	v_fragPos = vec3(u_model * vec4(position, 1.0));
        ///
        ///	gl_Position =  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string vertexShader {
            get {
                return ResourceManager.GetString("vertexShader", resourceCulture);
            }
        }
    }
}
