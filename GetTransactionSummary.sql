CREATE OR ALTER PROCEDURE GetTransactionSummary
AS
BEGIN
    -- Select from Purchase and PurchaseDetail tables
    SELECT 
        a.Date AS [Tanggal],
        a.No AS [No. Trx],
        c.Name AS [Keterangan],
        b.Quantity AS [Masuk (Qty)],
        0 AS [Keluar (Qty)],
        (b.Quantity * d.Price) AS [Saldo (Qty)]
    FROM 
        Purchase a
    JOIN 
        PurchaseDetail b ON a.PurchaseID = b.PurchaseID
    JOIN 
        Supplier c ON a.SupplierID = c.SupplierID
    JOIN 
        Product d ON b.ProductID = d.ProductID

    UNION ALL

    -- Select from Sales and SalesDetail tables
    SELECT 
        a.Date AS [Tanggal],
        a.No AS [No. Trx],
        c.Name AS [Keterangan],
        0 AS [Masuk (Qty)],
        b.Quantity AS [Keluar (Qty)],
        (b.Quantity * d.Price) AS [Saldo (Qty)]
    FROM 
        Sales a
    JOIN 
        SalesDetail b ON a.SalesID = b.SalesID
    JOIN 
        Customer c ON a.CustomerID = c.CustomerID
    JOIN 
        Product d ON b.ProductID = d.ProductID;
END;
