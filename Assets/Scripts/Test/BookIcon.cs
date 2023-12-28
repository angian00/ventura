using UnityEngine;

using Ventura.Generators;


namespace Ventura.Test
{

    public class BookIcon : MonoBehaviour
    {
        private bool hasCreatedIcon = false;
        private BookIconGenerator _generator;

        void Start()
        {
            _generator = BookIconGenerator.Instance;
            _generator.InitTemplateRendering();

            Camera.onPostRender += OnPostRenderCallback;
        }


        void OnPostRenderCallback(Camera cam)
        {
            if (cam.name != "Template Camera")
                return;

            Debug.Log("Camera callback: Camera name is " + cam.name);

            if (!hasCreatedIcon)
            {
                var icon = _generator.GenerateBookIcon();
                if (icon != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = icon;
                    Camera.onPostRender -= OnPostRenderCallback;
                    hasCreatedIcon = true;
                }
                else
                {
                    Debug.LogError("!! GenerateBookIcon failed");
                }
            }
        }

    }
}
