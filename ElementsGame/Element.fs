namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
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

        member this.Sprite with get() = elementSprite_
        member this.Name with get() = elementName_

        override this.Update = this.Sprite.Update


    (***************************************************************************
     *
     * ELEMENT MANAGER
     *
     **************************************************************************)
    /// Who uses the Elements.
    type EntitiesManager() =
        
        // Using standard "slow" purely functional lists,
        // skipping premature optimization
        let mutable entities_ : (GameEntity list) = []

        member this.Entities with get() = entities_
        member this.Attach(entity : GameEntity) : unit =
            entities_ <- entity :: entities_

        member this.Update : unit =
            List.iter (fun (e:GameEntity) -> e.Update) entities_