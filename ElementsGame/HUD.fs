namespace Elements

open Microsoft.Xna.Framework
open System
open Prefabs
open Components
open Microsoft.Xna.Framework.Input
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
                                           Color.White)
                    this.Sprite.End()


        (*
         * Elements counter
         *
         *)
        type ElementCounter(id : string, game : Game, fontName : string) =
            inherit TextComponent(id, game, fontName)

            let mutable counter_ = 0

            interface IGameComponent with
                member this.Type = "elementCounter"
                member this.Update (gameTime : GameTime) = ()


            interface IDrawable with
                member this.Draw (gameTime : GameTime) =
                    this.Text <- String.Format("Elements on screen: {0}", this.Counter)

                    this.Sprite.Begin()
                    this.Sprite.DrawString(this.Font, 
                                            this.Text, 
                                            new Vector2(float32 this.X, float32 this.Y), 
                                            Color.White)
                    this.Sprite.End()


            member this.Counter with get() = counter_ and set c = counter_ <- c


        (* *********************************************************************
         *
         * DEBUG HUD CLASS
         *
         **********************************************************************)
        type DebugHud(game : Game) as this =
            inherit GameEntity("DebugHudEntity")

            do
                let fpsCounter = new FramerateCounter("fpsCounter", game, "DebugHUD")
                fpsCounter.X <- 10
                fpsCounter.Y <- 20
                let speedWatcher = new SpeedWatcher("speedWatcher", game, "DebugHUD")
                speedWatcher.X <- 10
                speedWatcher.Y <- 40
                let elementCounter = new ElementCounter("elementCounter", game, "DebugHUD")
                elementCounter.X <- 10
                elementCounter.Y <- 60
                
                //Attach components to the HUD
                this.Attach(fpsCounter)
                this.Attach(speedWatcher)
                this.Attach(elementCounter)

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




        (*
         * This component keeps track of the number of discovered elements.
         *)
        type ElementTracker(game : Game, xpos : int32, ypos : int32) as this =
            inherit GameEntity("elementTracker")

            do 
                let tc_ = new TextComponent("TextC", game, "hud_caption")
                tc_.X <- xpos
                tc_.Y <- ypos
                this.Attach(tc_)
            let mutable counter_ = 0
            member this.Counter with get() = counter_ and set c = counter_ <- c

            override this.Draw (gameTime : GameTime) =
                match this.ComponentById("TextC") with
                    | Some(tc) -> 
                        (tc :?> TextComponent).Text <- String.Format("Discovered Elements: {0}", this.Counter)
                    | None -> ()

                for (e:IGameComponent) in this.Components.Values do
                    try
                        (e :?> IDrawable).Draw gameTime
                    with
                        //If the cast fails, ignore it.
                        | :? System.InvalidCastException -> ()

            override this.Update(gameTime : GameTime) =
                let fn = (fun (e:IGameComponent) -> e.Update gameTime)
                Seq.iter fn this.Components.Values

        
        type HudPanel(game : Game) =
            inherit GameEntity("HudPanel")

            override this.Update(gameTime : GameTime) : unit = ()
            override this.Draw (gameTime : GameTime) : unit = ()    

        
        type AddElementBtn(game : Game, xpos : int32, ypos : int32) as this =
            inherit GameEntity("AddElementBtn")

            do
                let sprite_ = new SmartSprite(game, "Media/HUD/add_element")
                let overlay_ = new SmartSprite(game, "Media/HUD/add_element_overlay")
                sprite_.X <- xpos
                sprite_.Y <- ypos
                overlay_.X <- xpos
                overlay_.Y <- ypos
                overlay_.IsVisible <- false
                this.Attach(sprite_)
                this.Attach(overlay_)

            override this.Update(gameTime : GameTime) = 
                //We should check whether the mouse collide with
                //the button. If yes, we should also display the overlay.
                let mouseState = Mouse.GetState()
                let overlay = this.ComponentById("add_element_overlay")
                match overlay with
                | Some(e) -> 
                    let collided = (e :?> SmartSprite).Bounds.Contains(mouseState.X, mouseState.Y)
                    in match collided with 
                        | true ->  (e :?> SmartSprite).IsVisible <- true
                        | false -> (e :?> SmartSprite).IsVisible <- false
                | None -> ()  

            override this.Draw(gameTime : GameTime) = 
                for (e:IGameComponent) in this.Components.Values do
                    try
                        (e :?> IDrawable).Draw gameTime
                    with
                        //If the cast fails, ignore it.
                        | :? System.InvalidCastException -> ()


        type TrashElementBtn(game : Game, xpos : int32, ypos : int32) as this =
            inherit GameEntity("TrashElementBtn")

            do
                let sprite_ = new SmartSprite(game, "Media/HUD/trash_element")
                sprite_.X <- xpos
                sprite_.Y <- ypos
                this.Attach(sprite_)

            override this.Update(gameTime : GameTime) = ()
            override this.Draw(gameTime : GameTime) = 
                for (e:IGameComponent) in this.Components.Values do
                    try
                        (e :?> IDrawable).Draw gameTime
                    with
                        //If the cast fails, ignore it.
                        | :? System.InvalidCastException -> ()


        (* 
         * HUD ENTITY 
         *)
        type Hud(game : Game) as this =
            inherit EntitiesManager()

            do
                let tracker = new ElementTracker(game, 1000, 20)
                tracker.Counter <- 4
                let addBtn = new AddElementBtn(game, 500,20)
                let trashBtn = new TrashElementBtn(game, 600,20)
                
                this.Attach(tracker)
                this.Attach(addBtn)
                this.Attach(trashBtn)

            override this.Update (gameTime : GameTime) : unit = 
                Seq.iter (fun (e:GameEntity) -> e.Update gameTime) this.Entities.Values