open System
open Services.AWSCloudwatch
open Logic
open Logic.DisplayMenu

type InputActions =
    | MoveDown
    | MoveUp
    | SelectItem
    | Escape
    | TypeText of string
    | Nothing

let waitForAction () =
    let x = Console.ReadKey()

    match x.Key with
    | ConsoleKey.UpArrow -> MoveUp
    | ConsoleKey.DownArrow -> MoveDown
    | ConsoleKey.Enter -> SelectItem
    | ConsoleKey.Escape -> Escape
    | k when int k >= 65 && int k <= 90 -> TypeText(k.ToString().ToLower())
    | _ -> Nothing


let clearConsole () = Console.Clear()

let white (text: string) =
    Console.ForegroundColor <- ConsoleColor.Gray
    Console.WriteLine(text)
    Console.ResetColor()

let green (text: string) =
    Console.ForegroundColor <- ConsoleColor.Green
    Console.WriteLine(text)
    Console.ResetColor()

let display (lines: list<list<TextToken>>) =
    for line in lines do
        for token in line do
            match token.Color with
            | Highlight -> green token.Value
            | Default -> white token.Value

let displayMenu menu =
    clearConsole ()
    printfn "Log Groups"
    printfn "__________"
    printfn "Filter: %s" menu.Filter
    let lines = toLines menu
    display lines

type Screens =
    | ViewLogGroups
    | ViewLogs

[<EntryPoint>]
let main argv =
    Dotenv.Configure()
    Console.CursorVisible <- false
    printfn "Fetching Log groups..."

    let logGroupNames =
        getLogGroups |> List.map (fun x -> x.Name)

    let mutable screen = ViewLogGroups
    let mutable menu = createMenu logGroupNames

    let mutable running = true

    while running do
        displayMenu menu

        match screen with
        | ViewLogGroups ->
            menu <-
                match waitForAction () with
                | MoveUp -> moveUp menu
                | MoveDown -> moveDown menu
                | TypeText text -> appendFilter menu text
                | Escape ->
                    running <- false
                    menu
                | SelectItem ->
                    screen <- ViewLogs

                    let logs =
                        menu
                        |> getSelectedItem
                        |> getLogEvents
                        |> List.map (fun x -> x.Message)

                    createMenu logs
                | Nothing -> menu

            ()
        | ViewLogs ->
            menu <-
                match waitForAction () with
                | MoveUp -> moveUp menu
                | MoveDown -> moveDown menu
                | TypeText text -> menu
                | Escape ->
                    screen <- ViewLogGroups
                    createMenu logGroupNames
                | SelectItem -> menu
                | Nothing -> menu


    0 // return an integer exit code
