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

    let getItems menu =
        menu.Items
        |> List.filter (fun x -> x.ToLower().Contains(menu.Filter.ToLower()))

    let moveDown menu =
        { menu with SelectedItem = clamp 0 ((getItems menu).Length - 1) (menu.SelectedItem + 1) }

    let moveUp menu =
        { menu with SelectedItem = clamp 0 ((getItems menu).Length - 1) (menu.SelectedItem - 1) }

    let appendFilter menu filterText =
        { menu with
            Filter = menu.Filter + filterText
            SelectedItem = 0 }

    let popFilter menu =
        { menu with
            Filter = menu.Filter.[0..menu.Filter.Length - 2]
            SelectedItem = 0 }

    let getSelectedItem menu = List.item menu.SelectedItem menu.Items

    let toLines menu =
        getItems menu
        |> List.indexed
        |> List.map (fun (i, x) ->
            if i = menu.SelectedItem then
                [ { Value = x; Color = Highlight } ]
            else
                [ { Value = x; Color = Default } ])
