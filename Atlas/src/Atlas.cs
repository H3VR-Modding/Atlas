using System;
using System.Collections.Generic;
using Atlas.Loaders;
using BepInEx.Logging;

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
        /// Internal state variable. Used to determine the correct value of <see cref="IsCustomLevel"/>
        /// </summary>
        internal static bool IsSceneLoadInitiatedByMe = false;
    }
}