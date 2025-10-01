using HarmonyLib;

namespace z15SlotToolbelt.Harmony.Patches
{
    [HarmonyPatch]
    public class HotkeyLabelNameProvider
    {
        [HarmonyPatch(typeof(XUiC_ToolbeltWindow), "GetBindingValueInternal")]
        [HarmonyPostfix]
        public static void XUiC_ToolbeltWindow_GetBindingValueInternal_HarmonyPostfix(ref string value, string bindingName/*, ref bool __result*/)
        {
            int result;
            if (bindingName == null || !bindingName.StartsWith("slot") || !int.TryParse(bindingName.Substring("slot".Length), out result))
                return;
            int index = result - 1;
            if (index < 0 || index >= Hotkey.s_labels.Length)
                return;
            value = Hotkey.s_labels[index] ?? "NULL";
            /*__result = true;*/
        }
    }
}

