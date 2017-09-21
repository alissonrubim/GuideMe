using LazySnake.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private GameMap currentMap = null;
        private GamePlayer[] players = new GamePlayer[2];
        public int BlockSize = 25;

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

        public void MoveTo(GameObject gameObject, Coordinate cordinates)
        {
            currentMap.MoveTo(gameObject, cordinates);
        }

        public void MoveTo(GameObject gameObject, Coordinate cordinates, GameAnimation animation)
        {
            currentMap.MoveTo(gameObject, cordinates, animation);
        }

        private void setupScreen()
        {
            int topCanvasHeight = 50;
            int bottomCanvasHeight = 30;

            topCanvas = new Canvas();
            topCanvas.Visibility = Visibility.Visible;
            topCanvas.Background = System.Windows.Media.Brushes.LightGray;
            topCanvas.Width = parentCanvas.ActualWidth;
            topCanvas.Height = topCanvasHeight;
            parentCanvas.Children.Add(topCanvas);
            Canvas.SetTop(topCanvas, 0);

            System.Windows.Media.FontFamily f = new System.Windows.Media.FontFamily("Comic Sans MS");

            Label labelTime = new Label();
            labelTime.Visibility = Visibility.Visible;
            labelTime.Content = "00:00:00";
            labelTime.FontFamily = f;
            topCanvas.Children.Add(labelTime);

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

            setupLayers();
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
        }

        private void setPlayers()
        {
            GameSpriteSheet playerSpriteSheet = new GameSpriteSheet(ResourceTextures.player_sheet, new System.Drawing.Point(32, 32));
            players[0] = new GamePlayer(this);
            players[0].TextureDictionary.Add(GamePlayer.GamePlayerDirection.Up, playerSpriteSheet.GetSprite(3, 1));
            players[0].TextureDictionary.Add(GamePlayer.GamePlayerDirection.UpLeft, playerSpriteSheet.GetSprite(2, 7));
            players[0].TextureDictionary.Add(GamePlayer.GamePlayerDirection.UpRight, playerSpriteSheet.GetSprite(3, 10));
            players[0].TextureDictionary.Add(GamePlayer.GamePlayerDirection.Left, playerSpriteSheet.GetSprite(1, 1));
            players[0].TextureDictionary.Add(GamePlayer.GamePlayerDirection.Right, playerSpriteSheet.GetSprite(2, 4));
            players[0].TextureDictionary.Add(GamePlayer.GamePlayerDirection.Bottom, playerSpriteSheet.GetSprite(0, 1));
            players[0].TextureDictionary.Add(GamePlayer.GamePlayerDirection.BottomLeft, playerSpriteSheet.GetSprite(1, 7));
            players[0].TextureDictionary.Add(GamePlayer.GamePlayerDirection.BottomRight, playerSpriteSheet.GetSprite(0, 7));

            players[0].AnimationDictionary.Add(GamePlayer.GamePlayerDirection.Up, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(3, 0),
                playerSpriteSheet.GetSprite(3, 1),
                playerSpriteSheet.GetSprite(3, 2),
                playerSpriteSheet.GetSprite(3, 3),
                playerSpriteSheet.GetSprite(3, 4)
            }, new System.Windows.Point(0, BlockSize * -1)));

            players[0].AnimationDictionary.Add(GamePlayer.GamePlayerDirection.UpLeft, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(3, 6),
                playerSpriteSheet.GetSprite(3, 7),
                playerSpriteSheet.GetSprite(3, 8),
                playerSpriteSheet.GetSprite(3, 9),
                playerSpriteSheet.GetSprite(3, 10)
            }, new System.Windows.Point(BlockSize * -1, BlockSize * -1)));

            players[0].AnimationDictionary.Add(GamePlayer.GamePlayerDirection.UpRight, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(2, 6),
                playerSpriteSheet.GetSprite(2, 7),
                playerSpriteSheet.GetSprite(2, 8),
                playerSpriteSheet.GetSprite(2, 9),
                playerSpriteSheet.GetSprite(2, 10)
            }, new System.Windows.Point(BlockSize, BlockSize * -1)));


            players[0].AnimationDictionary.Add(GamePlayer.GamePlayerDirection.Left, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(1, 0),
                playerSpriteSheet.GetSprite(1, 1),
                playerSpriteSheet.GetSprite(1, 2),
                playerSpriteSheet.GetSprite(1, 3),
                playerSpriteSheet.GetSprite(1, 4)
            }, new System.Windows.Point(BlockSize * -1, 0)));

            players[0].AnimationDictionary.Add(GamePlayer.GamePlayerDirection.Right, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(2, 0),
                playerSpriteSheet.GetSprite(2, 1),
                playerSpriteSheet.GetSprite(2, 2),
                playerSpriteSheet.GetSprite(2, 3),
                playerSpriteSheet.GetSprite(2, 4)
            }, new System.Windows.Point(BlockSize, 0)));

            players[0].AnimationDictionary.Add(GamePlayer.GamePlayerDirection.Bottom, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(0, 0),
                playerSpriteSheet.GetSprite(0, 1),
                playerSpriteSheet.GetSprite(0, 2),
                playerSpriteSheet.GetSprite(0, 3),
                playerSpriteSheet.GetSprite(0, 4)
            }, new System.Windows.Point(0, BlockSize)));

            players[0].AnimationDictionary.Add(GamePlayer.GamePlayerDirection.BottomLeft, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(1, 6),
                playerSpriteSheet.GetSprite(1, 7),
                playerSpriteSheet.GetSprite(1, 8),
                playerSpriteSheet.GetSprite(1, 9),
                playerSpriteSheet.GetSprite(1, 10)
            }, new System.Windows.Point(BlockSize * -1, BlockSize)));

            players[0].AnimationDictionary.Add(GamePlayer.GamePlayerDirection.BottomRight, createWalkAnimation(new Bitmap[]{
                playerSpriteSheet.GetSprite(0, 6),
                playerSpriteSheet.GetSprite(0, 7),
                playerSpriteSheet.GetSprite(0, 8),
                playerSpriteSheet.GetSprite(0, 9),
                playerSpriteSheet.GetSprite(0, 10)
            }, new System.Windows.Point(BlockSize, BlockSize)));
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
                    Time = 500 / sprites.Length
                };
            }

            return new GameAnimation(null, steps, runForever: false);
        }

        private void map_onRenderGameObject(GameObject gameObject)
        {
            if(gameObject.Type == GameObject.GameObjectType.Player)
            {
                GetPlayer(0).SetGameObject(gameObject);
                GetPlayer(0).TurnBottom();
                gameObject.Position = new System.Windows.Point(gameObject.Position.X, gameObject.Position.Y - 5);
                gameObject.Render(this.GetLayerByIndex(1));
            }
            else if (gameObject.Type == GameObject.GameObjectType.Target)
            {
                GameSpriteSheet foodSheet = new GameSpriteSheet(ResourceTextures.food_sheet, new System.Drawing.Point(32,32));
                gameObject.SetTexture(foodSheet.GetSprite(4,7));
                gameObject.Render(this.GetLayerByIndex(1));
            }
            else if (gameObject.Type == GameObject.GameObjectType.Wall)
            {
                gameObject.Texture = ResourceTextures.wall;

                if (!IsColisionObject(gameObject.Neighbors.Bottom))
                    gameObject.SetTexture(ResourceTextures.wall_bottom);

                if (!IsColisionObject(gameObject.Neighbors.Top) && !IsColisionObject(gameObject.Neighbors.Right))
                    gameObject.SetTexture(ResourceTextures.wall_top);

                if (IsColisionObject(gameObject.Neighbors.Bottom) && IsColisionObject(gameObject.Neighbors.Right) && !IsColisionObject(gameObject.Neighbors.BottomRight))
                    gameObject.SetTexture(ResourceTextures.wall_3);

                if (!IsColisionObject(gameObject.Neighbors.Bottom) && !IsColisionObject(gameObject.Neighbors.Right) && !IsColisionObject(gameObject.Neighbors.Left))
                    gameObject.SetTexture(ResourceTextures.wall_bottom_right);

                if (!IsColisionObject(gameObject.Neighbors.Bottom) && !IsColisionObject(gameObject.Neighbors.Right) && IsColisionObject(gameObject.Neighbors.Top))
                    gameObject.SetTexture(ResourceTextures.wall_7);

                if (!IsColisionObject(gameObject.Neighbors.Bottom) && !IsColisionObject(gameObject.Neighbors.Right)
                        && IsColisionObject(gameObject.Neighbors.Top)
                        && IsColisionObject(gameObject.Neighbors.Left))
                    gameObject.SetTexture(ResourceTextures.wall_6);

                if (IsColisionObject(gameObject.Neighbors.Bottom)
                        && IsColisionObject(gameObject.Neighbors.Top)
                        && !IsColisionObject(gameObject.Neighbors.Right))
                    gameObject.SetTexture(ResourceTextures.wall_side_right);

                if (!IsColisionObject(gameObject.Neighbors.Bottom) && !IsColisionObject(gameObject.Neighbors.Right) && !IsColisionObject(gameObject.Neighbors.Top)
                        && IsColisionObject(gameObject.Neighbors.Left))
                    gameObject.SetTexture(ResourceTextures.wall_5);

                if (!IsColisionObject(gameObject.Neighbors.Bottom) 
                        && IsColisionObject(gameObject.Neighbors.Right)
                        && !IsColisionObject(gameObject.Neighbors.Left))
                    gameObject.SetTexture(ResourceTextures.wall_4);

                gameObject.Render(this.GetLayerByIndex(2));
            }
        }

        public bool IsColisionObject(GameObject gameObject)
        {
            return gameObject != null && gameObject.MakeColision == true;
        }
    }
}
