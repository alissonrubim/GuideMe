using LazySnake.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static LazySnake.Engine.GameObject;

namespace LazySnake.Engine
{
    class GameEngine
    {
        private Canvas[] layers = new Canvas[5];
        private Canvas topCanvas = null;
        private Canvas middleCanvas = null;
        private Canvas bottomCanvas = null;
        private Canvas parentCanvas = null;
        private Canvas energyCanvas = null;
        private GameSprite[] energySprites;
        private GameMap currentMap = null;
        private GamePlayer[] players = new GamePlayer[2];
        public int BlockSize = 25;
        public int MaximumEnergy = 30;
        private int WalkTime = 500;

        public GameEngine(Canvas parentCanvas)
        {
            this.parentCanvas = parentCanvas;
            setupScreen();
            setPlayers();
        }

        public void LoadMap(string map)
        {
            currentMap = new GameMap(this);
            currentMap.OnRenderGameObject += map_onRenderGameObject;
            currentMap.OnProcessXMLMap += map_onProcessXMLMap;
            currentMap.LoadMap(map);
        }

        public GameMap GetMap()
        {
            return currentMap;
        }

        public GamePlayer GetPlayer(int index)
        {
            return players[index];
        }

        public Canvas GetLayerByIndex(int layerIndex)
        {
            return layers[layerIndex];
        }

        public void MoveTo(GameObject gameObject, Coordinate cordinates, GameAnimation animation)
        {
            GameObject oldGameObject = currentMap.GetGameObjectAt(cordinates.Row, cordinates.Col);
            currentMap.MoveTo(gameObject, cordinates, animation);


            /*if (oldGameObject != null && oldGameObject.Type == GameObject.GameObjectType.Batery)
            {
                MessageBox.Show("Bateria");
            }
            else if (oldGameObject != null && oldGameObject.Type == GameObject.GameObjectType.Target)
            {
                MessageBox.Show("Ganhou");
            }
            else
            {
                decreseEnergy();
            }*/
        }

        private void decreseEnergy()
        {
            int playerEnergy = players[0].GetEnergy();
            players[0].SetEnergy(playerEnergy - 1);
            refreshEnergyBar();
        }

        private void setupScreen()
        {
            int topCanvasHeight = 60;
            int bottomCanvasHeight = 30;

            topCanvas = new Canvas();
            topCanvas.Visibility = Visibility.Visible;
            topCanvas.Background = System.Windows.Media.Brushes.DarkGreen;
            topCanvas.Width = parentCanvas.ActualWidth;
            topCanvas.Height = topCanvasHeight;
            parentCanvas.Children.Add(topCanvas);
            Canvas.SetTop(topCanvas, 0);

            middleCanvas = new Canvas();
            middleCanvas.Visibility = Visibility.Visible;
            middleCanvas.Background = System.Windows.Media.Brushes.Transparent;
            middleCanvas.Width = parentCanvas.ActualWidth;
            middleCanvas.Height = parentCanvas.ActualHeight - (topCanvasHeight + bottomCanvasHeight);
            parentCanvas.Children.Add(middleCanvas);
            Canvas.SetTop(middleCanvas, topCanvasHeight);

            bottomCanvas = new Canvas();
            bottomCanvas.Visibility = Visibility.Visible;
            bottomCanvas.Background = System.Windows.Media.Brushes.LightGray;
            bottomCanvas.Width = parentCanvas.ActualWidth;
            bottomCanvas.Height = bottomCanvasHeight;
            parentCanvas.Children.Add(bottomCanvas);
            Canvas.SetTop(bottomCanvas, topCanvasHeight + middleCanvas.Height);

            setupHUD();
            setupLayers();
        }

        private void setupHUD()
        {
            //Set the topbar background
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = GameObject.ImageSourceForBitmap(ResourceTextures.Grass);
            brush.TileMode = TileMode.Tile;
            brush.Viewport = new Rect(0, 0, this.BlockSize, this.BlockSize);
            brush.ViewportUnits = BrushMappingMode.Absolute;
            topCanvas.Background = brush;

            //Energy
            Canvas energyCanvas = new Canvas();
            energyCanvas.Visibility = Visibility.Visible;
            energyCanvas.Background = System.Windows.Media.Brushes.Transparent;
            energyCanvas.Width = parentCanvas.ActualWidth;
            energyCanvas.Height = 60;
            Canvas.SetTop(energyCanvas, 10);
            topCanvas.Children.Add(energyCanvas);

            energySprites = new GameSprite[MaximumEnergy];
            energySprites[0] = new GameSprite();
            energySprites[0].SetPosition(new System.Windows.Point(10, 5));
            energySprites[0].SetSize(new System.Windows.Size(36, 20));
            energySprites[0].Render(energyCanvas);

            for (int i = 0; i < MaximumEnergy - 2; i++)
            {
                energySprites[i+1] = new GameSprite();
                energySprites[i + 1].SetPosition(new System.Windows.Point(energySprites[0].GetPosition().X + energySprites[0].GetSize().Width + (i * 18), 5));
                energySprites[i + 1].SetSize(new System.Windows.Size(18, 20));
                energySprites[i + 1].Render(energyCanvas);
            }

            energySprites[MaximumEnergy - 1] = new GameSprite();
            energySprites[MaximumEnergy - 1].SetPosition(new System.Windows.Point(energySprites[0].GetPosition().X + energySprites[0].GetSize().Width + ((MaximumEnergy - 4) * 18) + 36, 5));
            energySprites[MaximumEnergy - 1].SetSize(new System.Windows.Size(25, 20));
            energySprites[MaximumEnergy - 1].Render(energyCanvas);
        }

        public void refreshEnergyBar()
        {
            for(int i =0; i<MaximumEnergy; i++)
            {
                if(i < players[0].GetEnergy())
                {
                    if(i == 0)
                        energySprites[i].SetTexture(ResourceTextures.life_11);
                    else if (i == MaximumEnergy - 1)
                        energySprites[i].SetTexture(ResourceTextures.life_31);
                    else
                        energySprites[i].SetTexture(ResourceTextures.life_21);
                }
                else
                {
                    if (i == 0)
                        energySprites[i].SetTexture(ResourceTextures.life_10);
                    else if (i == MaximumEnergy - 1)
                        energySprites[i].SetTexture(ResourceTextures.life_30);
                    else
                        energySprites[i].SetTexture(ResourceTextures.life_20);
                }
            }
        }

        private void setupLayers()
        {
            Canvas parent = this.middleCanvas;
            for (int i = 0; i < layers.Length; i++)
            {
                Canvas layer = new Canvas();
                layer.Visibility = Visibility.Visible;
                layer.Background = System.Windows.Media.Brushes.Transparent;
                layer.Width = parent.Width;
                layer.Height = parent.Height;
                layer.Name = "Layer" + (i).ToString();
                layers[i] = layer;
                parent.Children.Add(layer);
                parent = layer;
            }

            Canvas backgroundLayer = this.GetLayerByIndex(0);
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = GameObject.ImageSourceForBitmap(ResourceTextures.Grass);
            brush.TileMode = TileMode.Tile;
            brush.Viewport = new Rect(0, 0, this.BlockSize, this.BlockSize);
            brush.ViewportUnits = BrushMappingMode.Absolute;
            backgroundLayer.Background = brush;
        }

        private void setPlayers()
        {
            GameSpriteSheet playerSpriteSheet = new GameSpriteSheet(ResourceTextures.player_sheet, new System.Drawing.Point(32, 32));
            players[0] = new GamePlayer(this);
            players[0].SetEnergy(MaximumEnergy);
            players[0].AddTexture(GamePlayer.GamePlayerDirection.Up, playerSpriteSheet.GetSprite(3, 1));
            players[0].AddTexture(GamePlayer.GamePlayerDirection.UpLeft, playerSpriteSheet.GetSprite(2, 7));
            players[0].AddTexture(GamePlayer.GamePlayerDirection.UpRight, playerSpriteSheet.GetSprite(3, 10));
            players[0].AddTexture(GamePlayer.GamePlayerDirection.Left, playerSpriteSheet.GetSprite(1, 1));
            players[0].AddTexture(GamePlayer.GamePlayerDirection.Right, playerSpriteSheet.GetSprite(2, 4));
            players[0].AddTexture(GamePlayer.GamePlayerDirection.Bottom, playerSpriteSheet.GetSprite(0, 1));
            players[0].AddTexture(GamePlayer.GamePlayerDirection.BottomLeft, playerSpriteSheet.GetSprite(1, 7));
            players[0].AddTexture(GamePlayer.GamePlayerDirection.BottomRight, playerSpriteSheet.GetSprite(0, 7));

            players[0].AddWalkAnimation(GamePlayer.GamePlayerDirection.Up, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(3, 0),
                playerSpriteSheet.GetSprite(3, 1),
                playerSpriteSheet.GetSprite(3, 2),
                playerSpriteSheet.GetSprite(3, 3),
                playerSpriteSheet.GetSprite(3, 4)
            }, new System.Windows.Point(0, BlockSize * -1)));

            players[0].AddWalkAnimation(GamePlayer.GamePlayerDirection.UpLeft, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(2, 6),
                playerSpriteSheet.GetSprite(2, 7),
                playerSpriteSheet.GetSprite(2, 8),
                playerSpriteSheet.GetSprite(2, 9),
                playerSpriteSheet.GetSprite(2, 10)
            }, new System.Windows.Point(BlockSize * -1, BlockSize * -1)));

            players[0].AddWalkAnimation(GamePlayer.GamePlayerDirection.UpRight, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(3, 6),
                playerSpriteSheet.GetSprite(3, 7),
                playerSpriteSheet.GetSprite(3, 8),
                playerSpriteSheet.GetSprite(3, 9),
                playerSpriteSheet.GetSprite(3, 10)
            }, new System.Windows.Point(BlockSize, BlockSize * -1)));


            players[0].AddWalkAnimation(GamePlayer.GamePlayerDirection.Left, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(1, 0),
                playerSpriteSheet.GetSprite(1, 1),
                playerSpriteSheet.GetSprite(1, 2),
                playerSpriteSheet.GetSprite(1, 3),
                playerSpriteSheet.GetSprite(1, 4)
            }, new System.Windows.Point(BlockSize * -1, 0)));

            players[0].AddWalkAnimation(GamePlayer.GamePlayerDirection.Right, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(2, 0),
                playerSpriteSheet.GetSprite(2, 1),
                playerSpriteSheet.GetSprite(2, 2),
                playerSpriteSheet.GetSprite(2, 3),
                playerSpriteSheet.GetSprite(2, 4)
            }, new System.Windows.Point(BlockSize, 0)));

            players[0].AddWalkAnimation(GamePlayer.GamePlayerDirection.Bottom, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(0, 0),
                playerSpriteSheet.GetSprite(0, 1),
                playerSpriteSheet.GetSprite(0, 2),
                playerSpriteSheet.GetSprite(0, 3),
                playerSpriteSheet.GetSprite(0, 4)
            }, new System.Windows.Point(0, BlockSize)));

            players[0].AddWalkAnimation(GamePlayer.GamePlayerDirection.BottomLeft, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(1, 6),
                playerSpriteSheet.GetSprite(1, 7),
                playerSpriteSheet.GetSprite(1, 8),
                playerSpriteSheet.GetSprite(1, 9),
                playerSpriteSheet.GetSprite(1, 10)
            }, new System.Windows.Point(BlockSize * -1, BlockSize)));

            players[0].AddWalkAnimation(GamePlayer.GamePlayerDirection.BottomRight, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(0, 6),
                playerSpriteSheet.GetSprite(0, 7),
                playerSpriteSheet.GetSprite(0, 8),
                playerSpriteSheet.GetSprite(0, 9),
                playerSpriteSheet.GetSprite(0, 10)
            }, new System.Windows.Point(BlockSize, BlockSize)));

            refreshEnergyBar();
        }

        private GameAnimation createWalkAnimation(Bitmap[] sprites, System.Windows.Point totalPositionDiff)
        {
            GameAnimation.AnimateStep[] steps = new GameAnimation.AnimateStep[sprites.Length];
            
            for(int i=0; i< sprites.Length; i++)
            {
                steps[i] = new GameAnimation.AnimateStep()
                {
                    PositionDiff = new System.Windows.Point(totalPositionDiff.X / sprites.Length, totalPositionDiff.Y / sprites.Length),
                    Texture = sprites[i],
                    Time = WalkTime / sprites.Length
                };
            }

            return new GameAnimation(null, steps, runForever: false);
        }

        private void map_onRenderGameObject(GameObject gameObject)
        {
            if(gameObject.Type == GameObject.GameObjectType.Player)
            {
                GetPlayer(0).SetGameObject(gameObject);
                GetPlayer(0).TurnDown();
                gameObject.SetPosition(new System.Windows.Point(gameObject.GetPosition().X, gameObject.GetPosition().Y - 5));
                gameObject.Render(this.GetLayerByIndex(1));
            }
            else if (gameObject.Type == GameObject.GameObjectType.Target)
            {
                GameSpriteSheet foodSheet = new GameSpriteSheet(ResourceTextures.food_sheet, new System.Drawing.Point(32,32));
                gameObject.SetTexture(foodSheet.GetSprite(4,7));
                gameObject.Render(this.GetLayerByIndex(1));
            }
            else if (gameObject.Type == GameObject.GameObjectType.Batery)
            {
                gameObject.SetTexture(ResourceTextures.batery);
                gameObject.SetSize(new System.Windows.Size(15, 15));
                gameObject.SetPosition(new System.Windows.Point(gameObject.GetPosition().X + 5, gameObject.GetPosition().Y));
                gameObject.Render(this.GetLayerByIndex(1));
            }
            else if (gameObject.Type == GameObject.GameObjectType.Wall)
            {
                gameObject.SetTexture(ResourceTextures.wall);

                if (!IsWallGameObject(gameObject.Neighbors.Bottom))
                    gameObject.SetTexture(ResourceTextures.wall_bottom);

                if (!IsWallGameObject(gameObject.Neighbors.Top) && !IsWallGameObject(gameObject.Neighbors.Right))
                    gameObject.SetTexture(ResourceTextures.wall_top);

                if (IsWallGameObject(gameObject.Neighbors.Bottom) && IsWallGameObject(gameObject.Neighbors.Right) && !IsWallGameObject(gameObject.Neighbors.BottomRight))
                    gameObject.SetTexture(ResourceTextures.wall_3);

                if (!IsWallGameObject(gameObject.Neighbors.Bottom) && !IsWallGameObject(gameObject.Neighbors.Right) && !IsWallGameObject(gameObject.Neighbors.Left))
                    gameObject.SetTexture(ResourceTextures.wall_bottom_right);

                if (!IsWallGameObject(gameObject.Neighbors.Bottom) && !IsWallGameObject(gameObject.Neighbors.Right) && IsWallGameObject(gameObject.Neighbors.Top))
                    gameObject.SetTexture(ResourceTextures.wall_7);

                if (!IsWallGameObject(gameObject.Neighbors.Bottom) && !IsWallGameObject(gameObject.Neighbors.Right)
                        && IsWallGameObject(gameObject.Neighbors.Top)
                        && IsWallGameObject(gameObject.Neighbors.Left))
                    gameObject.SetTexture(ResourceTextures.wall_6);

                if (IsWallGameObject(gameObject.Neighbors.Bottom)
                        && IsWallGameObject(gameObject.Neighbors.Top)
                        && !IsWallGameObject(gameObject.Neighbors.Right))
                    gameObject.SetTexture(ResourceTextures.wall_side_right);

                if (!IsWallGameObject(gameObject.Neighbors.Bottom) && !IsWallGameObject(gameObject.Neighbors.Right) && !IsWallGameObject(gameObject.Neighbors.Top)
                        && IsWallGameObject(gameObject.Neighbors.Left))
                    gameObject.SetTexture(ResourceTextures.wall_5);

                if (!IsWallGameObject(gameObject.Neighbors.Bottom) 
                        && IsWallGameObject(gameObject.Neighbors.Right)
                        && !IsWallGameObject(gameObject.Neighbors.Left))
                    gameObject.SetTexture(ResourceTextures.wall_4);

                gameObject.Render(this.GetLayerByIndex(2));
            }
        }

        private GameObject map_onProcessXMLMap(string value, int row, int col)
        {
            GameObject gameObject = null;
            if (value == "1")
            {
                gameObject = new GameObject(new Coordinate(row, col))
                {
                    Type = GameObject.GameObjectType.Wall,
                    CanColideWithMe = true
                };
                gameObject.AddColisionDetector(GameObjectType.Player, playerColideWithWall);
            }
            else if (value == "2")
                gameObject = new GameObject(new Coordinate(row, col))
                {
                    Type = GameObject.GameObjectType.Player,
                    CanColideWithMe = true
                };
            else if (value == "3")
            {
                gameObject = new GameObject(new Coordinate(row, col))
                {
                    Type = GameObject.GameObjectType.Target,
                    CanColideWithMe = true
                };

                gameObject.AddColisionDetector(GameObjectType.Player, playerColideWithTarget);
            }
            else if (value == "4")
            {
                gameObject = new GameObject(new Coordinate(row, col))
                {
                    Type = GameObject.GameObjectType.Batery,
                    CanColideWithMe = true
                };
                gameObject.AddColisionDetector(GameObjectType.Player, playerColideWithBattery);
            }
            return gameObject;
        }

        private bool playerColideWithTarget(GameObject targetObject, GameObject playerObject)
        {
            return true;
        }

        private bool playerColideWithBattery(GameObject bateryObject, GameObject playerObject)
        {
            bateryObject.SetTexture(null);
            return true;
        }

        private bool playerColideWithWall(GameObject wallObject, GameObject playerObject)
        {
            return false;
        }

        public bool IsWallGameObject(GameObject gameObject)
        {
            return gameObject != null && gameObject.CanColideWithMe == true && gameObject.Type == GameObjectType.Wall;
        }
    }
}
