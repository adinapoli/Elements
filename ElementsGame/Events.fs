namespace Elements

open Microsoft.Xna.Framework
open Elements.Entities

module Events =

    type IListener =
        abstract member Listen : unit
    
    type EventManager() =
        inherit EntitiesManager()

        let mutable listeners_ = []

        member this.Subscribe(l : IListener) =
            listeners_ <- l :: listeners_

        override this.Update (gameTime : GameTime) : unit = ()

