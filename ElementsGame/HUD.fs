namespace Elements

open Elements.Entities

    module HUD =
        
        type Hud() =
            inherit GameEntity("HudEntity")

            override this.Update : unit = ()
