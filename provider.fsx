#r "System.Core.dll"
#r "System.Xml.Linq.dll"
#r "FSharp.Data.dll"

open System.Xml.Linq
open FSharp.Data

// TODO: support commodities.

type Gnucash = XmlProvider<"personal.gnucash">
let content = Gnucash.GetSample()
let accounts = content.Book.Accounts

// allow us to look up accounts.
let accountGuids = Array.map (fun (a: Gnucash.Account) -> (a.Id.Value, a)) accounts
let accountMap = new Map<System.Guid, Gnucash.Account>(accountGuids)

let validAccount (a: Gnucash.Account): bool =
  System.String.Compare(a.Name, "Root Account", true) <> 0 &&
  System.String.Compare(a.Type, "ROOT", true) <> 0

let findParent (a: Gnucash.Account) =
  match a.Parent with
    | None -> None
    | _ as p -> Some accountMap.[p.Value.Value]

let rec accountLineage a accum: string =
  match findParent(a) with
    | Some(p) when validAccount(p) -> accountLineage p (p.Name + ":" + accum)
    | _ -> accum

// print out accounts (with full path).
let validAccounts = Array.filter validAccount accounts
for account in validAccounts do
  let lineage = accountLineage account account.Name
  printfn "account %s" lineage
  if account.Description.IsSome then
    printfn "\tnote %s" account.Description.Value
  printfn ""

// print out transactions (sorted by date).
let byDate (t: Gnucash.Transaction) = t.DatePosted.Date
let transactions = Array.sortBy byDate content.Book.Transactions
for transaction in transactions do
  let description =
    match transaction.Description.String with
      | None -> "(none)"
      | Some(s) -> s
  let date = transaction.DatePosted.Date.ToString("yyyy/MM/dd")
  printfn "%s %s" date description
  for split in transaction.Splits do
    let account = accountMap.[split.Account.Value]
    let lineage = accountLineage account account.Name
    printfn "\t%s %s" lineage split.Value
  printfn ""
