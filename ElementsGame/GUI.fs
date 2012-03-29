namespace Elements

open System
open Nuclex.UserInterface
open Nuclex.UserInterface.Controls
open Nuclex.UserInterface.Controls.Desktop
open Microsoft.Xna.Framework.Input

module GUI =

    
    let us (f : float32) : UniScalar = new UniScalar(f)
    
    type ElementBrowser() as this =
        inherit WindowControl()

        do
            this.EnableDragging <- false
            this.Bounds <- new UniRectangle(us 600.0f, us 10.0f, 
                                            us 512.0f, us 600.0f)
            this.Title <- "Element Browser"
            let okButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl()
            let cancelButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl()
            okButton.Bounds <- new UniRectangle(new UniScalar(1.0f, -180.0f), 
                                                new UniScalar(1.0f, -40.0f), 
                                                us 80.0f, us 24.0f)
            okButton.Text <- "Ok"
            okButton.ShortcutButton <- new Nullable<Buttons>(Buttons.A)
            okButton.Pressed.AddHandler((fun sender args -> this.Close()))
     
            cancelButton.Bounds <- new UniRectangle(new UniScalar(1.0f, -90.0f), 
                                                    new UniScalar(1.0f, -40.0f), 
                                                    us 80.0f, us 24.0f)
            cancelButton.Text <- "Cancel"
            cancelButton.ShortcutButton <- new Nullable<Buttons>(Buttons.B)
            cancelButton.Pressed.AddHandler((fun sender args -> this.Close()))


            this.Children.Add(okButton)
            this.Children.Add(cancelButton)