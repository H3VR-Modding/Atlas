namespace Atlas
{
    /// <summary>
    /// Static class for interfacing with Atlas
    /// </summary>
    public static class Atlas
    {
        /// <summary>
        /// True if the currently loaded level is custom, false if it's vanilla
        /// </summary>
        public static bool IsCustomLevel { get; internal set; }
        
        /// <summary>
        /// The SceneInfo for the currently loaded custom scene or null if the current scene is not custom
        /// </summary>
        public static SceneInfo? CurrentScene { get; internal set; }
    }
}