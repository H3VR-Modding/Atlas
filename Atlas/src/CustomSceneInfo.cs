using System.IO;
using UnityEngine;
using Valve.Newtonsoft.Json;

namespace Atlas
{
    /// <summary>
    /// Serialized data about a custom scene that is packaged alongside it's asset bundle
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CustomSceneInfo
    {
        /// <summary>
        /// Display name for this scene
        /// </summary>
        [JsonProperty]
        public string DisplayName { get; private set; } = "";

        /// <summary>
        /// The identifier for this scene. Max 16 characters, should be a short version of the name.
        /// </summary>
        [JsonProperty]
        public string Identifier { get; private set; } = "";

        /// <summary>
        /// Game mode for this scene. Can be 'sandbox' or 'takeandhold' by default.
        /// </summary>
        [JsonProperty]
        public string Mode { get; private set; } = "";

        /// <summary>
        /// Authors of this scene
        /// </summary>
        [JsonProperty]
        public string Author { get; private set; } = "";

        /// <summary>
        /// Description of this scene
        /// </summary>
        [JsonProperty]
        public string Description { get; private set; } = "";

        /// <summary>
        /// The thumbnail as a texture.
        /// </summary>
        public Texture2D? ThumbnailTexture { get; }
        
        /// <summary>
        /// The thumbnail as a sprite
        /// </summary>
        public Sprite? ThumbnailSprite { get; }
        
        // The file pointer to the scene bundle
        internal FileInfo SceneBundleFile { get; }
        
        // The loaded asset bundle
        internal AssetBundle? SceneBundle;

        internal CustomSceneInfo(FileInfo sceneBundle)
        {
            SceneBundleFile = sceneBundle;
            var sceneJsonFile = new FileInfo(sceneBundle.FullName + ".json");
            var thumbnailFile = new FileInfo(sceneBundle.FullName + ".png");

            // We need the scene json file, so if there isn't one we need to throw.
            if (!sceneJsonFile.Exists) throw new FileNotFoundException(sceneJsonFile.Name + " is missing! It should be beside the scene bundle file.");
            JsonConvert.PopulateObject(File.ReadAllText(sceneJsonFile.FullName), this);
            
            // Missing thumbnail file is non-fatal but we still want to let the user know.
            if (!thumbnailFile.Exists) Atlas.Logger.LogWarning($"No {thumbnailFile.Name} was found. This scene will not have a thumbnail.");
            else
            {
                // Load the texture
                ThumbnailTexture = new Texture2D(1, 1);
                ThumbnailTexture.LoadImage(File.ReadAllBytes(thumbnailFile.FullName));
                
                // And also convert it to a sprite
                ThumbnailSprite = Sprite.Create(ThumbnailTexture,
                    new Rect(0, 0, ThumbnailTexture.width, ThumbnailTexture.height), new Vector2(0, 0));
            }
        }
    }
}