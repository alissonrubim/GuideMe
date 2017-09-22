using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LazySnake.Engine
{
    public class GameAnimation
    {
        public delegate void GameAnimationEventHandler();
        public class AnimateStep
        {
            public Bitmap Texture { get; set; }
            public int Time { get; set; }
            public System.Windows.Point PositionDiff { get; set; }
            public System.Windows.Size Size { get; set; }
        }

        public string Name;

        public GameAnimationEventHandler OnFinish;
        public GameAnimationEventHandler OnStart;

        public AnimateStep[] Steps;

        public GameSprite GameSprite;

        public bool RunForever;

        public GameAnimation(string name, AnimateStep[] steps)
        {
            this.Name = name;
            this.Steps = steps;
        }

        public GameAnimation(string name, AnimateStep[] steps, GameSprite gameSprite = null, bool runForever = true, GameAnimationEventHandler handle = null)
        {
            this.Name = name;
            this.Steps = steps;
            this.GameSprite = gameSprite;
            this.RunForever = runForever;
            this.OnFinish += handle;
        }

        public void SetGameSprite(GameSprite gameSprite)
        {
            this.GameSprite = gameSprite;
        }

        public void Run()
        {
            int index = 0;
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler((o, e) =>
            {
                if (index < Steps.Length - 1)
                    index++;
                else
                {
                    if(RunForever == true)
                        index = 0;
                    else
                    {
                        timer.Stop();
                        Thread.Sleep(50);
                        if(OnFinish != null)
                            this.OnFinish();
                    }
                }

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new ThreadStart(delegate
                {
                    if (Steps[index].Texture != null)
                        GameSprite.SetTexture(Steps[index].Texture);
                    if (Steps[index].PositionDiff != null)
                        GameSprite.SetPosition(new System.Windows.Point(GameSprite.GetPosition().X + Steps[index].PositionDiff.X, GameSprite.GetPosition().Y + Steps[index].PositionDiff.Y));
                }));

                timer.Interval = Steps[index].Time;

            });
            timer.Interval = 1;
            timer.Start();
            if (OnStart != null)
                this.OnStart();
        }
    }
}
