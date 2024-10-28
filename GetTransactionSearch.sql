CREATE OR ALTER PROCEDURE GetTransactionSummaryWithFilters
    @Barang VARCHAR(50),       
    @StartDate DATE,           
    @EndDate DATE              
AS
BEGIN
    CREATE TABLE #TransactionSummary (
        Tanggal DATE,
        [No. Trx] VARCHAR(50),
        Keterangan VARCHAR(100),
        [Masuk (Qty)] INT,
        [Keluar (Qty)] INT,
        [Saldo (Qty)] DECIMAL(18, 2)
    );

    INSERT INTO #TransactionSummary (Tanggal, [No. Trx], Keterangan, [Masuk (Qty)], [Keluar (Qty)], [Saldo (Qty)])
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
    WHERE 
        d.Name = @Barang
    AND
        a.Date BETWEEN @StartDate AND @EndDate

    UNION ALL

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
        Product d ON b.ProductID = d.ProductID
    WHERE 
        d.Name = @Barang
    AND
        a.Date BETWEEN @StartDate AND @EndDate;

    SELECT * FROM #TransactionSummary ORDER BY Tanggal, [No. Trx] DESC;

    DROP TABLE #TransactionSummary;
END;