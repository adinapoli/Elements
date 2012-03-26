namespace Elements

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Elements.Components
open Elements.Entities
open Elements.Prefabs
open Elements.Utils
open System.Collections.Generic
open System.Xml.XPath

module Element =


    (***************************************************************************
     *
     * ELEMENT SPRITE
     *
     **************************************************************************)
    type ElementSprite(game: Game, id : string) =
        inherit SmartSprite(game, "Media/Assets/" + id)

    /// Type synonym for a better understanding.
    type ElementMix = string * string
    type ElementName = string



    (***************************************************************************
     *
     * ELEMENT CAPTION
     *
     **************************************************************************)
     type ElementCaption(id : string, game: Game, fontName : string) =
        inherit TextComponent(id, game, fontName)

        interface IMovable with
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
        member this.LoadData = do
            let doc = XPathDocument(@"XnaGameContent\Relations.xml").CreateNavigator()
            let relations = doc.Select("//relation")

            while(relations.MoveNext()) do
                relations.Current.MoveToFirstChild() |> ignore
                let e1 = relations.Current.Value
                relations.Current.MoveToNext() |> ignore
                let e2 = relations.Current.Value
                relations.Current.MoveToNext() |> ignore
                let e3 = relations.Current.Value
                relations_.Add((e1, e2), e3)

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
        inherit GameEntity(name)
        
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


        member this.CollideWith(e : Element) =
            match (this.Sprite.IsVisible && e.Sprite.IsVisible) with
                | true  -> this.Sprite.Bounds.Intersects(e.Sprite.Bounds)
                | false -> false


        override this.Update (gameTime : GameTime) = 
            let mouseState = Mouse.GetState()
            match mouseState.LeftButton = ButtonState.Pressed with
            | true -> 
                let collided = this.Sprite.Bounds.Contains(mouseState.X, mouseState.Y)
                in match collided with 
                    | true ->  selected_ <- true
                    | false -> ()
            | false -> selected_ <- false

            //Update the sprite
            Seq.iter (fun (e:IGameComponent) -> e.Update gameTime) this.Components.Values

        
        //Tries to call the draw function for every component.
        //Of course, this can fail, since not every IGameComponent must
        //implement also the IDrawable interface. If some cast fails,
        //just ignore it.
        override this.Draw (gameTime : GameTime) =
            for (e:IGameComponent) in this.Components.Values do
                try
                    (e :?> IDrawable).Draw gameTime
                with
                //If the cast fails, ignore it.
                | :? System.InvalidCastException -> ()



    (***************************************************************************
     *
     * ELEMENTS MANAGER
     *
     **************************************************************************)
    /// This is who ACTUALLY manages elements.
    type ElementsManager(game : Game) =
        inherit EntitiesManager()

        let game_:Game = game
        let elementRelations_:ElementRelations = new ElementRelations()
        do elementRelations_.LoadData
        let mutable lastSelectedElement_: Element option = None
        let mutable discoveredElements_:int = 4

        member this.Game = game_

        member this.DiscoveredElements 
            with get() = discoveredElements_

        member this.FindSelectedElement: Element option =
            //Mental note, :?> performs a DYNAMIC cast, resolved at run-time
            let pred = (fun (e:GameEntity) -> (e :?> Element).IsSelected)
            let res = Seq.filter pred this.Entities.Values |> Seq.toList
            match res with
                | head :: tail -> Some((head :?> Element))
                | [] -> None


        override this.Update (gameTime : GameTime) : unit =
            // Find if at least one element is selected
            // If yes, move it (it will follow the mouse)
            // 32 is 64/2, where 64 is a element side (64 * 64 tile)
            match this.FindSelectedElement with
                | Some(e) -> 
                    do
                        e.Move(Mouse.GetState().X - 32, 
                               Mouse.GetState().Y - 32)
                        lastSelectedElement_ <- Some(e)
                | None    -> 
                    match lastSelectedElement_ with
                        | Some(ls) ->
                             //Check collision with any other element
                             //To determine whether a new element must be added.
                             let pred = (fun (e:GameEntity) -> 
                                             (e :?> Element).CollideWith(ls))
                             let collidedElements = this.Entities.Values
                                                    |> Seq.filter (fun e -> e.Id.Equals(ls.Id) |> not)  
                                                    |> Seq.filter pred
                                                    |> Seq.toList
                             let attachFn = (fun (e:GameEntity) ->
                                                match elementRelations_.FindMix (e.Id, ls.Id) with
                                                    |Some(res) -> 
                                                        let x = ls.Sprite.X
                                                        let y = ls.Sprite.Y
                                                        //Remove matched elements
                                                        (e :?> Element).Sprite.IsVisible <- false
                                                        ls.Sprite.IsVisible <- false
                                                        this.Detach(e.Id)
                                                        this.Detach(ls.Id)
                                                        this.Attach(new Element(game_, res, x, y))
                                                        discoveredElements_ <- discoveredElements_ + 1
                                                    |None -> ())
                             List.iter attachFn collidedElements
                         | None -> ()
            
            //Updates components accordingly
            Seq.iter (fun (e:GameEntity) -> e.Update gameTime) this.Entities.Values