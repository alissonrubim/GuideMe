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
        private Canvas parentCanvas = null;
        private GameMap currentMap = null;
        private GamePlayer[] players = new GamePlayer[2]; 

        public GameEngine(Canvas parentCanvas)
        {
            this.parentCanvas = parentCanvas;
            setupLayers();
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

        private void setupLayers()
        {
            Canvas parent = this.parentCanvas;
            for (int i = 0; i < layers.Length; i++)
            {
                Canvas layer = new Canvas();
                layer.Visibility = Visibility.Visible;
                layer.Background = System.Windows.Media.Brushes.Transparent;
                layer.Width = parent.ActualWidth;
                layer.Height = parent.ActualHeight;
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
            players[0].TextureMap.Add(GamePlayer.GamePlayerDirection.Up, playerSpriteSheet.GetSprite(3, 1));
            players[0].TextureMap.Add(GamePlayer.GamePlayerDirection.UpLeft, playerSpriteSheet.GetSprite(2, 7));
            players[0].TextureMap.Add(GamePlayer.GamePlayerDirection.UpRight, playerSpriteSheet.GetSprite(3, 10));
            players[0].TextureMap.Add(GamePlayer.GamePlayerDirection.Left, playerSpriteSheet.GetSprite(1, 1));
            players[0].TextureMap.Add(GamePlayer.GamePlayerDirection.Right, playerSpriteSheet.GetSprite(2, 4));
            players[0].TextureMap.Add(GamePlayer.GamePlayerDirection.Bottom, playerSpriteSheet.GetSprite(0, 1));
            players[0].TextureMap.Add(GamePlayer.GamePlayerDirection.BottomLeft, playerSpriteSheet.GetSprite(1, 7));
            players[0].TextureMap.Add(GamePlayer.GamePlayerDirection.BottomRight, playerSpriteSheet.GetSprite(0, 7));
        }

        private void map_onRenderGameObject(GameObject gameObject)
        {
            if(gameObject.Type == GameObject.GameObjectType.Player)
            {
                GetPlayer(0).SetGameObject(gameObject);
                GetPlayer(0).TurnBottom();
                gameObject.Position = new System.Windows.Point(gameObject.Position.X, gameObject.Position.Y - 5);
                gameObject.Render(this.GetLayerByIndex(2));
            }
            else if (gameObject.Type == GameObject.GameObjectType.Wall)
            {
                gameObject.Texture = ResourceTextures.wall;

                if (gameObject.NeighborBottom == null)
                    gameObject.SetTexture(ResourceTextures.wall_bottom);

                if (gameObject.NeighborTop == null && gameObject.NeighborRight == null)
                    gameObject.SetTexture(ResourceTextures.wall_top);

                if (gameObject.NeighborBottom != null && gameObject.NeighborRight != null && gameObject.NeighborBottomRight == null)
                    gameObject.SetTexture(ResourceTextures.wall_3);

                if (gameObject.NeighborBottom == null && gameObject.NeighborRight == null && gameObject.NeighborLeft == null)
                    gameObject.SetTexture(ResourceTextures.wall_bottom_right);

                if (gameObject.NeighborBottom == null && gameObject.NeighborRight == null && gameObject.NeighborTop != null)
                    gameObject.SetTexture(ResourceTextures.wall_7);

                if (gameObject.NeighborBottom == null && gameObject.NeighborRight == null && gameObject.NeighborTop != null && gameObject.NeighborLeft != null)
                    gameObject.SetTexture(ResourceTextures.wall_6);

                if (gameObject.NeighborBottom != null && gameObject.NeighborTop != null && gameObject.NeighborRight == null)
                    gameObject.SetTexture(ResourceTextures.wall_side_right);

                if (gameObject.NeighborBottom == null && gameObject.NeighborRight == null && gameObject.NeighborTop == null && gameObject.NeighborLeft != null)
                    gameObject.SetTexture(ResourceTextures.wall_5);

                if (gameObject.NeighborBottom == null && gameObject.NeighborRight != null && gameObject.NeighborLeft == null)
                    gameObject.SetTexture(ResourceTextures.wall_4);

                gameObject.Render(this.GetLayerByIndex(1));
            }
        }

 
    }
}
