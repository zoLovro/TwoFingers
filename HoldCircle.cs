using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoFingers;

public class HoldCircle
{
    private Vector2 Position { get; }
    public int QueuePos { get; }
    public Rectangle Object { get; }
    
    public HoldCircle(Vector2 position, int queuePos)
    {
        Position = position;
        QueuePos = queuePos;
        Object = new Rectangle((int)position.X, (int)position.Y, 100, 50);
    }
    
    
    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        spriteBatch.Draw(pixel, Object, Color.Black);
    }
}