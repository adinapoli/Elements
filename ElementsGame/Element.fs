namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Elements.Components
open Elements.Entities
open Elements.Prefabs
open Elements.Utils
open System.Collections.Generic

module Element =


    (***************************************************************************
     *
     * ELEMENT SPRITE
     *
     **************************************************************************)
    type ElementSprite(game: Game, id : string) =
        inherit SmartSprite(game, "Media/Assets/" + id)
        
        let id_ = id
        member this.Update = (this :> IGameComponent).Update
        member this.Id = (this :> IGameComponent).Id

        interface IGameComponent with
            member this.Update = this.Draw
            member this.Id = id_

    /// Type synonym for a better understanding.
    type ElementMix = string * string
    type ElementName = string

    (***************************************************************************
     *
     * ELEMENT CAPTION
     *
     **************************************************************************)
     type ElementCaption(id : string, game: Game, fontName : string) as this =
        inherit TextComponent(id, game, fontName)

        member this.Move(x : int32, y : int32) =
            this.X <- x
            this.Y <- y + 64
    

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
    type Element(game: Game, name : string, x: int32, y: int32) as this =
        inherit GameEntity(name + "Entity")
        
        do
            let elementSprite_:ElementSprite = new ElementSprite(game, name)
            let elementCaption_:ElementCaption = new ElementCaption("eCaption", game, "ElementCaption")
            elementCaption_.Text <- name
            elementSprite_.Move(x,y)
            elementCaption_.Move(x, y)
            this.Attach(elementSprite_)
            this.Attach(elementCaption_)
        
        let elementName_:ElementName = name
        let mutable selected_:bool = false

        member this.Sprite 
            with get() = (this.ComponentsByType("sprite") |> Seq.head) :?> ElementSprite
        member this.Name with get() = elementName_
        
        member this.IsSelected 
            with get() = selected_
            and  set s = selected_ <- s
        
        member this.Move(x : int32, y : int32) =
            let fn = (fun (c :IGameComponent) -> (c :?> IMovable).Move(x,y))
            Seq.iter fn this.Components.Values

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
            Seq.iter (fun (e:IGameComponent) -> e.Update) this.Components.Values


    (***************************************************************************
     *
     * ELEMENTS MANAGER
     *
     **************************************************************************)
    /// This is who ACTUALLY manages elements.
    type ElementsManager() =
        
        inherit EntitiesManager() 

        member this.FindSelectedElement: Element option =

            //Mental note, :?> performs a DYNAMIC cast, resolved at run-time
            let pred = (fun (e:GameEntity) -> (e :?> Element).IsSelected)
            let res = List.filter pred this.Entities
            match res with
                | head :: tail -> Some((head :?> Element))
                | [] -> None


        override this.Update : unit =

            // Find if at least one element is selected
            // If yes, move it (it will follow the mouse)
            // 32 is 64/2, where 64 is a element side (64 * 64 tile)
            match this.FindSelectedElement with
                | Some(e) -> e.Move(Mouse.GetState().X - 32, 
                                    Mouse.GetState().Y - 32)
                | None    -> ()
            
            //Updates components accordingly
            List.iter (fun (e:GameEntity) -> e.Update) this.Entities