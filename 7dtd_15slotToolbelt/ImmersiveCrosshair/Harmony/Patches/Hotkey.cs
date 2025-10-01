using HarmonyLib;
using Platform;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace z15SlotToolbelt.Harmony.Patches
{
    [HarmonyPatch]
    public class Hotkey
    {
        public static string[] s_labels = new string[15];
        private static KeyCode[] s_keyCodes = new KeyCode[15];
        public static bool s_isEnabled = true;

        [HarmonyPatch(typeof(XUi), "PostLoadInit")]
        [HarmonyPostfix]
        public static void XUi_PostLoadInit_HarmonyPostfix()
        {
            PlayerActionsLocal primaryPlayer = PlatformManager.NativePlatform?.Input?.PrimaryPlayer;
            if (primaryPlayer != null)
            {
                Hotkey.s_labels[0] = primaryPlayer.InventorySlot1?.Bindings[0]?.Name ?? "NF";
                Hotkey.s_labels[1] = primaryPlayer.InventorySlot2?.Bindings[0]?.Name ?? "NF";
                Hotkey.s_labels[2] = primaryPlayer.InventorySlot3?.Bindings[0]?.Name ?? "NF";
                Hotkey.s_labels[3] = primaryPlayer.InventorySlot4?.Bindings[0]?.Name ?? "NF";
                Hotkey.s_labels[4] = primaryPlayer.InventorySlot5?.Bindings[0]?.Name ?? "NF";
                Hotkey.s_labels[5] = primaryPlayer.InventorySlot6?.Bindings[0]?.Name ?? "NF";
                Hotkey.s_labels[6] = primaryPlayer.InventorySlot7?.Bindings[0]?.Name ?? "NF";
                Hotkey.s_labels[7] = primaryPlayer.InventorySlot8?.Bindings[0]?.Name ?? "NF";
                Hotkey.s_labels[8] = primaryPlayer.InventorySlot9?.Bindings[0]?.Name ?? "NF";
                Hotkey.s_labels[9] = primaryPlayer.InventorySlot10?.Bindings[0]?.Name ?? "NF";
            }
            string str1 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.txt");
            Debug.Log((object)("Loading " + str1));
            if (!File.Exists(str1))
            {
                Debug.Log((object)" config.txt not found. Copy config.txt.default.");
                File.Copy(str1 + ".default", str1);
            }
            using (FileStream fileStream = new FileStream(str1, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader((Stream)fileStream))
                {
                    for (int index = 0; index < 5; ++index)
                    {
                        string str2 = streamReader.ReadLine();
                        if (!Enum.TryParse<KeyCode>(str2, out Hotkey.s_keyCodes[10 + index]))
                            Debug.LogError((object)$" Failed to KeyCode.TryParse(\"{str2}\").");
                        Hotkey.s_labels[10 + index] = Hotkey.s_keyCodes[10 + index].ToString();
                    }
                }
            }
            for (int index = 0; index < Hotkey.s_labels.Length; ++index)
                Debug.Log((object)$"  labels[{index.ToString()}]={Hotkey.s_labels[index]}");
        }

        [HarmonyPatch(typeof(PlayerActionsLocal), "InventorySlotWasPressed", MethodType.Getter)]
        [HarmonyPostfix]
        public static void PlayerActionsLocal_InventorySlotWasPressed_Getter_HarmonyPostfix(
          ref int __result)
        {
            if (!Hotkey.s_isEnabled || __result != -1)
                return;
            for (int index = 10; index < Hotkey.s_keyCodes.Length; ++index)
            {
                if (UnityEngine.Input.GetKey(Hotkey.s_keyCodes[index]))
                {
                    __result = index;
                    break;
                }
            }
        }
    }

}
