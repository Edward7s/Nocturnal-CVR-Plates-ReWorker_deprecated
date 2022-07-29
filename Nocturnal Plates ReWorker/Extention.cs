using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
namespace Nocturnal
{
    internal static class Extention
    {
        private static Texture2D _Texture2d { get; set; }
        internal static Image ChangeSpriteFromString(this Image Image, string ImageBase64, int pixels = 200, Vector4 border = new Vector4())
        { 
            _Texture2d = new Texture2D(256, 256);
            ImageConversion.LoadImage(_Texture2d, Convert.FromBase64String(ImageBase64));
            Image.sprite = Sprite.Create(_Texture2d, new Rect(0, 0, _Texture2d.width, _Texture2d.height), new Vector2(0, 0), pixels, 1000u, SpriteMeshType.FullRect, border, false);
            return Image;
        }
    }
}
