using System.IO;
using UnityEditor;
using UnityEngine;

namespace Extensions.NoiseGenerator
{
    [CreateAssetMenu(menuName = "Extensions/PerlinNoiseGenerator", fileName = "PerlinNoiseGenerator")]
    public class PerlinNoiseGenerator : ScriptableObject
    {
        [SerializeField] private Texture2D _Texture;

        [SerializeField] private int _WaveResolution, _GridResolution;
        [SerializeField] private float _Scaling, _OffsetX, _OffsetY;
        private bool _IsTextureNull;
        private string path;
        
        internal void Generate()
        {
            var resolution = _WaveResolution * _GridResolution;
            _Texture = new Texture2D(resolution, resolution);
            
            for (var i = 0; i < _Texture.width; i++)
            {
                for (var j = 0; j < _Texture.height; j++)
                {
                    var x = (float) i / resolution * _Scaling + _OffsetX;
                    var y = (float) j / resolution * _Scaling + _OffsetX;
                    
                    var sample = Mathf.PerlinNoise(x, y);
                    var color = new Color(sample, sample, sample);
                    _Texture.SetPixel(i, j, color);
                }
            }
            
            _Texture.Apply();
            path = AssetDatabase.GetAssetPath(this);
            var bytes = _Texture.EncodeToPNG();
            File.WriteAllBytes(path + "PerlinNoiseTexture2D" + ".png", bytes);
        }
    }

    [CustomEditor((typeof(PerlinNoiseGenerator)))]
    public class PerlinNoiseGeneratorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);
            
            var baseClass = (PerlinNoiseGenerator) target;
            if (GUILayout.Button("Generate"))
            {
                baseClass.Generate();
            }
        }
    }
}
