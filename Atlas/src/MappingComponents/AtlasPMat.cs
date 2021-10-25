using System.Collections.Generic;
using FistVR;
using UnityEngine;

// ReSharper disable StringLiteralTypo, IdentifierTypo, InconsistentNaming
namespace Atlas.MappingComponents
{
    public class AtlasPMat : PMat
    {
        public MatDefEnum AtlasMatDef;
        
        private void Awake()
        {
            if (!MatDef) MatDef = Resources.Load<MatDef>(MatDefResources[AtlasMatDef]);
        }

        #region MatDef

        public enum MatDefEnum
        {
            _Air = -366646783,
            _Impenetrable = 1725752409,
            Armor_Solid = 1242992293,
            Armor_Thick = -134865591,
            Armor_Thin = 1827179403,
            Claypot = 1943035998,
            ConcreteBarrier = 1461872896,
            Floor_CarpetOverConcrete = -1661987665,
            Floor_CarpetOverWood = -1942452169,
            Floor_Concrete = 757445278,
            Floor_Metal = 1483923932,
            Floor_MetalGrating = -1690371124,
            Floor_TileOverConcrete = 1213917816,
            Floor_TileOverConcreteSolid = 434755139,
            Floor_TileOverWood = 1335110134,
            Floor_Wood = 1960591488,
            GasMask = 1752877821,
            GasMask_heavy = 367776345,
            Ground_Dirt = -1675716207,
            Ground_Grass = 1942859388,
            Ground_Sand = -1487240928,
            Ground_Water = 383750667,
            HeavyArmor_Metal = -1347286364,
            HeavyFabric = 644301452,
            Hedge = 432136527,
            LightArmor_Kevlar = -1669265951,
            Meat = -871205235,
            MediumArmor_Metal = -635266500,
            MediumArmor_Padded = 332398969,
            Melee_Metal = 942411018,
            Melee_Polymer = 2144447471,
            Melee_Wood = -1896619314,
            Metal_Plate = 666790754,
            Metal_Sheet = -1853667375,
            Metal_Strut = -321340768,
            Paper = -1779142660,
            PlasticTableTop = -1149654923,
            Rock_Large = 1211416655,
            Rock_Small = -34193051,
            RotMeat = 1273826288,
            Sandbag = 1417815362,
            Solid_Armor = -529192027,
            Solid_Concrete = 1454812933,
            Solid_Metal = -213122109,
            Testing_MEat = 1985703140,
            Testing_Soft = 1729971855,
            Tires = -649847075,
            Wall_Brick = 892418936,
            Wall_DryWall = -1898909600,
            Wall_GlassBrick = -1258062406,
            Wall_GlassWindow = -1217966965,
            Wall_SheetMetal = 761218943,
            Wall_WoodSolid = 1081916419,
            Wall_WoodThick = -1271050433,
            Wall_WoodThin = 10600013,
            Water = -1779147911,
            WaterMelon = 610581220,
            WoodProp = -2101361410
        }

        private static readonly Dictionary<MatDefEnum, string> MatDefResources = new()
        {

            { MatDefEnum._Air, @"matdefs\_Air" },
            { MatDefEnum._Impenetrable, @"matdefs\structural\_Impenetrable" },
            { MatDefEnum.Armor_Solid, @"matdefs\structural\Armor_Solid" },
            { MatDefEnum.Armor_Thick, @"matdefs\structural\Armor_Thick" },
            { MatDefEnum.Armor_Thin, @"matdefs\structural\Armor_Thin" },
            { MatDefEnum.Claypot, @"matdefs\props\Claypot" },
            { MatDefEnum.ConcreteBarrier, @"matdefs\structural\ConcreteBarrier" },
            { MatDefEnum.Floor_CarpetOverConcrete, @"matdefs\structural\Floor_CarpetOverConcrete" },
            { MatDefEnum.Floor_CarpetOverWood, @"matdefs\structural\Floor_CarpetOverWood" },
            { MatDefEnum.Floor_Concrete, @"matdefs\structural\Floor_Concrete" },
            { MatDefEnum.Floor_Metal, @"matdefs\structural\Floor_Metal" },
            { MatDefEnum.Floor_MetalGrating, @"matdefs\structural\Floor_MetalGrating" },
            { MatDefEnum.Floor_TileOverConcrete, @"matdefs\structural\Floor_TileOverConcrete" },
            { MatDefEnum.Floor_TileOverConcreteSolid, @"matdefs\structural\Floor_TileOverConcreteSolid" },
            { MatDefEnum.Floor_TileOverWood, @"matdefs\structural\Floor_TileOverWood" },
            { MatDefEnum.Floor_Wood, @"matdefs\structural\Floor_Wood" },
            { MatDefEnum.GasMask, @"matdefs\agents\GasMask" },
            { MatDefEnum.GasMask_heavy, @"matdefs\agents\GasMask_heavy" },
            { MatDefEnum.Ground_Dirt, @"matdefs\nature\Ground_Dirt" },
            { MatDefEnum.Ground_Grass, @"matdefs\nature\Ground_Grass" },
            { MatDefEnum.Ground_Sand, @"matdefs\nature\Ground_Sand" },
            { MatDefEnum.Ground_Water, @"matdefs\nature\Ground_Water" },
            { MatDefEnum.HeavyArmor_Metal, @"matdefs\agents\HeavyArmor_Metal" },
            { MatDefEnum.HeavyFabric, @"matdefs\agents\HeavyFabric" },
            { MatDefEnum.Hedge, @"matdefs\nature\Hedge" },
            { MatDefEnum.LightArmor_Kevlar, @"matdefs\agents\LightArmor_Kevlar" },
            { MatDefEnum.Meat, @"matdefs\agents\Meat" },
            { MatDefEnum.MediumArmor_Metal, @"matdefs\agents\MediumArmor_Metal" },
            { MatDefEnum.MediumArmor_Padded, @"matdefs\agents\MediumArmor_Padded" },
            { MatDefEnum.Melee_Metal, @"matdefs\heldobjects\Melee_Metal" },
            { MatDefEnum.Melee_Polymer, @"matdefs\heldobjects\Melee_Polymer" },
            { MatDefEnum.Melee_Wood, @"matdefs\heldobjects\Melee_Wood" },
            { MatDefEnum.Metal_Plate, @"matdefs\structural\Metal_Plate" },
            { MatDefEnum.Metal_Sheet, @"matdefs\structural\Metal_Sheet" },
            { MatDefEnum.Metal_Strut, @"matdefs\structural\Metal_Strut" },
            { MatDefEnum.Paper, @"matdefs\props\Paper" },
            { MatDefEnum.PlasticTableTop, @"matdefs\props\PlasticTableTop" },
            { MatDefEnum.Rock_Large, @"matdefs\nature\Rock_Large" },
            { MatDefEnum.Rock_Small, @"matdefs\nature\Rock_Small" },
            { MatDefEnum.RotMeat, @"matdefs\agents\RotMeat" },
            { MatDefEnum.Sandbag, @"matdefs\structural\Sandbag" },
            { MatDefEnum.Solid_Armor, @"matdefs\structural\Solid_Armor" },
            { MatDefEnum.Solid_Concrete, @"matdefs\structural\Solid_Concrete" },
            { MatDefEnum.Solid_Metal, @"matdefs\structural\Solid_Metal" },
            { MatDefEnum.Testing_MEat, @"matdefs\heldobjects\Testing_MEat" },
            { MatDefEnum.Testing_Soft, @"matdefs\heldobjects\Testing_Soft" },
            { MatDefEnum.Tires, @"matdefs\props\Tires" },
            { MatDefEnum.Wall_Brick, @"matdefs\structural\Wall_Brick" },
            { MatDefEnum.Wall_DryWall, @"matdefs\structural\Wall_DryWall" },
            { MatDefEnum.Wall_GlassBrick, @"matdefs\structural\Wall_GlassBrick" },
            { MatDefEnum.Wall_GlassWindow, @"matdefs\structural\Wall_GlassWindow" },
            { MatDefEnum.Wall_SheetMetal, @"matdefs\structural\Wall_SheetMetal" },
            { MatDefEnum.Wall_WoodSolid, @"matdefs\structural\Wall_WoodSolid" },
            { MatDefEnum.Wall_WoodThick, @"matdefs\structural\Wall_WoodThick" },
            { MatDefEnum.Wall_WoodThin, @"matdefs\structural\Wall_WoodThin" },
            { MatDefEnum.Water, @"matdefs\structural\Water" },
            { MatDefEnum.WaterMelon, @"matdefs\props\WaterMelon" },
            { MatDefEnum.WoodProp, @"matdefs\props\WoodProp" }
        };

        #endregion
    }
}