module Dotenv

open System
open System.Text.RegularExpressions

let Configure () =
    let envFile = ".env"

    if IO.File.Exists(envFile) then
        IO.File.ReadAllLines(envFile)
        |> Seq.toList
        |> List.iter (fun x ->
            let line = Regex.Replace(x, "#.*", "")

            match line.Split([| '=' |]) |> Seq.toList with
            | [ key; value ] ->
                if String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(key)) then
                    Environment.SetEnvironmentVariable(key, value)
                else
                    ()
            | _ -> ())
    else
        raise (Exception("no env file found"))
