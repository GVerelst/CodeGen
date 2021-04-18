module Strings

let leftOf (s: string) (c: char) =
    let ndx = s.IndexOf(c)
    if ndx = -1 
       then s
       else s.Substring(0, ndx)

let normalizeName n = leftOf n '-'

let toCsv = List.reduce (fun a b -> a + ", " + b)

let captitalize (s:string) =
    if s.Length = 0 then ""
    else s.Substring(0,1).ToUpper() + s.Substring(1) 
