module Utils

let clamp minimum maximum value = value |> max minimum |> min maximum
