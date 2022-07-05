using LuccaDevises.Configuration;
using LuccaDevises.Services;

if (Environment.GetCommandLineArgs().Length != 2)
{
    Console.WriteLine("Usage: LuccaDevise.exe <input file>");
    Environment.Exit(1);
}

try
{
    ExchangeRateService exchangeRateService = new ExchangeRateService();
    string file = Environment.GetCommandLineArgs()[1];
    InputReader inputReader = new InputReader(exchangeRateService);
    inputReader.ReadFile(file);

    var result = exchangeRateService.Convert(inputReader.SourceCurrency, inputReader.TargetCurrency, inputReader.Amount);
    Console.Write(result);
}
catch (ArgumentException ex)
{
    Console.Write(ex.Message);
}

