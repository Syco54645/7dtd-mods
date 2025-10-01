namespace z15SlotToolbelt.Harmony
{
    public class Main : IModApi
    {
        
        public Main()
        {
            
        }

        public void InitMod(Mod modInstance)
        {
            var harmony = new HarmonyLib.Harmony("org.splra.7daystodie.mod.15_slot_toolbelt");
            harmony.PatchAll();
        }
    }
}