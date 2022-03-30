using FistVR;

namespace Atlas
{
    internal class CustomMainMenuSceneDef : MainMenuSceneDef
    {
        public CustomSceneInfo CustomSceneInfo = null!;
    }

    internal class CustomLevelData : TNH_UIManager.LevelData
    {
        public readonly CustomSceneInfo SceneInfo; 
        
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