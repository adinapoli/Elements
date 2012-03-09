namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Elements.SmartSprite


/// Element is a generic element that can be combined.
module Element =

    type ElementObject(game: Game, spritePath : string) =
        inherit SmartSprite(game, spritePath)
