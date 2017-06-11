using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNACrazyGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGameClass : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont _textFont;
        Vector2 _textPosition;

        Texture2D _simplePlaneTexture;
        Texture2D _advancedPlaneTexture;
        Texture2D _powerfulPlaneTexture;
        Texture2D _bossPlaneTexture;

        Texture2D _cannonTexture;
        Texture2D _backgroundTexture;

        List<PlaneBase> _planes;
        List<PlaneBase> _planesBuffer;
        List<Rocket> _rockets;
        List<Rocket> _rocketsBuffer;

        Cannon _cannon;

        Rectangle _gameFieldRectangle;

        int enemySpawnTimeInSeconds = 1;
        int elapsedMiliseconds = 0;

        int _score = 0;

        static Random r = new Random();

        public MainGameClass()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);// * 0.8);
            graphics.PreferredBackBufferWidth = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);// * 0.8);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _planes = new List<PlaneBase>();
            _rockets = new List<Rocket>();

            _planesBuffer = new List<PlaneBase>();
            _rocketsBuffer = new List<Rocket>();
            
            graphics.ToggleFullScreen();
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _gameFieldRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            _textPosition = new Vector2(0, 0);

            _backgroundTexture = this.Content.Load<Texture2D>(@"Background");

            _textFont = this.Content.Load<SpriteFont>(@"PrintFont");

            _simplePlaneTexture = this.Content.Load<Texture2D>(@"Planes\plane_1");
            _advancedPlaneTexture = this.Content.Load<Texture2D>(@"Planes\plane_2");
            _powerfulPlaneTexture = this.Content.Load<Texture2D>(@"Planes\plane_3");
            _bossPlaneTexture = this.Content.Load<Texture2D>(@"Planes\plane_4");

            _cannonTexture = this.Content.Load<Texture2D>(@"Cannon");
            _rocketTexture = this.Content.Load<Texture2D>(@"Rocket");

            _rocketFactory = new RocketFactory(_rocketTexture, _gameFieldRectangle);
            _cannon = new Cannon(_cannonTexture, _gameFieldRectangle, _rocketFactory);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleKeyboard(gameTime);
            UpdatePlanes(gameTime);
            UpdateRockets();
            CheckCollisions();
            CheckObjectsOutsideGameField();
            _cannon.Update(gameTime);
            SpawnEnemy(gameTime);

            base.Update(gameTime);
        }

        private List<Vector2> CheckCollisions()
        {
            List<Rocket> destroyedRockets = new List<Rocket>();
            List<PlaneBase> destroyedPlanes = new List<PlaneBase>();

            List<Vector2> explodions = new List<Vector2>();
            for (int i = 0; i < _rockets.Count; i++)
            {
                for (int j = 0; j < _planes.Count; j++)
                {
                    if (_rockets[i].Body.Intersects(_planes[j].Body))
                    {
                        destroyedRockets.Add(_rockets[i].Destroy());
                        destroyedPlanes.Add(_planes[j].Destroy());

                        explodions.Add(_rockets[i].Position);
                    }
                }
            }

            _rockets = _rockets.Except(destroyedRockets).ToList();
            _planes = _planes.Except(destroyedPlanes).ToList();

            _score += explodions.Count;
            return explodions;
        }

        private void CheckObjectsOutsideGameField()
        {
            List<Rocket> rocketsOutsideGameField = new List<Rocket>();
            List<PlaneBase> planesOutsideGameField = new List<PlaneBase>();

            for (int i = 0; i < _rockets.Count; i++)
                if (!_rockets[i].IsAlive)
                    rocketsOutsideGameField.Add(_rockets[i]);

            for (int i = 0; i < _planes.Count; i++)
                if (!_planes[i].IsAlive)
                    planesOutsideGameField.Add(_planes[i]);

            _rockets = _rockets.Except(rocketsOutsideGameField).ToList();
            _planes = _planes.Except(planesOutsideGameField).ToList();
        }

        private void SpawnEnemy(GameTime gameTime)
        {
            elapsedMiliseconds += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedMiliseconds > enemySpawnTimeInSeconds * 1000)
            {
                GenerateEnemy();
                elapsedMiliseconds = 0;
            }
        }

        private void HandleKeyboard(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _cannon.Reloaded())
            {
                Rocket rocket = _cannon.Fire();
                _rockets.Add(rocket);
            }
        }

        private void UpdatePlanes(GameTime gameTime)
        {
            for (int i = 0; i < _planes.Count; i++)
            {
                _planes[i].Update(gameTime);
            }
        }

        public void UpdateRockets()
        {
            for (int i = 0; i < _rockets.Count; i++)
            {
                _rockets[i].Update();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero,
                new Vector2(2,2), SpriteEffects.None, 0);

            var text = string.Format("Planes: {0}\nRockets: {1}\nScore: {2}", _planes.Count, _rockets.Count, _score);
            spriteBatch.DrawString(_textFont, text, _textPosition, Color.Black);

            for (int i = 0; i < _planes.Count; i++)
            {
                _planes[i].Draw(gameTime, spriteBatch);
            }

            _cannon.Draw(spriteBatch);
            for (int i = 0; i < _rockets.Count; i++)
            {
                _rockets[i].Draw(spriteBatch);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        private void GenerateEnemy()
        {
            switch (r.Next(1, 5))
            {
                case 1:
                    _planes.Add(CreateSimplePlane());
                    break;
                case 2:
                    _planes.Add(CreateAdvancedPlane());
                    break;
                case 3:
                    _planes.Add(CreatePowerfulPlane());
                    break;
                case 4:
                    _planes.Add(CreateBossPlane());
                    break;
                default:
                    _planes.Add(CreateSimplePlane());
                    break;
            }
        }



        private const int SIMPLE_PLANE_SPEED = 1;
        private const int SIMPLE_PLANE_HEALTH = 100;
        private RocketFactory _rocketFactory;
        private Texture2D _rocketTexture;


        public  SimplePlane CreateSimplePlane()
        {
            SimplePlane plane = new SimplePlane(
                SIMPLE_PLANE_SPEED, SIMPLE_PLANE_HEALTH,
                _simplePlaneTexture,
                _gameFieldRectangle);

            return plane;
        }

        public  AdvancedPlane CreateAdvancedPlane()
        {
            AdvancedPlane plane = new AdvancedPlane(
                SIMPLE_PLANE_SPEED + 2, SIMPLE_PLANE_HEALTH * 2,
                _advancedPlaneTexture,
                _gameFieldRectangle);

            return plane;
        }

        public  PowerfulPlane CreatePowerfulPlane()
        {
            PowerfulPlane plane = new PowerfulPlane(
                SIMPLE_PLANE_SPEED + 3, SIMPLE_PLANE_HEALTH * 3,
                _powerfulPlaneTexture,
                _gameFieldRectangle);

            return plane;
        }

        public  BossPlane CreateBossPlane()
        {
            BossPlane plane = new BossPlane(
                SIMPLE_PLANE_SPEED + 4, SIMPLE_PLANE_HEALTH * 4,
                _bossPlaneTexture,
                _gameFieldRectangle);

            return plane;
        }

    }
}
