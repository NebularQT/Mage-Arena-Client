namespace MageArenaClient
{
    public enum FireMode
    {
        Normal,
        Burst,
        Shotgun
    }
    public enum WardMode
    {
        Normal,
        Nova,
        Burst,
        Tornado,
        Rain
    }
    public enum FrostMode
    {
        Normal,
        Railgun,
        Wall
    }
    public enum HealingAmount
    {
        Normal,
        NormalPlus,
        Stew,
        HackerProof
    }

    public static class ToggleStates
    {
        public static bool SpeedEnabled = false;
        public static bool InfiniteHealth = false;
        public static bool DisableWind = false;
        public static bool InfiniteJump = false;
        public static bool NoFallEnable = false;
        public static bool NoClipEnabled = false;
        public static bool InfiniteStamina = false;
        public static bool FastPortcullisOpen = false;
        public static bool SkipCooldown = false;
        public static bool IceBoxTimers = false;
        public static bool ESPEnabled = false;
        public static bool MenuEnabled = false;
        public static bool InputEnabled = false;

        public static float PlayerScale = 1f;
        public const float ScaleStep = 0.25f;
        public const float MinScale = 0.25f;
        public const float MaxScale = 10f;

        public static FireMode CurrentFireMode = FireMode.Normal;
        public static WardMode CurrentWardMode = WardMode.Normal;
        public static FrostMode CurrentFrostMode = FrostMode.Normal;
        public static HealingAmount CurrentHealingAmount = HealingAmount.Normal;
    }
}
