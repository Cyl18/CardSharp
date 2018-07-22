using System;
using System.IO;

namespace CardSharp.GameComponents
{
    public class PlayerConfig : IPlayerConfig
    {
        static PlayerConfig()
        {
            if (!Directory.Exists(Constants.ConfigDir)) Directory.CreateDirectory(Constants.ConfigDir);
        }

        public PlayerConfig(string playerid, long point = default, DateTime lastTime = default, bool isAdmin = default)
        {
            PlayerID = playerid ?? throw new ArgumentNullException(nameof(playerid));
            Point = point;
            LastTime = lastTime;
            IsAdmin = isAdmin;
        }

        public string PlayerID { get; }
        public long Point { get; set; }
        public DateTime LastTime { get; set; }

        private bool _isAdmin = false;

        public bool IsAdmin
        {
            get => PlayerID == "775942303" || _isAdmin;
            private set => _isAdmin = value;
        }

        public static PlayerConfig GetConfig(Player player)
        {
            var path = GetConfigPath(player.PlayerId);
            return File.Exists(path)
                ? FromJson(File.ReadAllText(path))
                : new PlayerConfig(player.PlayerId);
        }

        public void Save()
        {
            if (uint.TryParse(PlayerID, out var _)) // e.g. 机器人233
            {
                File.WriteAllText(GetConfigPath(PlayerID), this.ToJsonString());
            }
        }

        public string ToAtCode()
        {
            return $"[CQ:at,qq={PlayerID}]";
        }

        public void AddPoint()
        {
            Point += Constants.PointAdd;
            LastTime = DateTime.Now;
            Save();
        }

        public void AddPoint(long point)
        {
            Point += point;
            LastTime = DateTime.Now;
            Save();
        }

        public static PlayerConfig FromJson(string json) => json.JsonDeserialize<PlayerConfig>();

        private static string GetConfigPath(string playerid)
        {
            return Path.Combine(Constants.ConfigDir, $"{playerid}.json");
        }
    }
}