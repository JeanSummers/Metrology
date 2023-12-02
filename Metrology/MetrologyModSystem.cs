using Vintagestory.API.Common;

namespace Metrology
{
    public class MetrologyModSystem : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            api.RegisterCollectibleBehaviorClass("CompassBehavior", typeof(CompassBehavior));
        }
    }
}
