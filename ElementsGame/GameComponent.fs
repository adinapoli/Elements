namespace Elements

    module Components =
        
        type IGameComponent =
            abstract member Update : unit
            abstract member Id : string
    
    
    module Entities =

        open Components


        /// A base entity class.
        /// Maintain a list of components, that can be retrieved, updated,
        /// removed, etc 
        [<AbstractClass>]
        type GameEntity(id : string) =

            let id_ = id
            let mutable components_: (IGameComponent list) = []
            member this.Id with get() = id_
            member this.Components with get() = components_
            member this.Attach(c : IGameComponent):unit = 
                (c :: components_) |> ignore

            abstract member Update : unit

