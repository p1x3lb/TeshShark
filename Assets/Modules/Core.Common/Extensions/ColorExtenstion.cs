using UnityEngine;
using UnityEngine.UI;

namespace Utils.Project.Scripts.Modules.Utils.Extensions
{
    public static class ColorExtenstion
    {
        public static Color SetAlpha(this Color color, float value)
        {
            var newColor = color;
            newColor.a = value;
            return newColor;
        }
        
        public static void SetAlpha(this Image image, float value)
        {
            var newColor = image.color;
            newColor.a = value;
            image.color = newColor;
        }
    }
}