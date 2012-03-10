namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Elements.SmartSprite
open Elements.Element

module Game =
    type XnaGame() as this =
        inherit Game()
    
        do this.Content.RootDirectory <- "XnaGameContent"
        let graphicsDeviceManager = new GraphicsDeviceManager(this)

        //Allows null reference. We need this for .NET XNA interop
        let mutable waterE = Unchecked.defaultof<Element>
        let mutable earthE = Unchecked.defaultof<Element>

        override game.Initialize() =
            graphicsDeviceManager.GraphicsProfile <- GraphicsProfile.HiDef
            graphicsDeviceManager.PreferredBackBufferWidth <- 1260
            graphicsDeviceManager.PreferredBackBufferHeight <- 740
            graphicsDeviceManager.IsFullScreen <- false
            this.IsMouseVisible <- true
            graphicsDeviceManager.ApplyChanges() 
            waterE <- new Element(game, "water")
            earthE <- new Element(game, "earth")
            earthE.Sprite.X <- 250.0f
            earthE.Sprite.Y <- 300.0f
            waterE.Sprite.X <- 200.0f
            waterE.Sprite.Y <- 300.0f
            base.Initialize()
        
        //override game.Update gameTime = 
            //NO-OP


        override game.Draw gameTime = 
            game.GraphicsDevice.Clear(Color.Gray)
            waterE.Sprite.Draw
            earthE.Sprite.Draw

    let game = new XnaGame()
    game.Run()