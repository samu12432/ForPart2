using System.Globalization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ForParts.Models.Budgetes;
public interface IBudgetPdfService
{
    byte[] Generate(Budget b);
}
