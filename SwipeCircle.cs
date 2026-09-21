using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoFingers;

public class SwipeCircle
{
    private Vector2 Position { get; }
    public int QueuePos { get; }
    public Rectangle Object { get; }
    
    public SwipeCircle(Vector2 position, int queuePos)
    {
        Position = position;
        QueuePos = queuePos;
        Object = new Rectangle((int)position.X, (int)position.Y, 100, 200);
    }
    
    
    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        spriteBatch.Draw(pixel, Object, Color.Purple);
    }
}