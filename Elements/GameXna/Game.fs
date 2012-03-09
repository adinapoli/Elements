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
        let mutable ball = Unchecked.defaultof<ElementObject>
        let mutable background = Unchecked.defaultof<SmartSprite>
        let mutable dx = 4.f
        let mutable dy = 4.f

        override game.Initialize() =
            graphicsDeviceManager.GraphicsProfile <- GraphicsProfile.HiDef
            graphicsDeviceManager.PreferredBackBufferWidth <- 1260
            graphicsDeviceManager.PreferredBackBufferHeight <- 762
            graphicsDeviceManager.IsFullScreen <- false
            graphicsDeviceManager.ApplyChanges() 
            ball <- new ElementObject(game, "Sprite")
            background <- new SmartSprite(game, "Media/Backgrounds/bg_lab_1200_800")
            base.Initialize()
        
        override game.Update gameTime = 
            if ball.X > 608.f || ball.X < 0.f then dx <- -dx
            if ball.Y > 448.f || ball.Y < 0.f then dy <- -dy
        
            ball.X <- ball.X + dx
            ball.Y <- ball.Y + dy


        override game.Draw gameTime = 

            background.Draw
            ball.Draw

    let game = new XnaGame()
    game.Run()