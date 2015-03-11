.PHONY: balance
LEDGERFILE = test.ledger

all:
	fsharpi -I FSharp.Data.2.1.1/lib/net40/ provider.fsx > $(LEDGERFILE)

balance:
	ledger --real bal -p "last month" -f $(LEDGERFILE)

cashflow:
	ledger bal -p "last month" ^exp ^inc -f $(LEDGERFILE)

networth:
	ledger bal ^assets ^liab -f $(LEDGERFILE)
