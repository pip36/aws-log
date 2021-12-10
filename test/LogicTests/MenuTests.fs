module Tests

open Logic
open Logic.DisplayMenu
open Xunit

[<Fact>]
let ``Can't moveUp above the first menu item `` () =
    let menu = createMenu [ "one"; "two" ]
    let result = moveUp menu
    Assert.Equal(0, result.SelectedItem)

[<Fact>]
let ``Can't moveDown below the last menu item `` () =
    let menu =
        { Items = [ "one"; "two" ]
          SelectedItem = 1
          Filter = "" }

    let result = moveDown menu
    Assert.Equal(1, result.SelectedItem)

[<Fact>]
let ``Can moveDown`` () =
    let menu =
        { Items = [ "one"; "two" ]
          SelectedItem = 0
          Filter = "" }

    let result = moveDown menu
    Assert.Equal(1, result.SelectedItem)

[<Fact>]
let ``Can moveUp`` () =
    let menu =
        { Items = [ "one"; "two" ]
          SelectedItem = 1
          Filter = "" }

    let result = moveUp menu
    Assert.Equal(0, result.SelectedItem)
