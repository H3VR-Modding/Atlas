using System;
using System.Collections.Generic;
using Atlas.Loaders;
using BepInEx.Logging;

namespace Atlas
{
    /// <summary>
    /// Constant values for Atlas
    /// </summary>
    public static class AtlasConstants
    {
        /// <summary>
        /// BepInEx Name of Atlas
        /// </summary>
        public const string Name = "Atlas";
        
        /// <summary>
        /// BepInEx GUID of Atlas
        /// </summary>
        public const string Guid = "nrgill28.Atlas";
        
        /// <summary>
        /// Version of Atlas
        /// </summary>
        public const string Version = "0.1.0";

        /// <summary>
        /// Stratum loader name for custom scenes
        /// </summary>
        public const string StratumLoaderName = "scene";
        
        internal const string LoaderSandbox = "sandbox";
        internal const string LoaderTakeAndHold = "takeandhold";
    }
    
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
        public static CustomSceneInfo? CurrentScene { get; internal set; }

        /// <summary>
        /// Registers a new scene loader
        /// </summary>
        /// <param name="mode">The ID of the mode</param>
        /// <param name="loader">The loader to register</param>
        public static void AddLoader(string mode, ISceneLoader loader)
        {
            Loaders.Add(mode, loader);
        }
        
        internal static readonly Dictionary<string, ISceneLoader> Loaders = new();

        /// <summary>
        /// Internal logger. Helpful for logging stuff in static contexts
        /// </summary>
        internal static ManualLogSource Logger = null!;

        /// <summary>
        /// Internal leaderboard lock. When loading custom scenes leaderboard calls are intercepted and blocked
        /// </summary>
        internal static IDisposable? LeaderboardLock;
    }
}