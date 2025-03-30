using UnityEngine;
using Logic = Packages.com.terallite.gamelogic.Runtime;

public static class GameLogicVectorExtensions
{
    public static Vector3 ToUnityVector3(this Logic.Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }
}