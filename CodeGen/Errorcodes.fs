module Errorcodes

open FSharp.Interop.Excel

// Let the type provider do it's work (at compile time)
type Errorlist = ExcelFile<"./Data/Errorcodes.csv">

let fixString (s:string) = s.Replace("\"", "\\\"")

let genErrorcodes =
    let file = new Errorlist()

    let errors = file.Data
                    |> Seq.filter (fun x -> x.``Code erreur/Foutcode`` <> null)
                    |> Seq.map (fun x -> 
                        [
                            sprintf "[\"%s\"] = new Error " x.``Code erreur/Foutcode``
                            "{"
                            sprintf "Rejectionletter = \"%s\"," x.``Nature erreur/Aard fout``
                            sprintf "Rejectioncode = \"%s\"," x.``Code erreur/Foutcode``
                            sprintf "ErrorNL = \"%s\"," (fixString x.Omschrijving)
                            sprintf "ErrorFR = \"%s\"" (fixString x.Libellé)
                            "},"
                        ]
                        )

    let classDeclaration = 
         [
             "namespace HdmpCloud.eHealth.eFact.Serializer.Recordformats"
             "{"
             "internal static class Errorcodes"
             "{"
             "    private static readonly IDictionary<string, Error> _errors = new Dictionary<string, Error>"
             "    {"
         ]
    let classRest =
         [
             "    };"
             ""
             "public static Error Get(string code)"
             "{"
             "    if (_errors.TryGetValue(code, out Error err))"
             "        return err;"
             "    return new Error"
             "    {"
             "        Rejectionletter = \"?\","
             "        Rejectioncode = code,"
             "        ErrorNL = \"Onbekende errorcode.\","
             "        ErrorFR = \"Code érreur inconnu.\""
             "    };"
             "}"
             "}"
             "}"
         ]

    List.concat 
        [
            classDeclaration
            errors |> List.concat
            classRest
        ]
