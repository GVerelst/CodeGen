module Generator
open Codebase
open Segments
open Errorcodes
open Strings

let outputLength (l: Length) =
    sprintf "%d%A" l.length l.ltype

// EFactMetadata("312", "449", "352-800", "Dutch", "French") 
let outputEFactMetadata zone pos =

    let nl = captitalize zone.nl
    let fr = captitalize zone.fr
    let rng = sprintf "%d-%d" pos (pos + zone.length.length - 1)
    sprintf "EFactMetadata(\"%s\", \"%s\", \"%s\", \"%s\", \"%s\")" zone.zone (outputLength zone.length) rng nl fr 

// public string Reserve9 { get; set; } = new string(' ', 449);
let outputDeclaration zone =
    let (dt, att) = match zone.datatype with
                     | CRC -> ("byte", "FieldTrim(TrimMode.Both)")
                     | Int -> ("int", if (zone.length.ltype = LengthType.S )
                                         then sprintf "FieldConverter(typeof(SignedIntConverter), %d)" zone.length.length
                                         else "FieldAlign(AlignMode.Right, '0')")
                     | DateTime -> ("DateTime?", "FieldConverter(typeof(DateConverter), \"yyyyMMdd\")")
                     | Time -> ("DateTime?", "FieldConverter(typeof(DateConverter), \"hhmm\")")
                     | String -> ("string", "FieldTrim(TrimMode.Both)")
                     | Money -> ("decimal", sprintf "FieldConverter(typeof(MoneyConverter), %d)" zone.length.length)
                     | Error -> ("byte", "FieldAlign(AlignMode.Right, '0')")
                     | Byte -> ("byte", "FieldAlign(AlignMode.Right, '0')")
                     | Short -> ("short", "FieldAlign(AlignMode.Right, '0')")
                     | Bool -> ("bool", "FieldConverter(typeof(BoolIntConverter))")
                     | AmbHos -> ("AmbHos", sprintf "FieldConverter(typeof(EnumIntConverter),%d)" zone.length.length)
                     | Gender -> ("Gender", "FieldConverter(typeof(EnumIntConverter),1)")

    (sprintf "public %s %s { get; set; }" dt zone.name, att)

let outputDefaultValue zone =
    if zone.defaultvalue = "" then 
        match (zone.length.ltype, zone.datatype) with 
        | (N, String) -> sprintf " = new string ('0', %d);" zone.length.length
        | _ -> ""
    else " = " + zone.defaultvalue + ";"

// [EFactMetadata("312", "449", "352-800", "Reserve", "Reserve"), FieldFixedLength(450), FieldValueDiscarded] public string Reserve9 { get; set; } = new string(' ', 449);
let outputZone pos zone  =
    let (declaration, att3) = outputDeclaration zone
    let att1 = outputEFactMetadata zone pos
    let att2 = sprintf "FieldFixedLength(%d)" zone.length.length

    let atts = [ att1; att2; att3 ] |> toCsv
    let comment = if zone.comments.Length = 0 then "" else (C2 zone.comments)

    "[" + atts + "] " + declaration + (outputDefaultValue zone) + " " + comment

let outputSegment start (seg: Segment)  =
    let (endpos, lines) = 
        seg.zones |> List.fold (fun (pos, lines) z -> 
            let z2 = outputZone pos z
            (pos + z.length.length, z2::lines)
                                    ) (start, [])

    let zs2 = (C2 ("Segment " + seg.name)) :: (lines |> List.rev)

    (endpos, zs2)

let outputRecord _ (rcrd: Record) =
    let (baselist, start) = match rcrd.inherits with
                            | None -> ([], 1)
                            | Some x ->
                                let st = if x.name <> recordWithCRC.name then 351 else 1    // correct in most cases
                                ([x.name], st)

    let baseclass =  (rcrd.implements |> List.map (fun x -> x.name)) |> List.append baselist

    let inheritancelist = if List.isEmpty baseclass then ""
                          else ":" + (baseclass |> toCsv)

    let (_, ls) = 
        rcrd.segments 
            |> List.fold (fun (pos, lines) s ->
                let (endpos, lines2) = outputSegment pos s
                (endpos, List.append lines lines2)
            ) (start, [])

    [
        [
            ""
            C2 "//////////////////////////////////////"
            C2 rcrd.name
            C2 "//////////////////////////////////////"
            ""
            "[FixedLengthRecord()]"
            "public partial class " + rcrd.name + inheritancelist
            "{"
        ]
        ls
        rcrd.implements |> List.map (fun interf -> interf.lines) |> List.concat
        [
            "}"
        ]
    ] |> List.concat

let outputNamespace bns ns =
    [
        [
            C2 "//////////////////////////////////////"
            C2 bns + ns.name
            C2 "//////////////////////////////////////"
            "namespace " + bns + ns.name
            "{"
        ];
        (ns.records |> List.map (outputRecord 1)) |> List.concat;
        [
            "}"
            ""
            ""
        ]
    ] |> List.concat

let generate prg =
    let header =
        [
            C2 "///////////////////////////////////////////////////////////////////////////////////////"
            C2 "Generated code"
            C2 "Generated by F# code generator"
            C2 "The definitions for the output of this file are in the F# code generator"
            C2 "///////////////////////////////////////////////////////////////////////////////////////"
            ""
            ""
            "using FileHelpers;"
            "using HdmpCloud.eHealth.eFact.Serializer.Attributes;"
            "using HdmpCloud.eHealth.eFact.Serializer.FieldConverters;"
            "using HdmpCloud.eHealth.eFact.Serializer.Models;"
            "using System;"
            "using System.Collections.Generic;"
            "using HdmpCloud.eHealth.eFact.Serializer.Recordformats.Requests;"
            ""
        ]

    let nss = prg.namespaces |> List.map (fun ns -> outputNamespace prg.baseNamespace ns)
                             |> List.concat

    [header; nss; genErrorcodes] |> List.concat
