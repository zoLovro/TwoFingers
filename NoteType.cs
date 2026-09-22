

using Microsoft.Xna.Framework;

namespace TwoFingers;

public enum NoteType { Tap, Swipe, Hold }

public struct NoteData
{
    public NoteType Type;
    public Vector2 Position;
    public double TimeToAppear;
    public int SwipeDir;
    public float HoldDuration;
}