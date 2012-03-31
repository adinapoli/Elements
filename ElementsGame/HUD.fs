namespace Elements

open Microsoft.Xna.Framework
open System
open Prefabs
open Components
open Microsoft.Xna.Framework.Input
open Elements.Entities
open Nuclex.UserInterface
open Nuclex.UserInterface.Controls
open Nuclex.UserInterface.Controls.Desktop
open Events
open Utils


    module HUD =


       /// This class monitors the framerate.
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
        

       /// This class monitors whether the game is running slow.
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


       /// <summary>Element Counter. Counts elements on screen.</summary>
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


       /// The DebugHUD Entity.
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


       /// Keeps track of discovered elements.
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

       
       /// An element browser, this encapsulated a GUI panel with (TODO)
       /// event list for importing in the main work area.    
       type ElementBrowser(screen : Screen, collisionBounds : Rectangle) as this =
        inherit WindowControl()

        let screen_:Screen = screen
        let bounds_:Rectangle = collisionBounds
        let mutable isOpened_:bool = false
        let id_ : string = "ElementBrowser"

        do
            this.EnableDragging <- true
            this.Bounds <- new UniRectangle(us 600.0f, us 10.0f, 
                                            us 512.0f, us 600.0f)
            this.Title <- "Element Browser"
            let okButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl()
            let cancelButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl()
            okButton.Bounds <- new UniRectangle(new UniScalar(1.0f, -180.0f), 
                                                new UniScalar(1.0f, -40.0f), 
                                                us 80.0f, us 24.0f)
            okButton.Text <- "Ok"
            okButton.ShortcutButton <- new Nullable<Buttons>(Buttons.A)
            okButton.Pressed.AddHandler((fun sender args -> 
                                            isOpened_ <- false
                                            this.Close()))
     
            cancelButton.Bounds <- new UniRectangle(new UniScalar(1.0f, -90.0f), 
                                                    new UniScalar(1.0f, -40.0f), 
                                                    us 80.0f, us 24.0f)
            cancelButton.Text <- "Cancel"
            cancelButton.ShortcutButton <- new Nullable<Buttons>(Buttons.B)
            cancelButton.Pressed.AddHandler((fun sender args -> 
                                                isOpened_ <- false
                                                this.Close()))


            this.Children.Add(okButton)
            this.Children.Add(cancelButton)


        interface IGameComponent with
            member this.Update (gameTime : GameTime) =
                match isOpened_ with
                    |true -> ()
                    |false ->
                        let mouseState = Mouse.GetState()
                        let leftPressed = mouseState.LeftButton = ButtonState.Pressed
                        let collided = bounds_.Contains(mouseState.X, mouseState.Y)
                        in match collided && leftPressed  with 
                            | true ->
                                isOpened_ <- true
                                screen_.Desktop.Children.Add(this)
                            | false ->()

            member this.Id = id_
            member this.Type = "GUIElement"
        end


       /// A button who models addition of one element. 
       type AddElementBtn(game : Game, screen: Screen,
                          xpos : int32, ypos : int32) as this =
        inherit GameEntity("AddElementBtn")

        let screen_ : Screen = screen

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

            let success_fn = (fun () -> overlay_.IsVisible <- true)
            let failure_fn = (fun () -> overlay_.IsVisible <- false)
            this.Attach(new MouseCollisionHandler("mch", sprite_.Bounds, 
                                                  success_fn, failure_fn))
            this.Attach(new ElementBrowser(screen, sprite_.Bounds))

        override this.Update(gameTime : GameTime) = 
            this.Components.Values |> Seq.iter (fun c -> c.Update gameTime)

        override this.Draw(gameTime : GameTime) = 
            for (e:IGameComponent) in this.Components.Values do
                try
                    (e :?> IDrawable).Draw gameTime
                with
                    //If the cast fails, ignore it.
                    | :? System.InvalidCastException -> ()


       /// The trash bin.
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


       /// The HUD.
       type Hud(game : Game, screen : Screen) as this =
        inherit EntitiesManager()

        do
            let tracker = new ElementTracker(game, 1000, 20)
            tracker.Counter <- 4
            let addBtn = new AddElementBtn(game, screen, 500,20)
            let trashBtn = new TrashElementBtn(game, 600,20)
                
            this.Attach(tracker)
            this.Attach(addBtn)
            this.Attach(trashBtn)

        override this.Update (gameTime : GameTime) : unit = 
            Seq.iter (fun (e:GameEntity) -> e.Update gameTime) this.Entities.Values