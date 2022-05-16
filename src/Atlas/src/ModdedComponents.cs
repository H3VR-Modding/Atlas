using FistVR;

namespace Atlas
{
    /// <summary>
    /// Extension of the MainMenuSceneDef which links a main menu scene tile for a custom scene with its scene info.
    /// </summary>
    internal class CustomMainMenuSceneDef : MainMenuSceneDef
    {
        /// <summary>The custom scene info of the tile.</summary>
        public CustomSceneInfo CustomSceneInfo = null!;
    }

    /// <summary>
    /// Extension of the TNH LevelData object used for the level selector which links the level with the custom scene info.
    /// </summary>
    internal class CustomLevelData : TNH_UIManager.LevelData
    {
        /// <summary>The custom scene info of the level</summary>
        public readonly CustomSceneInfo SceneInfo; 
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="info">The custom scene info.</param>
        public CustomLevelData(CustomSceneInfo info)
        {
            IsModLevel = true;
            LevelAuthor = info.Author;
            LevelDescription = info.Description;
            LevelDisplayName = info.DisplayName;
            LevelID = info.Identifier;
            LevelImage = info.ThumbnailSprite;
            SceneInfo = info;
        }
    }
}