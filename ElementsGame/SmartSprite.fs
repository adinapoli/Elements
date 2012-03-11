namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Elements.Components

module SmartSprite =
    type SmartSprite(game: Game, spritePath : string) =
        
        let mutable sprite_: Texture2D = game.Content.Load<Texture2D>(spritePath)
        let mutable spriteBatch_ = new SpriteBatch(game.GraphicsDevice)
        let mutable x_:int32 = 0
        let mutable y_:int32 = 0
        let mutable visible_ = true

        member this.X with get() = x_ and set x = x_ <- x
        member this.Y with get() = y_ and set y = y_ <- y
        member this.IsVisible with get() = visible_ and set v = visible_ <- v
        member this.Bounds
            with get() = new Rectangle(x_, y_, 64, 64)

        member this.Draw =
            match visible_ with
            | true ->
                spriteBatch_.Begin()
                spriteBatch_.Draw(sprite_, Vector2(float32 x_, float32 y_), Color.White)
                spriteBatch_.End()
            | false -> ()