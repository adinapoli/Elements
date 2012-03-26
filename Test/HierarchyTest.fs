namespace Test

open NaturalSpec
open Elements

module HierarchyTest = 

    [<Scenario>]
    let ``Simple mix search``() =
        let rels = new Element.ElementRelations()
        rels.LoadData
        let mix = ("earth", "air")
        Given mix
        |> When calculating rels.FindMix
        |> It should equal (Some "sand")
        |> Verify


    [<Scenario>]
    let ``Another mix search``() =
        let rels = new Element.ElementRelations()
        rels.LoadData
        let mix = ("sand", "air")
        Given mix
        |> When calculating rels.FindMix
        |> It should equal (Some "sandstorm")
        |> Verify

    [<Scenario>]
    let ``Simple mix not found``() =
        let rels = new Element.ElementRelations()
        let mix = ("doesnot", "exist")
        Given mix
        |> When calculating rels.FindMix
        |> It should equal None
        |> Verify
    

    

