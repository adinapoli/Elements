namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Elements.Element

module Game =
    type XnaGame() as this =
        inherit Game()
    
        do this.Content.RootDirectory <- "XnaGameContent"
        let graphicsDeviceManager = new GraphicsDeviceManager(this)

        // Manages elements entities
        let elementManager = new ElementsManager()

        override game.Initialize() =
            graphicsDeviceManager.GraphicsProfile <- GraphicsProfile.HiDef
            graphicsDeviceManager.PreferredBackBufferWidth <- 1260
            graphicsDeviceManager.PreferredBackBufferHeight <- 735
            graphicsDeviceManager.IsFullScreen <- false
            this.IsMouseVisible <- true
            this.Window.Title <- "Elements"
            graphicsDeviceManager.ApplyChanges()
            
            elementManager.Attach(new Element(game, "water", 200, 300))
            elementManager.Attach(new Element(game, "earth", 250, 300))
            elementManager.Attach(new Element(game, "fire",  400, 200))
            elementManager.Attach(new Element(game, "air",   400, 300))
            
            base.Initialize()
        
        //override game.Update gameTime = 
            //NO-OP


        override game.Draw gameTime = 
            game.GraphicsDevice.Clear(Color.Gray)
            elementManager.Update gameTime

    let game = new XnaGame()
    game.Run()