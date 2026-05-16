using System.Diagnostics;
using IndependentWork24.Composite;
using IndependentWork24.Decorator;
using IndependentWork24.Proxy;


Console.OutputEncoding = System.Text.Encoding.UTF8;

PrintHeader("COMPOSITE — Файлова система");

var root = new DirectoryComposite("C:\\Projects");
var src  = new DirectoryComposite("src");
var docs = new DirectoryComposite("docs");

var mainFile    = new FileLeaf("Program.cs",   3_200, "// entry point");
var serviceFile = new FileLeaf("Service.cs",   8_500, "// service logic");
var readmeFile  = new FileLeaf("README.md",    1_200, "# Project");
var reportFile  = new FileLeaf("report.docx", 25_600, "Annual report");

src.Add(mainFile);
src.Add(serviceFile);
docs.Add(readmeFile);
docs.Add(reportFile);
root.Add(src);
root.Add(docs);

root.Display();
Console.WriteLine($"\n  - Загальний розмір дерева: {root.Size:N0} bytes\n");

PrintHeader("DECORATOR — Декорування файлів");

IFileSystemComponent baseFile   = new FileLeaf("secret.txt", 10_000, "confidential");

IFileSystemComponent encrypted  = new EncryptionDecorator(baseFile);
IFileSystemComponent compressed = new CompressionDecorator(baseFile);

IFileSystemComponent both       = new CompressionDecorator(new EncryptionDecorator(baseFile));

var logged = new LoggingDecorator(baseFile);

Console.WriteLine($"  Оригінал:              {baseFile.Name,-30} {baseFile.Size,8:N0} bytes");
Console.WriteLine($"  + Шифрування:          {encrypted.Name,-30} {encrypted.Size,8:N0} bytes");
Console.WriteLine($"  + Стиснення:           {compressed.Name,-30} {compressed.Size,8:N0} bytes");
Console.WriteLine($"  + Шифрув. + Стискання: {both.Name,-30} {both.Size,8:N0} bytes");

string cipher = EncryptionDecorator.Encrypt("Hello World");
string plain  = EncryptionDecorator.Decrypt(cipher);
Console.WriteLine($"\n  Шифр Цезаря: \"Hello World\" - \"{cipher}\" - \"{plain}\"");

_ = logged.Size;
logged.Display();
Console.WriteLine($"\n  Журнал логування ({logged.GetLog().Count} записів):");
foreach (var entry in logged.GetLog())
    Console.WriteLine($"    {entry}");

PrintHeader("PROXY — Контроль доступу за роллю");

var realFile = new RealFileAccess("confidential.txt", "Дуже секретні дані");

var adminProxy  = new FileAccessProxy(realFile, UserRole.Admin);
var editorProxy = new FileAccessProxy(realFile, UserRole.Editor);
var readerProxy = new FileAccessProxy(realFile, UserRole.Reader);
var guestProxy  = new FileAccessProxy(realFile, UserRole.Guest);

Console.WriteLine($"  Admin  читає:  \"{adminProxy.Read()}\"");
adminProxy.Write("Оновлено адміністратором");
Console.WriteLine($"  Admin  записав. Нові дані: \"{adminProxy.Read()}\"");

editorProxy.Write("Оновлено редактором");
Console.WriteLine($"  Editor записав. Нові дані: \"{editorProxy.Read()}\"");

Console.WriteLine($"  Reader читає:  \"{readerProxy.Read()}\"");
TryAction("  Reader  - Write", () => readerProxy.Write("спроба"));

TryAction("  Guest   - Read",  () => guestProxy.Read());
TryAction("  Guest   - Write", () => guestProxy.Write("спроба"));

Console.WriteLine("\n  Журнал доступу Admin:");
foreach (var entry in adminProxy.GetAccessLog())
    Console.WriteLine($"    {entry}");

PrintHeader("ІНТЕГРАЦІЯ: Composite + Decorator + Proxy");

var sensitiveFile = new FileLeaf("payroll.xlsx", 50_000, "Employee salaries");
var securedDecoratedFile = new EncryptionDecorator(sensitiveFile);

var secureRoot = new DirectoryComposite("HR");
secureRoot.Add(securedDecoratedFile);
secureRoot.Add(new FileLeaf("policy.pdf", 3_000));

Console.WriteLine("  Структура директорії HR (з декорованим файлом):");
secureRoot.Display(2);
Console.WriteLine($"\n  Розмір директорії HR: {secureRoot.Size:N0} bytes");

var hrReal    = new RealFileAccess("payroll.xlsx", "John: $5000, Jane: $6000");
var hrReader  = new FileAccessProxy(hrReal, UserRole.Reader);
var hrGuest   = new FileAccessProxy(hrReal, UserRole.Guest);

Console.WriteLine($"\n  HR Reader читає: \"{hrReader.Read()}\"");
TryAction("  HR Guest - Read", () => hrGuest.Read());

PrintHeader("ПОРІВНЯННЯ ПРОДУКТИВНОСТІ");

const int Iterations = 500_000;
var sw = Stopwatch.StartNew();

for (int i = 0; i < Iterations; i++)
{
    var f = new FileLeaf("t.txt", 1000);
    _ = f.Size;
}
sw.Stop();
long baseMs = sw.ElapsedMilliseconds;
Console.WriteLine($"  Базовий FileLeaf       ({Iterations:N0} ітерацій): {baseMs} мс");

sw.Restart();
for (int i = 0; i < Iterations; i++)
{
    var f = new FileLeaf("t.txt", 1000);
    var d = new CompressionDecorator(new EncryptionDecorator(f));
    _ = d.Size;
}
sw.Stop();
long decoratedMs = sw.ElapsedMilliseconds;
Console.WriteLine($"  Подвійний Decorator    ({Iterations:N0} ітерацій): {decoratedMs} мс");

sw.Restart();
var realPerf = new RealFileAccess("perf.txt", "data");
var proxy    = new FileAccessProxy(realPerf, UserRole.Reader);
for (int i = 0; i < Iterations; i++)
{
    _ = proxy.Read();
}
sw.Stop();
long proxyMs = sw.ElapsedMilliseconds;
Console.WriteLine($"  Proxy Read             ({Iterations:N0} ітерацій): {proxyMs} мс");

double decOverhead  = baseMs > 0 ? (double)(decoratedMs - baseMs) / baseMs * 100 : 0;
double proxyOverhead = baseMs > 0 ? (double)proxyMs / baseMs * 100 : 0;

Console.WriteLine();
Console.WriteLine($"  Накладні витрати Decorator: +{decOverhead:F1}%");
Console.WriteLine($"  Відсоток Proxy від базового: {proxyOverhead:F1}%");


static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine($"  {title}");
}

static void TryAction(string label, Action action)
{
    try
    {
        action();
        Console.WriteLine($"{label} - Дозволено");
    }
    catch (UnauthorizedAccessException ex)
    {
        Console.WriteLine($"{label} - {ex.Message.Split('\n')[0]}");
    }
}