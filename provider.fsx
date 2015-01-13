#r "System.Core.dll"
#r "System.Xml.Linq.dll"
#r "FSharp.Data.dll"
open System.Xml.Linq
open FSharp.Data
// test.
type Gnucash = XmlProvider<"personal.gnucash">
let content = Gnucash.GetSample()
for account in content.Book.Accounts do
  printfn "%A" account.Name
