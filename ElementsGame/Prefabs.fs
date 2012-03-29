namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Elements.Components
open Elements.Entities
open Ruminate.GUI.Content
open Ruminate.GUI.Framework

module Prefabs =

    type SmartSprite(game: Game, spritePath : string) =
        
        let mutable sprite_: Texture2D = game.Content.Load<Texture2D>(spritePath)
        let spriteBatch_ = new SpriteBatch(game.GraphicsDevice)
        let mutable x_:int32 = 0
        let mutable y_:int32 = 0
        let mutable visible_ = true
        let id_ = spritePath.Split '/' |> (fun it -> it.[it.Length - 1])

        member this.X with get() = x_ and set x = x_ <- x
        member this.Y with get() = y_ and set y = y_ <- y
        member this.IsVisible with get() = visible_ and set v = visible_ <- v
        member this.Bounds
            with get() = new Rectangle(x_, y_, 64, 64)
        
        
        member this.Move(x,y) = (this :> IMovable).Move(x,y)
        member this.Type   = (this :> IGameComponent).Type
        member this.Id   = (this :> IGameComponent).Id
        member this.Draw (gameTime : GameTime) = 
            (this :> IDrawable).Draw gameTime


        (* INTERFACE IMPLEMENTATIONS *)
        interface IMovable with
            member this.Move(x:int32, y:int32) =
                x_ <- x
                y_ <- y


        interface IGameComponent with
            member this.Update (gameTime : GameTime) = ()
            member this.Id = id_
            member this.Type = "sprite"


        interface IDrawable with
            member this.Draw (gameTime : GameTime) =
                match visible_ with
                | true ->
                    spriteBatch_.Begin()
                    spriteBatch_.Draw(sprite_, Vector2(float32 x_, float32 y_), Color.White)
                    spriteBatch_.End()
                | false -> ()




    // A simple text component that can be displayed on screen,
    // in it's own SpriteBatch
    type TextComponent(id : string, game : Game, fontName : string) =

        let id_ : string = id
        let mutable text_:string = ""
        let assetPath_ : string = "Media/Fonts/"
        let mutable font_ = game.Content.Load<SpriteFont>(assetPath_ + fontName)
        let spriteBatch_ : SpriteBatch = new SpriteBatch(game.GraphicsDevice)
        let mutable color_ : Color = Color.White
        let mutable x_ : int32 = 0
        let mutable y_ : int32 = 0
        
        member this.Text with get() = text_ and set t = text_ <- t
        member this.Font with get() = font_ and set f = font_ <- f
        member this.Sprite with get() = spriteBatch_
        member this.X with get() = x_ and set x = x_ <- x
        member this.Y with get() = y_ and set y = y_ <- y
        
        member this.Id        = (this :> IGameComponent).Id
        member this.Type      = (this :> IGameComponent).Type
        member this.Move      = (this :> IMovable).Move
        member this.Draw   gt = (this :> IDrawable).Draw gt 
        member this.Update gt = (this :> IGameComponent).Update gt


        (* INTERFACE IMPLEMENTATIONS *)
        interface IMovable with
            member this.Move(x:int32, y:int32) =
                x_ <- x
                y_ <- y

        interface IDrawable with
            member this.Draw (gameTime : GameTime) =
                spriteBatch_.Begin()
                spriteBatch_.DrawString(font_, 
                                        text_, 
                                        new Vector2(float32 x_, float32 y_), 
                                        color_)
                spriteBatch_.End()

        interface IGameComponent with
            member this.Update (gameTime : GameTime) = ()
            member this.Id = id_
            member this.Type = "text"



    (*
     * GUI CONTAINER
     * An Entity that manages all GUI component. It support
     * creation and (disposal?) of components.
     *)
    type GuiContainer(game : Game) =
        inherit GameEntity("guiContainer")

        //Specific GUI library coupling
        let gui_:Gui = new Gui(game)
        do 
            let defaultTheme_ = new EmbeddedTheme(gui_)
            gui_.SetTheme(defaultTheme_)
            gui_.AddElement(new Panel(800, 200, 100, 400))

        member this.AddElement(guiElement) =
            gui_.AddElement(guiElement)

        member this.RemoveElement(guiElement) =
            gui_.RemoveElement(guiElement)
        
        override this.Update (gameTime : GameTime) : unit = 
            gui_.Update()
        override this.Draw (gameTime : GameTime) : unit = 
            gui_.Draw()