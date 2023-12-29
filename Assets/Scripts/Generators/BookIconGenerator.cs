using UnityEngine;


namespace Ventura.Generators
{

    public class BookIconGenerator
    {

        private RenderTexture _renderTexture;

        private static BookIconGenerator _instance = new BookIconGenerator();
        public static BookIconGenerator Instance { get => _instance; }


        public void InitTemplateRendering()
        {
            _renderTexture = new RenderTexture(512, 512, 16);
            RenderTexture.active = _renderTexture;

            var renderingCamera = GameObject.Find("Template Camera");
            renderingCamera.GetComponent<Camera>().targetTexture = _renderTexture;
        }


        public Sprite GenerateBookIcon()
        {
            Debug.Log("GenerateBookIcon");

            var tex = new Texture2D(512, 512, TextureFormat.ARGB32, false);
            var sourceRegion = new Rect(0, 0, 512, 512);
            tex.ReadPixels(sourceRegion, 0, 0, false);

            //var pixelColors = tex.GetPixels();
            //for (int i = 0; i < pixelColors.Length; ++i)
            //{
            //    pixelColors[i] = Color.green;
            //}
            //tex.SetPixels(pixelColors);
            tex.Apply();

            var rect = new Rect(0, 0, tex.width, tex.height);
            var pivot = new Vector2(0.5f, 0.5f);
            var pixelsPerUnit = 512;
            return Sprite.Create(tex, rect, pivot, pixelsPerUnit);
        }
    }
}