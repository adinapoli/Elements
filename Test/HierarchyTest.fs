namespace Test

open NaturalSpec
open Elements

module HierarchyTest = 

    [<Scenario>]
    let ``Simple mix search``() =
        let rels = new Element.ElementRelations()
        let mix = ("earth", "wind")
        Given mix
        |> When calculating rels.FindMix
        |> It should equal (Some "sand")
        |> Verify

    [<Scenario>]
    let ``Simple mix not found``() =
        let rels = new Element.ElementRelations()
        let mix = ("doesnot", "exist")
        Given mix
        |> When calculating rels.FindMix
        |> It should equal None
        |> Verify
    

    

