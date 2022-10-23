using System.Globalization;
using CsvHelper;
using OfficeOpenXml;
using NotSupportedException = System.NotSupportedException;

namespace Stellarity.Domain.Import;

public class KeyImportService
{
    private Func<string, IEnumerable<ImportKey>> _import = null!;

    public IEnumerable<ImportKey> ImportFrom(string path)
    {
        var type = GetFileType(path);
        SetImportMethod(type);
        var importKeys = _import(path);
        var optimized = OptimizeImported(importKeys);
        return optimized;
    }

    private static ImportFileType GetFileType(string path)
    {
        var file = new FileInfo(path);
        var ext = file.Extension[1..];
        return ext switch
        {
            "xlsx" => ImportFileType.Excel,
            "csv" => ImportFileType.Csv,
            _ => throw new NotSupportedException()
        };
    }

    private void SetImportMethod(ImportFileType type)
    {
        _import = type switch
        {
            ImportFileType.Csv => ImportFromCsv,
            ImportFileType.Excel => ImportFromExcel,
            _ => throw new NotSupportedException()
        };
    }

    private static IEnumerable<ImportKey> ImportFromCsv(string path)
    {
        using TextReader textReader = new StreamReader(path);
        using var reader = new CsvReader(textReader, CultureInfo.CurrentCulture);
        var records = reader.GetRecords<ImportKey>().ToArray();
        return records;
    }

    private static IEnumerable<ImportKey> ImportFromExcel(string path)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var file = new FileInfo(path);
        using var xl = new ExcelPackage(file);
        var sheet = xl.Workbook.Worksheets.FirstOrDefault();
        if (sheet is null) return ArraySegment<ImportKey>.Empty;

        var rowCount = sheet.Dimension.End.Row;
        var keys = new ImportKey[rowCount];
        for (var row = 1; row <= rowCount; row++)
        {
            var value = sheet.Cells[row, 1].Value?.ToString() ?? string.Empty;
            keys[row - 1] = new ImportKey(value);
        }

        return keys;
    }

    private static IEnumerable<ImportKey> OptimizeImported(IEnumerable<ImportKey> import)
    {
        var nonDuplicates = import.Distinct();
        var nonEmpty = nonDuplicates.Where(key => key.Value != string.Empty);
        return nonEmpty.ToArray();
    }
}