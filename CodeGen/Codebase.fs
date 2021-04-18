module Codebase
open Strings

type Datatypes = Bool | CRC | Byte | Short | Int | DateTime | Time | String | Money | AmbHos | Gender | Error 
type LengthType = A | N | S
type Length = { ltype: LengthType; length: int }
type Zone = { zone: string; length: Length; nl: string; fr: string; datatype: Datatypes; name: string; defaultvalue: string; comments: string}
type Segment = { name: string; zones: Zone list }
type Interface = { name: string; lines: string list }
type Record = { name: string; inherits: Record option; implements: Interface list; segments: Segment list }
type Namespace = { name: string; records: Record list }
type Domain = { filename: string; baseNamespace: string; namespaces: Namespace list }

let N x = { ltype= N; length= x }
let A x = { ltype= A; length= x }
let S x = { ltype= S; length= x }

let matchType lt =
    match lt with
    | A -> String
    | N -> Int
    | S -> Int

let Z zone length nl fr dt name dft comments =
    { zone=zone; length = length; nl = nl; fr = fr; datatype=dt; name=name; defaultvalue=dft; comments=comments }

let Errorcode zone =
    let nzone = normalizeName zone
    Z zone (N 2) "Code fout" "Code érreur" Error ("Error" + nzone) "0" ""

let Reserved l zone =
    let nzone = normalizeName zone
    let dft = match l.ltype with
                | A -> sprintf "new string(' ', %d)" l.length 
                | N -> sprintf "new string('0', %d)" l.length 
                | S -> sprintf "'+' + new string('0', %d)" (l.length - 1)
    Z zone l "Reserve" "Reserve" String ("Reserved" + nzone) dft ""

let Recordtype rectype zone = 
    let rt = rectype.ToString()
    Z zone (N 2) ("recordtype " + rt) ("enregistrement de type " + rt) Byte "Recordtype" rt ("Always " + rt);

let Mutuality zone =
    Z zone (N 3) "Nummer mutualiteit" "Numéro de Mutualité" Int "MutualityNumber" "" ""

let Mutuality2 zone =
    Z zone (N 3) "Nummer mutualiteit" "Numéro de Mutualité" Int "MutualityNumber2" "" ""

let Rejectionletter zone name =
    Z zone (A 1) "Verwerpingsletter 1" "" String name "" ""
let Rejectioncode zone name =
    Z zone (A 6)  "Verwerpingscode 1"  "" String name "" ""

let segment1 x = 
    { 
        name= "segment1-2"; 
        zones= 
        [
            Recordtype x "1"
            Z "2" (N 6) "volgnummer record" "numero d'ordre de l'enregistrement" Int "SequenceNr" "1" ""
        ] 
    }

let segmentCRC =
    { 
        name= "CRC"; 
        zones= 
        [
            Z "99" (N 2) "controle cijfer record" "chiffres de controle de l'enregistrement" CRC "RecordCRC" "" ""
        ] 
    }

let C2 s = "// " + s
