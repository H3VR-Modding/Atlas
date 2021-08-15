using System;
using System.Collections.Generic;
using System.Linq;
using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents.TakeAndHold
{
    /// <summary>
    /// Use this component to override values on the TNH_Manager
    /// </summary>
    public class TNH_ManagerOverride : MonoBehaviour
    {
        [Serializable]
        public class AtlasPointSequence
        {
            public bool GenerateAutomatically = true;
            public TNH_SupplyPoint StartSupplyPoint = null!;
            public TNH_HoldPoint[] HoldPoints = null!;
        }

        [Header("Game State")] public bool IsBigLevel = false;
        public bool UsesClassicPatrolBehaviour = true;

        [Header("Data Settings")] public AtlasPointSequence[] PossibleSequences = null!;
        public TNH_SafePositionMatrix SafePosMatrix = null!;

        [Header("System Connections")] public List<TNH_HoldPoint> HoldPoints = null!;
        public List<TNH_SupplyPoint> SupplyPoints = null!;
        public Transform ScoreDisplayPoint = null!;

        internal void ApplyOverrides(TNH_Manager manager)
        {
            // Setup the simple stuff
            manager.LevelName = Atlas.CurrentScene?.Identifier;
            manager.IsBigLevel = IsBigLevel;
            manager.UsesClassicPatrolBehavior = UsesClassicPatrolBehaviour;
            manager.HoldPoints = HoldPoints;
            manager.SupplyPoints = SupplyPoints;
            manager.ScoreDisplayPoint = ScoreDisplayPoint;

            // Fix some references that may not have been set
            foreach (var supplyPoint in manager.SupplyPoints) supplyPoint.M = manager;
            
            // Generate a point sequence based on the info provided in the editor
            manager.PossibleSequnces = new List<TNH_PointSequence>();
            foreach (var sequence in PossibleSequences) manager.PossibleSequnces.Add(GeneratePointSequence(sequence));

            // If the safe position matrix is set use it, otherwise generate a default one.
            manager.SafePosMatrix = SafePosMatrix ? SafePosMatrix : GenerateSafePositionMatrix(manager);
        }

        // Generates a basic safe position matrix if one was not provided
        private TNH_SafePositionMatrix GenerateSafePositionMatrix(TNH_Manager manager)
        {
            TNH_SafePositionMatrix matrix = ScriptableObject.CreateInstance<TNH_SafePositionMatrix>();

            // Loop over each hold point index
            for (var i = 0; i < manager.HoldPoints.Count; i++)
            {
                // Make a new safe position matrix entry and add it to the matrix
                TNH_SafePositionMatrix.PositionEntry entry = new()
                {
                    SafePositions_HoldPoints = new List<bool>(),
                    SafePositions_SupplyPoints = new List<bool>()
                };
                matrix.Entries_HoldPoints.Add(entry);
                
                // For endless mode, let the game know which hold points are allowed to come after this one.
                // For simplicity, this will just be every point except itself.
                for (var j = 0; j < manager.HoldPoints.Count; j++)
                    entry.SafePositions_HoldPoints.Add(i != j);
                
                // This lets the game know which supply points are allowed to spawn given a specific hold point
                // For simplicity (and because this should NOT be used on big maps) it'll just be any points
                // that aren't marked for only spawning
                foreach (TNH_SupplyPoint supplyPoint in manager.SupplyPoints)
                    if (supplyPoint is AtlasSupplyPoint asp)
                        entry.SafePositions_SupplyPoints.Add(!asp.OnlySpawn);
                    else entry.SafePositions_SupplyPoints.Add(true);
            }
            
            // This method just copies the hold point entries into the supply point entries.
            // I think the supply point entries are only used on the level start.
            // Also yes this method name has a typo haha.
            matrix.GenereateSupplyPointsData();
            return matrix;
        }

        private TNH_PointSequence GeneratePointSequence(AtlasPointSequence sequence)
        {
            // Create a sequence object
            var pointSequence = ScriptableObject.CreateInstance<TNH_PointSequence>();
            if (sequence.GenerateAutomatically)
            {
                // TODO: Generate a sequence if it's selected
            }
            else
            {
                // If the sequence was authored manually just translate it to the index based sequence the game wants
                pointSequence.StartSupplyPointIndex = SupplyPoints.IndexOf(sequence.StartSupplyPoint);
                pointSequence.HoldPoints = sequence.HoldPoints.Select(x => HoldPoints.IndexOf(x)).ToList();
            }

            // Return the generated sequence
            return pointSequence;
        }
    }
}