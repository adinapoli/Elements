namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Prefabs
open Elements.Element
open HUD

module Game =
    type XnaGame() as this =
        inherit Game()
    
        do this.Content.RootDirectory <- "XnaGameContent"
        let graphicsDeviceManager = new GraphicsDeviceManager(this)

        // Manages elements entities
        let elementManager = new ElementsManager(this)
        let elementRelations = new ElementRelations()

        let mutable background = Unchecked.defaultof<SmartSprite>
        let mutable debugHud = Unchecked.defaultof<DebugHud>
        let mutable hud = Unchecked.defaultof<Hud>
        

        override game.Initialize() =
            graphicsDeviceManager.GraphicsProfile <- GraphicsProfile.HiDef
            graphicsDeviceManager.PreferredBackBufferWidth <- 1260
            graphicsDeviceManager.PreferredBackBufferHeight <- 735
            graphicsDeviceManager.IsFullScreen <- false
            this.IsMouseVisible <- true
            this.Window.Title <- "Elements"
            graphicsDeviceManager.ApplyChanges()
            
            //Load the background first
            background <- new SmartSprite(game, "Media/Backgrounds/bg_wood_1200_800")

            //Load the starting elements
            elementManager.Attach(new Element(game, "water", 500, 300))
            elementManager.Attach(new Element(game, "earth", 600, 300))
            elementManager.Attach(new Element(game, "fire",  500, 400))
            elementManager.Attach(new Element(game, "air",   600, 400))

            //Initialize the debug HUD, only if in debug mode
            if System.Diagnostics.Debugger.IsAttached
                then debugHud <- new DebugHud(game)      
            
            //Initialize the real hud
            hud <- new Hud(game)

            base.Initialize()
        

        override game.Update gameTime = 
            
            //Update the hud first
            match hud.Entity("elementTracker") with
                | Some(e) ->   
                    (e :?> ElementTracker).Counter <- elementManager.DiscoveredElements
                | _ -> ()
            hud.Update gameTime

            //Update managers
            elementManager.Update gameTime

            //Update the debug HUD, only if in debug mode
            if System.Diagnostics.Debugger.IsAttached
                then 
                    (debugHud.ComponentsByType("elementCounter")
                    |> Seq.head :?> ElementCounter).Counter <- elementManager.Entities.Count 
                    debugHud.Update gameTime             
            
            

        override game.Draw gameTime = 
            //Draw the background first
            //game.GraphicsDevice.Clear(Color.Gray)
            background.Draw gameTime

            //Draw the hud first
            hud.Draw gameTime

            //Draw the managers
            elementManager.Draw gameTime
            
            //Draw the debug HUD, only if in debug mode
            if System.Diagnostics.Debugger.IsAttached
                then debugHud.Draw gameTime

            

    let game = new XnaGame()
    game.Run()