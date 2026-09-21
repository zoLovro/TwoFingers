using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TwoFingers;

public class Gameplay : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _pixel;
    
    private Rectangle _hitLine;
    private Vector2 _hitLinePos;
    private bool _hitLineMoveDown;

    private TapCircle _tapCircle;
    private SwipeCircle _swipeCircle;
    private HoldCircle _holdCircle;
    
    private KeyboardState _previousKeyboard;
    private MouseState _previousMouse;
    private int _counter;
    
    private bool _isDragging = false;
    private bool _finishedSwiping = false;
    private Vector2 _dragStartPos;
    private double _dragStartTime;
    private float _xDragThreshhold = 100f;

    private bool _isHolding = false;
    private bool _finishedHolding = false;
    private double _holdStartTime;
    private float _tempTimeToHold = 4000f;
    
    private Vector2 _mousePos;
    private Rectangle _mousePosRect;

    private Song _song;

    public Gameplay()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] {Color.White});
        _hitLinePos = new Vector2(0, 360);
        _hitLineMoveDown = false;

        _tapCircle = new TapCircle(new Vector2(200, 200), 1);
        _swipeCircle = new SwipeCircle(new Vector2(500, 500), 1);
        _holdCircle = new HoldCircle(new Vector2(700, 400), 1);
        _counter = 0;
        _song = Content.Load<Song>("Sawai Miku - Colorful Asterisk Remix");
        MediaPlayer.Play(_song);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState currentKeyboard = Keyboard.GetState();
        MouseState currentMouse = Mouse.GetState();
        _mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        _mousePosRect = new Rectangle((int)_mousePos.X, (int)_mousePos.Y, 1, 1);

        MoveHitLine();

        //////
        if (_mousePosRect.Intersects(_tapCircle.Object) && _hitLine.Intersects(_tapCircle.Object) &&
            currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
        {
            Console.WriteLine("Tapped");
        }
        
        //////
        if (_mousePosRect.Intersects(_swipeCircle.Object) && _hitLine.Intersects(_swipeCircle.Object) &&
            currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
        {
            _isDragging = true;
            _dragStartPos = _mousePos;
            _dragStartTime = gameTime.TotalGameTime.TotalMilliseconds;
        }
        if (_isDragging && currentMouse.LeftButton == ButtonState.Pressed &&
            DragDistanceX(_dragStartPos, _mousePos) > _xDragThreshhold)
        {
            _finishedSwiping = true;
        }
        if (_finishedSwiping)
        {
            _isDragging = false;
            _finishedSwiping = false;
            Console.WriteLine("Swiped");
        }
        
        //////
        if (_mousePosRect.Intersects(_holdCircle.Object) && _hitLine.Intersects(_holdCircle.Object) &&
            currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
        {
            _isHolding = true;
            _finishedHolding = false;
            _holdStartTime = gameTime.TotalGameTime.TotalMilliseconds;
        }
        if (_isHolding && currentMouse.LeftButton == ButtonState.Pressed && 
            Math.Abs(_holdStartTime - gameTime.TotalGameTime.TotalMilliseconds) >= _tempTimeToHold)
        {
            _finishedHolding = true;
        }
        if (_finishedHolding)
        {
            _isHolding = false;
            _finishedHolding = false;
            Console.WriteLine("Held");
        }
        
        
        if (currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
        {
            _isDragging = false;
            _finishedSwiping = false;
            _isHolding = false;
            _finishedHolding = false;
        }
        if(currentKeyboard.IsKeyDown(Keys.Escape) && _previousKeyboard.IsKeyUp(Keys.Escape))
        {
            if(MediaPlayer.State != MediaState.Paused) MediaPlayer.Pause();
            else MediaPlayer.Resume();
        }

        _previousKeyboard = currentKeyboard;
        _previousMouse = currentMouse;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        //////////////////////
        _tapCircle.Draw(_spriteBatch, _pixel);
        _swipeCircle.Draw(_spriteBatch, _pixel);
        _holdCircle.Draw(_spriteBatch, _pixel);
        _spriteBatch.Draw(_pixel, _hitLine, Color.Red);
        //////////////////////
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void MoveHitLine()
    {
        if (_hitLinePos.Y <= 0) _hitLineMoveDown = true;
        if (_hitLinePos.Y >= 720) _hitLineMoveDown = false;
        switch (_hitLineMoveDown)
        {
            case true:
                _hitLinePos.Y += 2;
                break;
            case false:
                _hitLinePos.Y -= 2;
                break;
        }
        _hitLine = new Rectangle((int)_hitLinePos.X, (int)_hitLinePos.Y, 1280, 5);
    }

    private float DragDistanceX(Vector2 startMousePos, Vector2 currMousePos)
    {
        return Math.Abs(startMousePos.X - currMousePos.X);
    }
}