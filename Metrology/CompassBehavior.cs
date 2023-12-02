using AnimationManagerLib.API;
using AnimationManagerLib.CollectibleBehaviors;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Metrology
{
    internal class CompassBehavior : CollectibleBehavior
    {
        private IAnimatableBehavior? behavior;
        private int animationId;
        double latestAngle = double.NaN;

        public CompassBehavior(CollectibleObject collObj) : base(collObj) {}

        public override void OnLoaded(ICoreAPI api)
        {
            behavior = collObj.GetCollectibleBehavior<AnimatableProcedural>(true);
            if (behavior == null)
            {
                api.Logger.Error("Unable to load AnimatableProcedural behavior for compass item");
                return;
            }

            animationId = behavior.RegisterAnimation(
                code: "rotate",
                category: "metrology_compass"
            );
        }

        public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
        {
            if (behavior == null) return;

            double angle = GetAngle(capi);
            if (angle != latestAngle)
            {
                behavior.RunAnimation(animationId, RunParameters.EaseIn(0.1f, (float)angle));
                latestAngle = angle;
            }

            base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
        }

        private static double GetAngle(ICoreClientAPI capi)
        {
            var yaw = capi.World.Player.Entity.Pos.Yaw / (2 * Math.PI);
            var north = ShiftLeft(yaw, 0.25, 1);
            return (1 - yaw) * 360;
        }

        private static double ShiftLeft(double value, double offset, double max)
        {
            var s = value - offset;
            return s < 0 ? max - s : s;
        }
    }
}
