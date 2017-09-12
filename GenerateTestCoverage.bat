cd MyWpfApp.Tests\bin\Debug\

..\..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -target:"..\..\..\packages\xunit.runner.console.2.2.0\tools\xunit.console.exe" -targetargs:"\"MyWpfApp.Tests.dll\" -xml \"..\..\..\TestResults\MyWpfApp.Tests.xunit.xml\"" -output:"..\..\..\TestResults\coverage.xml" -skipautoprops -returntargetcode -filter:"+[MyWpfApp*]* -[MyWpfApp.Tests]*

..\..\..\packages\ReportGenerator.2.5.11\tools\ReportGenerator.exe -reports:"..\..\..\TestResults\coverage.xml" -targetdir:"..\..\..\TestResults\TestCoverage"
