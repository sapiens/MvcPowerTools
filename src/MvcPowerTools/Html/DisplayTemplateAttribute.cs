using System;

namespace MvcPowerTools.Html
{
    /// <summary>
    /// Tells html conventions to use the template with the same name as the type, instead of the defined html conventions.
    /// Template must be in a 'DisplayTemplates' directory
    /// </summary>
    public class DisplayTemplateAttribute:Attribute
    {
         
    }
    
    /// <summary>
    /// Tells html conventions to use the template with the same name as the type, instead of the defined html conventions.
    /// Template must be in a 'EditorTemplates' directory
    /// </summary>
    public class EditorTemplateAttribute:Attribute
    {
         
    }
}