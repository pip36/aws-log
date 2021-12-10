namespace Logic

open Utils

type Color =
    | Default
    | Highlight

type TextToken = { Value: string; Color: Color }
type Text = list<TextToken>
type MenuItem = { Text: Text }

type Menu =
    { Items: list<string>
      SelectedItem: int
      Filter: string }

module DisplayMenu =

    let createMenu lines =
        { Items = lines
          SelectedItem = 0
          Filter = "" }

    let moveDown menu =
        { menu with SelectedItem = clamp 0 (menu.Items.Length - 1) (menu.SelectedItem + 1) }

    let moveUp menu =
        { menu with SelectedItem = clamp 0 (menu.Items.Length - 1) (menu.SelectedItem - 1) }

    let appendFilter menu filterText =
        { menu with Filter = menu.Filter + filterText }

    let getSelectedItem menu = List.item menu.SelectedItem menu.Items


    let toLines menu =
        menu.Items
        |> List.indexed
        |> List.map (fun (i, x) ->
            if i = menu.SelectedItem then
                [ { Value = x; Color = Highlight } ]
            else
                [ { Value = x; Color = Default } ])
