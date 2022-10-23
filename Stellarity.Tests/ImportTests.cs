using Stellarity.Domain.Import;

namespace Stellarity.Tests;

public class ImportTests
{
    private KeyImportService _importService = null!;
    private const string Csv = @"C:\Users\ender\Desktop\import.csv";
    private const string Excel = @"C:\Users\ender\Desktop\import.xlsx";
    private const string ExcelOld = @"C:\Users\ender\Desktop\import.xls";

    [SetUp]
    public void SetUp()
    {
        _importService = new KeyImportService();
    }

    [Test]
    public void LoadCsv()
    {
        var keys = _importService.ImportFrom(Csv).ToArray();
        Assert.That(keys, Has.Length.EqualTo(4));
    }

    [Test]
    public void LoadExcel()
    {
        var keys = _importService.ImportFrom(Excel).ToArray();
        Assert.That(keys, Has.Length.EqualTo(4));
    }

    [Test]
    public void LoadOldExcel()
    {
        Assert.Throws<NotSupportedException>(() =>
        {
            var keys = _importService.ImportFrom(ExcelOld).ToArray();
        });
    }
}