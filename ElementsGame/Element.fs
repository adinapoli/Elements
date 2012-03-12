namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Elements.SmartSprite
open Elements.Components
open Elements.Entities
open Elements.Utils
open System.Collections.Generic

module Element =


    (***************************************************************************
     *
     * ELEMENT SPRITE
     *
     **************************************************************************)
    type ElementSprite(game: Game, spritePath : string) =
        inherit SmartSprite(game, spritePath)
        member this.Update = (this :> IGameComponent).Update
        member this.Id = (this :> IGameComponent).Id

        interface IGameComponent with
            member this.Update = this.Draw
            member this.Id = (spritePath.Split '/') |> (fun x -> x.[x.Length-1])

    /// Type synonym for a better understanding.
    type ElementMix = string * string
    type ElementName = string
    

    (***************************************************************************
     *
     * ELEMENT RELATIONS
     *
     **************************************************************************)
    type ElementRelations() =
    /// This dictionary keeps the state about element creation.
        let mutable relations_:Dictionary<ElementMix, ElementName> = Dictionary()

        // Data entry. This will be moved in an external source.
        let LoadData = do
            relations_.Add(("wind", "earth"), "sand")

        member this.Relations with get() = relations_
    

        /// Find wheter two elements, given a pair of names, form a brand
        /// new element. Return a result wrapped into the option monad.
        member this.FindMix(elemPair: ElementMix): ElementName option =
            try
                Some(relations_.[elemPair])
            with
              | :? KeyNotFoundException -> 
                    let swappedPair = swap elemPair in
                        try
                            Some(relations_.[swappedPair])
                        with
                          | :? KeyNotFoundException -> None



    (***************************************************************************
     *
     * ELEMENT
     *
     **************************************************************************)
    /// This class model a domain object, the Element.
    /// Element is a generic element that can be combined.
    type Element(game: Game, name : string, x: int32, y: int32) =
        inherit GameEntity(name + "Entity")
        
        static let assetPath:string = "Media/Assets/"
        let mutable elementSprite_:ElementSprite = new ElementSprite(game, assetPath + name)
        do
            elementSprite_.X <- x
            elementSprite_.Y <- y
            printf "%d" elementSprite_.X
        let elementName_:ElementName = name
        let mutable selected_:bool = false

        member this.Sprite with get() = elementSprite_
        member this.Name with get() = elementName_
        member this.IsSelected 
            with get() = selected_
            and  set s = selected_ <- s
        member this.Move(x : int32, y : int32) =
            elementSprite_.X <- x
            elementSprite_.Y <- y

        override this.Update = 
            let mouseState = Mouse.GetState()
            match mouseState.LeftButton = ButtonState.Pressed with
            | true -> 
                let collided = this.Sprite.Bounds.Contains(mouseState.X, mouseState.Y)
                in match collided with
                    | true ->  selected_ <- true
                    | false -> ()
            | false -> selected_ <- false

            //Update the sprite
            this.Sprite.Update


    type ElementsManager() =
        
        inherit EntitiesManager() 

        member this.FindSelectedElement: Element option =
            let pred = (fun (e:GameEntity) -> (e :?> Element).IsSelected)
            let res = List.filter pred this.Entities
            match res with
                | head :: tail -> Some((head :?> Element))
                | [] -> None


        override this.Update : unit =

            //Find if at least one element is selected
            match this.FindSelectedElement with
                | Some(e) -> e.Move(Mouse.GetState().X, Mouse.GetState().Y)
                | None    -> ()
            
            //Updates components accordingly
            List.iter (fun (e:GameEntity) -> e.Update) this.Entities