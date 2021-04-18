module Segments

open Codebase

(*
    C# fields are Zone objects. This is the basic output unit. 
    Segments contain Zones
    Records contain Segments => C# class
    Namespaces contain Records
    Domain contains Namespaces
*)

let ierror = 
    {
        name = "IError";
        lines = 
            [
                ""
                C2 "IError implementation"
                "public IEnumerable<Error> GetErrors()"
                "{"
                "    return Error.GetErrors(Rejectionletter1, Rejectioncode1, Rejectionletter2, Rejectioncode2, Rejectionletter3, Rejectioncode3);"
                "}"
            ]
    }

let segment200 = 
    { 
        name= "segment200"; 
        zones= 
        [
            Z "200" (N 6) "Naam van het bericht" "Nom du message" Int " MessageName" "920000" "920000|920900|..."
            Errorcode "2001" 
            Z "201" (N 2) "Versienummer formaat van het bericht" "N° version du format du message" Byte " MessageVersionNumber" "" "2"
            Errorcode "2011" 
            // ...
            Reserved (N 15) "206"
        ] 
    }

let segment300 = 
    { 
        name= "segment300"; 
        zones= 
        [
            Z "300a" (N 4) "Factureringsjaar"  "Année de facturation"  Int "YearBilled" "" ""
            Z "300b" (N 2) "Factureringsmaand" "Mois de facturation"  Byte "MonthBilled" "" ""
            Errorcode "3001" 
            Z "301" (N 3) "Nummer van de verzendingen" "Numero d''envoi" Int " RequestNr" "" ""
            Errorcode "3011" 
            // ...
       ] 
    }

let segment300a = 
    { 
        name= "segment300a"; 
        zones= 
        [
            Reserved (N 20) "310"
        ] 
    }

let segment300b = 
    { 
        name= "segment300b"; 
        zones= 
        [
            Z "310" (N 5) "Foutenpercentage" "" Int "PercentageErrors" "" ""
            Errorcode "3101"
// ...
        ] 
    }

let segment400b = 
    { 
        name= "segment400b"; 
        zones= 
        [
            Z "310" (N 5) "Foutenpercentage" "" Int "PercentageErrors" "" ""
            Errorcode "3101"
            Z "311" (N 2) "Type weigering facturatie" "" Byte "RefusalType" "" ""
            Errorcode "3111"
            Reserved (A 459) "312"
        ] 
    }

// Segments for requests

let segment10 =
    { 
        name= "segment10"; 
        zones= 
        [
            Reserved (N 1) "3"
            Z "5" (N 7) "Versie bestand" "Version fichier" String "VersionFile" "\"0001999\"" "0001999 (9991999 voor een test)"
            Reserved (N 12) "5-6a"
            Reserved (N 4) "6b"
            Z "7" (N 3) "Zendingsnummer" "Numero de l'envoi" Int " MailingNumber" "" "Sequentieel, enig per jaar per dokter, <> 0 "
// ...
            Reserved (N 6) "42"
            Reserved (A 11) "43a"
            Reserved (N 5) "43b-44"
            Reserved (A 34) "45-47a"
            Reserved (N 2) "47b-48"
            Reserved (A 34) "49-52"
            Reserved (A 11) "53-54a"
            Reserved (N 33) "54b-98"
        ] 
    }

let segment20 =
    { 
        name= "segment20"; 
        zones= 
        [
            Z "3" (N 1) "toestemming derdebetalende " "autorisation tiers payant" Bool "ThirdPayingConsent" "true" "0 or 1"
            Reserved (N 7) "4"
            Reserved (N 8) "5"
// ...
            Reserved (N 4) "57"
            Reserved (N 4) "58"
            Reserved (N 8) "59"
        ] 
    }

let segment50 =
    { 
        name= "segment50"; 
        zones= 
        [
            Z "3" (N 1) "norm verstrekking (percentage) " "norme prestation (pourcentage)" Byte "PrestationPercentage" "0" ""
            Z "4" (N 7) "nomenclatuurcode of pseudo‐nomenclatuurcode " "code nomenclature ou pseudo-code nomenclature" String " PrestationCode" "" ""
            Z "5" (N 8) "datum eerste verrichte verstrekking " "date premiere prestation effectuee" DateTime "PrestationBeginDate" "" ""
            Z "6a-6b" (N 8) "datum laatste verrichte verstrekking" "date derniere prestation effectuee" DateTime "PrestationEndDate" "" "=ET 50 Z 5"
            Mutuality "7"
// ...
            Reserved (N 4) "54c"
            Reserved (N 12) "55-56"
            Reserved (N 14) "57-59"
            Reserved (N 2) "98"
        ] 
    }
 
let segment51 =
     { 
         name= "segment51"; 
         zones= 
         [
            Reserved (N 1) "3"
            Z "4" (N 7) "(Pseudo‐)nomenclatuurcode " "code nomenclature ou pseudo-code nomenclature" String " PrestationCode" "" ""
            Z "5" (N 8) "Datum verstrekking" "date prestation" DateTime " PrestationDate" "" ""
// ...
         ] 
     }

let segment52 =
    { 
        name= "segment52"; 
        zones= 
        [
            Reserved (N 1) "3"
            Z "4"  (N 7)  "(Pseudo‐)nomenclatuurcode"  "Code nomenclature ou pseudo-code nomenclature" String "PrestationCode" "" ""
            Z "5"  (N 8)  "Datum verstrekking"  "Date prestation" DateTime "PrestationDate" "" ""
            Reserved (N 11) "6a-7"
// ...
        ] 
    }

let segment80 =
    { 
        name= "segment80"; 
        zones= 
        [
            Mutuality "7"
            Z "8a-8b" (A 13) "identificatie rechthebbende" "identification beneficiaire" String " Patient_NISS" "" ""
            Z "9" (N 1) "geslacht rechthebbende " "sexe beneficiaire" Gender " Patient_Sexe" "" ""
// ...
            Z "98" (N 2) "controle cijfer factuur " "chiffres de controle de la facture" Byte " InvoiceCRC" "" "Checkdigit"
        ] 
    }

let segment90 =
    { 
        name= "segment90"; 
        zones= 
        [
            Reserved (N 8) "3-4"
            Reserved (N 12) "5-6a"
            Reserved (N 4) "6b"
            Z "7" (N 3) "zendingsnummer " "numero d'envoi" Int " MailingNumber" "" ""
// ...
            Reserved (S 8) "55"
            Reserved (N 18) "56-59"
            Z "98" (N 2) "controle cijfer zending " "chiffres de controle de l'envoi" Byte " MailingCRC" "" ""
        ]
    }

let segment91 =
    { 
        name= "segment91"; 
        zones= 
        [
            Recordtype 91 "400"
            Errorcode "4001"
            Mutuality "401"
            Errorcode "4011"
            Z "402" (A 12) "Nummer van verzamelfactuur" " " String " InvoiceNr" "" ""
// ...
        ]
    }

let segment92 =
    { 
        name= "segment92"; 
        zones= 
        [
            Recordtype 92 "500"
            Errorcode "5001"
            Mutuality "501"
// ...
        ]
    }

let segment95 =
    { 
        name= "segment95"; 
        zones= 
        [
            Recordtype 95 "400"
            Errorcode "4001"
            Mutuality "401"
            Errorcode "4011" 
            Z "402" (N 12) "Nummer van verzamelfactuur" "Numéro de facture récapitulative" String "RecapInvoiceNumber" "" ""
// ...
            Reserved (N 257) "413"
        ]
    }

let segment96 =
    { 
        name= "segment96"; 
        zones= 
        [
            Recordtype 96 "500"
            Errorcode "5001" 
            Mutuality "501"
// ...
        ]
    }

// Segments for responses from OA

let segmentResponse_identification =
    { 
        name= "segmentResponse_identification"; 
        zones= 
        [
            Z "100" (N 3) "Identificatie zending" "" Int "Identification" "" "(= R 10 Z 7)"
        ]
    }

let segmentResponse_datum_10 =
    { 
        name= "segmentResponse_datum_10"; 
        zones= 
        [
            Z "101" (N 8) "Datum aanmaak V.I." "" DateTime "Creationdate" "" ""
        ]
    }

let segmentResponse_datum =
    { 
        name= "segmentResponse_datum_10"; 
        zones= 
        [
            Z "101" (N 8) "Datum aanmaak zending" "" DateTime "Creationdate" "" ""
        ]
    }

let segmentResponse_datum_90 =
    { 
        name= "segmentResponse_datum_90"; 
        zones= 
        [
            Reserved (N 8) "101"
        ]
    }

let segmentResponse_102_103 =
    { 
        name= "segmentResponse_102_103"; 
        zones= 
        [
            Reserved (N 6) "102"
            Reserved (N 3) "103"
        ]
    }

let segmentResponse_104_110 =
    { 
        name= "segmentResponse_104_110"; 
        zones= 
        [
            Reserved (N 12) "104"
            Reserved (N 12) "105"
            Reserved (N 12) "106"
            Reserved (N 3) "107"
            Reserved (N 3) "108"
            Reserved (A 22) "109"
            Reserved (A 22) "110"
        ]
    }

let segmentResponse_Rejection =
    { 
        name= "segmentResponse_Rejection"; 
        zones= 
        [
            Rejectionletter "111a" "Rejectionletter1"
            Rejectioncode "111b"   "Rejectioncode1"
            Rejectionletter "112a" "Rejectionletter2"
            Rejectioncode "112b"   "Rejectioncode2"
            Rejectionletter "113a" "Rejectionletter3"
            Rejectioncode "113b"   "Rejectioncode3"
        ]
    }

let segmentResponse_114_151 =
    { 
        name= "segmentResponse_Rejection"; 
        zones= 
        [
            Reserved (S 12)  "114"
            Reserved (S 12)  "115"
            Reserved (S 12)  "116"
            Reserved (N 7)   "117"
            Reserved (N 1)   "118"
            Reserved (S 12)  "119"
            Reserved (A 200) "149"
            Reserved (A 61)  "150"
            Reserved (N 6)   "151"
        ]
    }

let segmentResponse_114_118 =
    { 
        name= "segmentResponse_Rejection"; 
        zones= 
        [
            Reserved (S 12)  "114"
            Reserved (S 12)  "115"
            Reserved (S 12)  "116"
            Reserved (N 7)   "117"
            Reserved (N 1)   "118"
        ]
    }

let segmentResponse_50 =
    { 
        name= "segmentResponse_50"; 
        zones= 
        [
            Z "119" (S 12) "Resultaat VI" "Résultat OA" Int "ResultOA" "" ""
            Z "149" (A 200) "Commentaar bij foutcode" "" String "CommentErrorcode" "" ""
            Reserved (A 61)  "150"
            Z "151" (N 6) "Index" "Index" Int "Index" "" ""
        ]
    }

let segmentResponse_50a =
    { 
        name= "segmentResponse_50a"; 
        zones= 
        [
            Z "102a" (N 4) "Factureringsjaar" "Année de facturation" Int " YearBilled" "" ""
            Z "102b" (N 2) "Factureringsmaand" "Mois de facturation" Byte " MonthBilled" "" ""
            Mutuality2 "103"
        ]
    }

let segmentResponse20_a =
    { 
        name= "segmentResponse20_a"; 
        zones= 
        [
            Z "100" (N 3) "Identificatie zending" "" Int "Identification" "" "(= R 10 Z 7)"
            Z "101" (N 8) "Datum aanmaak V.I." "" DateTime "Creationdate" "" ""
            Reserved (N 6) "102"
// ...
            Rejectionletter "111a" "Rejectionletter1"
            Rejectioncode "111b"   "Rejectioncode1"
            Rejectionletter "112a" "Rejectionletter2"
            Rejectioncode "112b"   "Rejectioncode2"
// ...
        ]
    }

let recordWithCRC =
   {
        name= "Record"; 
        inherits = None;
        implements = [];
        segments= 
        [
        ]
   }

let fileInfoBase =
   {
        name= "FileInfoBase"; 
        inherits = None;
        implements = [];
        segments= 
        [
           segment200 
           segment300
        ]
   }

let fileInfo =
    {
        name= "FileInfo"; 
        inherits = Some fileInfoBase;
        implements = [];
        segments= 
        [
            segment300a
        ]
    }

let fileInfo_931000 =
    {
        name= "FileInfo931000"; 
        inherits = None;
        implements = [];
        segments= 
        [
            segment200
        ]
    }

let record10 = 
    {
        name= "FileHeader_10"; 
        inherits = Some recordWithCRC;
        implements = [];
        segments= 
        [
            segment1 10
            segment10
            segmentCRC
        ]
    }

let record20 = 
    {
        name= "InvoiceHeader_20"; 
        inherits = Some recordWithCRC;
        implements = [];
        segments= 
        [
            (segment1 20)
            segment20
            segmentCRC
        ]
    }

let record50 = 
    {
        name= "ProvisionDetails_50"; 
        inherits = Some recordWithCRC;
        implements = [];
        segments= 
        [
            (segment1 50)
            segment50
            segmentCRC
        ]
    }

let record51 = 
    {
        name= "TariffCommitment_51"; 
        inherits = Some recordWithCRC;
        implements = [];
        segments= 
        [
            (segment1 51)
            segment51
            segmentCRC
        ]
    }

let record52 = 
    {
        name= "ProvisionDetailsContinuation_52"; 
        inherits = Some recordWithCRC;
        implements = [];
        segments= 
        [
            (segment1 52)
            segment52
            segmentCRC
        ]
    }

let record80 = 
    {
        name= "InvoiceFooter_80"; 
        inherits = Some recordWithCRC;
        implements = [];
        segments= 
        [
            (segment1 80)
            segment80
            segmentCRC
        ]
    }

let record90 = 
    {
        name= "AccountTotals_90"; 
        inherits = Some recordWithCRC;
        implements = [];
        segments= 
        [
            (segment1 90)
            segment90
            segmentCRC
        ]
    }

let record91 = 
    {
        name= "MutualityRejections_91"; 
        inherits = None;
        implements = [];
        segments= 
        [
            segment91
        ]
    }
    
let record92 = 
    {
        name= "SettlementTotals_92"; 
        inherits = None;
        implements = [];
        segments= 
        [
            segment92
        ]
    }
    
let record95 = 
    {
        name= "MutualityTotals_95"; 
        inherits = None;
        implements = [];
        segments= 
        [
            segment95
        ]
    }
    
let record96 = 
    {
        name= "FileTotals_96"; 
        inherits = None;
        implements = [];
        segments= 
        [
            segment96
        ]
    }

let fileInfoExtended =
    {
        name= "FileInfo_Extended"; 
        inherits = Some fileInfoBase;
        implements = [];
        segments= 
        [
            segment300b
        ]
    }
   
let record10Extended = 
    {
        name= "FileHeader_10_Extended"; 
        inherits = Some record10;
        implements = [ierror];
        segments= 
        [
            segmentResponse_identification 
            segmentResponse_datum_10 
            segmentResponse_102_103
            segmentResponse_104_110
            segmentResponse_Rejection
            segmentResponse_114_151
        ]
    }

let record20Extended = 
    {
        name= "InvoiceHeader_20_Extended"; 
        inherits = Some record20;
        implements = [ierror];
        segments= 
        [
            segmentResponse_identification 
            segmentResponse_datum
            segmentResponse_102_103
            segmentResponse_104_110
            segmentResponse_Rejection
            segmentResponse_114_151
        ]
    }

let record50Extended = 
    {
        name= "ProvisionDetails_50_Extended"; 
        inherits = Some record50;
        implements = [ierror];
        segments= 
        [
            segmentResponse_identification 
            segmentResponse_datum
            segmentResponse_50a
            segmentResponse_104_110
            segmentResponse_Rejection
            segmentResponse_114_118
            segmentResponse_50
        ]
    }

let record51Extended = 
    {
        name= "TariffCommitment_51_Extended"; 
        inherits = Some record51;
        implements = [ierror];
        segments= 
        [
            segmentResponse_identification 
            segmentResponse_datum
            segmentResponse_50a
            segmentResponse_104_110
            segmentResponse_Rejection
            segmentResponse_114_118
            segmentResponse_50
        ]
    }

let record80Extended = 
    {
        name= "InvoiceFooter_80_Extended"; 
        inherits = Some record80;
        implements = [ierror];
        segments= 
        [
            segmentResponse_identification 
            segmentResponse_datum
            segmentResponse_102_103
            segmentResponse_104_110
            segmentResponse_Rejection
            segmentResponse_114_151
        ]
    }

let record90Extended = 
    {
        name= "AccountTotals_90_Extended"; 
        inherits = Some record90;
        implements = [ierror];
        segments= 
        [
            segmentResponse_identification 
            segmentResponse_datum_90        
            segmentResponse_102_103
            segmentResponse_104_110
            segmentResponse_Rejection
            segmentResponse_114_151
        ]
    }

let namespaceRequests =
    {
        name="Requests";
        records=
        [
            fileInfoBase
            fileInfo
            fileInfo_931000
            record10
            record20
            record50
            record51
            record52
            record80
            record90
            record95
            record96
        ]
    }

let namespaceSettlement =
    {
        name="Settlement";
        records=
        [
            fileInfoExtended
            record10Extended
            record20Extended
            record50Extended
            record51Extended
            record80Extended
            record90Extended
            record91
            record92
        ]
    }

let prog = 
    { 
        filename="eFact.cs";
        baseNamespace="HdmpCloud.eHealth.eFact.Serializer.Recordformats.";
        namespaces = 
        [
            namespaceRequests
            namespaceSettlement
        ]
    }
