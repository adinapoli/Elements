namespace Test

open NaturalSpec
open Elements

module HierarchyTest = 

    [<Scenario>]
    let ``Simple hierarchy istantiation``() =
        let rels = new Element.ElementRelations()
        let mix = ("earth", "wind")
        Given mix
        |> When calculating rels.FindMix
        |> It should equal (Some "sand")
        |> Verify

