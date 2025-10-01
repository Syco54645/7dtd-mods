using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace z15SlotToolbelt.Harmony.Patches
{
    [HarmonyPatch]
    public class KhaineGB_Patch
    {
        [HarmonyPatch(typeof(Inventory), "PUBLIC_SLOTS_PLAYMODE", MethodType.Getter)]
        [HarmonyPatch(typeof(Inventory), "SHIFT_KEY_SLOT_OFFSET", MethodType.Getter)]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log((object)"Patching get_PUBLIC_SLOTS_PLAYMODE or get_SHIFT_KEY_SLOT_OFFSET.");
            List<CodeInstruction> source = new List<CodeInstruction>(instructions);
            for (int index = 0; index < source.Count; ++index)
            {
                if (source[index].opcode == OpCodes.Ldc_I4_S && source[index].operand.ToString() == "10")
                {
                    source[index].operand = (object)15;
                    Debug.Log((object)"  Done.");
                    return source.AsEnumerable<CodeInstruction>();
                }
            }
            Debug.LogWarning((object)"  Failed.");
            return source.AsEnumerable<CodeInstruction>();
        }
    }

}
