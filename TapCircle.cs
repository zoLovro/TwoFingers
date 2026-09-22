using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoFingers;

public class TapCircle
{
    private Vector2 Position { get; }
    public int QueuePos { get; }
    public Rectangle Object { get; }
    
    public TapCircle(Vector2 position)
    {
        Position = position;
        Object = new Rectangle((int)position.X, (int)position.Y, 100, 100);
    }
    
    
    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        spriteBatch.Draw(pixel, Object, Color.Green);
    }
}