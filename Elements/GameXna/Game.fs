namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Elements.Element
open HUD

module Game =
    type XnaGame() as this =
        inherit Game()
    
        do this.Content.RootDirectory <- "XnaGameContent"
        let graphicsDeviceManager = new GraphicsDeviceManager(this)

        // Manages elements entities
        let elementManager = new ElementsManager()
        let mutable hud = Unchecked.defaultof<Hud>

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

            //Initialize the HUD
            hud <- new Hud(game)
            
            base.Initialize()
        
        override game.Update gameTime = 
            elementManager.Update gameTime
            hud.Update gameTime


        override game.Draw gameTime = 
            game.GraphicsDevice.Clear(Color.Gray)
            elementManager.Draw gameTime
            hud.Draw gameTime

    let game = new XnaGame()
    game.Run()