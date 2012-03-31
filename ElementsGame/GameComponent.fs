namespace Elements

open Microsoft.Xna.Framework

    module Components =

        type IMovable =
            abstract member Move : x:int32 * y:int32 -> unit

        type IDrawable =
            abstract member Draw : GameTime -> unit

        type IGameHandler =
            abstract member Handle : unit
        
        type IGameComponent =
            abstract member Update : GameTime -> unit
            abstract member Id : string
            abstract member Type : string
    
    
    module Entities =

        open Components
        open System.Collections.Generic

        /// A base entity class.
        /// Maintain a list of components, that can be retrieved, updated,
        /// removed, etc 
        [<AbstractClass>]
        type GameEntity(id : string) =

            let id_ = id
            let mutable components_: (Dictionary<string,IGameComponent>) = Dictionary()
            member this.Id with get() = id_
            member this.Components with get() = components_
            member this.Attach(c : IGameComponent):unit = 
                components_.Add(c.Id, c)


            member this.Component(cid : string): IGameComponent option =
                try
                    Some(components_.[cid])
                with
                    | :? KeyNotFoundException -> None


            member this.ComponentsByType(tp : string) =
                let fn = (fun (c:IGameComponent) -> c.Type = tp)
                Seq.filter fn components_.Values   
              
              
            member this.ComponentById(id : string) : IGameComponent option =
                let fn = (fun (c:IGameComponent) -> c.Id = id)
                
                try
                    Seq.filter fn components_.Values |> Seq.head  |> Some
                with
                    | :? System.ArgumentNullException -> None 


            abstract member Update : GameTime -> unit
            abstract member Draw   : GameTime -> unit


        /// A generic Entities Manager, who uses entities
        [<AbstractClass>]
        type EntitiesManager() =
        
            // Using standard "slow" purely functional lists,
            // skipping premature optimization
            let mutable entities_ : (Dictionary<string,GameEntity>) = Dictionary()

            member this.Entities with get() = entities_
            member this.Attach(e : GameEntity) : unit = 
                entities_.Add(e.Id, e)

            member this.Detach(entityId : string) : unit =
                try 
                    entities_.Remove(entityId) |> ignore
                with 
                    | :? System.ArgumentNullException -> ()

            member this.Entity(eid : string): GameEntity option =
                try
                    Some(entities_.[eid])
                with
                    | :? KeyNotFoundException -> None

            abstract member Update : GameTime -> unit

            member this.Draw (gameTime : GameTime) =
                Seq.iter (fun (e:GameEntity) -> e.Draw gameTime) this.Entities.Values