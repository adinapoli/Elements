namespace Elements

open Microsoft.Xna.Framework
open System
open Components

open Elements.Entities

    module HUD =

       type FramerateCounter(id : string, game : Game, fontName : string) =
         
            let id_:string = id
            let mutable elapsedTime_:TimeSpan = TimeSpan.Zero
            let mutable frameRate_:int = 0;
            let mutable frameCounter_:int = 0;

            interface IGameComponent with
                member this.Id = id_
                member this.Type = "graphicCounter"
                member this.Update (gameTime : GameTime) =
                    elapsedTime_ <- elapsedTime_ + gameTime.ElapsedGameTime

                    if (elapsedTime_ > TimeSpan.FromSeconds(1.0))
                    then do
                        elapsedTime_ <- elapsedTime_ - TimeSpan.FromSeconds(1.0)
                        frameRate_ <- frameCounter_;
                        frameCounter_ <- 0
        



        type Hud() =
            inherit GameEntity("HudEntity")

            override this.Update (gameTime : GameTime) : unit = ()
            override this.Draw   (gameTime : GameTime) : unit = ()