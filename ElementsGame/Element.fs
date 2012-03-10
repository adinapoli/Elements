namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Elements.SmartSprite
open Elements.Utils
open System.Collections.Generic

module Element =

    type ElementSprite(game: Game, spritePath : string) =
        inherit SmartSprite(game, spritePath)


    type ElementMix = string * string
    type ElementName = string
    /// This dictionary keeps the state about element creation.
    
    type ElementRelations() =
        static let mutable relations_:Dictionary<ElementMix, ElementName> = Dictionary()

        // Data entry. This will be moved in an external source.
        let LoadData = do
            relations_.Add(("wind", "earth"), "sand")

        static member Relations with get() = relations_
    

        member this.FindMix(elemPair: ElementMix): ElementName option =
            match relations_.ContainsKey(elemPair) with
            | true -> Some(relations_.[elemPair])
            | false -> let swappedPair = swap elemPair in
                       match relations_.ContainsKey(swappedPair) with
                        | true -> Some(relations_.[swappedPair])
                        | false -> None


    /// This class model a domain object, the Element.
    /// Element is a generic element that can be combined.
    type Element(game: Game, name : string) =
        static let assetPath:string = "Media/Assets/"
        let mutable elementSprite_:ElementSprite = new ElementSprite(game, assetPath + name)
        let elementName_:ElementName = name

        member this.Sprite with get() = elementSprite_
        member this.Name with get() = elementName_