using HarmonyLib;
using Mod.Classes;

namespace Mod.Patches
{
    [HarmonyPatch(typeof(EnemyIdentifier), nameof(EnemyIdentifier.DeliverDamage))]
    class EnemyDamagePatch
    {
        static void Prefix(ref float multiplier)
        {
            if (EnragedController.isEnraged)
            {
                multiplier *= Plugin.damageMultiplier.Value;
            }
        }
    }
}
