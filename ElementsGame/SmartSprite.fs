namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

module SmartSprite =
    type SmartSprite(game: Game, spritePath : string) =
    
        let mutable sprite_: Texture2D = game.Content.Load<Texture2D>(spritePath)
        let mutable spriteBatch_ = new SpriteBatch(game.GraphicsDevice)
        let mutable x_:float32 = 0.0f
        let mutable y_:float32 = 0.0f
        let mutable visible_ = true

        member this.X with get() = x_ and set x = x_ <- x
        member this.Y with get() = y_ and set y = y_ <- y
        member this.IsVisible with get() = visible_ and set v = visible_ <- v

        member this.Draw =
            match visible_ with
            | true ->
                spriteBatch_.Begin()
                spriteBatch_.Draw(sprite_, Vector2(x_, y_), Color.White)
                spriteBatch_.End()
            | false -> ()