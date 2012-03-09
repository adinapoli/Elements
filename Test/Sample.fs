module Test.NaturalSpecSample

open NaturalSpec

[<Scenario>]
let ``After removing 3 from a list it should not contain 3``() =
  Given [1;2;3;4;5]              // Arrange test context
    |> When removing 3           // Act
    |> It shouldn't contain 3    // Assert
    |> It should have (length 4) // Assert      
    |> It should contain 4       // Assert
    |> Verify                   // Verify scenario


[<Scenario>]
let ``square(2) equals 4``() =
  Given 2
    |> When calculating (fun x -> x*2)
    |> It should equal 4
    |> Verify