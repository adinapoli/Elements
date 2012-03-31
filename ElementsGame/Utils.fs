namespace Elements

open Nuclex.UserInterface

module Utils =
    let swap(a,b) = (b,a)
    let us (f : float32) : UniScalar = new UniScalar(f)
