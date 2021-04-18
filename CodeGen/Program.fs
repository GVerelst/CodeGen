open System.IO
open Generator
open Segments
open System.Text

[<EntryPoint>]
let main argv =
    System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)

    let output = @"C:\temp\Domain.cs"
    let f = File.CreateText output

    generate prog
        |> List.iter (fun s -> 
                            printfn "%s" s
                            f.WriteLine s       
                     )

    f.Close()

    0
