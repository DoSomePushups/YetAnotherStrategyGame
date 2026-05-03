using Model;
using Svg;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace YetAnotherStrategyGame.Views
{
    public class SvgClass
    {
        private static Image LoadSvg(string fileName, int drawScale)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"YetAnotherStrategyGame.Views.SVGs.{fileName}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new Exception($"Could not find resource: {resourceName}");

                return SvgDocument.Open<SvgDocument>(stream).Draw(drawScale, drawScale);
            }
        }

        public static readonly Dictionary<EntityType, Image> SvgImagesEntities = new()
        {
            [EntityType.Human] = LoadSvg("Human.svg", 80),
            [EntityType.Warrior] = LoadSvg("warrior.svg", 80),
            [EntityType.Crossbowman] = LoadSvg("crossbowman.svg", 80),
            [EntityType.Cannon] = LoadSvg("cannon_unit.svg", 80),
            [EntityType.Farm] = LoadSvg("farm.svg", 80),
            [EntityType.Mine] = LoadSvg("GoldMine.svg", 80),
            [EntityType.Castle] = LoadSvg("castle.svg", 80),
            [EntityType.Barracks] = LoadSvg("SwordBarracks.svg", 80),
            [EntityType.CrossbowWorkshop] = LoadSvg("CrossbowBarracks.svg", 80),
            [EntityType.CannonFactory] = LoadSvg("CannonFactory.svg", 80)
        };

        public static readonly Dictionary<EntityType, Image> SvgImagesEntitiesBig = new()
        {
            [EntityType.Human] = LoadSvg("Human.svg", 160),
            [EntityType.Warrior] = LoadSvg("warrior.svg", 160),
            [EntityType.Crossbowman] = LoadSvg("crossbowman.svg", 160),
            [EntityType.Cannon] = LoadSvg("cannon_unit.svg", 160),
            [EntityType.Farm] = LoadSvg("farm.svg", 160),
            [EntityType.Mine] = LoadSvg("GoldMine.svg", 160),
            [EntityType.Castle] = LoadSvg("castle.svg", 160),
            [EntityType.Barracks] = LoadSvg("SwordBarracks.svg", 160),
            [EntityType.CrossbowWorkshop] = LoadSvg("CrossbowBarracks.svg", 160),
            [EntityType.CannonFactory] = LoadSvg("CannonFactory.svg", 160)
        };

        public static readonly Dictionary<SvgType, Image> SvgImagesSmall = new()
        {
            [SvgType.Wheat] = LoadSvg("wheat.svg", 45),
            [SvgType.Gold] = LoadSvg("gold.svg", 45)
        };

        public static readonly Dictionary<SvgType, Image> SvgImagesBig = new()
        {
            [SvgType.Wheat] = LoadSvg("wheat.svg", 100),
            [SvgType.Gold] = LoadSvg("gold.svg", 100),
            [SvgType.Clock] = LoadSvg("clock.svg", 100)
        };
    }
}
