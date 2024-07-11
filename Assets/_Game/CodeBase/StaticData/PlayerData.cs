using System;

namespace CodeBase.StaticData
{
    [Serializable]
    public class PlayerData
    {
        public int StartMoney;
        public int HitPoints;
        public int Damage;
        public int BulletsInMagazine;
        public float ReloadTime;
        public float ShotTime;
        public float ShotCooldown;
        public int ShotsCountInBurst;
    }
}