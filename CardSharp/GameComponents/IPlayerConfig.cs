using System;

namespace CardSharp.GameComponents
{
    public interface IPlayerConfig
    {
        DateTime LastTime { get; set; }
        int Point { get; set; }
    }
}