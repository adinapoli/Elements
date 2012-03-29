namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

open Nuclex.UserInterface
open Nuclex.Input

open Prefabs
open Elements.Element
open HUD
open GUI

module Game =
    type XnaGame() as this =
        inherit Game()
    
        do this.Content.RootDirectory <- "XnaGameContent"
        let graphicsDeviceManager = new GraphicsDeviceManager(this)
        let input = new InputManager(this.Services, this.Window.Handle)
        let gui = new GuiManager(this.Services)
        do
            this.Components.Add(input)
            this.Components.Add(gui)
            gui.DrawOrder <- 1000

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
            
            //Call the base initializer
            base.Initialize()

            // Viewport and screen defs
            let viewport:Viewport = this.GraphicsDevice.Viewport
            let mainScreen:Screen = new Screen(float32 viewport.Width, 
                                               float32 viewport.Height)

            //HUD initialization
            hud <- new Hud(game, mainScreen)

            //GUI initialization
            gui.Screen <- mainScreen
            mainScreen.Desktop.Bounds <- new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
            );

            mainScreen.Desktop.Children.Add(new ElementBrowser())
        

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
                               
            base.Update(gameTime)
            

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

            base.Draw(gameTime)
            

    let game = new XnaGame()
    game.Run()