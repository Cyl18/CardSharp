using System;
using System.Numerics;
using Newtonsoft.Json;

namespace CardSharp.GameComponents
{
    public interface IPlayerConfig
    {
        DateTime LastTime { get; set; }
        long Point { get; set; }
        string PlayerID { get; }
        bool IsAdmin { get; }
    }
    
}