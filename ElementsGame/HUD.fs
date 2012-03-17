namespace Elements

open Microsoft.Xna.Framework
open System
open Prefabs
open Components

open Elements.Entities

    module HUD =


       (* *********************************************************************
        *
        * FRAMECOUNTER CLASS
        *
        **********************************************************************)
       type FramerateCounter(id : string, game : Game, fontName : string) =
            inherit TextComponent(id, game, fontName)

            interface IGameComponent with
                member this.Type = "speedWatcher"
                member this.Update (gameTime : GameTime) = ()


            interface IDrawable with
                member this.Draw (gameTime : GameTime) =
                    this.Text <- String.Format("running slowly: {0}", gameTime.IsRunningSlowly)

                    this.Sprite.Begin()
                    this.Sprite.DrawString(this.Font, 
                                           this.Text, 
                                           new Vector2(float32 this.X, float32 this.Y), 
                                           Color.Black)
                    this.Sprite.DrawString(this.Font, 
                                           this.Text, 
                                           new Vector2(float32 this.X, float32 this.Y), 
                                           Color.White)
                    this.Sprite.End()
        

       (* *********************************************************************
       *
       * SPEEDWATCHER CLASS
       *
       ***********************************************************************)
       // This class monitors whether the game is running slow.
       type SpeedWatcher(id : string, game : Game, fontName : string) =
            inherit TextComponent(id, game, fontName)

            let mutable elapsedTime_:TimeSpan = TimeSpan.Zero
            let mutable frameRate_:int = 0
            let mutable frameCounter_:int = 0


            interface IGameComponent with
                member this.Type = "graphicCounter"
                member this.Update (gameTime : GameTime) =
                    elapsedTime_ <- elapsedTime_ + gameTime.ElapsedGameTime

                    if (elapsedTime_ > TimeSpan.FromSeconds(1.0))
                    then do
                        elapsedTime_ <- elapsedTime_ - TimeSpan.FromSeconds(1.0)
                        frameRate_ <- frameCounter_
                        frameCounter_ <- 0


            interface IDrawable with
                member this.Draw (gameTime : GameTime) =
                
                    frameCounter_ <- frameCounter_ + 1
                    this.Text <- String.Format("fps: {0}", frameRate_)

                    this.Sprite.Begin()
                    this.Sprite.DrawString(this.Font, 
                                           this.Text, 
                                           new Vector2(float32 this.X, float32 this.Y), 
                                           Color.Black)
                    this.Sprite.DrawString(this.Font, 
                                           this.Text, 
                                           new Vector2(float32 this.X, float32 this.Y), 
                                           Color.White)
                    this.Sprite.End()


        (* *********************************************************************
         *
         * HUD CLASS
         *
         **********************************************************************)
        type Hud(game : Game) as this =
            inherit GameEntity("HudEntity")

            do
                let fpsCounter = new FramerateCounter("fpsCounter", game, "DebugHUD")
                fpsCounter.X <- 10
                fpsCounter.Y <- 20
                let speedWatcher = new SpeedWatcher("speedWatcher", game, "DebugHUD")
                speedWatcher.X <- 10
                speedWatcher.Y <- 40
                
                //Attach components to the HUD
                this.Attach(fpsCounter)
                this.Attach(speedWatcher)

            override this.Update (gameTime : GameTime) : unit = 
                let fn = (fun (c:IGameComponent) -> c.Update gameTime)
                    in Seq.iter fn this.Components.Values
            
            override this.Draw   (gameTime : GameTime) : unit =
                for (e:IGameComponent) in this.Components.Values do
                    try
                        (e :?> IDrawable).Draw gameTime
                    with
                    //If the cast fails, ignore it.
                    | :? System.InvalidCastException -> ()